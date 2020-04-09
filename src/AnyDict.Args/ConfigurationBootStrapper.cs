using System;
using Microsoft.Extensions.DependencyInjection;
using WebApiClient;

namespace AnyDict.Args
{
    public static class ConfigurationBootStrapper
    {
        public static void AddHttpApiServices(this IServiceCollection services)
        {
            var usiStr = "http://localhost:17235/";
            services.AddHttpApi<IDictHttpApi>();
            services.ConfigureHttpApi<IDictHttpApi>(o =>
            {
                o.HttpHost = new Uri(usiStr);
            });
        }
    }
}