using Cognex.VisionPro;

namespace TeslaNE42Vision2D.Services.Camera
{
    public interface ICameraService
    {
        bool Status { get; }
        string Name { get; }
        void Initialize();
        void Start();
        void Stop();
        void ClearImageData();
        ICogImage Snap();
        ICogImage Snap(double exposure);
        void Release();
    }
}
