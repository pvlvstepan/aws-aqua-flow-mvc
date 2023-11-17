using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;
using AquaFlow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AquaFlow.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AquaFlowContext _context;
        private readonly UserManager<AquaFlowUser> _userManager;
        private readonly OrderManager _orderManager;

        public OrdersController(AquaFlowContext context, UserManager<AquaFlowUser> userManager, OrderManager orderManager)
        {
            _context = context;
            _userManager = userManager;
            _orderManager = orderManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            // Use your OrderManager to get the user's orders
            var orders = await _orderManager.GetUserOrdersAsync(user);

            return View(orders);
        }
    }
}


