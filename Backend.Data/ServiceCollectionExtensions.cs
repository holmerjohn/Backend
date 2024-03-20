﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBackendDataServices(this IServiceCollection services, BackendConfiguration configuration)
        {

            services.AddDbContext<BackendDbContext>(options => 
            {
                options.UseSqlite(configuration.ConnectionString)
                    .EnableSensitiveDataLogging(configuration.EnableSensitiveSqlLogging);
            });

            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));

            return services;
        }
    }
}