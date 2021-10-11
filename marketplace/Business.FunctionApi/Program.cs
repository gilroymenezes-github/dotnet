using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Business.Shared.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.FunctionApi
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration(config => config
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json")
                .AddEnvironmentVariables())
                .ConfigureServices(services =>
                {
                    services.AddSingleton(sp =>
                    {
                        IConfiguration configuration = sp.GetService<IConfiguration>();
                        ILogger<CountsTableStore> logger = sp.GetService<ILogger<CountsTableStore>>();
                        return new CountsTableStore(configuration, logger);
                    });
                })
                .Build();

            host.Run();
        }
    }
}