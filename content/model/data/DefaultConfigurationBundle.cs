#if DEBUG
using System;
#endif
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Amrv.ConfigurableCompany.content.model.data
{
    public class DefaultConfigurationBundle : ConfigurationBundle
    {
        public string TOKEN_FILE_START => "{";
        public string TOKEN_FILE_END => "}";

        public string TOKEN_ENTRY_START => "[";
        public string TOKEN_ENTRY_END => "]";

        public string TOKEN_ENTRY_SEPARATOR => ":";
        public string TOKEN_ENTRY_VALUE_SEPARATOR => "=";

        public string TOKEN_SKIP => "\\";

        private readonly Dictionary<string, string> _mapping = new();

        public override void AddIfMissing(string key, string value)
        {
            if (!_mapping.ContainsKey(key))
            {
                _mapping[key] = value;
            }
        }

        public override void Add(string key, string value) => _mapping[key] = value;

        public override void Clear() => _mapping.Clear();

        public override string Get(string key, string defaultValue) => _mapping.TryGetValue(key, out var value) ? value : defaultValue;

        public override IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _mapping.GetEnumerator();

        public override void Remove(string key) => _mapping.Remove(key);

        public override bool TryGet(string key, out string value) => _mapping.TryGetValue(key, out value);

        public override string Pack()
        {
            StringBuilder builder = new();

            builder.Append(TOKEN_FILE_START);

            foreach (KeyValuePair<string, string> entry in _mapping)
            {
                builder
                    .Append(TOKEN_ENTRY_START) // [
                    .Append(entry.Key.Replace(TOKEN_ENTRY_VALUE_SEPARATOR, TOKEN_SKIP + TOKEN_ENTRY_VALUE_SEPARATOR)) // [......
                    .Append(TOKEN_ENTRY_VALUE_SEPARATOR) // [......=
                    .Append(entry.Value.Replace(TOKEN_ENTRY_END, TOKEN_SKIP + TOKEN_ENTRY_END)) // [......=......
                    .Append(TOKEN_ENTRY_END) // [......=......]
                    .Append(TOKEN_ENTRY_SEPARATOR); // [......=......]:
            }

            if (_mapping.Count > 0)
            {
                // If configurations were written, the last separator must be deleted
                builder.Remove(builder.Length - TOKEN_ENTRY_SEPARATOR.Length, TOKEN_ENTRY_SEPARATOR.Length);
            }

            builder.AppendLine(TOKEN_FILE_END);

            return builder.ToString();
        }

        public override void Unpack(string data)
        {
            Step0_findBoundary(data, out int minIndex, out int maxIndex);

            // File is empty or not found
            if (minIndex == -1 || maxIndex == -1)
                return;

            if (maxIndex < minIndex)
                throw new IOException($"Begining {minIndex} and EOD {maxIndex} file are not valid");

            foreach (string entry in Step1_getAllEntries(data, minIndex, maxIndex))
            {
                int splitIndex = FindNextToken(entry, 0, TOKEN_ENTRY_VALUE_SEPARATOR);

                if (splitIndex == -1)
                    continue;

                Add(
                    entry.Substring(0, splitIndex)
                    .Replace(TOKEN_SKIP + TOKEN_ENTRY_VALUE_SEPARATOR, TOKEN_ENTRY_VALUE_SEPARATOR),
                    entry.Substring(splitIndex + 1)
                    .Replace(TOKEN_SKIP + TOKEN_ENTRY_END, TOKEN_ENTRY_END)
                    );
            }

#if DEBUG && false
            foreach (KeyValuePair<string, string> loaded in _mapping)
            {
                Console.WriteLine($"Entry: '{loaded.Key}={loaded.Value}'");
            }
#endif
        }

        internal string[] Step1_getAllEntries(string data, int minIndex, int maxIndex)
        {
            int currentIndex = minIndex;
            List<string> entries = new();

            while (Step11_getNextEntry(data, out int foundBeingIndex, out int foundFinalIndex, currentIndex, maxIndex))
            {
                entries.Add(data.Substring(foundBeingIndex + 1, foundFinalIndex - 1 - foundBeingIndex));
                currentIndex = foundFinalIndex;
            }

            return entries.ToArray();
        }

        // Return true if an entry was found, false otherwise
        private bool Step11_getNextEntry(string data, out int foundBeginIndex, out int foundFinalIndex, int index, int maxIndex)
        {
            //Console.WriteLine($"Step11_getNextEntry::INDEX({index},{maxIndex})");
            foundBeginIndex = FindNextToken(data, index, TOKEN_ENTRY_START, maxIndex);

            if (foundBeginIndex == -1)
            {
                foundFinalIndex = -1;
                return false;
            }

            foundFinalIndex = FindNextToken(data, foundBeginIndex, TOKEN_ENTRY_END, maxIndex);

            return foundFinalIndex != -1;
        }

        private void Step0_findBoundary(string data, out int minIndex, out int maxIndex)
        {
            minIndex = data.IndexOf(TOKEN_FILE_START);
            maxIndex = data.LastIndexOf(TOKEN_FILE_END);
        }

        internal int FindNextToken(string data, int startIndex, string token, int endIndex = -1)
        {
            int found = 0;

            if (endIndex < 0)
                endIndex = data.Length;

            for (int i = startIndex; i < endIndex; i++)
            {
                //Console.WriteLine($"Trying to compare data[{i}] (l: {data.Length}) with token[{found}] (l: {token.Length})");
                if (data[i] == token[found])
                {
                    found++;
                }
                else
                {
                    found = 0;
                }

                if (found == token.Length)
                {
                    if (IsTokenBefore(data, i - token.Length + 1, TOKEN_SKIP))
                    {
                        found = 0;
                    }
                    else
                    {
                        return i - token.Length + 1;
                    }
                }
            }
            return -1;
        }

        internal bool IsTokenBefore(string data, int index, string token)
        {
            int found = token.Length;

            for (int i = index; i > 0; i--)
            {
                if (data[i - 1] == token[found - 1])
                {
                    found--;
                }
                else
                    return false;

                if (found == 0)
                    return true;
            }

            return false;
        }
    }
}

