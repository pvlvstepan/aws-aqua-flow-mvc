using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AquaFlow.Data;
using AquaFlow.Models;
using Microsoft.AspNetCore.Identity;
using AquaFlow.Areas.Identity.Data;

namespace AquaFlow.Views.Orders
{
    public class OrdersListModel : PageModel
    {
        private readonly AquaFlowContext _context;
        private readonly UserManager<AquaFlowUser> _userManager;

        public OrdersListModel(AquaFlowContext context, UserManager<AquaFlowUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Order> Order { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                Order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.User.Id == user.Id)
                    .ToListAsync();
            }
        }
    }
}


