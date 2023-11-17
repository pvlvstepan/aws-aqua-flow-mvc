using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AquaFlow.Data;
using AquaFlow.Models;

namespace AquaFlow.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AquaFlowContext _context;

        public ProductsController(AquaFlowContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
              return _context.Products != null ? 
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'AquaFlowContext.Products'  is null.");
        }
    }
}
