using Microsoft.Extensions.DependencyInjection;
using Galaxy.DentalTurn.Repositories.Interfaces;
using Galaxy.DentalTurn.Repositories.Implementations;

namespace Galaxy.DentalTurn.Repositories
{
    public static class DependencyInyections
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IAdminUserRepository, AdminUserRepository>();
            // Add other repository registrations here
            return services;
        }
    }
       
}
