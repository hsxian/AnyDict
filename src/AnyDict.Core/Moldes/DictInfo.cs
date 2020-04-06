using System;
using System.Collections.Generic;

namespace AnyDict.Core.Moldes
{
    public class DictInfo
    {
        public string BookName { get; set; }
        public string Version { get; set; }
        public string WordCount { get; set; }
        public string IdxFileSize { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string SameTypeSequence { get; set; }
        public Tuple<string, string> DirAndFileName { get; set; }
        public Dictionary<string, WordIdx> IdxMap { get; set; }
    }
}