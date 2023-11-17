using Microsoft.EntityFrameworkCore;
using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AquaFlowContextConnection") ?? throw new InvalidOperationException("Connection string 'AquaFlowContextConnection' not found.");

builder.Services.AddDbContext<AquaFlowContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AquaFlowUser>(options => options.SignIn.RequireConfirmedEmail = false).AddEntityFrameworkStores<AquaFlowContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<CartManager>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
        name: "cart",
        pattern: "{controller=Cart}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "orders",
    pattern: "{controller=Orders}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
