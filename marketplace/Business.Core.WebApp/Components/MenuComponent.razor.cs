using Business.WebApp.Shared;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business.Core.WebApp.Components
{
    public partial class MenuComponent : BaseComponent
    {
        
        protected string UserRole { get; set; }

        protected override async Task OnInitializedAsync()
        {
            UserRole = await GetUserRole();
        }
    }
}
