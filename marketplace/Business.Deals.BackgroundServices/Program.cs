using Business.Deals.Abstractions.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Business.Deals.BackgroundServices
{
    public class Program
    {
        private static IConfiguration configuration;
        
        public static void Main(string[] args)
        {
            configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, true)
               .Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<DealsService>();

                    services
                    .Configure<DealsRepository>(configuration).AddSingleton<DealsRepository>()
                    .Configure<DealsQueueClient>(configuration).AddSingleton<DealsQueueClient>();
                });
        
    }
}
