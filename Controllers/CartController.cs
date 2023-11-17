using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;
using AquaFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AquaFlow.Controllers
{
    public class CartController : Controller
    {

        private readonly AquaFlowContext _context;

        public CartController(AquaFlowContext context)
        {
            _context = context;
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
    }
}
