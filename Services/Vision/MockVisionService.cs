using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using System;

namespace TeslaNE42Vision2D.Services.Vision
{
    public class MockVisionService : IVisionService
    {
        private readonly Random _random = new Random();

        public bool IsLoaded => true;

        public CogToolBlock CogToolBlock { get; } = null;

        public void Load(string toolBlockPath) { }

        public bool RunInspection(
            ICogImage image,
            out double pixelX,
            out double pixelY,
            out double angle,
            out string barcode,
            out string polarity)
        {
            pixelX = 320 + (_random.NextDouble() - 0.5) * 10;
            pixelY = 240 + (_random.NextDouble() - 0.5) * 10;
            angle = (_random.NextDouble() - 0.5) * 5;
            barcode = "MOCK_" + _random.Next(10000, 99999).ToString();
            polarity = _random.Next(2) == 0 ? "正极" : "负极";
            return _random.Next(10) > 1; // 90% OK
        }

        public void Release() { }

        public InspectionOutput RunInspection(InspectionInput input)
        {
            return new InspectionOutput();
        }
    }
}
