using DATN_Back_end.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end
{
    public static class DatabaseConfigure
    {
        public static void ConfigDatabase(this IServiceCollection services)
        {
            services
                .AddDbContext<DataContext>(options => PostgresDatabaseConnection.ConfigPosgressDb(services, options));
        }

        public static void MigrateDatabase(this IServiceCollection services)
        {
            services.BuildServiceProvider()
                .GetService<DataContext>()
                .Database
                .Migrate();
        }
    }
}
