using Microsoft.EntityFrameworkCore;
using PharmacyApp.Infrastructure;
using PharmacyApp.Infrastructure.Repositories;
using PharmacyApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var rootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../.."));
var dbPath = Path.Combine(rootPath, "PharmacyApp.db");

builder.Services.AddDbContext<PharmacyAppContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(ICrudServiceAsync<>), typeof(EfCrudServiceAsync<>));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PharmacyAppContext>();
    context.Database.Migrate();
}

app.Run();