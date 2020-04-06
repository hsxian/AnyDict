using System.Net.Mime;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyDict.Core.Interfaces;
using AnyDict.Core.Moldes;

namespace AnyDict.Core.Implements
{
    public class DictSearcher : IDictSearcher
    {
        public async Task<IEnumerable<SearchResult>> Search(string word, byte[] dict, DictInfo info, int maxPrompts = 5)
        {
            if (string.IsNullOrWhiteSpace(word) || dict?.Any() == false || info?.IdxMap == null) return null;
            var result = new List<SearchResult>();

            if (info.IdxMap.TryGetValue(word, out var idx))
            {
                result.Add(Convert(dict, idx));
            }
            else if (info.IdxMap.TryGetValue(word.ToLower(), out var idx2))
            {
                result.Add(Convert(dict, idx2));
            }
            else if (info.IdxMap.TryGetValue(word.ToLower(), out var idx3))
            {
                result.Add(Convert(dict, idx3));
            }
            else
            {
                info.IdxMap.Where(t => t.Key.StartsWith(word)).Take(maxPrompts).ToList().ForEach(t => result.Add(Convert(dict, t.Value)));
            }
            return await Task.FromResult(result);
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
    }
}