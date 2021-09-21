using Business.Abstractions;
using Business.WebAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.WebAdmin.Areas.Admin.Services
{
    public class AdminInitializer
    {
        public bool IsSysAdminReady { get; private set; }

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger logger;
        public AdminInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            ILogger<AdminInitializer> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }

       
        internal async Task CreateSysAdminUserRole() 
        {
            var success = default(bool);
            logger.LogInformation("System roles and administrator initializing");

            //var result = await roleManager.CreateAsync(SeedConfig.Roles.First());
            var tasks = new List<Task>();
            SeedConfig.Roles.ToList().ForEach(r => tasks.Add(roleManager.CreateAsync(r)));
            await Task.WhenAll(tasks);
            if (roleManager.Roles.Count() == 2) success = true;
            if (!success) logger.LogError("Could not create system roles, perhaps already exists.");
            else logger.LogInformation("Created system roles.");
            
            var result = await userManager.CreateAsync(SeedConfig.Users.First(), ApplicationConstant.TempPassword);
            success = success && result.Succeeded;
            if (!success) logger.LogError("Could not create system user, perhaps already exists.");
            else logger.LogInformation("Created system administrator.");
            
            var user = await userManager.FindByNameAsync(SeedConfig.Users.First().UserName);
            if (user == null) success = default(bool);
            if (!success) logger.LogError("Could not find administrator in system");
            
            result = await userManager.AddToRoleAsync(user, SeedConfig.Roles.First().Name);
            success = success && result.Succeeded;
            if (!success) logger.LogError("Could not assign sysadmin role to sysadmin user");

            logger.LogInformation("System roles and administrator initialized");
            IsSysAdminReady = success;
        }

        //internal void PopulateRoles()
        //{
        //    logger.LogInformation("Populating roles");

        //    SeedConfig.Roles.ToList().ForEach(async r =>
        //    {
        //        var role = await roleManager.FindByNameAsync(r.Name);
        //        if (role is null) await roleManager.CreateAsync(r);
        //    });

        //    logger.LogInformation("Populated roles");
        //}

        //internal void PopulateUsers()
        //{
        //    logger.LogInformation("Populating users");
            
        //    SeedConfig.Users.ToList().ForEach(async u =>
        //    {
        //        var user = await userManager.FindByEmailAsync(u.Email);
        //        if (user is null) await userManager.CreateAsync(u, SeedConfig.TempPassword);
        //    });

        //    logger.LogInformation("Populated users");
        //}

        //public void PopulateUsersToRoles()
        //{
        //    logger.LogInformation("Populating users to roles");
        //    var resultSucceeded = true;
        //    SeedConfig.Users.ToList().ForEach(async u =>
        //    {
        //        var user = await userManager.FindByEmailAsync(u.Email);
        //        if (user is null) return;
        //        var emailPrefix = u.Email.Split("@")[0];
        //        if (emailPrefix is null) return;
        //        var userRole = SeedConfig.Roles.FirstOrDefault(r => r.Name.ToLowerInvariant() == emailPrefix);
        //        var result = await userManager.AddToRoleAsync(user, userRole.Name);
        //        resultSucceeded = resultSucceeded && result.Succeeded;
        //    });
        //    if (resultSucceeded) logger.LogInformation("Populated users to roles");
        //    else logger.LogError("Error populating users to roles");
        //    return;
        //}
    }
}
