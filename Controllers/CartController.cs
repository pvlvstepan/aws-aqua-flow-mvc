using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;
using AquaFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AquaFlow.Controllers
{
    public class CartController : Controller
    {

        private readonly AquaFlowContext _context;
        private readonly UserManager<AquaFlowUser> _userManager;

        public CartController(AquaFlowContext context, UserManager<AquaFlowUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "User")]
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

        public async Task AddToCart(int productId, int quantity, AquaFlowUser user)
        {
            var product = await _context.Products.FindAsync(productId);

            var cart = await GetOrCreateCartForUserAsync(user);

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Product.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    Cart = cart,
                    Product = product,
                    Quantity = quantity
                };

                cart.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public decimal CalculateTotalWithTaxAndShipping(Cart cart)
        {
            // Calculate the total without tax
            decimal totalWithoutTax = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            // Calculate tax (6%)
            decimal tax = totalWithoutTax * 0.06M;

            // Add shipping cost (assuming a fixed shipping cost, replace with your logic)
            decimal shippingCost = 10.0M; // Replace with your shipping cost logic

            // Calculate the total including tax and shipping
            decimal totalWithTaxAndShipping = totalWithoutTax + tax + shippingCost;

            return totalWithTaxAndShipping;
        }

        public async Task ClearCartAsync(AquaFlowUser user)
        {
            var cart = await GetOrCreateCartForUserAsync(user);

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
        }
    }
}
