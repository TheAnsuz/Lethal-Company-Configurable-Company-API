using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurableCompany.model
{
    public sealed class FileDataCache
    {
        private static readonly Dictionary<string, FileDataCache> _cacheCache = new();

        public static FileDataCache GetFileCache(string file)
        {
            if (_cacheCache.TryGetValue(file, out FileDataCache cache))
                return cache;

            cache = new FileDataCache(file);

            _cacheCache.Add(file, cache);

            return cache;
        }

        private const long TICKS_IN_MILISECOND = 10000;
        private const long MILIS_IN_SECOND = 1000;
        private const long SECONDS_IN_MINUTE = 60;

        public long ReadThresholdTicks = TICKS_IN_MILISECOND * MILIS_IN_SECOND * SECONDS_IN_MINUTE * 5;
        public long WriteThresholdTicks = TICKS_IN_MILISECOND * MILIS_IN_SECOND * SECONDS_IN_MINUTE * 5;
        private long _lastRead;
        private long _lastWrite;
        private readonly string Filename;
        private readonly Dictionary<string, string> _data = new();

        private FileDataCache(string file)
        {
            _lastRead = 0;
            Filename = file;
        }

        public void Write(string key, string value)
        {
            if (_data.ContainsKey(key))
                _data[key] = value;
            else
                _data.Add(key, value);
        }

        public bool TryRead(string key, out string value)
        {
            return _data.TryGetValue(key, out value);
        }

        public bool RequestLoad()
        {
            ForceLoad();
            return true;
            /*
            if (_lastRead + ReadThresholdTicks < DateTime.Now.Ticks)
            {
                return true;
            }

            return false;
            */
        }

        public bool RequestSave()
        {
            ForceSave();
            return true;
            /*
            if (_lastWrite + WriteThresholdTicks < DateTime.Now.Ticks)
            {
                ForceSave();
                return true;
            }

            return false;
            */
        }

        public void ForceLoad()
        {
            if (!File.Exists(Filename))
            {
                Console.Error.WriteLine($"{Filename} does not exist");
                return;
            }

            using (var fileStream = File.OpenRead(Filename))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, 512))
            {
                _data.Clear();
                string line;
                string[] segment;
                do
                {
                    line = streamReader.ReadLine();

                    if (line == null)
                        break;

                    segment = line.Split(':');

                    if (segment.Length != 2)
                        continue;

                    _data.Add(segment[0], segment[1]);
                } while (true);
            }
            _lastRead = DateTime.Now.Ticks;
        }

        public void ForceSave()
        {
            if (!File.Exists(Filename))
            {
                File.Create(Filename).Close();
            }

            if (!File.Exists(Filename))
            {
                Console.Error.WriteLine($"{Filename} cannot be created");
                return;
            }

            using (var fileStream = File.OpenWrite(Filename))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8, 512))
            {
                foreach (var entry in _data)
                {
                    streamWriter.WriteLine($"{entry.Key}:{entry.Value}");
                }
            }
            _lastWrite = DateTime.Now.Ticks;
        }

        public Dictionary<string, string>.ValueCollection GetDataValues() => _data.Values;

        ~FileDataCache()
        {
            ForceSave();
        }
    }
}
