using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityManager.Models;
using IdentityManager.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace IdentityManager.Areas.Admin.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<MongoIdentityUser> _userManager;
        private readonly SignInManager<MongoIdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginModel(UserManager<MongoIdentityUser> userManager, SignInManager<MongoIdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Email, Password, false, false);
                if (result.Succeeded)
                {
                    var user = _userManager.Users.SingleOrDefault(r => r.Email == Email);
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties() { });
                    return RedirectToPage("/Index");
                }
            }
            Message = "Invalid Attempt";
            return Page();
        }
    }
}
