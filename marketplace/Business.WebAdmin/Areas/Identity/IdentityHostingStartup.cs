using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Business.WebAdmin.Areas.Identity.IdentityHostingStartup))]
namespace Business.WebAdmin.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}