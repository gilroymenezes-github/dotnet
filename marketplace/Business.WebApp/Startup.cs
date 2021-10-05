using Business.Core.Financials.Connections;
using Business.Core.Orders.Connections;
using Business.Shared;
using Business.Shared.Auth;
using Business.Shared.Auth.Authorizations;
using Business.WebApp.Dashboards.GitHub;
using Business.WebApp.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radzen;

namespace Business.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticationCore();
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
            services.AddSignalR().AddAzureSignalR();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
                {

                    options.Authority = Configuration.GetSection("Oidc:Authority").Value;
                    options.ClientId = Configuration.GetSection("Oidc:ClientId").Value;
                    options.ClientSecret = "businessappsecret";
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    //options.UseTokenLifetime = false;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    //options.Scope.Add("role");
                    options.Scope.Add("company");
                    options.Scope.Add("businessapi");
                    //options.CallbackPath = "/signin-oidc";

                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("EmailPolicy", policy
                    => policy.Requirements.Add(new AuthorizationRequirementForEmail()));
            });

            services.AddSingleton<IAuthorizationHandler, ReadWritePermissionHandler<BaseModel>>();
            services.AddSingleton<IAuthorizationHandler, AuthorizationHandlerForEmail<BaseModel>>();

            // HttpContextAccessor
            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.AddScoped<AuthenticationServiceForUser>();     // webapp abstraction
            services.AddScoped<AccessTokenService>();               // webapi abstraction
                        
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();
            services.AddScoped<GitHubService>();

            var uriPowerUnit = new System.Uri(Configuration.GetSection("PowerUnitApi").Value);
            services.AddHttpClient<FinancialsHttpClientWithAuth>(client => client.BaseAddress = uriPowerUnit);
            services.AddHttpClient<OrdersHttpClientWithAuth>(client => client.BaseAddress = uriPowerUnit);

            services.AddScoped<IClaimsTransformation, RoleClaimTransformation>(); // only after AuthenticationServiceForUser
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
