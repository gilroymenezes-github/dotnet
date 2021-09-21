// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Business.WebAdmin.Areas.Admin.Services;
using Business.WebAdmin.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IdentityServerHost.Quickstart.UI
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly AdminInitializer adminInitializer;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;

        public HomeController(
            AdminInitializer adminInitializer,
            IIdentityServerInteractionService interaction, 
            IWebHostEnvironment environment, 
            ILogger<HomeController> logger)
        {
            this.adminInitializer = adminInitializer;
            _interaction = interaction;
            _environment = environment;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (!adminInitializer.IsSysAdminReady)
            {
                await adminInitializer.CreateSysAdminUserRole();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Internal()
        {
            if (_environment.IsDevelopment())
            {
                // only show in development
                return View();
            }

            _logger.LogInformation("Homepage is disabled in production. Returning 404.");
            return NotFound();
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        /// 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
                return View("Error", vm);
            }

            // fallback to asp.net identity error tracing
            var avm = new ApplicationErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View("Error", avm);
 
        }
    }
}