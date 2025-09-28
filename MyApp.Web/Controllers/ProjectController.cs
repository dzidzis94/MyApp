using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services;
using MyApp.Web.Models;
using System.Linq;
using System.Security.Claims;

namespace MyApp.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly DataService _dataService;

        public ProjectController(DataService dataService)
        {
            _dataService = dataService;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var projects = _dataService.Data.Projects;
                return View(projects);
            }
            else
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var projects = _dataService.Data.Projects.Where(p => p.AssignedUserIds.Contains(userId));
                return View(projects);
            }
        }

        public IActionResult Details(int id)
        {
            var project = _dataService.Data.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.Id = _dataService.GetNextProjectId();
                _dataService.Data.Projects.Add(project);
                _dataService.SaveData();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddSubSection(int projectId, string title)
        {
            var project = _dataService.Data.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return NotFound();
            }

            var newSubSection = new SubSection
            {
                Id = _dataService.GetNextSubSectionId(project),
                Title = title,
                ParentId = null // Simple, non-nested structure for now
            };

            project.SubSections.Add(newSubSection);
            _dataService.SaveData();

            return RedirectToAction("Details", new { id = projectId });
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        public IActionResult UpdateSubSectionData(int projectId, int subSectionId, string data)
        {
            var project = _dataService.Data.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null)
            {
                return NotFound();
            }

            var subSection = project.SubSections.FirstOrDefault(s => s.Id == subSectionId);
            if (subSection == null)
            {
                return NotFound();
            }

            subSection.Data = data;
            _dataService.SaveData();

            return RedirectToAction("Details", new { id = projectId });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var project = _dataService.Data.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            var viewModel = new ProjectEditViewModel
            {
                Project = project,
                AllClients = _dataService.Data.Users.Where(u => u.Role == UserRole.Client).ToList(),
                SelectedClientIds = project.AssignedUserIds
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id, ProjectEditViewModel viewModel)
        {
            var project = _dataService.Data.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            project.Title = viewModel.Project.Title;
            project.AssignedUserIds = viewModel.SelectedClientIds;
            _dataService.SaveData();

            return RedirectToAction(nameof(Index));
        }
    }
}