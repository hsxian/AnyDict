using System.Threading.Tasks;
using AnyDict.Core.Moldes;

namespace AnyDict.Args
{
    public interface ICommandProcessing
    {
        Task Processing(string[] args);
         Task Processing(CommandOptions options);
    }
}