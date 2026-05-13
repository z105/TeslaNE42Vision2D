using Cognex.VisionPro;
using Cognex.VisionPro.CalibFix;
using Cognex.VisionPro.ToolBlock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TeslaNE42Vision2D.Entity;

namespace TeslaNE42Vision2D.Services.Calibration
{
    public class NinePointCalibrationVisionProService
    {
        private CogToolBlock findMarkBlock;

        public CogToolBlock FindMarkBlock
        {
            get => findMarkBlock;
            set => findMarkBlock = value;
        }

        private CogCalibNPointToNPointTool calibTool;

        public CogCalibNPointToNPointTool CalibTool
        {
            get => calibTool;
            set => calibTool = value;
        }

        public bool IsCalibrated { get; private set; } = false;

        public List<CalibrationPoint> Points { get; } = new List<CalibrationPoint>();

        public NinePointCalibrationVisionProService()
        {


        }

        public void LoadFromConfig(CalibConfig config)
        {
            // load vp tools
            string toolBlockPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config.MarkPointToolBlockPath);
            findMarkBlock = (CogToolBlock)(CogSerializer.LoadObjectFromFile(toolBlockPath));

            string calibToolBlockPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config.CalibrationToolBlockPath);
            calibTool = (CogCalibNPointToNPointTool)(CogSerializer.LoadObjectFromFile(calibToolBlockPath));

        }

        public bool Calibrate()
        {
            return true;
        }

        public void ClearPoints()
        {

        }

        public void AddPoint(double pixelX, double pixelY, double physicalX, double physicalY)
        {

        }

        /// <summary>
        /// 将像素坐标转换为物理坐标
        /// </summary>
        public (double PhysicalX, double PhysicalY) Transform(double pixelX, double pixelY)
        {
            return (pixelX, pixelY);
        }

        public CalibConfig ToConfig()
        {
            return new CalibConfig();
        }
    }
}
