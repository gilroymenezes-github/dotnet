using IdentityManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace IdentityManager.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<MongoIdentityUser> _userManager;
        private readonly SignInManager<MongoIdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public IndexModel(UserManager<MongoIdentityUser> userManager, SignInManager<MongoIdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
    }
}
