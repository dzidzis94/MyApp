using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Web.Data;
using MyApp.Web.Models.Entities;
using MyApp.Web.Models.ViewModels;
using MyApp.Web.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly DarbuContext _context;
        private readonly AuthService _authService;

        public AdminController(DarbuContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var recentActivities = await _context.RecentActivities
                .Include(ra => ra.User)
                .OrderByDescending(ra => ra.CreatedAt)
                .Take(20)
                .ToListAsync();
            return View(recentActivities);
        }

        // Project Management
        public async Task<IActionResult> Projects()
        {
            var projects = await _context.Projects.Include(p => p.CreatedBy.User).ToListAsync();
            return View(projects);
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(Project project)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var admin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == userId);
                if (admin == null)
                {
                    return Unauthorized("You are not an authorized administrator.");
                }

                project.CreatedById = admin.Id;
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                await LogActivity($"Created new project: '{project.Title}'", project.Id);

                return RedirectToAction(nameof(Projects));
            }
            return View(project);
        }

        public async Task<IActionResult> ProjectDetails(int id)
        {
            var project = await _context.Projects
                .Include(p => p.SubSections)
                    .ThenInclude(ss => ss.Submissions)
                        .ThenInclude(sub => sub.Client.User)
                .Include(p => p.CreatedBy.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpGet]
        public IActionResult AddSubSection(int projectId)
        {
            var model = new ProjectSubSection { ProjectId = projectId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubSection(ProjectSubSection subSection)
        {
            if (ModelState.IsValid)
            {
                _context.ProjectSubSections.Add(subSection);
                await _context.SaveChangesAsync();
                await LogActivity($"Added sub-section '{subSection.Title}' to project ID {subSection.ProjectId}", subSection.Id);
                return RedirectToAction("ProjectDetails", new { id = subSection.ProjectId });
            }
            return View(subSection);
        }

        // Client Management
        public async Task<IActionResult> Clients()
        {
            var clients = await _context.Clients.Include(c => c.User).ToListAsync();
            return View(clients);
        }

        [HttpGet]
        public IActionResult AddClient()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClient(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PasswordHash = _authService.HashPassword(model.Password),
                    Role = "Client"
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var client = new Client { UserId = user.Id };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                await LogActivity($"Added new client: {user.Email}", client.Id);

                return RedirectToAction(nameof(Clients));
            }
            return View(model);
        }

        private async Task LogActivity(string action, int? relatedId = null)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var activity = new RecentActivity
            {
                UserId = userId,
                Action = action,
                RelatedId = relatedId,
                CreatedAt = DateTime.UtcNow
            };
            _context.RecentActivities.Add(activity);
            await _context.SaveChangesAsync();
        }
    }
}