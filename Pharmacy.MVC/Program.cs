using Microsoft.EntityFrameworkCore;
using PharmacyApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var rootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../.."));
var dbPath = Path.Combine(rootPath, "PharmacyApp.db");

builder.Services.AddDbContext<PharmacyAppContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PharmacyAppContext>();
    context.Database.Migrate();
}

app.Run();