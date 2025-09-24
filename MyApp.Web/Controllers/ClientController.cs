using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Web.Data;
using MyApp.Web.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; //  PIEVIENO ŠO

namespace MyApp.Web.Controllers
{
    [Authorize(Roles = "Client,Admin")] //  PIEVIENO ŠO - gan klienti, gan admini var piekļūt
    public class ClientController : Controller
    {
        private readonly DarbuContext _context;

        public ClientController(DarbuContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Ja lietotājs ir klients, rāda tikai viņa datus
            if (User.IsInRole("Client"))
            {
                var userEmail = User.Identity.Name;
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == userEmail);

                if (client != null)
                {
                    // Novirza uz projektiem, ja klients
                    return RedirectToAction("Projects", new { clientId = client.Id });
                }
            }

            // Adminiem rāda visu klientu sarakstu
            var clients = await _context.Clients.ToListAsync();
            return View(clients);
        }

        [HttpGet]
        public async Task<IActionResult> Projects(int? clientId)
        {
            // Nosaka, kurš klients skatās projekti
            int currentClientId;

            if (User.IsInRole("Admin") && clientId.HasValue)
            {
                // Adminis skata konkrēta klienta projektus
                currentClientId = clientId.Value;
            }
            else if (User.IsInRole("Client"))
            {
                // Klients skata savus projektus
                var userEmail = User.Identity.Name;
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == userEmail);

                if (client == null)
                {
                    return RedirectToAction("Logout", "Account");
                }

                currentClientId = client.Id;
            }
            else
            {
                return RedirectToAction("Index");
            }

            // Iegūst klienta projektus
            var assignedProjects = await _context.Projects
                .Where(p => p.AssignedClientIds != null && p.AssignedClientIds.Contains(currentClientId))
                .ToListAsync();

            var clientInfo = await _context.Clients.FindAsync(currentClientId);
            ViewBag.ClientName = clientInfo != null ? $"{clientInfo.FirstName} {clientInfo.LastName}" : "Klients";
            ViewBag.ClientId = currentClientId;

            return View(assignedProjects);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProjectData(int projectId, string sectionData)
        {
            // Ļauj klientam atjaunināt projekta datus
            if (User.IsInRole("Client"))
            {
                var project = await _context.Projects.FindAsync(projectId);
                if (project != null)
                {
                    // Šeit var implementēt datu atjaunināšanas loģiku
                    // Piemēram: project.SectionData = sectionData;
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Dati veiksmīgi atjaunināti!";
                }
            }

            return RedirectToAction("Projects");
        }

        // Admina funkcija - projekta piešķiršana klientam
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AssignProjectToClient(int projectId, int clientId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project != null)
            {
                if (project.AssignedClientIds == null)
                {
                    project.AssignedClientIds = new List<int>();
                }

                if (!project.AssignedClientIds.Contains(clientId))
                {
                    project.AssignedClientIds.Add(clientId);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Projekts veiksmīgi piešķirts klientam!";
                }
            }

            return RedirectToAction("Projects", new { clientId = clientId });
        }
    }
}