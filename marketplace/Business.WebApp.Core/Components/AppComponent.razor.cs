using Business.WebApp.Shared;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business.WebApp.Core.Components
{
    public partial class AppComponent : BaseComponent
    {
        protected string UserRole { get; set; }

        protected override async Task OnInitializedAsync()
        {
            UserRole = await GetUserRole();
        }
    }
}
