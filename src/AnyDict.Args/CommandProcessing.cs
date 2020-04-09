using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Drawing;
using System.Threading.Tasks;
using AnyDict.Core.Interfaces;
using AnyDict.Core.Moldes;
using CommandLine;

namespace AnyDict.Args
{
    public class CommandProcessing : ICommandProcessing
    {
        private readonly IDrawer _drawer;
        private readonly IDictHttpApi _dictHttpApi;

        public CommandProcessing(IDrawer drawer, IDictHttpApi dictHttpApi)
        {
            this._drawer = drawer;
            this._dictHttpApi = dictHttpApi;
        }
        public async Task Processing(string[] args)
        {
            Parser.Default.ParseArguments<CommandOptions>(args)
               .WithParsed(o =>
              {
                  try
                  {
                      Processing(o).Wait();
                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine(ex);
                  }
              });
            await Task.CompletedTask;
        }

        public async Task Processing(CommandOptions options)
        {

            if (options.ListDicts)
            {
                _drawer.ListAllDictInfo(await _dictHttpApi.GetInfoList().Safety());
            }
            else if (options.DictNum != null)
            {
                var bookName = await _dictHttpApi.SetCurrentDict(options.DictNum.Value).Safety();
                if (string.IsNullOrWhiteSpace(bookName) == false)
                {
                    Console.WriteLine($"set current dict to: {bookName}.", Color.Gold);
                }
            }
            else if (false == options.Prompt && false == string.IsNullOrWhiteSpace(options.Word))
            {
                var result = await _dictHttpApi.Search(options.Word).Safety();
                _drawer.DrawSearchResult(result);
            }
            else if (options.Prompt && false == string.IsNullOrWhiteSpace(options.Word))
            {
                var prompts = await _dictHttpApi.Prompts(options.Word);
                if (prompts == null) return;
                // var dir = "/tmp/any-dict/prompt/";
                // if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
                // await File.WriteAllLinesAsync(dir + options.Word, prompts);
                var ps = prompts.Select(t => t.Trim().Replace("\n", ""));

                _drawer.DrawPromptResult(ps);
            }
            await Task.CompletedTask;
        }


    }
}