using FarmasiCase.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Service
{
    public static class ServiceRegistration
    {
        public static void ImplementServiceServices(this IServiceCollection services)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<CartService>();
            services.AddScoped<OrderService>();
            services.AddScoped<ProductService>();
            services.AddScoped<UserService>();
        }
    }
}
