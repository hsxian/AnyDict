using System.Collections.Generic;
using AnyDict.Core.Moldes;

namespace AnyDict.Core.Interfaces
{
    public interface IDrawer
    {
        void ListAllDictInfo(IEnumerable<DictInfo> infos);
        void DrawSearchResult(SearchResult result);
        void DrawPromptResult(IEnumerable<string> prompts, int minBlankCount = 0);
    }
}