using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MyApp.Web.Models.Entities;
using MyApp.Web.Services;
using System;
using System.Linq;

namespace MyApp.Web.Data
{
    public static class DataSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DarbuContext>();
                var authService = serviceScope.ServiceProvider.GetService<AuthService>();

                if (context == null || authService == null)
                {
                    throw new InvalidOperationException("Could not resolve services for data seeding.");
                }

                // It's generally better to apply migrations at deployment time,
                // but this ensures the database is created for local development.
                context.Database.Migrate();

                if (!context.Users.Any())
                {
                    // 1. Create Admin User and Admin Entity
                    var adminUser = new User
                    {
                        UserName = "admin",
                        Email = "admin@myapp.com",
                        PasswordHash = authService.HashPassword("password"),
                        Role = "Admin"
                    };
                    context.Users.Add(adminUser);
                    context.SaveChanges(); // Save to get the User ID

                    var admin = new Admin { UserId = adminUser.Id };
                    context.Admins.Add(admin);
                    context.SaveChanges(); // Save to get the Admin ID

                    // 2. Create Client User and Client Entity
                    var clientUser = new User
                    {
                        UserName = "client",
                        Email = "client@myapp.com",
                        PasswordHash = authService.HashPassword("password"),
                        Role = "Client"
                    };
                    context.Users.Add(clientUser);
                    context.SaveChanges(); // Save to get the User ID

                    var client = new Client { UserId = clientUser.Id };
                    context.Clients.Add(client);

                    // 3. Create Project linked to Admin
                    var project = new Project
                    {
                        Title = "Sample Project",
                        Description = "This is a sample project seeded by the application.",
                        CreatedById = admin.Id
                    };
                    context.Projects.Add(project);

                    // 4. Create Project Sub-Sections
                    project.SubSections.Add(new ProjectSubSection
                    {
                        Title = "Sub-Section 1: Introduction",
                        Description = "Please provide an introduction."
                    });
                    project.SubSections.Add(new ProjectSubSection
                    {
                        Title = "Sub-Section 2: Main Content",
                        Description = "Please provide the main content."
                    });
                    project.SubSections.Add(new ProjectSubSection
                    {
                        Title = "Sub-Section 3: Conclusion",
                        Description = "Please provide a conclusion."
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}