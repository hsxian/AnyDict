using System.Collections.Generic;
using System.Threading.Tasks;
using AnyDict.Core.Moldes;

namespace AnyDict.Core.Interfaces
{
    public interface IDictSearcher
    {
        DictInfo GetCurrentDictInfo();
        Task<bool> SetDict(string numOrName);
        Task<bool> SetDict(byte[] dict, DictInfo info);
        Task<SearchResult> Search(string word);
        Task<IEnumerable<SearchResult>> SearchWithStarts(string startsWord, int maxPrompts = 5);
    }
}