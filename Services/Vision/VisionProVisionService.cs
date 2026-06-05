using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using TeslaNE42Vision2D.Services.Camera;
using TeslaNE42Vision2D.Utils;

namespace TeslaNE42Vision2D.Services.Vision
{
    public class VisionProVisionService : IVisionService
    {
        private CogToolBlock _toolBlock;
        private readonly object _lock = new object();
        private readonly string _toolBlockPath;

        public bool IsLoaded => _toolBlock != null;

        public CogToolBlock CogToolBlock => _toolBlock;

        public VisionProVisionService(string toolBlockPath)
        {
            _toolBlockPath = toolBlockPath;
        }

        public void Load(string toolBlockPath = null)
        {
            string path = toolBlockPath ?? _toolBlockPath;
            try
            {
                if (!File.Exists(path))
                {
                    LogHelper.Warning($"ToolBlock文件不存在: {path}");
                    return;
                }

                _toolBlock = CogSerializer.LoadObjectFromFile(path) as CogToolBlock;
                LogHelper.Info($"VisionPro ToolBlock 加载成功: {path}");
            }
            catch (Exception ex)
            {
                LogHelper.Error("加载 VisionPro ToolBlock 失败", ex);
            }
        }

        public void Release()
        {
            try
            {
                _toolBlock?.Dispose();
                _toolBlock = null;
            }
            catch (Exception ex)
            {
                LogHelper.Error("释放 VisionPro ToolBlock 失败", ex);
            }
        }

        public InspectionOutput RunInspection(InspectionInput input)
        {
            if (_toolBlock == null)
            {
                LogHelper.Warning("VisionPro ToolBlock 未加载.");
                throw new InvalidOperationException("VisionPro ToolBlock 未加载.");
            }

            lock (_lock)
            {
                try
                {
                    var config = RunDataService.Instance.AppConfigService.Config.Cameras.FirstOrDefault(x => x.Name == input.Name);
                    _toolBlock.Inputs["ImagePolarity"].Value = input.ImagePolarity;
                    _toolBlock.Inputs["ImageBarcode"].Value = input.ImageBarcode;
                    _toolBlock.Run();

                    InspectionOutput inspectionOutput = new InspectionOutput();
                    inspectionOutput.Name = input.Name;
                    inspectionOutput.OkNg = (int)_toolBlock.Outputs["Result"].Value == 1 ? true : false;
                    inspectionOutput.X = (double[])_toolBlock.Outputs["Center_X"].Value;
                    inspectionOutput.Y = (double[])_toolBlock.Outputs["Center_Y"].Value;
                    inspectionOutput.BarcodeList = ((string[])_toolBlock.Outputs["CodeString"].Value).ToList();
                    inspectionOutput.PolarityList = ((int[])_toolBlock.Outputs["Polarity"].Value).ToList();

                    ICogRecord record = _toolBlock.CreateLastRunRecord();
                    inspectionOutput.CogRecord = VisionProTool.GetRecord(record, config.RecordKey);

                    return inspectionOutput;

                }
                catch (Exception ex)
                {
                    LogHelper.Error("VisionPro 检测失败", ex);
                    throw new Exception("VisionPro 检测失败", ex);
                }
            }
        }
    }

    public class SnapAndInspectionInput
    {
        public string TimeStamp { get; set; }
        public ICameraService CameraService { get; set; }

        public string CameraName { get; set; }
        public int CameraIndex { get; set; }

        public int State { get; set; }
    }

    public class InspectionInput
    {

        public string Name { get; set; }
        public ICogImage ImagePolarity { get; set; }
        public ICogImage ImageBarcode { get; set; }
        public int State { get; set; }
    }

    public class InspectionOutput
    {
        public string Name { get; set; }
        public int Index { get;set; }
        public ICogImage ImagePolarity { get; set; }
        public ICogImage ImageBarcode { get; set; }
        public ICogRecord CogRecord { get; set;  }
        public List<string> BarcodeList { get; set; } = new List<string>();

        /// <summary>
        /// 正极还是负极
        /// </summary>
        public List<int> PolarityList { get; set; } = new List<int>();

        public double[] X { get; set; }
        public double[] Y { get; set; }

        public bool OkNg { get; set; }
    }
}
