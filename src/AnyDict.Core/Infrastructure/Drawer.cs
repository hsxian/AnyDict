using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using AnyDict.Core.Interfaces;
using AnyDict.Core.Moldes;
using System.Text.RegularExpressions;
using System;
using Console = Colorful.Console;

namespace AnyDict.Core.Infrastructure
{
    public class Drawer : IDrawer
    {
        public void ListAllDictInfo(IEnumerable<DictInfo> infos)
        {
            var i = 0;
            foreach (var info in infos)
            {
                Console.WriteLine($"===={++i}===", Color.Yellow);
                Console.WriteLine("BookName: " + info.BookName);
                Console.WriteLine("Author: " + info.Author);
                Console.WriteLine("WordCount: " + info.WordCount);
                Console.WriteLine("Version: " + info.Version);
                Console.WriteLine("Date: " + info.Date);
                Console.WriteLine("Description: " + info.Description);
                Console.WriteLine();
            }
        }

        public void DrawSearchResult(SearchResult result)
        {
            if (result == null)
            {
                Console.WriteLine(":(  nothing is here.", Color.Gold);
                return;
            }
            Console.WriteLine(result.Word, Color.LimeGreen);
            DrawAllData(result.Data);
        }

        private void DrawAllData(string data)
        {
            var ds = data.Split(new[] { "\n\n" }, StringSplitOptions.None);
            foreach (var d in ds)
            {
                DrawData(d);
            }
        }

        private void DrawData(string data)
        {
            var pRegex = new Regex(@" (\d){1,2} |(\d){1,2} | (\d){1,2}");
            if (pRegex.IsMatch(data))
            {
                var ps = pRegex.Split(data);
                var idx = ps[0].LastIndexOf("/") + 1;
                Console.WriteLine(ps[0].Substring(0, idx), Color.Cyan);
                Console.WriteLine(ps[0].Substring(idx, ps[0].Length - idx), Color.Yellow);
                for (var i = 1; i < ps.Length; i += 2)
                {
                    Console.Write($"{ps[i]}. ", Color.Yellow);
                    DrawLine($"{ps[i + 1]}");
                }
            }
            else
            {
                DrawLine(data);
            }
        }

        private void DrawLine(string line)
        {
            var zhRegex = new Regex("([\u4e00-\u9fa5]|[、，。；？~！：‘“”’【】（）《》 ])");
            if (zhRegex.IsMatch(line))
            {
                var zh_en = zhRegex.Split(line);
                foreach (var l in zh_en)
                {
                    Console.Write(l, zhRegex.IsMatch(l) ? Color.DarkGoldenrod : Color.DarkGreen);
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(line, Color.WhiteSmoke);
            }
        }

        public void DrawPromptResult(IEnumerable<string> prompts, int minBlankCount = 0)
        {
            if (prompts == null) return;
            var ps = prompts.ToList();
            var mc = CalMaxColumnCount(ps, minBlankCount);

            var w = Console.WindowWidth / mc;

            for (int i = 0; i < ps.Count; i++)
            {
                if (i % mc != mc - 1)
                {
                    var format = $"{{0,-{w + minBlankCount}}}";
                    Console.Write(format, ps[i]);
                }
                else
                {
                    Console.WriteLine(ps[i]);
                }
            }

            if (ps.Count % mc != 0)
                Console.WriteLine();
        }

        private int CalMaxColumnCount(IEnumerable<string> prompts, int minBlankCount = 0)
        {
            var result = 1;
            if (prompts == null) return result;
            var ps = prompts.ToList();
            var w = Console.WindowWidth;
            do
            {
                var lens = new int[++result];
                for (int i = 0; i < ps.Count; i++)
                {
                    var idx = i % result;
                    lens[idx] = Math.Max(lens[idx], ps[i].Length + minBlankCount);
                }
                if (lens.Sum() > w)
                {
                    --result;
                    break;
                }
            }
            while (true);
            return Math.Max(1, result);
        }
    }
}