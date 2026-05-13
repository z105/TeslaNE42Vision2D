using System;
using System.IO;

namespace TeslaNE42Vision2D.Utils
{
    public static class DiskHelper
    {
        public static double GetAvailableFreeSpaceGB(string path)
        {
            try
            {
                string root = Path.GetPathRoot(Path.GetFullPath(path));
                DriveInfo drive = new DriveInfo(root);
                return Math.Round(drive.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0), 2);
            }
            catch
            {
                return 0;
            }
        }
    }
}
