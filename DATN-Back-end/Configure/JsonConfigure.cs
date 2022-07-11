using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace DATN_Back_end.Configure
{
    public static class JsonConfigure
    {
        public static void ConfigureJson(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson(ConfigJson);
        }

        private static void ConfigJson(MvcNewtonsoftJsonOptions jsonOptions)
        {
            jsonOptions.SerializerSettings.ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter());
        }
    }
}
