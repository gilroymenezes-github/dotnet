using Business.Competencies.Abstractions.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Business.Competencies.BackgroundServices
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
                    services.AddHostedService<CompetenciesService>();

                    services
                    .Configure<CompetenciesRepository>(configuration).AddSingleton<CompetenciesRepository>()
                    .Configure<CompetenciesQueueClient>(configuration).AddSingleton<CompetenciesQueueClient>();
                });
    }
}
