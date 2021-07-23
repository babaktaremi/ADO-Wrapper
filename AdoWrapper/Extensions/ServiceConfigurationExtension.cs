using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoWrapper.Contracts;
using AdoWrapper.Infrastructure;
using AdoWrapper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AdoWrapper.Extensions
{
   public static class ServiceConfigurationExtension
    {
        /// <summary>
        /// Adds The Required Services For Ado Wrapper
        /// </summary>
        /// <param name=""></param>
        /// <param name="connectionString">The Specified Connection String</param>
        /// <returns></returns>
        public static IServiceCollection AddAdoWrapper(this IServiceCollection services, string connectionString)
        {
            services.Configure<ConnectionStringModel>(o => o.ConnectionString = connectionString);
            services.AddScoped<IAdoProvider, AdoProvider>();
            return services;
        }
    }
}
