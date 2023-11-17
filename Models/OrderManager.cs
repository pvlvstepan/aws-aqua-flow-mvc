using AquaFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;

public class OrderManager
{
    private readonly AquaFlowContext _context;
    private readonly UserManager<AquaFlowUser> _userManager;

    public OrderManager(AquaFlowContext context, UserManager<AquaFlowUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task PlaceOrderAsync(string userId, string shippingAddress)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            var cart = await GetOrCreateCartForUserAsync(user);

            if (cart != null && cart.CartItems.Any())
            {
                var order = CreateOrderFromCart(user, shippingAddress, cart);

                _context.Orders.Add(order);
                _context.Carts.Remove(cart);

                await _context.SaveChangesAsync();
            }
        }
    }
    private async Task<Cart> GetOrCreateCartForUserAsync(AquaFlowUser user)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
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

    private Order CreateOrderFromCart(AquaFlowUser user, string shippingAddress, Cart cart)
    {
        return new Order
        {
            User = user,
            Address = shippingAddress,
            OrderDate = DateTime.Now,
            TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity),
            OrderItems = cart.CartItems.Select(ci => new OrderItem
            {
                Quantity = ci.Quantity
            }).ToList()
        };
    }

    internal Task<string?> GetUserOrdersAsync(AquaFlowUser? user)
    {
        throw new NotImplementedException();
    }
}
