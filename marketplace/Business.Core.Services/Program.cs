using Business.Core.Customers.Repositories;
using Business.Core.Financials.Connections;
using Business.Core.Financials.Repositories;
using Business.Core.Orders.Connections;
using Business.Core.Orders.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Business.Core.BackgroundServices
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
                    services.AddHostedService<FinancialsService>();
                    services.AddHostedService<OrdersService>();

                    services
                    .Configure<CustomersRepository>(configuration).AddSingleton<CustomersRepository>()
                    .Configure<FinancialsRepository>(configuration).AddSingleton<FinancialsRepository>()
                    .Configure<FinancialsQueueClient>(configuration).AddSingleton<FinancialsQueueClient>()
                    .Configure<OrdersRepository>(configuration).AddSingleton<OrdersRepository>()
                    .Configure<OrdersQueueClient>(configuration).AddSingleton<OrdersQueueClient>();
                });
    }
}
