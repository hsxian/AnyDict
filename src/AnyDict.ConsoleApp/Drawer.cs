using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using AnyDict.Core.Moldes;
using Colorful;

namespace AnyDict.ConsoleApp
{
    public class Drawer
    {
        public void ListAllDictInfo(List<DictInfo> infos)
        {
            var i = 0;
            foreach (var info in infos)
            {
                Console.WriteLine($"===={++i}===");
                Console.WriteLine("BookName: " + info.BookName);
                Console.WriteLine("Author: " + info.Author);
                Console.WriteLine("WordCount: " + info.WordCount);
                Console.WriteLine("Version: " + info.Version);
                Console.WriteLine("Date: " + info.Date);
                Console.WriteLine("Description: " + info.Description);
                Console.WriteLine("Root path: " + info.DirAndFileName.Item1);
                Console.WriteLine();
            }
        }

        public void DrawHelp()
        {
            Console.WriteLine($@"
-h,--help                       display this help
-l,--list                          list all dict
-c,--choose  [num]         choose [num] dict to use
");
        }

        public void DrawSearchResult(SearchResult result)
        {
            Console.WriteLine(result.Word, Color.LimeGreen);
            DrawAllData(result.Data);
        }

        private void DrawAllData(string data)
        {
            var ds = data.Split("\n\n");
            foreach (var d in ds)
            {
                DrawData(d);
            }
        }

        private void DrawData(string data)
        {
            var pRegex = new Regex(@" (\d) ");
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
    }
}