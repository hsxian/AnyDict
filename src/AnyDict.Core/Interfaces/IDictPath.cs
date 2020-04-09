using System.Collections.Generic;

namespace AnyDict.Core.Interfaces
{
    public interface IDictPath
    {
        IEnumerable<(string path, string name)> GetAllDictDirAndName(string homeFolder);

    }
}