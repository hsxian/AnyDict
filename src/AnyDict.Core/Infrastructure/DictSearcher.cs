using System.Net.Mime;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyDict.Core.Interfaces;
using AnyDict.Core.Moldes;

namespace AnyDict.Core.Infrastructure
{
    public class DictSearcher : IDictSearcher
    {
        private readonly IDictParser _dictParser;
        public DictSearcher(IDictParser dictParser)
        {
            this._dictParser = dictParser;

        }
        public async Task<SearchResult> Search(string word)
        {
            SearchResult result = null;
            if (string.IsNullOrWhiteSpace(word) || _dict?.Any() == false || _info?.IdxMap == null) return result;

            if (_info.IdxMap.TryGetValue(word, out var idx))
            {
                result = Convert(_dict, idx);
            }
            else if (_info.IdxMap.TryGetValue(word.ToLower(), out var idx2))
            {
                result = Convert(_dict, idx2);
            }
            else if (_info.IdxMap.TryGetValue(word.ToUpper(), out var idx3))
            {
                result = Convert(_dict, idx3);
            }
            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<SearchResult>> SearchWithStarts(string startsWord, int maxPrompts = 5)
        {
            if (string.IsNullOrWhiteSpace(startsWord) || _dict?.Any() == false || _info?.IdxMap == null) return null;
            var result = _info.IdxMap.Where(t => t.Key.StartsWith(startsWord)).Take(maxPrompts).Select(t => Convert(_dict, t.Value));
            return await Task.FromResult(result);
        }
        private byte[] _dict;
        private DictInfo _info;

        public async Task<bool> SetDict(byte[] dict, DictInfo info)
        {
            if (dict?.Any() == false || info?.IdxMap == null) return false;
            this._info = info;
            this._dict = dict;
            return await Task.FromResult(true);
        }

        private SearchResult Convert(byte[] dict, WordIdx idx)
        {
            var buffer = dict.Skip(idx.DataOffset).Take(idx.DataSize).ToArray();
            var result = new SearchResult
            {
                Word = idx.Word,
                Data = System.Text.Encoding.Default.GetString(buffer)
            };
            return result;
        }

        public async Task<bool> SetDict(string numOrName)
        {
            if (string.IsNullOrWhiteSpace(numOrName)) return false;
            DictInfo info;
            if (int.TryParse(numOrName, out var num) && num > 0 && num <= _dictParser.InfoList?.Count)
            {
                info = _dictParser.InfoList[num - 1];
            }
            else
            {
                info = _dictParser.InfoList.FirstOrDefault(t => t.BookName == numOrName);
            }
            if (info != null)
            {
                var dict = await _dictParser.GetDictBytes(info.DirAndFileName.Item1, info.DirAndFileName.Item2);
                return await SetDict(dict, info);
            }
            return false;
        }

        public DictInfo GetCurrentDictInfo()
        {
            return _info;
        }
    }
}