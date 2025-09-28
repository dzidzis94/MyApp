using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Web.Data;
using MyApp.Web.Models.Entities;
using MyApp.Web.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyApp.Web.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private readonly DarbuContext _context;

        public ClientController(DarbuContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // NOTE: The current database schema does not support assigning specific projects to clients.
            // As a result, we are displaying all available projects.
            // This could be revised if an assignment feature is added to the schema.
            var allProjects = await _context.Projects.ToListAsync();
            return View(allProjects);
        }

        public async Task<IActionResult> ProjectDetails(int id)
        {
            var project = await _context.Projects
                .Include(p => p.SubSections)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpGet]
        public async Task<IActionResult> FillSubSection(int id)
        {
            var subSection = await _context.ProjectSubSections.FindAsync(id);
            if (subSection == null)
            {
                return NotFound();
            }

            var model = new SubSectionFillViewModel
            {
                SubSectionId = subSection.Id,
                Title = subSection.Title,
                Content = "" // Or load previous data if needed
            };

            ViewBag.ProjectId = subSection.ProjectId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FillSubSection(SubSectionFillViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var subSectionForProjectId = await _context.ProjectSubSections.FindAsync(model.SubSectionId);
                ViewBag.ProjectId = subSectionForProjectId?.ProjectId;
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                return Unauthorized("You are not an authorized client.");
            }

            var submission = new ProjectSubSectionSubmission
            {
                SubSectionId = model.SubSectionId,
                ClientId = client.Id,
                Content = model.Content,
                SubmittedAt = DateTime.UtcNow
            };

            _context.ProjectSubSectionSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            var subSection = await _context.ProjectSubSections.FindAsync(model.SubSectionId);
            return RedirectToAction("ProjectDetails", new { id = subSection.ProjectId });
        }
    }
}