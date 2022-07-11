﻿using DATN_Back_end.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var postgressConfig = Activator.CreateInstance<PostgresConfig>();
            configuration.Bind(typeof(PostgresConfig).Name, postgressConfig);

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            optionsBuilder.UseNpgsql(postgressConfig.BuildConnectionString(), b => b.MigrationsAssembly("DATN-Back-end"));
            return new DataContext(optionsBuilder.Options);
        }
    }
}
