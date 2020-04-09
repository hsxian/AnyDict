using System.Linq;
using System.IO;
using AnyDict.CrossCutting.IoC;
using System;
using CommandLine;
using AnyDict.Core.Moldes;
using AnyDict.Core.Infrastructure;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AnyDict.Core.Interfaces;

namespace AnyDict.Args
{
    class Program
    {

        static async Task Main(string[] args)
        {
            if (args.Any() == false) { args = new[] { "--help" }; }
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.RegisterServices();
                    services.AddScoped<ICommandProcessing, CommandProcessing>();
                    services.AddHttpApiServices();
                })
                ;

            var host = builder.Build();
            var commandProcessing = host.Services.GetService<ICommandProcessing>();
            await commandProcessing.Processing(args);
        }
    }
}
