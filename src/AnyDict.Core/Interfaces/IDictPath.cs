using System.Collections.Generic;

namespace AnyDict.Core.Interfaces
{
    public interface IDictPath
    {
        string DictStoreDir { get; set; }
        IEnumerable<(string path, string name)> GetAllDictDirAndName();

    }
}