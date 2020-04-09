using System.Collections.Generic;
using CommandLine;

namespace AnyDict.Core.Moldes
{
    public class CommandOptions
    {
        [Value(0, MetaName = "[word...]", HelpText = "word to be queried.")]
        public string Word { get; set; }

        [Option('c', "choose", HelpText = "choose one of the dictionaries listed in the list command according to the serial number.")]
        public int? DictNum { get; set; }

        [Option('l', "list", HelpText = "list all dictionaries.")]
        public bool ListDicts { get; set; }
        [Option('p', "prompt", HelpText = "complementary tips.")]
        public bool Prompt { get; set; }
    }
}