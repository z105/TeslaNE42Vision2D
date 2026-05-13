using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using System;
using System.IO;
using System.Linq;
using TeslaNE42Vision2D.Utils;

namespace TeslaNE42Vision2D.Services.Camera
{
    public class MockCameraService : ICameraService
    {
        private readonly string _imageFolder;
        private readonly string[] _imageFiles;
        private int _currentIndex = 0;
        private readonly object _indexLock = new object();

        public bool Status => true;
        public string Name { get; }

        public MockCameraService(string imageFolder, string name = "MockCamera")
        {
            _imageFolder = imageFolder;
            Name = name;
            if (Directory.Exists(imageFolder))
            {
                _imageFiles = Directory.GetFiles(imageFolder, "*.bmp")
                    .Concat(Directory.GetFiles(imageFolder, "*.jpg"))
                    .Concat(Directory.GetFiles(imageFolder, "*.png"))
                    .ToArray();
            }
            else
            {
                _imageFiles = new string[0];
                LogHelper.Warning($"MockCamera: 图片目录不存在 {imageFolder}");
            }
        }

        public void Initialize() { }
        public void Start() { }
        public void Stop() { }
        public void ClearImageData() { }
        public void Release() { }

        public ICogImage Snap()
        {
            if (_imageFiles.Length == 0)
                return CreateBlankImage();

            string filePath;
            lock (_indexLock)
            {
                filePath = _imageFiles[_currentIndex % _imageFiles.Length];
                _currentIndex++;
            }

            CogImageFile file = new CogImageFile();
            file.Open(filePath, CogImageFileModeConstants.Read);
            ICogImage image = file[0];
            file.Close();
            return image;
        }

        private ICogImage CreateBlankImage()
        {
            return new CogImage8Grey(640, 480);
        }

        public ICogImage Snap(double exposure)
        {
            if (_imageFiles.Length == 0)
                return CreateBlankImage();

            string filePath;
            lock (_indexLock)
            {
                filePath = _imageFiles[_currentIndex % _imageFiles.Length];
                _currentIndex++;
            }

            CogImageFile file = new CogImageFile();
            file.Open(filePath, CogImageFileModeConstants.Read);
            ICogImage image = file[0];
            file.Close();
            return image;
        }
    }
}
