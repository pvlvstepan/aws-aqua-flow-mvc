using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;
using AquaFlow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AquaFlow.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AquaFlowContext _context;
        private readonly CartController _cartController;
        private readonly UserManager<AquaFlowUser> _userManager;

        public HomeController(ILogger<HomeController> logger, AquaFlowContext context, CartController cartController, UserManager<AquaFlowUser> userManager)
        {
            _logger = logger;
            _context = context;
            _cartController = cartController;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        // Action to handle adding items to the cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {

            var user = await _userManager.GetUserAsync(User);

            // Call the AddToCart function from CartController
            await _cartController.AddToCart(productId, 1, user);

            return LocalRedirect("/Identity/Cart");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Community()
        {
            return View();
        }

        public IActionResult Partners()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}