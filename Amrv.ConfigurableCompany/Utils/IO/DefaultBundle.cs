using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Amrv.ConfigurableCompany.Utils.IO
{
    [Obsolete("")]
    public class DefaultBundle : Bundle
    {
        public const string TOKEN_FILE_START = "{";
        public const string TOKEN_FILE_END = "}";

        public const string TOKEN_ENTRY_START = "[";
        public const string TOKEN_ENTRY_END = "]";

        public const string TOKEN_ENTRY_SEPARATOR = ":";
        public const string TOKEN_ENTRY_VALUE_SEPARATOR = "=";

        public string TOKEN_SKIP => "\\";

        private readonly Dictionary<string, string> _data = [];

        public override IEnumerable<string> Keys => _data.Keys;

        public override IEnumerable<string> Values => _data.Values;

        public override int Count => _data.Count;

        public override void Set(string key, string value) => _data[key] = value;

        public override void Add(string key, string value) => _data.Add(key, value);

        public override void Clear() => _data.Clear();

        public override bool Contains(string key) => _data.ContainsKey(key);

        public override string Get(string key, string defaultValue) => _data.GetValueOrDefault(key, defaultValue);

        public override IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _data.GetEnumerator();

        public override void Remove(string key) => _data.Remove(key);

        public override bool TryGet(string key, out string value) => _data.TryGetValue(key, out value);

        public override string Pack()
        {
            StringBuilder builder = new();

            builder.Append(TOKEN_FILE_START);

            foreach (KeyValuePair<string, string> entry in _data)
            {
                builder
                    .Append(TOKEN_ENTRY_START) // [
                    .Append(entry.Key.Replace(TOKEN_ENTRY_VALUE_SEPARATOR, TOKEN_SKIP + TOKEN_ENTRY_VALUE_SEPARATOR)) // [......
                    .Append(TOKEN_ENTRY_VALUE_SEPARATOR) // [......=
                    .Append(entry.Value.Replace(TOKEN_ENTRY_END, TOKEN_SKIP + TOKEN_ENTRY_END)) // [......=......
                    .Append(TOKEN_ENTRY_END) // [......=......]
                    .Append(TOKEN_ENTRY_SEPARATOR); // [......=......]:
            }

            if (_data.Count > 0)
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

                Set(
                    entry[..splitIndex]
                    .Replace(TOKEN_SKIP + TOKEN_ENTRY_VALUE_SEPARATOR, TOKEN_ENTRY_VALUE_SEPARATOR),
                    entry[(splitIndex + 1)..]
                    .Replace(TOKEN_SKIP + TOKEN_ENTRY_END, TOKEN_ENTRY_END)
                    );
            }
        }

        private string[] Step1_getAllEntries(string data, int minIndex, int maxIndex)
        {
            int currentIndex = minIndex;
            List<string> entries = [];

            while (Step11_getNextEntry(data, out int foundBeingIndex, out int foundFinalIndex, currentIndex, maxIndex))
            {
                entries.Add(data.Substring(foundBeingIndex + 1, foundFinalIndex - 1 - foundBeingIndex));
                currentIndex = foundFinalIndex;
            }

            return [.. entries];
        }

        // Return true if an entry was found, false otherwise
        private bool Step11_getNextEntry(string data, out int foundBeginIndex, out int foundFinalIndex, int index, int maxIndex)
        {
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

        private int FindNextToken(string data, int startIndex, string token, int endIndex = -1)
        {
            int found = 0;

            if (endIndex < 0)
                endIndex = data.Length;

            for (int i = startIndex; i < endIndex; i++)
            {
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

        private bool IsTokenBefore(string data, int index, string token)
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
