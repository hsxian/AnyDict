namespace AnyDict.Core.Moldes
{
    public class WordIdx
    {
        public string Word { get; set; }
        public int DataOffset { get; set; }
        public int DataSize { get; set; }
        public override string ToString()
        {
            return $"{Word}, {DataOffset}, {DataSize}.";
        }
    }
}