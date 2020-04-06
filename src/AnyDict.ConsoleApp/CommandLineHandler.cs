using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnyDict.Core.Implements;
using AnyDict.Core.Interfaces;
using AnyDict.Core.Moldes;
using System.Drawing;
using Console = Colorful.Console;

namespace AnyDict.ConsoleApp
{
    public class CommandLineHandler
    {
        private static readonly string _settingFile = "setting.json";
        public static SettingModel Setting { get; private set; }
        private readonly IDictPath _dictPath;
        private readonly IDictParser _dictParser;
        private readonly IDictSearcher _dictSearch;
        private readonly Drawer _drawer;
        public byte[] CurrentDict { get; private set; }
        public DictInfo CurrentInfo { get; private set; }
        private List<DictInfo> Infos { get; set; }

        static CommandLineHandler()
        {
            Setting = SettingModel.GetSetting(_settingFile).Result;
            if (Setting == null)
            {
                Setting = new SettingModel
                {
                    DictionaryHome = "home",
                    MaxPromptsCount = 5,
                };
                _ = SettingModel.SaveSetting(_settingFile, Setting);
            }
        }

        public CommandLineHandler()
        {
            _dictParser = new DictParser();
            _dictPath = new DictPath { DictStoreDir = Setting.DictionaryHome };
            _dictSearch = new DictSearcher();
            Infos = new List<DictInfo>();
            _drawer = new Drawer();
            _ = InitDictInfoList();
            _ = InitDict();
        }

        private async Task SetCurrentDict(DictInfo info)
        {
            if (info == null) return;
            TipCurrentDict(info);
            Setting.CurrentBookName = info.BookName;
            await SettingModel.SaveSetting(_settingFile, Setting);
            CurrentInfo = info;
            CurrentDict = await _dictParser.GetDictBytes(info.DirAndFileName.Item1, info.DirAndFileName.Item2);
        }

        private async Task InitDict()
        {
            CurrentInfo = Infos.FirstOrDefault(t => t.BookName == Setting.CurrentBookName);

            if (CurrentInfo == null)
            {
                CurrentInfo = Infos.FirstOrDefault();
            }
            await SetCurrentDict(CurrentInfo);
        }
        private void TipCurrentDict(DictInfo info)
        {
            if (info == null) return;
            Console.WriteLine($"set current dict: {info.BookName}", Color.Gold);
        }
        private async Task InitDictInfoList()
        {
            var dir_names = _dictPath.GetAllDictDirAndName().ToList();

            foreach ((string dir, string name) in dir_names)
            {
                var info = await _dictParser.GetDictInfo(dir, name);
                info.DirAndFileName = new Tuple<string, string>(dir, name);
                Infos.Add(info);
                var path = Path.Combine(dir, name) + ".dict";
                if (File.Exists(path) == false)
                {
                    var res = await _dictParser.DecompressDzDict(dir, name);
                }
            }
        }
        private async Task ChooseDict(string line)
        {
            var reg = new Regex(@"\d+");
            if (reg.IsMatch(line))
            {
                var str = reg.Match(line);
                if (int.TryParse(str.Value, out var n) && n > 0 && n < Infos.Count + 1)
                {
                    await SetCurrentDict(Infos[n - 1]);
                }
            }
        }
        public async Task Handle(string line)
        {
            line = line.Trim();
            if (line.StartsWith("-c ") || line.StartsWith("--choose "))
            {
                await ChooseDict(line);
                return;
            }
            switch (line)
            {
                case "-h":
                case "--help":
                    _drawer.DrawHelp();
                    break;
                case "-l":
                case "--list":
                    _drawer.ListAllDictInfo(Infos);
                    break;
                default:
                    await GetWordAndDraw(line);
                    break;
            }
        }

        private async Task GetWordAndDraw(string line)
        {
            var result = (await _dictSearch.Search(line, CurrentDict, CurrentInfo))?.ToList();
            if (result?.Any() == true)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    Console.Write("|--", Color.Chartreuse);
                    Console.Write(i + 1, Color.Red);
                    Console.Write(new string('-', Console.WindowWidth - 6), Color.Chartreuse);
                    Console.Write("|\n", Color.Chartreuse);
                    _drawer.DrawSearchResult(result[i]);
                }
            }
            else
            {
                Console.WriteLine(":(  nothing here...", Color.Gold);
            }
        }
    }
}