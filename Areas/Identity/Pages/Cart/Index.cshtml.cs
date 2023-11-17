using AquaFlow.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AquaFlow.Controllers;
using Microsoft.AspNetCore.Mvc;
using AquaFlow.Data;
using AquaFlow.Models;

namespace AquaFlow.Areas.Identity.Pages.Cart
{
    public class CartModel : PageModel
    {

        private readonly CartController _cartController;
        private readonly AquaFlowContext _context;
        private readonly UserManager<AquaFlowUser> _userManager;

        public string SuccessMessage { get; set; }

        public CartModel(AquaFlowContext context, CartController cartController, UserManager<AquaFlowUser> userManager)
        {
            _cartController = cartController;
            _userManager = userManager;
            _context = context;
        }
        public Models.Cart Cart { get; set; }

        public async Task OnGetAsync()
        {

            var user = await _userManager.GetUserAsync(User);

            Cart = await _cartController.GetOrCreateCartForUserAsync(user);
        }

        public async Task OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            // Ensure the cart is loaded for the user
            Cart = await _cartController.GetOrCreateCartForUserAsync(user);

            // Create a new order
            var order = new Order
            {
                User = user,
                Address = $"{user.Street}, {user.City}, {user.State} {user.ZipCode}", // Replace with the actual user's address
                OrderDate = DateTime.Now,
                TotalAmount = _cartController.CalculateTotalWithTaxAndShipping(Cart), // Assuming you have a method to calculate the total amount in your Cart class
                Status = "Pending", // Set the initial status as per your requirements
                OrderItems = Cart.CartItems.Select(item => new OrderItem
                {
                    Quantity = item.Quantity,
                    Product = item.Product // Assuming you have a Product property in your CartItem class
                }).ToList()
            };

            // Save the order to the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear the user's cart after placing the order
            await _cartController.ClearCartAsync(user);

            SuccessMessage = "Order placed successfully!";
        }
    }
}
