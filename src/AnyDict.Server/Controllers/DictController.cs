using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AnyDict.Core.Moldes;
using AnyDict.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnyDict.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictController : ControllerBase
    {
        private readonly IDictService _dictService;

        public DictController(IDictService dictService)
        {
            this._dictService = dictService;
        }
        [HttpGet("search")]
        public async Task<SearchResult> Search(string word)
        {
            return await _dictService.GetSearchResult(word);
        }
        [HttpGet("prompts")]
        public async Task<IEnumerable<string>> Prompts(string word)
        {

            return await _dictService.GetPrompts(word);
        }
        [HttpGet("setCurrentDict")]
        public async Task<string> setCurrentDict(string num)
        {
            return await _dictService.SetCurrentDict(num);
        }
        [HttpGet("infoList")]
        public async Task<IEnumerable<DictInfo>> GetInfoList(string num)
        {
            return await _dictService.GetListInfo();
        }
    }
}