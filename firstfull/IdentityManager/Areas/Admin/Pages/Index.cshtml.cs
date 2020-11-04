using IdentityManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace IdentityManager.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentitiesUser> _userManager;
        private readonly SignInManager<IdentitiesUser> _signInManager;
        private readonly IConfiguration _configuration;

        public IndexModel(UserManager<IdentitiesUser> userManager, SignInManager<IdentitiesUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
    }
}
