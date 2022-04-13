using Microsoft.Extensions.DependencyInjection;
using TestLocalization.BLL.Service;

namespace TestLocalization.Worker
{
    public static class Registrations
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ICountriesService, CountriesService>();
            return services;
        }
    }
}
