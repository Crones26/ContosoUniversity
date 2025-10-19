using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder
	.Services
	.AddDbContext<UniversityContext>
	(
		opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"))
	);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	
	app.UseHsts();
}

IServiceScope scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;

UniversityContext context = services.GetRequiredService<UniversityContext>();
DbInitializer.Initialize(context);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
