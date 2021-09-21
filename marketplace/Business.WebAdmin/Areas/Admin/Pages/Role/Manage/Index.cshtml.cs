using Business.Abstractions;
using Business.WebAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.WebAdmin.Areas.Admin.Pages.Role.Manage
{
    [Authorize(Roles = ApplicationConstant.SysAdminRoleName)]
    public class IndexModel : PageModel
    {
        public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;

        public IndexModel(
            RoleManager<IdentityRole> roleManager,
            ILogger<IndexModel> logger
            )
        {
            this.roleManager = roleManager;
            this.logger = logger;
        }
        public void OnGet()
        {
            Roles = roleManager.Roles.ToList();
        }

        //public async Task<IActionResult> OnPostDeleteAsync(string id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var role = roleManager.Roles.FirstOrDefault(r => r.Id == id);
        //            if (string.CompareOrdinal(role.Name, ApplicationConstant.SysAdminRoleName) == 0)
        //            {
        //                return Redirect("/Admin/Role/Manage");
        //            }
        //            if (string.CompareOrdinal(role.Name, ApplicationConstant.SelfRegUserRoleName) == 0)
        //            {
        //                return Redirect("/Admin/Role/Manage");
        //            }
        //            var result = await roleManager.DeleteAsync(role);
        //            if (!result.Succeeded)
        //            {
        //                ModelState.AddModelError("", result.Errors.First().ToString());
        //                return Redirect("/Admin/Role/Manage");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Log(LogLevel.Error, ex.Message, ex.InnerException);
        //        }
        //    }
        //    return Redirect("/Admin/Role/Manage");
        //}
    }
}
