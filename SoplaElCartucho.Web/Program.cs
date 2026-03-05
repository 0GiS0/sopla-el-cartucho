using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Servir archivos estáticos desde Content/ (compatibilidad con ASP.NET MVC legacy)
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Content")),
    RequestPath = "/Content"
});

// Servir archivos estáticos desde Scripts/ (compatibilidad con ASP.NET MVC legacy)
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Scripts")),
    RequestPath = "/Scripts"
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

// Map routes (migrated from Global.asax.cs RegisterRoutes)
app.MapControllerRoute(
    name: "CatalogoConsola",
    pattern: "Catalogo/Consola/{consolaId}",
    defaults: new { controller = "Catalogo", action = "PorConsola" });

app.MapControllerRoute(
    name: "DetalleJuego",
    pattern: "Juego/{id}/{slug?}",
    defaults: new { controller = "Catalogo", action = "Detalle" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

Console.WriteLine("🎮 ¡Sopla el Cartucho ha iniciado! 💨");

app.Run();
