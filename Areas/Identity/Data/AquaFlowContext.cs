using AquaFlow.Areas.Identity.Data;
using AquaFlow.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AquaFlow.Data;

public class AquaFlowContext : IdentityDbContext<AquaFlowUser>
{
    public AquaFlowContext(DbContextOptions<AquaFlowContext> options)
        : base(options)
    {
    }

    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>().HasData(
            new Product { ProductId = 1, Name = "Product 1", Price = 10.99m, StockQuantity = 100, CreatedAt = DateTime.Now },
            new Product { ProductId = 2, Name = "Product 2", Price = 19.99m, StockQuantity = 50, CreatedAt = DateTime.Now }
        );

        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
