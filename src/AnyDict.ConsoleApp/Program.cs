using System.Linq;
using System;
using OnConsoleKey;
using Console = Colorful.Console;
using System.Threading.Tasks;
using AnyDict.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using AnyDict.CrossCutting.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace AnyDict.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {


            var setting = new SettingModel();
            var temp = await SettingModel.GetSetting("setting.json");
            if (temp != null)
            {
                setting = temp;
            }
            var builder = new HostBuilder()
              .ConfigureServices((hostContext, services) =>
              {
                  services.RegisterServices();
                services.AddScoped<ICommandProcessing, CommandProcessing>();
              })
              ;

            var host = builder.Build();
            var dictParser = host.Services.GetService<IDictParser>();
            await dictParser.LoadInfoList(setting.DictionaryHome);

            var commandProcessing = host.Services.GetService<ICommandProcessing>();
            commandProcessing.OnSetCurrentDict += async name =>
            {
                if (setting.CurrentBookName == name) return;
                setting.CurrentBookName = name;
                await SettingModel.SaveSetting(setting.HistoryFile, setting);
            };
            var dictSearch = host.Services.GetService<IDictSearcher>();

            var info = dictParser.InfoList.FirstOrDefault(t => t.BookName == setting.CurrentBookName);
            if (info != null)
            {
                var dict = await dictParser.GetDictBytes(info.DirAndFileName.Item1, info.DirAndFileName.Item2);
                await dictSearch.SetDict(dict, info);
            }

            var sc = new SimpleConsole();
            Console.CancelKeyPress += async (sen, arg) =>
             {
                 Console.WriteLine("exit...");
                 await SettingModel.SaveSetting(setting.HistoryFile, setting);
             };
            sc.Histories.MaxHistoryCount = setting.MaxHistoryCount;
            sc.Histories.RestoreFromFile(setting.HistoryFile);
            sc.Prompts = "any word> ";
            sc.PromptsColor = ConsoleColor.Red;
            sc.CursorCountFromCharactersHandle = c => c > 128 ? (byte)2 : (byte)1;
            sc.AutoCompleteHandle = async str =>
            {
                return (await dictSearch
                    .SearchWithStarts(str, setting.MaxPromptsCount))
                    ?.Select(t => t.Word)
                    .ToList();
            };
            sc.OnConsoleKeyInfo += async (sen, key) =>
            {
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    await commandProcessing.Processing(sc.Buffer.ToString().Split(" "));
                }
            };
            sc.Start();
        }
    }
}