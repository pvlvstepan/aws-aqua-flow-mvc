using Amazon.ApiGatewayManagementApi.Model;
using Amazon.ApiGatewayManagementApi;
using Amazon.Runtime;
using AquaFlow.Areas.Identity.Data;
using AquaFlow.Data;
using AquaFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace AquaFlow.Controllers
{
    public class CartController : Controller
    {

        private readonly AquaFlowContext _context;
        private readonly UserManager<AquaFlowUser> _userManager;
        private readonly string _apiGatewayEndpoint; //
        private readonly SQSManager _sqsManager;
        private readonly SNSManager snsManager;


        public CartController(AquaFlowContext context, UserManager<AquaFlowUser> userManager, string apiGatewayEndpoint)
        {
            _context = context;
            _userManager = userManager;
            _apiGatewayEndpoint = apiGatewayEndpoint;
            _sqsManager = sqsManager;
            snsManager = new SNSManager();
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

        private async Task<int> GetConnectionIdAsync(AquaFlowUser user)
        {
        //
            var cart = await GetOrCreateCartForUserAsync(user);
            return cart.CartItems?.Sum(ci => ci.Quantity) ?? 0;
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

            // After saving to the local database, notify the AWS Lambda function through API Gateway
            await NotifyLambdaFunctionAsync(user, productId, quantity);
        }

        private Task NotifyLambdaFunctionAsync(AquaFlowUser user, int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        private async Task NotifyLambdaFunctionAsync(int userId, int productId, int quantity)
        {
            var apiGatewayClient = new AmazonApiGatewayManagementApiClient(new AnonymousAWSCredentials(), new AmazonApiGatewayManagementApiConfig
            {
                ServiceURL = _apiGatewayEndpoint
            });

            var connectionId = await GetConnectionIdAsync(userId);

            if (connectionId != null)
            {
                var message = new
                {
                    ProductId = productId,
                    Quantity = quantity
                };

                // Convert the message to a byte array
                var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                // Create a MemoryStream from the byte array
                using (var memoryStream = new System.IO.MemoryStream(messageBytes))
                {
                    var postToConnectionRequest = new PostToConnectionRequest
                    {
                        ConnectionId = connectionId,
                        Data = memoryStream
                    };

                    await apiGatewayClient.PostToConnectionAsync(postToConnectionRequest);
                }
            }
        }

        private Task<string> GetConnectionIdAsync(object user)
        {
            throw new NotImplementedException();
            // Send a message to SQS when an item is added to the cart
            _sqsManager.SendMessageToQueue($"Item added to cart: {product.ProductName}");

            // Publish a notification to SNS when an item is added to the cart
            snsManager.PublishMessage($"Item added to cart: ProductId={productId}, Quantity={quantity}, User={user.UserName}");
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

            // Send a message to SQS when the cart is cleared
            _sqsManager.SendMessageToQueue("Cart cleared");
        }
    }
}
