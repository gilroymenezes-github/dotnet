using Business.SalesOrders.Abstractions.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Business.SalesOrders.BackgroundServices
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<SalesOrdersService>();

                    services
                    .Configure<SalesOrdersRepository>(configuration).AddSingleton<SalesOrdersRepository>()
                    .Configure<SalesOrdersQueueClient>(configuration).AddSingleton<SalesOrdersQueueClient>();
                });
    }
}
