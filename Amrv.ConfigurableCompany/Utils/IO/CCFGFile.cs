using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.Utils.IO
{
    public class CCFGFile(string file)
    {
        public const int META_CAPACITY_ESTIMATE = 15;
        public const int ENTRY_CAPACITY_ESTIMATE = 90;

        public const char TOKEN_ENTRY_START = '[';
        public const char TOKEN_ENTRY_END = ']';
        public const char TOKEN_META_SEPARATOR = '|';
        public const char TOKEN_VALUE_SEPARATOR = ':';
        public const char TOKEN_DATA_SKIP = '\\';

        public const string STRING_ENTRY_START = "[";
        public const string STRING_ENTRY_END = "]";
        public const string STRING_META_SEPARATOR = "|";
        public const string STRING_VALUE_SEPARATOR = ":";
        public const string STRING_DATA_SKIP = @"\";

        private static string MakeDataSafe(in string data)
        {
            StringBuilder builder = new(data);
            builder.Replace(STRING_DATA_SKIP, STRING_DATA_SKIP + STRING_DATA_SKIP);
            builder.Replace(STRING_ENTRY_START, STRING_DATA_SKIP + STRING_ENTRY_START);
            builder.Replace(STRING_ENTRY_END, STRING_DATA_SKIP + STRING_ENTRY_END);
            builder.Replace(STRING_META_SEPARATOR, STRING_DATA_SKIP + STRING_META_SEPARATOR);
            builder.Replace(STRING_VALUE_SEPARATOR, STRING_DATA_SKIP + STRING_VALUE_SEPARATOR);
            return builder.ToString();
        }

        private static string RevertDataSafe(in string data)
        {
            StringBuilder builder = new(data);
            builder.Replace(STRING_DATA_SKIP + STRING_ENTRY_START, STRING_ENTRY_START);
            builder.Replace(STRING_DATA_SKIP + STRING_ENTRY_END, STRING_ENTRY_END);
            builder.Replace(STRING_DATA_SKIP + STRING_META_SEPARATOR, STRING_META_SEPARATOR);
            builder.Replace(STRING_DATA_SKIP + STRING_VALUE_SEPARATOR, STRING_VALUE_SEPARATOR);
            builder.Replace(STRING_DATA_SKIP + STRING_DATA_SKIP, STRING_DATA_SKIP);
            return builder.ToString();
        }

        public class CCFGEntry
        {
            public readonly Dictionary<string, string> Metadata = [];
            public string Value;

            public string this[string meta]
            {
                get => Metadata[meta];
                set => Metadata[meta] = value;
            }
        }

        private readonly Dictionary<string, string> _meta = [];
        private readonly Dictionary<string, CCFGEntry> _entries = [];
        public readonly string File = file;

        public void AddMetadata(string name, string value) => _meta[name] = value;

        public void RemoveMetadata(string name) => _meta.Remove(name);

        public bool TryGetMetadata(string name, out string value) => _meta.TryGetValue(name, out value);

        public IReadOnlyDictionary<string, string> Metadata() => _meta;

        public void ClearMetadata() => _meta.Clear();

        public CCFGEntry AddEntry(string name, string value, Dictionary<string, string> meta = null)
        {
            if (TryGetEntry(name, out CCFGEntry entry))
            {
                entry.Value = value;
                entry.Metadata.Clear();

                if (meta != null)
                    foreach (var kv in meta)
                        entry[kv.Key] = kv.Value;
            }
            else
            {
                entry = new CCFGEntry() { Value = value };
                _entries[name] = entry;

                if (meta != null)
                    foreach (var kv in meta)
                        entry[kv.Key] = kv.Value;
            }

            return entry;
        }

        public void RemoveEntry(string name) => _entries.Remove(name);

        public bool TryGetEntry(string name, out CCFGEntry entry) => _entries.TryGetValue(name, out entry);

        public IReadOnlyDictionary<string, CCFGEntry> Entries() => _entries;


        public void Read()
        {
            if (!System.IO.File.Exists(File))
                return;

            string content = System.IO.File.ReadAllText(File);
            _meta.Clear();
            _entries.Clear();
            ReadEntries(content);
        }

        public void Write()
        {
            int metaCapacity = META_CAPACITY_ESTIMATE * _meta.Count;
            int entryCapacity = ENTRY_CAPACITY_ESTIMATE * _entries.Count;

            StringBuilder builder = new(metaCapacity + entryCapacity);

            // Write metadata
            builder.Append(TOKEN_ENTRY_START); // [.......
            foreach (var entry in _meta)
            {
                builder.Append(TOKEN_META_SEPARATOR); // [|.......
                builder //[|key:value
                    .Append(MakeDataSafe(entry.Key))
                    .Append(TOKEN_VALUE_SEPARATOR)
                    .Append(MakeDataSafe(entry.Value));
            }
            builder.Append(TOKEN_ENTRY_END);// [|].......
            builder.Append(Environment.NewLine); // Just for better visualization of the file content externally

            foreach (var entry in _entries)
            {
                builder
                    .Append(TOKEN_ENTRY_START)
                    .Append(MakeDataSafe(entry.Key))
                    .Append(TOKEN_VALUE_SEPARATOR)
                    .Append(MakeDataSafe(entry.Value.Value));

                foreach (var meta in entry.Value.Metadata)
                {
                    builder.Append(TOKEN_META_SEPARATOR); // [|.......
                    builder //[|key:value
                        .Append(MakeDataSafe(meta.Key))
                        .Append(TOKEN_VALUE_SEPARATOR)
                        .Append(MakeDataSafe(meta.Value));
                }

                builder.Append(TOKEN_ENTRY_END);// [|].......
                builder.Append(Environment.NewLine); // Just for better visualization of the file content externally
            }

            System.IO.File.WriteAllText(File, builder.ToString());
        }

        private void ReadEntries(string content)
        {
            bool skip = false;
            for (int i = 0; i < content.Length; i++)
            {
                if (skip)
                {
                    skip = false;
                    continue;
                }

                if (content[i] == TOKEN_DATA_SKIP)
                {
                    skip = true;
                }
                else if (content[i] == TOKEN_ENTRY_START)
                {
                    ReadSingleEntry(content, i, out i);
                }
            }
        }

        private void ReadSingleEntry(string content, in int index, out int endIndex)
        {
            int keyStart = index + 1;
            int keyEnd = -1;
            int valueStart = -1;
            bool inMeta = false;
            bool inValue = false;
            bool skip = false;
            string config = null;
            endIndex = index;
            for (int i = index; i < content.Length; i++)
            {
                if (skip)
                {
                    skip = false;
                    continue;
                }

                switch (content[i])
                {
                    case TOKEN_DATA_SKIP:
                        skip = true;
                        break;
                    case TOKEN_META_SEPARATOR:
                        if (inValue)
                        {
                            string key = content[keyStart..keyEnd];
                            string value = content[valueStart..i];

                            if (inMeta)
                            {
                                if (config == null)
                                    AddMetadata(RevertDataSafe(key), RevertDataSafe(value));
                                else if (TryGetEntry(config, out var entry))
                                {
                                    entry.Metadata.Add(RevertDataSafe(key), RevertDataSafe(value));
                                }
                            }
                            else
                            {
                                config = RevertDataSafe(key);
                                AddEntry(config, RevertDataSafe(value));
                            }
                        }

                        keyStart = i + 1;
                        inMeta = true;
                        inValue = false;
                        break;
                    case TOKEN_VALUE_SEPARATOR:
                        inValue = true;
                        keyEnd = i;
                        valueStart = i + 1;
                        break;
                    case TOKEN_ENTRY_END:
                        endIndex = i;
                        if (inValue)
                        {
                            string key = content[keyStart..keyEnd];
                            string value = content[valueStart..i];

                            if (inMeta)
                            {
                                if (config == null)
                                    AddMetadata(RevertDataSafe(key), RevertDataSafe(value));
                                else if (TryGetEntry(config, out var entry))
                                {
                                    entry.Metadata.Add(RevertDataSafe(key), RevertDataSafe(value));
                                }
                            }
                            else
                            {
                                config = RevertDataSafe(key);
                                AddEntry(config, RevertDataSafe(value));
                            }
                        }
                        return;
                }
            }
        }
    }
}
