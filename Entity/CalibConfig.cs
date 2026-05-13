using System.Collections.Generic;

namespace TeslaNE42Vision2D.Entity
{
    public class CalibrationPoint
    {
        public double PixelX { get; set; }
        public double PixelY { get; set; }
        public double PhysicalX { get; set; }
        public double PhysicalY { get; set; }
    }

    public class CalibConfig
    {
        /// <summary>
        ///标定
        /// </summary>
        public string CalibrationToolBlockPath { get; set; }

        /// <summary>
        /// Mark点工具
        /// </summary>
        public string MarkPointToolBlockPath { get; set; }

        public List<CalibrationPoint> Points { get; set; } = new List<CalibrationPoint>();
        // 仿射矩阵6个参数展开：[a, b, c, d, e, f]
        // X_world = a*X_pix + b*Y_pix + c
        // Y_world = d*X_pix + e*Y_pix + f
        public double[] AffineMatrix { get; set; } = new double[6];
        public bool IsCalibrated { get; set; } = false;
    }
}
