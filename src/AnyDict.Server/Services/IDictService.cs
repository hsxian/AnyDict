using System.Threading.Tasks;
using AnyDict.Core.Moldes;
using System.Collections.Generic;

namespace AnyDict.Server.Services
{
    public interface IDictService
    {
        Task<IEnumerable<DictInfo>> GetListInfo();
        Task<SearchResult> GetSearchResult(string word);
        Task<IEnumerable<string>> GetPrompts(string word);
        Task<string> SetCurrentDict(string numOrName);
    }
}