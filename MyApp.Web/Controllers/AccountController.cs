using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Models;
using MyApp.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MyApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DarbuContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(DarbuContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Account/Login
        public async Task<IActionResult> Login()
        {
            // Pārbauda vai sistēma ir jāiestata (nav neviena administratora)
            if (await IsFirstTimeSetupRequired())
            {
                return RedirectToAction("FirstTimeSetup");
            }

            // Ja lietotājs jau ir autentificēts, novirza uz attiecīgo lapu
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToDashboard();
            }

            return View();
        }

        // GET: /Account/FirstTimeSetup
        [AllowAnonymous]
        public async Task<IActionResult> FirstTimeSetup()
        {
            // Pārbauda vai setup joprojām ir nepieciešams
            if (!await IsFirstTimeSetupRequired())
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // POST: /Account/FirstTimeSetup
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FirstTimeSetup(SetupModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Pārbauda vai setup joprojām ir nepieciešams
                    if (!await IsFirstTimeSetupRequired())
                    {
                        ModelState.AddModelError("", "Sistēma jau ir iestatīta");
                        return View(model);
                    }

                    // Izveido pirmo administratoru
                    var admin = new Admin
                    {
                        Name = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password, // Šeit vajadzētu hash paroli
                        IsActive = true
                    };

                    _context.Admins.Add(admin);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Pirmais administrators izveidots: {Email}", model.Email);

                    // Automātiski pieslēdz administratoru
                    await SignInUser(admin.Email, "Admin", admin.Id, false);

                    TempData["SetupSuccess"] = true;
                    return RedirectToAction("Index", "Admin");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Kļūda iestatot sistēmu");
                    ModelState.AddModelError("", "Radās kļūda, mēģiniet vēlreiz: " + ex.Message);
                }
            }

            return View(model);
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Pārbauda vai sistēma ir jāiestata
                    if (await IsFirstTimeSetupRequired())
                    {
                        return RedirectToAction("FirstTimeSetup");
                    }

                    // Meklē administratoru
                    var admin = await _context.Admins
                        .FirstOrDefaultAsync(a => a.Email == model.Email && a.Password == model.Password);

                    if (admin != null)
                    {
                        await SignInUser(admin.Email, "Admin", admin.Id, model.RememberMe);
                        _logger.LogInformation("Administrators {Email} veiksmīgi pieteicās", admin.Email);
                        return RedirectToAction("Index", "Admin");
                    }

                    // Meklē klientu
                    var client = await _context.Clients
                        .FirstOrDefaultAsync(c => c.Email == model.Email);

                    // Vienkārša parole (nākotnē vajadzētu hash)
                    if (client != null && model.Password == "client123") // Pagaidu parole klientiem
                    {
                        await SignInUser(client.Email, "Client", client.Id, model.RememberMe);
                        _logger.LogInformation("Klients {Email} veiksmīgi pieteicās", client.Email);
                        return RedirectToAction("Projects", "Client");
                    }

                    ModelState.AddModelError("", "Nepareizs e-pasts vai parole");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Kļūda pieslēgšanās laikā");
                    ModelState.AddModelError("", "Radās kļūda, mēģiniet vēlreiz");
                }
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Notīra autentifikācijas cookie
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Notīra sesiju
                HttpContext.Session.Clear();

                _logger.LogInformation("Lietotājs veiksmīgi izlogojās");

                // Veiksmīgs redirect uz login lapu
                TempData["SuccessMessage"] = "Veiksmīgi izlogojāties no sistēmas";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kļūda izlogošanās laikā");
                // Ja kaut kas noiet greizi, tomēr novirza uz login lapu
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: /Account/Logout (alternatīva GET metode)
        [HttpGet]
        public async Task<IActionResult> LogoutGet()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Veiksmīgi izlogojāties no sistēmas";
            return RedirectToAction("Login", "Account");
        }

        // ✅ PIEVIENO ŠO METODI - Lietotāja pieslēgšana
        private async Task SignInUser(string email, string role, int userId, bool rememberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("UserType", role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        // ✅ PIEVIENO ŠO METODI - Novirzīšana uz kontrolpaneli
        private IActionResult RedirectToDashboard()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (User.IsInRole("Client"))
            {
                return RedirectToAction("Projects", "Client");
            }

            return RedirectToAction("Login");
        }

        // ✅ PIEVIENO ŠO METODI - Piekļuves liegšanas lapa
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Palīgmetode - pārbauda vai nepieciešams setup
        private async Task<bool> IsFirstTimeSetupRequired()
        {
            return !await _context.Admins.AnyAsync();
        }
    }
}