using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;
using AquaFlow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AquaFlow.Controllers
{
    public class CartController : Controller
    {

        private readonly AquaFlowContext _context;
        private readonly UserManager<AquaFlowUser> _userManager;
        private readonly CartManager _cartManager;

        public CartController(AquaFlowContext context, UserManager<AquaFlowUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _cartManager = new CartManager(context, userManager);
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _cartManager.GetOrCreateCartForUserAsync(user);

            return View(cart);
        }
    }
}
