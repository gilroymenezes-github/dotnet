using Business.Db;
using Business.ExchangeRates.Abstractions.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Business.ExchangeRates.BackgroundServices
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
                    services.AddHostedService<CurrenciesService>();
                    services.AddHostedService<ExchangeRatesService>();

                    services
                    .Configure<CurrenciesRepository>(configuration).AddSingleton<CurrenciesRepository>()
                    .Configure<CurrenciesQueueClient>(configuration).AddSingleton<CurrenciesQueueClient>()
                    .Configure<ExchangeRatesRepository>(configuration).AddSingleton<ExchangeRatesRepository>()
                    .Configure<ExchangeRatesQueueClient>(configuration).AddSingleton<ExchangeRatesQueueClient>();
                });
    }
}
