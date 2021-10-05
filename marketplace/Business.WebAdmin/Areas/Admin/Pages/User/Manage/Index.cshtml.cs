using Business.Shared;
using Business.WebAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Business.WebAdmin.Areas.Admin.Pages.User.Manage
{
    [Authorize(Roles = ApplicationConstant.SysAdminRoleName)]
    public class IndexModel : PageModel
    {
        public List<ApplicationUser> Users { get; set;  }  = new List<ApplicationUser>();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger logger;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            ILogger<IndexModel> logger
            )
        {
            this.userManager = userManager;
            this.logger = logger;
        }
        public void OnGet()
        {
            Users = userManager.Users.ToList();
        }
    }
}
