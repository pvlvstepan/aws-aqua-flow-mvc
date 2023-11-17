using AquaFlow.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AquaFlow.Controllers;

namespace AquaFlow.Areas.Identity.Pages.Cart
{
    public class CartModel : PageModel
    {

        private readonly CartController _cartController;
        private readonly UserManager<AquaFlowUser> _userManager;

     public CartModel(CartController cartController, UserManager<AquaFlowUser> userManager)
        {
            _cartController = cartController;
            _userManager = userManager;
        }
        public Models.Cart Cart { get; set; }

        public async Task OnGetAsync()
        {

            var user = await _userManager.GetUserAsync(User);

            Cart = await _cartController.GetOrCreateCartForUserAsync(user);
        }
    }
}
