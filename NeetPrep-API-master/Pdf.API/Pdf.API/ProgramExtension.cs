using Pdf.DataAccess;
using Pdf.Service.BusinessLogic;
using Pdf.Service.Interface;

namespace Pdf.API
{
    public static class ProgramExtension
    {
        public static IServiceCollection CustonServicesExtension(this IServiceCollection service, ConfigurationManager configuration)
        {
            try
            {
                service.AddScoped<IAuth, AuthService>();
                service.AddScoped<IDashboard, DashboardService>();
                service.AddScoped(opt => new AuthDA(configuration.GetSection("Connections:ConnectionString").Value));
                service.AddScoped(opt => new DashboardDA(configuration.GetSection("Connections:ConnectionString").Value));
                return service;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
