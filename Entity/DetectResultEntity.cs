using Cognex.VisionPro;
using System.Collections.Generic;
using TeslaNE42Vision2D.Services.Vision;

namespace TeslaNE42Vision2D.Entity
{
    public class DetectResultEntity
    {
        public ICogImage[] Images { get; set; }
        public List<InspectionOutput> InspectionOutputs { get; set; } = new List<InspectionOutput>();
        public bool OkNg { get; set; }
        public double PhysicalX { get; set; }
        public double PhysicalY { get; set; }
        public double PhysicalAngle { get; set; }
        public ulong OkCount { get; set; }
        public ulong NgCount { get; set; }
        public string NgRate => Total == 0 ? "0%" : ((double)NgCount / Total * 100).ToString("F2") + "%";
        public ulong Total => OkCount + NgCount;

    }
}
