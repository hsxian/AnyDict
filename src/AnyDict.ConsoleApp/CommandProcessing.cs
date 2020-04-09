using System.Drawing;
using System;
using System.Linq;
using System.Threading.Tasks;
using AnyDict.Core.Interfaces;
using AnyDict.Core.Moldes;
using CommandLine;
using Colorful;
using Console = Colorful.Console;

namespace AnyDict.ConsoleApp
{
    public class CommandProcessing : ICommandProcessing
    {
        private readonly IDictParser _dictParser;
        private readonly IDictSearcher _searcher;
        private readonly IDrawer _drawer;

        public CommandProcessing(IDictParser dictParser, IDictSearcher searcher, IDrawer drawer)
        {
            _dictParser = dictParser;
            _searcher = searcher;
            _drawer = drawer;
        }

        public event Func<string, Task> OnSetCurrentDict;

        public async Task Processing(CommandOptions options)
        {
            if (options.ListDicts)
            {
                _drawer.ListAllDictInfo(_dictParser.InfoList);
            }
            else if (options.DictNum != null && options.DictNum > 0 && options.DictNum <= _dictParser.InfoList?.Count)
            {
                var info = _dictParser.InfoList[options.DictNum.Value - 1];
                var dict = await _dictParser.GetDictBytes(info.DirAndFileName.Item1, info.DirAndFileName.Item2);
                if (await _searcher.SetDict(dict, info))
                {
                    Console.WriteLine($"set current dict to: {info.BookName}.", Color.Gold);
                    await OnSetCurrentDict?.Invoke(info.BookName);
                }
            }
            else if (false == string.IsNullOrWhiteSpace(options.Word))
            {
                var result = await _searcher.Search(options.Word);
                _drawer.DrawSearchResult(result);
            }
        }

        public async Task Processing(string[] args)
        {
            Parser.Default.ParseArguments<CommandOptions>(args)
                .WithParsed(async o =>
                {
                    try
                    {
                        await Processing(o);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
            await Task.CompletedTask;
        }
    }
}