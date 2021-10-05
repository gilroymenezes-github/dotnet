using Business.Shared;
using Business.WebAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Business.WebAdmin.Areas.Admin.Pages.Role.Manage
{
    [Authorize(Roles = ApplicationConstant.SysAdminRoleName)]
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public IdentityRole AppRole { get; set; } = new IdentityRole();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;

        public DeleteModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<DeleteModel> logger
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return Page();
            AppRole = await roleManager.FindByIdAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromRoute] string id)
        {
            var role = roleManager.Roles?.FirstOrDefault(r => r.Id == id);
            if (string.CompareOrdinal(role.Name, ApplicationConstant.SysAdminRoleName) == 0) return RedirectToPage("Index");
            if (string.CompareOrdinal(role.Name, ApplicationConstant.SelfRegUserRoleName) == 0) return RedirectToPage("Index");
            if (role is null) return RedirectToPage("Index");
            role.Name = AppRole.Name;
            var result = await roleManager.DeleteAsync(role);

            return RedirectToPage("Index");
        }
    }
}
