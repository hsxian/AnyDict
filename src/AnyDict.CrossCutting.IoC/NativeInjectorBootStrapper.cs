using System;
using AnyDict.Core.Infrastructure;
using AnyDict.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AnyDict.CrossCutting.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IDictPath, DictPath>();
            services.AddScoped<IDictParser, DictParser>();
            services.AddScoped<IDictSearcher, DictSearcher>();
            services.AddScoped<IDrawer, Drawer>();
        }
    }
}
