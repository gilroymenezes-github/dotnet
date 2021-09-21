using Business.Db;
using Business.Users.Abstractions.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Business.Users.BackgroundServices
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

                    services.AddHostedService<HarvestUsersService>();

                    services
                    .Configure<UsersRepository>(configuration)
                    .AddSingleton<UsersRepository>()
                    .AddSingleton<UsersHarvestApiClient>();
                });
    }
}
