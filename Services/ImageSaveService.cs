using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using System;
using System.IO;
using TeslaNE42Vision2D.Utils;

namespace TeslaNE42Vision2D.Services
{
    public static class ImageSaveService
    {
        private static readonly object _lock = new object();

        public static string SaveImage(ICogImage image, string okNg, string basePath)
        {
            lock (_lock)
            {
                try
                {
                    string dateFolder = DateTime.Now.ToString("yyyy-MM-dd");
                    string dir = Path.Combine(basePath, dateFolder);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    string fileName = $"{DateTime.Now:yyyyMMdd_HHmmss_fff}_{okNg}.bmp";
                    string filePath = Path.Combine(dir, fileName);

                    CogImageFileTool imageFileTool = new CogImageFileTool();
                    try
                    {
                        imageFileTool.Operator.Open(filePath, CogImageFileModeConstants.Write);
                        imageFileTool.InputImage = image;
                        imageFileTool.Run();
                    }
                    finally
                    {
                        imageFileTool.Dispose();
                    }

                    return filePath;
                }
                catch (Exception ex)
                {
                    LogHelper.Error("保存图片失败", ex);
                    return string.Empty;
                }
            }
        }

        public static void DeleteOldImages(string basePath, int retainDays)
        {
            try
            {
                if (!Directory.Exists(basePath)) return;
                foreach (string dir in Directory.GetDirectories(basePath))
                {
                    DateTime createTime = Directory.GetCreationTime(dir);
                    if ((DateTime.Now - createTime).TotalDays > retainDays)
                        Directory.Delete(dir, true);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("删除旧图片失败", ex);
            }
        }
    }
}
