using AquaFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;

public class CartManager
{
    private readonly AquaFlowContext _context;
    private readonly UserManager<AquaFlowUser> _userManager;

    public CartManager(AquaFlowContext context, UserManager<AquaFlowUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Cart> GetOrCreateCartForUserAsync(AquaFlowUser user)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.User.Id == user.Id);

        if (cart == null)
        {
            cart = new Cart
            {
                User = user,
                CreatedAt = DateTime.Now,
                CartItems = new List<CartItem>()
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    public async Task<int> GetCartItemCountAsync(AquaFlowUser user)
    {
        var cart = await GetOrCreateCartForUserAsync(user);
        return cart.CartItems?.Sum(ci => ci.Quantity) ?? 0;
    }

    public async Task AddTestCartItemsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            var cart = await GetOrCreateCartForUserAsync(user);

            // Add test products to the cart
            var product1 = new Product { Name = "Test Product 1", Price = 19.99m, StockQuantity = 10, CreatedAt = DateTime.Now };
            var product2 = new Product { Name = "Test Product 2", Price = 29.99m, StockQuantity = 15, CreatedAt = DateTime.Now };

            var cartItem1 = new CartItem { Cart = cart, Product = product1, Quantity = 2 };
            var cartItem2 = new CartItem { Cart = cart, Product = product2, Quantity = 1 };

            cart.CartItems.Add(cartItem1);
            cart.CartItems.Add(cartItem2);

            await _context.SaveChangesAsync();
        }
    }

    // Add more methods for cart operations (e.g., AddToCart, UpdateQuantity, CalculateTotal, etc.)
}
