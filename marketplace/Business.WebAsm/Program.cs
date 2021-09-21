using Business.Abstractions.Auth.Authorizations;
using Business.Deals.Abstractions.Clients;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business.WebAsm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<ApiAuthorizationMessageHandler>();
            builder.Services.AddHttpClient<DealsWebApiClient>("businessapi", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetSection("PowerUnitApi").Value);
            }).AddHttpMessageHandler<ApiAuthorizationMessageHandler>();
            builder.Services.AddHttpClient("businessapi.Unauthorized", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetSection("PowerUnitApi").Value);
            });
            builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("businessapi"));

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Oidc", options.ProviderOptions);
                options.ProviderOptions.ResponseType = "code";
                options.ProviderOptions.DefaultScopes.Add("profile");
                options.ProviderOptions.DefaultScopes.Add("email");
                options.ProviderOptions.DefaultScopes.Add("businessapi");
                options.ProviderOptions.DefaultScopes.Add("position");
                options.ProviderOptions.DefaultScopes.Add("unit");
                options.UserOptions.RoleClaim = "role";
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                //builder.Configuration.Bind("Local", options.ProviderOptions);
            }).AddAccountClaimsPrincipalFactory<MultipleRoleClaimsPrincipalFactory<RemoteUserAccount>>();

            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(UnitAuthorizationPolicies.UnitPolicy, UnitAuthorizationPolicies.UnitAuthorizationPolicy());
                options.AddPolicy(UnitAuthorizationPolicies.UnitAdministratorAccess, UnitAuthorizationPolicies.UnitAdministratorPolicy());
            });
            //builder.Services.AddSingleton<IAuthorizationPolicyProvider, UnitAuthorizationPolicyProvider>();   // work in progress
            //builder.Services.AddSingleton<IAuthorizationHandler, UnitAuthorizationHandler>();                 // work in progress

            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();

            await builder.Build().RunAsync();
        }
    }
}
