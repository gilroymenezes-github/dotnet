using Business.Customers.Abstractions.Clients;
using Business.Db;
using Business.ExchangeRates.Abstractions.Clients;
using Business.Projects.Abstractions.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Business.Projects.BackgroundServices
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

                    services.AddHostedService<HarvestProjectsService>();
                    services.AddHostedService<ProjectsService>();
                    
                    services
                    //.Configure<ProjectsHarvestApiClient>(configuration).AddSingleton<ProjectsHarvestApiClient>()
                    .Configure<ProjectsRepository>(configuration)
                    .Configure<ProjectsQueueClient>(configuration)
                    .Configure<CustomersHttpClient>(configuration)
                    .AddSingleton<ProjectsRepository>()
                    .AddSingleton<ProjectsQueueClient>()
                    .AddSingleton<ProjectsHarvestApiClient>()
                    .AddHttpClient<CustomersHttpClient>(client => { client.BaseAddress = functionsUrl; client.DefaultRequestHeaders.Add("api-key", "temp-api-code"); });
                });
    }
}
