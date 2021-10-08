using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Business.WebApp.Shared;

namespace Business.WebApp.Shell.Shared
{
    public partial class MainLayoutComponent
    {
        [Inject] EnvironmentService AppEnvironment { get; set; }
        [Inject] IWebHostEnvironment Environment {  get; set; }

        protected override void OnInitialized()
        {
            AppEnvironment.ContentRootPath = Environment.ContentRootPath;
            AppEnvironment.EnvironmentName = Environment.EnvironmentName;
        }
    }
}
