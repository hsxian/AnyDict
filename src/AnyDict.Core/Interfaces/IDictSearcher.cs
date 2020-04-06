using System.Collections.Generic;
using System.Threading.Tasks;
using AnyDict.Core.Moldes;

namespace AnyDict.Core.Interfaces
{
    public interface IDictSearcher
    {
        Task<IEnumerable<SearchResult>> Search(string word, byte[] dict, DictInfo info, int maxPrompts = 5);
    }
}