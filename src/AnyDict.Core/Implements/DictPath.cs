using System.IO;
using System.Collections.Generic;
using AnyDict.Core.Interfaces;
using System.Linq;

namespace AnyDict.Core.Implements
{
    public class DictPath : IDictPath
    {
        public string DictStoreDir { get; set; }

        public IEnumerable<(string path, string name)> GetAllDictDirAndName()
        {
            if (string.IsNullOrWhiteSpace(DictStoreDir) || false == Directory.Exists(DictStoreDir)) return null;
            return Directory.GetDirectories(DictStoreDir)
               .Where(t =>
               {
                   return Directory.GetFiles(t).Any(tt =>
                   {
                       return tt.ToLower().EndsWith(".ifo");
                   });
               })
               .Select(t =>
               {
                   var fp = Directory.GetFiles(t, "*.ifo").First();
                   var path = Path.GetDirectoryName(fp);
                   var name = Path.GetFileNameWithoutExtension(fp);
                   return (path, name);
               })
               .Distinct()
               ;

        }
    }
}