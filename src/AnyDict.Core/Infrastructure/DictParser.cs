using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using AnyDict.Core.Interfaces;
using AnyDict.Core.Moldes;
using System;
using System.Text;
using System.IO.Compression;

namespace AnyDict.Core.Infrastructure
{
    public class DictParser : IDictParser
    {
        private readonly IDictPath _dictPath;

        public List<DictInfo> InfoList { get; protected set; }
        public DictParser(IDictPath dictPath)
        {
            this._dictPath = dictPath;
        }
        public async Task<DictInfo> GetDictInfo(string dir, string name)
        {
            DictInfo result = await GenerateBaseInfoFromInfoFile(Path.Combine(dir, name) + ".ifo");
            if (result == null) return null;
            result.IdxMap = await GetWordIdxMap(Path.Combine(dir, name) + ".idx");
            return result;
        }
        private async Task<DictInfo> GenerateBaseInfoFromInfoFile(string path)
        {
            if (false == File.Exists(path))
            {
                return null;
            }
            var result = new DictInfo();
            var type = typeof(DictInfo);
            var props = type.GetProperties();
            var lines = File.ReadAllLines(path).Where(t => false == string.IsNullOrWhiteSpace(t));
            foreach (var line in lines)
            {
                var kv = line.Split('=');
                if (kv.Length != 2) continue;
                var prop = props.FirstOrDefault(t => t.Name.ToLower() == kv[0]);
                if (prop == null) continue;
                prop.SetValue(result, kv[1]);
            }
            return await Task.FromResult(result);
        }
        private async Task<Dictionary<string, WordIdx>> GetWordIdxMap(string path)
        {
            if (false == File.Exists(path))
            {
                return null;
            }
            var fIdx = File.ReadAllBytes(path);
            var buffer = new List<byte>();

            var isWord = true;
            var isWordDataOffset = false;
            var currentWordDataOffset = 0;
            var isWordDataOffsetBytesCount = 0;
            var isWordDataSize = false;
            var currentWordDataSize = 0;
            var isWordDataSizeBytesCount = 0;
            var currentWord = "";
            var isStop = false;
            var idxMap = new Dictionary<string, WordIdx>();
            foreach (var item in fIdx)
            {
                if (item == 0 && isWord)
                {
                    currentWord = Encoding.Default.GetString(buffer.ToArray());
                    isWord = false;
                    isWordDataOffset = true;
                    buffer.Clear();
                }
                else if (isStop)
                {
                    if (item != 0) { throw new Exception(); }
                    isWordDataOffset = true;
                    isStop = false;
                }
                else if (isWordDataOffset)
                {
                    isWordDataOffsetBytesCount++;
                    currentWordDataOffset = currentWordDataOffset * 256 + item;
                    if (isWordDataOffsetBytesCount == DictIdx.IdxOffsetBits / 32)
                    {
                        isWordDataOffset = false;
                        isWordDataSize = true;
                        isWordDataOffsetBytesCount = 0;
                    }
                }
                else if (isWordDataSize)
                {
                    isWordDataSizeBytesCount++;
                    currentWordDataSize = currentWordDataSize * 256 + item;
                    if (isWordDataSizeBytesCount == DictIdx.IdxOffsetBits / 32)
                    {
                        isWordDataSize = false;
                        isWord = true;
                        isWordDataSizeBytesCount = 0;
                        var wi = new WordIdx { Word = currentWord, DataOffset = currentWordDataOffset, DataSize = currentWordDataSize };
                        // System.Console.WriteLine(wi);
                        if (idxMap.ContainsKey(currentWord) == false)
                            idxMap.Add(currentWord, wi);
                        currentWord = ""; currentWordDataOffset = 0; currentWordDataSize = 0;
                    }
                }
                else
                {
                    buffer.Add(item);
                }
            }
            return await Task.FromResult(idxMap);
        }
        public async Task<bool> DecompressDzDict(string dir, string name)
        {
            var path = Path.Combine(dir, name) + ".dict.dz";
            if (false == File.Exists(path))
            {
                return false;
            }

            using (var fs = new FileStream(path, FileMode.Open))
            {
                var dictPath = Path.Combine(dir, name) + ".dict";
                using (FileStream decompressedFileStream = File.Create(dictPath))
                {
                    using (GZipStream decompressionStream = new GZipStream(fs, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine($"Decompressed: {dictPath}");
                    }
                }
            }
            return await Task.FromResult(true);
        }

        public async Task<byte[]> GetDictBytes(string dir, string name)
        {
            var dictPath = Path.Combine(dir, name) + ".dict";
            if (false == File.Exists(dictPath))
            {
                return null;
            }
            return await Task.FromResult(File.ReadAllBytes(dictPath));
        }

        public async Task LoadInfoList(string homeFolder)
        {
            var dir_names = _dictPath.GetAllDictDirAndName(homeFolder);
            InfoList = new List<DictInfo>();
            foreach ((string dir, string name) in dir_names)
            {
                var info = await GetDictInfo(dir, name);
                info.DirAndFileName = new Tuple<string, string>(dir, name);
                InfoList.Add(info);
                var path = Path.Combine(dir, name) + ".dict";
                if (File.Exists(path) == false)
                {
                    var res = await DecompressDzDict(dir, name);
                }
            }
        }
    }
}