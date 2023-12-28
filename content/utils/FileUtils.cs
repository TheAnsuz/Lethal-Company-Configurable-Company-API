using System.IO;
using UnityEngine;

namespace Amrv.ConfigurableCompany.content.utils
{
    public class FileUtils
    {
        private FileUtils() { }

        public const string FILE_SUFFIX = "Config";

        public static readonly string FILE_PATH = Application.persistentDataPath;

        public static string GetCurrentConfigFileName() => GetFullFileName(GameNetworkManager.Instance.currentSaveFileName);

        public static string GetFullFileName(string file) => FILE_PATH + Path.DirectorySeparatorChar + file + FILE_SUFFIX;
    }
}
