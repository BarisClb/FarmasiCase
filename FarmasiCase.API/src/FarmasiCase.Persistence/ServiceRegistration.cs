using FarmasiCase.Persistence;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Persistence
{
    public static class ServiceRegistration
    {
        public static void ImplementPersistenceServices(this IServiceCollection services, string connectionString)
        {
        }
    }
}
