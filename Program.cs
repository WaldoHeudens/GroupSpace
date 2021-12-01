using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GroupSpace.Data;
using Microsoft.AspNetCore.Identity;
using GroupSpace.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// de connectionString moet manueel toegewezen worden om de nieuwe datacontext te koppelen aan onze bestaande databank
var connectionString = builder.Configuration.GetConnectionString("GroupSpaceContext");

builder.Services.AddDbContext<GroupSpaceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GroupSpaceContext")));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityContext>();builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();

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

// Toegevoegd op met de identity-pages te kunnen werken
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedDatacontext.Initialize(services);
}
app.Run();
