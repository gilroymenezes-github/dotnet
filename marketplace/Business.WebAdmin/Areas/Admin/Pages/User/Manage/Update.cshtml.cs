using Business.Shared;
using Business.Shared.Statics;
using Business.WebAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.WebAdmin.Areas.Admin.Pages.User.Manage
{
    [Authorize(Roles = ApplicationConstant.SysAdminRoleName)]
    public class UpdateModel : PageModel
    {
        [BindProperty]
        public ApplicationUser AppUser { get; set; } = new ApplicationUser();

        [BindProperty]
        public List<string> AppRolesChecked { get; set; }
        
        public IDictionary<string, bool> AppRoles { get; private set; }

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;

        public UpdateModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UpdateModel> logger
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
            var userRoles = await userManager.GetRolesAsync(AppUser);

            AppRoles = roleManager.Roles
                .ToDictionary(
                    k => k.Name, 
                    v => userRoles.Any(u => u == v.Name)
                 );

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            var user = await userManager.FindByIdAsync(AppUser.Id);
            var rolesToRemove = roleManager.Roles
                //.Where(r => r.Name != ApplicationConstant.SysAdminRoleName)
                .Where(r => !AppRolesChecked.Contains(r.Name))
                .Select(r => r.Name);

            var userRoles = await userManager.GetRolesAsync(user);
            var rolesToAdd = AppRolesChecked
                //.Where(a => a != ApplicationConstant.SysAdminRoleName)
                .Where(a => !userRoles.Contains(a));

            var tasks = new List<Task>();
            tasks.Add(userManager.RemoveFromRolesAsync(user, rolesToRemove));
            tasks.Add(userManager.AddToRolesAsync(user, rolesToAdd));
            await Task.WhenAll(tasks);

            return RedirectToPage("Detail", new { id = AppUser.Id});
        }
    }
}
