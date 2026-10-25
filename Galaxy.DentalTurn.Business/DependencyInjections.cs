using Galaxy.DentalTurn.Business.Implementations;
using Galaxy.DentalTurn.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Galaxy.DentalTurn.Business
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IPractitionerService, PractitionerService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBusinessHoursService, BusinessHoursService>();
            return services;
        }   
    }
}
