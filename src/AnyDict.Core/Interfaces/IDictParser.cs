using System.Threading.Tasks;
using AnyDict.Core.Moldes;

namespace AnyDict.Core.Interfaces
{
    public interface IDictParser
    {
        Task<DictInfo> GetDictInfo(string dir, string name);
        Task<bool> DecompressDzDict(string dir, string name);
        Task<byte[]> GetDictBytes(string dir, string name);
    }
}