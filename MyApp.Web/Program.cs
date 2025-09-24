using MyApp.Web.Services;
using Microsoft.EntityFrameworkCore;
using MyApp.Web.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddSingleton<DataService>();

// Pievienojam DbContext
builder.Services.AddDbContext<DarbuContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DarbuDatabase")));

// ✅ PIEVIENO AUTENTIFIKĀCIJAS SERVISUS
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

// ✅ PIEVIENO AUTORIZĀCIJAS SERVISUS
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("ClientOnly", policy =>
        policy.RequireRole("Client"));

    options.AddPolicy("Authenticated", policy =>
        policy.RequireAuthenticatedUser());
});

// ✅ PIEVIENO SESIJAS SERVISUS (ja vajag)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ PIEVIENO LOGGING (noderīgi autentifikācijai)
builder.Services.AddLogging();

var app = builder.Build();

// ✅ Seed datu bāzi
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DarbuContext>();
    context.Database.Migrate(); // Izmanto Migrate() nevis EnsureCreated()

    var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "database.json");
    DataSeeder.SeedFromJson(context, jsonPath);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ ĻOTI SVARĪGI: Šīm līnijām JĀBUT ŠAJĀ SECĪBĀ!
app.UseAuthentication();    // Vispirms autentifikācija
app.UseAuthorization();     // Tad autorizācija
app.UseSession();           // Tad sesija (ja lieto)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

// ✅ PIEVIENO ACCOUNT MARŠRUTUS
app.MapControllerRoute(
    name: "account",
    pattern: "Account/{action=Login}/{id?}",
    defaults: new { controller = "Account" });

app.MapControllerRoute(
    name: "client",
    pattern: "Client/{action=Projects}/{id?}",
    defaults: new { controller = "Client" });

app.Run();