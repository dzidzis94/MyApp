using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Models;
using MyApp.Web.Services;
using System.Linq;

namespace MyApp.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataService _dataService;

        public AdminController(DataService dataService)
        {
            _dataService = dataService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Clients()
        {
            var clients = _dataService.Data.Clients;
            return View(clients);
        }

        [HttpGet]
        public IActionResult AddClient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddClient(Client client)
        {
            if (ModelState.IsValid)
            {
                client.Id = _dataService.Data.Clients.Any() ? _dataService.Data.Clients.Max(c => c.Id) + 1 : 1;
                _dataService.Data.Clients.Add(client);
                _dataService.SaveData();
                return RedirectToAction("Clients");
            }
            return View(client);
        }

        [HttpGet]
        public IActionResult EditClient(int id)
        {
            var client = _dataService.Data.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        public IActionResult EditClient(Client client)
        {
            if (ModelState.IsValid)
            {
                var existingClient = _dataService.Data.Clients.FirstOrDefault(c => c.Id == client.Id);
                if (existingClient != null)
                {
                    existingClient.FirstName = client.FirstName;
                    existingClient.LastName = client.LastName;
                    _dataService.SaveData();
                }
                return RedirectToAction("Clients");
            }
            return View(client);
        }

        [HttpPost]
        public IActionResult RemoveClient(int id)
        {
            var client = _dataService.Data.Clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                _dataService.Data.Clients.Remove(client);
                _dataService.SaveData();
            }
            return RedirectToAction("Clients");
        }
    }
}
