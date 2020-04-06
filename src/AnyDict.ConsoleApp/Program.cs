using System.Linq;
using System;
using AnyDict.Core.Implements;
using OnConsoleKey;
using Console = Colorful.Console;

namespace AnyDict.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new CommandLineHandler();
            var search = new DictSearcher();
            Console.CancelKeyPress += (sen, arg) =>
            {
                Console.WriteLine("exit...");
            };
            var sc = new SimpleConsole();
            sc.Prompts = "any word> ";
            sc.PromptsColor = ConsoleColor.Red;
            sc.CursorCountFromCharactersHandle = c => c > 128 ? (byte)2 : (byte)1;
            sc.AutoCompleteHandle = async str =>
            {
                return (await search.Search(str, handler.CurrentDict, handler.CurrentInfo))?.Select(t => t.Word).ToList();
            };
            sc.OnConsoleKeyInfo += async (sen, key) =>
            {
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    await handler.Handle(sc.Buffer.ToString());
                }
            };
            sc.Start();
        }


    }
}