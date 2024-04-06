using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProyectoPAW.Services;
using ProyectoPAW.Areas.Identity.Data;
using ProyectoPAW.Models;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UsersContextConnection") ?? throw new InvalidOperationException("Connection string 'UsersContextConnection' not found.");

builder.Services.AddDbContext<UsersContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ProyectoWebAvanzadoContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("UsersContextConnection")));


builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UsersContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICantidadRecetas, CantidadRecetas>();

#region Authorization

AddAuthorizationPolicies(builder.Services);

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

void AddAuthorizationPolicies(IServiceCollection services)
{
    services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Administrador"));
        options.AddPolicy("RequireProfesor", policy => policy.RequireRole("Profesor"));
        options.AddPolicy("RequireEstudiante", policy => policy.RequireRole("Estudiante"));
    });

}