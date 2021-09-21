using Business.Customers.Abstractions.Clients;
using Business.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Business.Customers.BackgroundServices
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
                    var functionsUrl = new Uri(configuration.GetSection("Azure:FunctionsUrl").Value);

                    services
                    .Configure<CustomersRepository>(configuration)
                    .AddSingleton<CustomersRepository>()
                    .AddSingleton<CustomersHarvestApiClient>();
                    
                });
    }
}
