using System;
using System.Threading.Tasks;

namespace AnyDict.Args
{
    public static class TaskHelper
    {
        public static async Task<T> Safety<T>(this Task<T> task)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex);
# endif
            }
            return default;
        }
    }
}