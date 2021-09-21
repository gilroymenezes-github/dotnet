using Business.WebApp.Abstractions;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business.WebApp.Shared
{
    public partial class LeftMenu : BaseComponent
    {
        
        protected string UserRole { get; set; }

        protected override async Task OnInitializedAsync()
        {
            UserRole = await GetUserRole();
        }
    }
}
