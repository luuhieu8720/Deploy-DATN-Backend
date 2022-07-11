﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DATN_Back_end.Extensions
{
    public static class ServiceCollectionExts
    {
        public static T CreateConfig<T>(this IConfiguration configuration)
        {
            var configObject = Activator.CreateInstance<T>();
            configuration.Bind(typeof(T).Name, configObject);

            return configObject;
        }

        public static T ConfigType<T>(this IServiceCollection services, IConfiguration configuration)
        {
            var configObject = configuration.CreateConfig<T>();
            services.AddSingleton(typeof(T), configObject);
            return configObject;
        }
    }
}
