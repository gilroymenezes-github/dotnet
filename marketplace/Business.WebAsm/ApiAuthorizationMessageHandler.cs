using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.WebAsm
{
    public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        protected IConfiguration configuration;
        public ApiAuthorizationMessageHandler(IAccessTokenProvider provider, IConfiguration configuration, NavigationManager navigation)
            : base(provider, navigation)
        {
            this.configuration = configuration;
            ConfigureHandler(
                authorizedUrls: new[] { configuration.GetSection("PowerUnitApi").Value },
                scopes: new[] { "businessapi" });
        }
    }
}
