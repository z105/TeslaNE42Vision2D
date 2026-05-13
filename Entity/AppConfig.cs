using System;
using System.Collections.Generic;

namespace TeslaNE42Vision2D.Entity
{
    public class AppConfig
    {
        public string Ip { get; set; } = "192.168.1.10";
        public int Port { get; set; } = 5000;
        public string MachineID { get; set; } = "ABCD";
        public int CameraCount { get; set; } = 2;
        public bool UseMock { get; set; } = true;
        public bool UseMockCamera { get; set; } = true;
        public string MockImageFolder { get; set; } = @"C:\MockImages";
        public string ImageSavePath { get; set; } = @"C:\Images";
        public string CalcToolBlockPath { get; set; } = @"C:\ToolBlock.vpp";
        public string DatabasePath { get; set; } = "Data Source=data.db";
        public string AdminPassword { get; set; } = "1234";
        public int ImageRetainDays { get; set; } = 30;
        public List<CameraInfo> Cameras { get; set; } = new List<CameraInfo>();

    }

    public class CameraInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Sn { get; set; }
        public float ExposurePolarity { get; set; }
        public float ExposureBarcode { get; set; }
        public string ToolBlockPath { get; set; }
        public string Position { get; set; }
        public string VpNameX { get; set; }
        public string VpNameY { get; set; }
        public string RecordKey { get; set; }
    }
}
