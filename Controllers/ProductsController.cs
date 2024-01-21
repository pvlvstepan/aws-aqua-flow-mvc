using AquaFlow.Areas.Identity.Data;
using AquaFlow.Controllers;
using AquaFlow.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class ProductsController : Controller
{
    private readonly AquaFlowContext _context;
    private readonly CartController _cartController;
    private readonly UserManager<AquaFlowUser> _userManager;

    public ProductsController(AquaFlowContext context, CartController cartController, UserManager<AquaFlowUser> userManager)
    {
        _context = context;
        _cartController = cartController;
        _userManager = userManager;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        return _context.Products != null
            ? View(await _context.Products.ToListAsync())
            : Problem("Entity set 'AquaFlowContext.Products' is null.");
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
}
