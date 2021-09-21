using Business.WebAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using Business.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Business.WebAdmin.Areas.Admin.Pages.Role.Manage
{
    [Authorize(Roles = ApplicationConstant.SysAdminRoleName)]
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public IdentityRole AppRole { get; set; } = new IdentityRole();

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;

        public UpdateModel(
            RoleManager<IdentityRole> roleManager,
            ILogger<UpdateModel> logger
            )
        {
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
            var result = await roleManager.UpdateAsync(role);
            
            return RedirectToPage("Index");
        }
    }
}
