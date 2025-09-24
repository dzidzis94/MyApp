using Microsoft.EntityFrameworkCore;
using MyApp.Web.Data;
using MyApp.Web.Models;

public static class DataSeeder
{
    public static void SeedFromJson(DarbuContext context, string jsonPath)
    {
        // ✅ 1. VISPIRMS: Izveido pirmo administratoru
        if (!context.Admins.Any())
        {
            var defaultAdmin = new Admin
            {
                Name = "Sistēmas",
                LastName = "Administrators",
                Email = "admin@example.com",
                Password = "admin123",
                PhoneNumber = "+371 12345678",
                IsActive = true
            };
            context.Admins.Add(defaultAdmin);
            context.SaveChanges();
            Console.WriteLine("✅ Izveidots pirmais administrators: admin@example.com / admin123");
        }

        // ✅ 2. TAD: Importē pārējos datus no JSON (ja vēlies)
        if (System.IO.File.Exists(jsonPath))
        {
            try
            {
                var jsonData = System.IO.File.ReadAllText(jsonPath);
                var data = System.Text.Json.JsonSerializer.Deserialize<ApplicationData>(jsonData);

                if (data != null)
                {
                    // Importē klientus
                    if (data.Clients != null && !context.Clients.Any())
                    {
                        context.Clients.AddRange(data.Clients);
                    }

                    // Importē projektus
                    if (data.Projects != null && !context.Projects.Any())
                    {
                        context.Projects.AddRange(data.Projects);
                    }

                    context.SaveChanges();
                    Console.WriteLine("✅ Dati veiksmīgi importēti no JSON");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Kļūda importējot datus: {ex.Message}");
            }
        }
    }
}