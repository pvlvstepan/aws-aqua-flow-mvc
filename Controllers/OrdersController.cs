using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;
using AquaFlow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AquaFlow.Controllers
{
    // This controller handles actions related to user orders
    public class OrdersController : Controller
    {
        private readonly AquaFlowContext _context;          // Database context for AquaFlow application
        private readonly UserManager<AquaFlowUser> _userManager;   // User manager for managing AquaFlowUser
        private readonly OrderManager _orderManager;       // Custom order manager for handling order-related operations

        // Constructor to initialize required services and managers
        public OrdersController(AquaFlowContext context, UserManager<AquaFlowUser> userManager, OrderManager orderManager)
        {
            _context = context;
            _userManager = userManager;
            _orderManager = orderManager;
        }

        // Action method to display the user's order history
        public async Task<IActionResult> Index()
        {
            // Get the current user using the UserManager
            var user = await _userManager.GetUserAsync(User);

            // Get the user's orders using the custom OrderManager
            var orders = await _orderManager.GetUserOrdersAsync(user);

            // Pass the orders to the view and render the Index view
            return View(orders);
        }
    }
}
