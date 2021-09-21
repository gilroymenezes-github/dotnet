using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Business.WebApp.Pages
{
    public class LoginModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(string redirectUri)
        {
            await Task.CompletedTask;

            if (string.IsNullOrEmpty(redirectUri)) redirectUri = Url.Content("~/");

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect(redirectUri);
            }

            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = redirectUri
                }, "oidc");
        }
    }
}
