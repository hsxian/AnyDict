using System;
using System.Threading.Tasks;
using AnyDict.Core.Moldes;

namespace AnyDict.ConsoleApp
{
    public interface ICommandProcessing
    {
        event Func<string, Task> OnSetCurrentDict;
        Task Processing(CommandOptions options);
        Task Processing(string[] args);
    }
}