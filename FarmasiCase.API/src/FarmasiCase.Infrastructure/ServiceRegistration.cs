using FarmasiCase.Infrastructure.Jwt;
using FarmasiCase.Service.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void ImplementInfrastructureServices(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            //// Dependency Injections

            // Jwt
            services.AddScoped<IJwtService, JwtService>();


            //// Services

            // Automapper
            services.AddAutoMapper(assembly);
        }
    }
}
