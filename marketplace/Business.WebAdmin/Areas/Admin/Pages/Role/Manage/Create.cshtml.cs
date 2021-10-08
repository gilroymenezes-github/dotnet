using Business.Shared;
using Business.Shared.Statics;
using Business.WebAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Business.WebAdmin.Areas.Admin.Pages.Role.Manage
{
    [Authorize(Roles = ApplicationConstant.SysAdminRoleName)]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public IdentityRole AppRole {  get; set; } = new IdentityRole();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;
        public CreateModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<CreateModel> logger
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public IActionResult OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.CompareOrdinal(AppRole.Name, ApplicationConstant.SysAdminRoleName) == 0)
            {
                return Redirect("Index");
            }
            await roleManager.CreateAsync(AppRole);
            return Redirect("Index");
        }
    }
}
