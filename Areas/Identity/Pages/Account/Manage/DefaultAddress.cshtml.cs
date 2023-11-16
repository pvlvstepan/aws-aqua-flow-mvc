using AquaFlow.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection.Emit;

namespace AquaFlow.Areas.Identity.Pages.Account.Manage
{
    public class DefaultAddressModel : PageModel
    {
        private readonly UserManager<AquaFlowUser> _userManager;

        public DefaultAddressModel(UserManager<AquaFlowUser> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Street")]
            public string Street { get; set; }

            [Required]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required]
            [Display(Name = "State")]
            public string State { get; set; }

            [Required]
            [Display(Name = "Zip Code")]
            public string ZipCode { get; set; }
        }

        private async Task LoadAsync(AquaFlowUser user)
        {
            var street = user.Street;
            var city = user.City;
            var state = user.State;
            var zipCode = user.ZipCode;

            Input = new InputModel
            {
                Street = street,
                City = city,
                State = state,
                ZipCode = zipCode
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.Street = Input.Street;
            user.City = Input.City;
            user.State = Input.State;
            user.ZipCode = Input.ZipCode;

            // Save changes to the database
            var result = _userManager.UpdateAsync(user).Result;

            if (!result.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }

            StatusMessage = "Your default address has been updated";
            return RedirectToPage();
        }

    }
}
