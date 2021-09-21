using Business.Abstractions;
using Business.WebAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.WebAdmin.Areas.Admin.Pages.User.Manage
{
    [Authorize(Roles = ApplicationConstant.SysAdminRoleName)]
    public class DetailModel : PageModel
    {
        [BindProperty]
        public ApplicationUser AppUser {  get; set; }  = new ApplicationUser();
       
        public IList<string> AppRoles { get; set; } = new List<string>();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;

        public DetailModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<DetailModel> logger
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return Page();
            AppUser = await userManager.FindByIdAsync(id);
            AppRoles = await userManager.GetRolesAsync(AppUser);
            return Page();
        }
    }
}
