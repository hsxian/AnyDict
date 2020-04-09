using System;
using System.IO;
using System.Collections.Generic;
using AnyDict.Core.Interfaces;
using System.Linq;

namespace AnyDict.Core.Infrastructure
{
    public class DictPath : IDictPath
    {
        public IEnumerable<(string path, string name)> GetAllDictDirAndName(string homeFolder)
        {
            if (string.IsNullOrWhiteSpace(homeFolder)) throw new ArgumentNullException(nameof(homeFolder));
            if (false == Directory.Exists(homeFolder)) throw new DirectoryNotFoundException($"{nameof(homeFolder)}: {homeFolder} not found.");
            return Directory.GetDirectories(homeFolder)
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