
using Business.Harvest.Projects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Harvest.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExternalIntegrations(
            this IServiceCollection services,
            IConfigurationSection configurationSection)
        {
            services.AddSingleton<HarvestClient>();
            services.AddSingleton<ProjectsContext>();

            return services;
        }
    }
}
