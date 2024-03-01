using System;
using System.IO;

namespace Amrv.ConfigurableCompany.Utils.IO
{
    public static class PathUtils
    {
        public static string FormPath(params string[] paths)
        {
            if (paths == null)
                return null;

            string fullPath = string.Empty;

            for (int i = 0; i < paths.Length; i++)
            {
                fullPath = Path.Combine(fullPath, paths[i]);
            }

            return fullPath;
        }

        public static string TryCreateFile(string path, string filename) => TryCreateFile(Path.Combine(path, filename));
        public static string TryCreateFile(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.Create(path);
            return path;
        }

        public static bool TryCreateFolder(string path)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
