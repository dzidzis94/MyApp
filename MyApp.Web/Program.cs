using MyApp.Web.Services;

var builder = WebApplication.CreateBuilder(args);

using Microsoft.AspNetCore.Authentication.Cookies;
using MyApp.Web.Services;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DataService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

SeedData(app);

app.Run();

void SeedData(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dataService = services.GetRequiredService<DataService>();
        var authService = services.GetRequiredService<AuthService>();

        if (!dataService.Data.Users.Any())
        {
            dataService.Data.Users.Add(new MyApp.Web.Models.User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@myapp.com",
                PasswordHash = authService.HashPassword("password"),
                Role = MyApp.Web.Models.UserRole.Admin
            });

            dataService.Data.Users.Add(new MyApp.Web.Models.User
            {
                Id = 2,
                FirstName = "Client",
                LastName = "User",
                Email = "client@myapp.com",
                PasswordHash = authService.HashPassword("password"),
                Role = MyApp.Web.Models.UserRole.Client
            });

            dataService.SaveData();
        }
    }
}
