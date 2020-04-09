using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnyDict.ConsoleApp
{
    public class SettingModel
    {
        public string CurrentBookName { get; set; }
        public string DictionaryHome { get; set; }
        public int MaxPromptsCount { get; set; }
        public int MaxHistoryCount { get; set; } = 30;
        public int MaxResultCount { get; set; } = 5;
        public string HistoryFile { get; set; } = "history.txt";

        public static async Task<SettingModel> GetSetting(string file)
        {
            if (false == File.Exists(file)) return null;
            var json = await File.ReadAllTextAsync(file);
            try
            {
                return JsonConvert.DeserializeObject<SettingModel>(json);

            }
            catch (Exception)
            {
                Console.WriteLine(":(  read setting.json error.");
            }
            return null;
        }
        public static async Task<bool> SaveSetting(string file, SettingModel model)
        {
            if (model == null) return false;
            var dir = Path.GetDirectoryName(file);
            if (false == string.IsNullOrWhiteSpace(dir) && false == Directory.Exists(dir)) return false;

            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            await File.WriteAllTextAsync(file, json);
            return true;
        }
    }
}