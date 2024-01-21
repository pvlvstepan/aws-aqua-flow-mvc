using AquaFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;

public class OrderManager
{
    private readonly AquaFlowContext _context;                // Database context for AquaFlow application
    private readonly UserManager<AquaFlowUser> _userManager;  // User manager for managing AquaFlowUser

    // Constructor to initialize required services and managers
    public OrderManager(AquaFlowContext context, UserManager<AquaFlowUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Method to place an order for a user with the specified shipping address
    public async Task PlaceOrderAsync(string userId, string shippingAddress)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            var (cart, _) = await GetOrCreateCartAndOrdersForUserAsync(user);

            if (cart != null && cart.CartItems.Any())
            {
                var order = CreateOrderFromCart(user, shippingAddress, cart);

                _context.Orders.Add(order);
                _context.Carts.Remove(cart);

                await _context.SaveChangesAsync();
            }
        }
    }

    // Method to retrieve a list of orders for a given user
    public async Task<List<Order>> GetUserOrdersAsync(AquaFlowUser user)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.User.Id == user.Id)
            .ToListAsync();
    }

    // Private method to get or create a cart and orders for a user
    private async Task<(Cart cart, List<Order> orders)> GetOrCreateCartAndOrdersForUserAsync(AquaFlowUser user)
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

        var orders = await GetUserOrdersAsync(user);

        return (cart, orders);
    }

    // Private method to create an order from a user's cart
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
}
