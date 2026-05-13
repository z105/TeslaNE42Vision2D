using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;

namespace TeslaNE42Vision2D.Services.Vision
{
    public interface IVisionService
    {
        bool IsLoaded { get; }
        void Load(string toolBlockPath);

        CogToolBlock CogToolBlock { get; }
        InspectionOutput RunInspection(InspectionInput input);
        void Release();
    }
}
