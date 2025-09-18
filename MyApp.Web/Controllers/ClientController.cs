using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Services;
using MyApp.Web.Models;
using System.Linq;

namespace MyApp.Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly DataService _dataService;

        public ClientController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var clients = _dataService.Data.Clients;
            return View(clients);
        }

        [HttpPost]
        public IActionResult SelectClient(int clientId)
        {
            if (clientId == 0)
            {
                // If no client is selected, return to the selection page with an error.
                ViewBag.ErrorMessage = "Please select a client.";
                var clients = _dataService.Data.Clients;
                return View("Index", clients);
            }

            // Store the selected client ID in TempData, which persists for one redirect.
            TempData["SelectedClientId"] = clientId;
            return RedirectToAction("Projects");
        }

        [HttpGet]
        public IActionResult Projects()
        {
            if (TempData["SelectedClientId"] is not int clientId)
            {
                // If no client is selected (e.g., direct navigation), redirect to the selection page.
                return RedirectToAction("Index");
            }

            // Keep the client ID in TempData for subsequent requests within the client's session.
            TempData.Keep("SelectedClientId");

            var assignedProjects = _dataService.Data.Projects
                .Where(p => p.AssignedClientIds.Contains(clientId))
                .ToList();

            var client = _dataService.Data.Clients.FirstOrDefault(c => c.Id == clientId);
            ViewBag.ClientName = client != null ? $"{client.FirstName} {client.LastName}" : "Client";

            return View(assignedProjects);
        }
    }
}
