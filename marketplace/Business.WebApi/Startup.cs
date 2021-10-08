using Business.Core.Customers.Connections;
using Business.Core.Financials.Connections;
using Business.Core.Orders.Connections;
using Business.Shared;
using Business.Shared.Abstractions;
using Business.Shared.Connections;
using Business.Shared.Authorizations;
using Business.Shared.Repositories;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Business.Core.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
               .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
               {
                   options.Authority = Configuration.GetSection("Oidc:Authority").Value;
                   options.RequireHttpsMetadata = false;
                   options.Audience = "businessapi";
                   options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                   {
                       ValidateAudience = false
                   };
               });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("EmailPolicy", policy
                    => policy.Requirements.Add(new AuthorizationRequirementForEmail()));
            });
            services.AddSingleton<IAuthorizationHandler, ReadWritePermissionHandler<BaseModel>>();
            services.AddSingleton<IAuthorizationHandler, AuthorizationHandlerForEmail<BaseModel>>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllers();

            services.AddSignalR();
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Business.Api", Version = "v1" });
            });

            var functionsUrl = new System.Uri(Configuration.GetSection("Azure:FunctionsUrl").Value);
            
            services.AddHttpClient<FinancialsHttpClient>(client => { client.BaseAddress = functionsUrl; client.DefaultRequestHeaders.Add("api-key", "temp-api-code"); });
            services.AddHttpClient<OrdersHttpClient>(client => { client.BaseAddress = functionsUrl; client.DefaultRequestHeaders.Add("api-key", "temp-api-code"); });
            services.AddHttpClient<CustomersHttpClient>(client => { client.BaseAddress = functionsUrl; client.DefaultRequestHeaders.Add("api-key", "temp-api-code"); });
            
            services.Configure<FinancialsQueueClient>(Configuration).AddSingleton<FinancialsQueueClient>();
            services.Configure<OrdersQueueClient>(Configuration).AddSingleton<OrdersQueueClient>();

            services.AddTransient<IBlobStorage, AzureBlobStorage>();

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Business.Api v1"));
            }
            // temporarily in prod swagger and swaggerui
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Business.Api v1"));

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationsHub>("/api/notifications");
            });
        }      
    }
}
