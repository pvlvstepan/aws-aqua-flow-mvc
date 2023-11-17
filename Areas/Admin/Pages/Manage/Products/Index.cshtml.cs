using System.Collections.Generic;
using System.Threading.Tasks;
using AquaFlow.Data;
using AquaFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AquaFlow.Areas.Admin.Pages.Manage.Products
{
    public class IndexModel : PageModel
    {
        private readonly AquaFlowContext _context;

        public IndexModel(AquaFlowContext context)
        {
            _context = context;
        }

        public List<Product> ProductsList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve the list of products from the context asynchronously
            ProductsList = await _context.Products.ToListAsync();

            return Page();
        }
    }
}
