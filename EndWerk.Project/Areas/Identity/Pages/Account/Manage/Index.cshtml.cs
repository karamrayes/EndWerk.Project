// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Order.Object;

namespace Order.Project.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
        public int PostCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public DateTime DateBirth { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Postcode")]
            public int Postcode { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "City")]
            public string City { get; set; }
            [DataType(DataType.Text)]
            [Display(Name = "Country")]
            public string Country { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Name")]
            [Required(ErrorMessage = "Please fill in a name")]
            public string Name { get; set; }

            [DataType(DataType.DateTime)]
            [Display(Name = "DateBirth")]
            public DateTime? DateBirth { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
                       
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            //PhoneNumber = phoneNumber;
            //Address = user.Address;
            //City = user.City;
            //Country = user.Country;
            //Name= user.Name;
            //DateBirth = (DateTime)user.DateBirth;
            //PostCode = user.Postcode;


            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Address = user.Address,
                City = user.City,
                Country = user.Country,
                Name = user.Name,
                DateBirth = (DateTime)user.DateBirth,
                Postcode = user.Postcode


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

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }
            if (Input.Address != user.Address)
            {
                user.Address = Input.Address;
            }
            if (Input.Postcode != user.Postcode)
            {
                user.Postcode = Input.Postcode;
            }
            if (Input.Country != user.Country)
            {
                user.Country = Input.Country;
            }
            if (Input.City != user.City)
            {
                user.City = Input.City;
            }
            if (Input.DateBirth != user.DateBirth)
            {
                user.DateBirth = Input.DateBirth;
            }
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
