using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AquaFlow.Data;
using AquaFlow.Models;

namespace AquaFlow.Areas.Admin.Pages.Manage.Orders
{
    public class IndexModel : PageModel
    {
        private readonly AquaFlow.Data.AquaFlowContext _context;

        public IndexModel(AquaFlow.Data.AquaFlowContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                Order = await _context.Orders.ToListAsync();
            }
        }
    }
}
