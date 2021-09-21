using Business.Db;
using Business.ExchangeRates.Abstractions.Clients;
using Business.Projections.Abstractions.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Business.Projections.BackgroundServices
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
                    var functionsUrl = new System.Uri(configuration.GetSection("Azure:FunctionsUrl").Value);

                    services.AddHostedService<ProjectionsService>();
                    services.AddLogging();

                    services
                    .Configure<ProjectionsRepository>(configuration).AddSingleton<ProjectionsRepository>()
                    .Configure<ProjectionsQueueClient>(configuration).AddSingleton<ProjectionsQueueClient>();
                });
    }
}
