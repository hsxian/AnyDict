using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using AnyDict.Core.Interfaces;
using AnyDict.Core.Moldes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AnyDict.Server.Services
{
    public class DictService : IDictService
    {
        private readonly IConfiguration _configuration;
        private readonly IDictParser _dictParser;
        private readonly IDictSearcher _dictSearcher;

        public DictService(
            IConfiguration configuration,
            IServiceProvider serviceProvider
            )
        {
            this._configuration = configuration;

            using var provider = serviceProvider.CreateScope();
            this._dictParser = provider.ServiceProvider.GetService<IDictParser>();
            this._dictSearcher = provider.ServiceProvider.GetService<IDictSearcher>();

            _ = _dictParser.LoadInfoList(_configuration["DictionaryHome"]);
            _ = SetCurrentDict(_configuration["CurrentBookName"]);
        }

        public async Task<IEnumerable<DictInfo>> GetListInfo()
        {
            return await Task.FromResult(_dictParser.InfoList);
        }

        public async Task<IEnumerable<string>> GetPrompts(string word)
        {
            return (await _dictSearcher.SearchWithStarts(word, int.MaxValue))?.Select(t => t.Word).ToList();
        }

        public async Task<SearchResult> GetSearchResult(string word)
        {
            return await _dictSearcher.Search(word);
        }

        public async Task<string> SetCurrentDict(string numOrName)
        {
            if (await _dictSearcher.SetDict(numOrName))
            {
                return _dictSearcher.GetCurrentDictInfo()?.BookName;
            }
            return null;
        }
    }
}