using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.Utils.IO
{
    public class CCATFile
    {
        public const int SIZE_KEY_ESTIMATE = 60;
        public const int SIZE_VALUE_ESTIMATE = 6;

        public const char TOKEN_VALUE_SEPARATOR = ':';
        public const char TOKEN_DATA_SKIP = '\\';

        public const char TOKEN_NEXT_ENTRY = '\n';

        public const string STRING_VALUE_SEPARATOR = ":";
        public const string STRING_DATA_SKIP = @"\";
        public const string STRING_NEXT_ENTRY = "\n";

        private static string MakeDataSafe(in string data)
        {
            StringBuilder builder = new(data);
            builder.Replace(STRING_DATA_SKIP, STRING_DATA_SKIP + STRING_DATA_SKIP);
            builder.Replace(STRING_VALUE_SEPARATOR, STRING_DATA_SKIP + STRING_VALUE_SEPARATOR);
            builder.Replace(STRING_NEXT_ENTRY, STRING_DATA_SKIP + STRING_NEXT_ENTRY);
            return builder.ToString();
        }

        private static string RevertDataSafe(in string data)
        {
            StringBuilder builder = new(data);
            builder.Replace(STRING_DATA_SKIP + STRING_VALUE_SEPARATOR, STRING_VALUE_SEPARATOR);
            builder.Replace(STRING_DATA_SKIP + STRING_NEXT_ENTRY, STRING_NEXT_ENTRY);
            builder.Replace(STRING_DATA_SKIP + STRING_DATA_SKIP, STRING_DATA_SKIP);
            return builder.ToString();
        }

        private readonly Dictionary<string, bool> _states = new();
        public readonly string File;

        public CCATFile(string filepath)
        {
            File = filepath;
        }

        public bool TryGetState(string name, out bool state) => _states.TryGetValue(name, out state);
        public void SetState(string name, bool state) => _states[name] = state;
        public void RemoveState(string name) => _states.Remove(name);
        public IReadOnlyDictionary<string, bool> States() => _states;

        public void Read()
        {
            if (!System.IO.File.Exists(File))
                return;

            string content = System.IO.File.ReadAllText(File);
            _states.Clear();

            int entryStartIndex = 0;
            bool skip = false;
            for (int i = 0; i < content.Length; i++)
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
                    case TOKEN_NEXT_ENTRY:
                        ReadSingleEntry(content, entryStartIndex, i);
                        entryStartIndex = i + 1;
                        break;
                }
            }
        }

        private void ReadSingleEntry(string content, int startIndex, int endIndex)
        {
            bool skip = false;
            for (int i = startIndex; i < endIndex; i++)
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
                else if (content[i] == TOKEN_VALUE_SEPARATOR)
                {
                    string key = content.Substring(startIndex, i - startIndex);
                    string value = content.Substring(i + 1, endIndex - (i + 1));

                    SetState(RevertDataSafe(key), value.Equals("true"));
                    break;
                }
            }
        }

        public void Write()
        {
            int estimate = SIZE_KEY_ESTIMATE + SIZE_VALUE_ESTIMATE * _states.Count;
            StringBuilder builder = new(estimate);

            foreach (var entry in _states)
            {
                builder.Append(MakeDataSafe(entry.Key))
                    .Append(TOKEN_VALUE_SEPARATOR)
                    .Append(entry.Value.ToString().ToLower())
                    .Append('\n');
            }

            System.IO.File.WriteAllText(File, builder.ToString());
        }
    }
}
