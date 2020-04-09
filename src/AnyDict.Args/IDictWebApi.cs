using System.Collections.Generic;
using System.Threading.Tasks;
using AnyDict.Core.Moldes;
using WebApiClient;
using WebApiClient.Attributes;

namespace AnyDict.Args
{
    [TraceFilter]
    public interface IDictHttpApi : IHttpApi
    {
        [HttpGet("api/dict/search")]
        Task<SearchResult> Search(string word);
        [HttpGet("api/dict/prompts")]
        Task<IEnumerable<string>> Prompts(string word);
        [HttpGet("api/dict/setCurrentDict")]
        Task<string> SetCurrentDict(int num);
        [HttpGet("api/dict/infoList")]
        Task<IEnumerable<DictInfo>> GetInfoList();
    }
}