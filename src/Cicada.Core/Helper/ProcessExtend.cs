using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cicada.Core.Helper
{
    public class ProcessExtend
    {
        private static string[] win_run_suffixs = new string[] { "", ".com", ".exe", ".bat"};

        public static bool ExistsOnPath(string fileName)
        {
            return GetFullPath(fileName) != null;
        }

        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32S 
                    || Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.WinCE)
                {
                    foreach (var suffix in win_run_suffixs)
                    {
                        var fullPath = Path.Combine(path, fileName + suffix);
                        if (File.Exists(fullPath))
                            return fullPath;
                    }
                }
                else //maybe unix or macosx
                {
                    var fullPath = Path.Combine(path, fileName);
                    if (File.Exists(fullPath))
                        return fullPath;
                }
            }
            return null;
        }
    }
}
