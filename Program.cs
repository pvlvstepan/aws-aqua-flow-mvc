using Microsoft.EntityFrameworkCore;
using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;
using Microsoft.AspNetCore.Identity;
using AquaFlow.Controllers;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AquaFlowContextConnection") ?? throw new InvalidOperationException("Connection string 'AquaFlowContextConnection' not found.");

builder.Services.AddDbContext<AquaFlowContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AquaFlowUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<AquaFlowContext>();

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<CartController>();
builder.Services.AddScoped<OrderManager>();

builder.Services.AddSingleton<IStorageService, S3StorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

var superuserEmail = builder.Configuration["AppSettings:UserEmail"];
var superuserPassword = builder.Configuration["AppSettings:UserPassword"];

CreateRolesAndDefaultUser(app.Services, builder.Configuration).Wait();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "products",
    pattern: "{controller=Products}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Orders",
    pattern: "{controller=Orders}/{action=OrdersList}/{id?}");

app.MapRazorPages();

app.Run();
async Task CreateRolesAndDefaultUser(IServiceProvider serviceProvider, IConfiguration configuration)
{
    using var scope = serviceProvider.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AquaFlowUser>>();

    string userEmail = configuration["AppSettings:UserEmail"];
    string userPassword = configuration["AppSettings:UserPassword"];

    string[] roleNames = { "Admin", "User" };
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);

        if (!roleExist)
        {
            // Create the roles and seed them to the database
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create a superuser who will maintain the app
    var superuser = new AquaFlowUser
    {
        UserName = userEmail,
        Email = userEmail,
    };

    var user = await userManager.FindByEmailAsync(userEmail);

    if (user == null)
    {
        var createSuperUser = await userManager.CreateAsync(superuser, userPassword);
        if (createSuperUser.Succeeded)
        {
            // Assign the new user the "Admin" role
            await userManager.AddToRoleAsync(superuser, "Admin");
            Console.WriteLine("Superuser created successfully.");
        }
        else
        {
            Console.WriteLine("Error creating superuser:");
            foreach (var error in createSuperUser.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }
    else
    {
        Console.WriteLine("Superuser already exists.");
    }
}