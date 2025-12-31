using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using projekt_zaliczeniowy.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");;

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

//builder.Services.AddRazorPages();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    string[] roleNames = { "Admin", "User" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            // Tworzenie roli w tabeli AspNetRoles
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Tworzenie Administratora
    var adminEmail = "a@a.pl";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var newAdmin = new AppUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "rafal", // Twoje dodatkowe pola
            LastName = "lagosz",
            EmailConfirmed = true
        };

        var createAdmin = await userManager.CreateAsync(newAdmin, "Rafal-123");
        if (createAdmin.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }

    // Tworzenie Usera
    var stdEmail = "u@u.pl";
    var stdUser = await userManager.FindByEmailAsync(stdEmail);

    if (stdUser == null)
    {
        var newUser = new AppUser
        {
            UserName = stdEmail,
            Email = stdEmail,
            FirstName = "std", // Twoje dodatkowe pola
            LastName = "user",
            EmailConfirmed = true
        };

        var createUser = await userManager.CreateAsync(newUser, "Rafal-123");
        if (createUser.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "User");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();
app.Run();
