using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AquaFlow.Data;
using AquaFlow.Models;
using Microsoft.Identity.Client.Extensions.Msal;
using AquaFlow.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AquaFlow.Areas.Admin.Pages.Manage.Products
{
    public class CreateModel : PageModel
    {
        private readonly AquaFlow.Data.AquaFlowContext _context;
        private readonly IStorageService _storage;

        public CreateModel(AquaFlow.Data.AquaFlowContext context, IStorageService storage)
        {
            _context = context;
            _storage = storage;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        [BindProperty]
        public IFormFile ImageFile { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid || _context.Products == null || Product == null)
            {
                return Page();
            }

            if (ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is required.");
                return Page();
            }

            // Upload image and get the file path
            var filePath = await _storage.AddItem(ImageFile, ImageFile.FileName);

            if (filePath == null)
            {
                ModelState.AddModelError("ImageFile", "Failed to upload the image.");
                return Page();
            }

            // Set the image path in the Product entity
            Product.ImagePath = filePath;

            Product.CreatedAt = DateTime.Now;

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
