using Cognex.VisionPro;
using Polly;
using Polly.Retry;
using System;
using System.Threading;
using System.Threading.Tasks;
using TeslaNE42Vision2D.Entity;
using TeslaNE42Vision2D.Utils;

namespace TeslaNE42Vision2D.Services.Camera
{
    public class VisionProCameraService : ICameraService
    {
        private ICogAcqFifo _acqFifo;
        private ICogFrameGrabber _frameGrabber;
        private readonly string _serialNumber;
        private readonly int _cameraIndex;
        private bool _isInitialized = false;
        private readonly object _snapLock = new object();

        private readonly ResiliencePipeline _retryPipeline;
        private const int MaxRetryCount = 99;

        public bool Status => _isInitialized && _acqFifo != null;
        public string Name { get; }

        public VisionProCameraService(int cameraIndex, string name = "Camera")
        {
            _cameraIndex = cameraIndex;
            Name = name;
            _retryPipeline = BuildRetryPipeline();
        }

        public VisionProCameraService(string serialNumber, string name = "Camera")
        {
            _serialNumber = serialNumber;
            Name = name;
            _retryPipeline = BuildRetryPipeline();
        }

        private ResiliencePipeline BuildRetryPipeline()
        {
            return new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = MaxRetryCount,
                    Delay = TimeSpan.FromMilliseconds(200),
                    OnRetry = args =>
                    {
                        LogHelper.Warning($"{Name}: 拍照失败，第 {args.AttemptNumber + 1}/{MaxRetryCount} 次重试，原因: {args.Outcome.Exception?.Message}");
                        return default(ValueTask);
                    },
                    ShouldHandle = new PredicateBuilder().Handle<Exception>()
                })
                .Build();
        }

        public void Initialize()
        {
            try
            {
                CogFrameGrabbers grabbers = new CogFrameGrabbers();
                if (grabbers.Count == 0)
                {
                    LogHelper.Warning($"{Name}: 未找到任何采集卡");
                    return;
                }

                if (!string.IsNullOrEmpty(_serialNumber))
                {
                    foreach (ICogFrameGrabber g in grabbers)
                    {
                        if (g.SerialNumber == _serialNumber)
                        {
                            _frameGrabber = g;
                            break;
                        }
                    }
                }
                else if (_cameraIndex < grabbers.Count)
                {
                    _frameGrabber = grabbers[_cameraIndex];
                }

                if (_frameGrabber == null)
                {
                    LogHelper.Warning($"{Name}: 未找到指定相机");
                    return;
                }

                _acqFifo = _frameGrabber.CreateAcqFifo("Generic GigEVision (Mono)", CogAcqFifoPixelFormatConstants.Format8Grey, 0, true);
                _isInitialized = true;
                LogHelper.Info($"{Name}: 初始化成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"{Name}: 初始化失败", ex);
            }
        }

        public void Start()
        {
            _acqFifo?.Flush();
        }

        public void Stop()
        {
            _acqFifo?.Flush();
        }

        public void ClearImageData()
        {
            _acqFifo?.Flush();
        }

        public ICogImage Snap()
        {
            lock (_snapLock)
            {
                if (_acqFifo == null)
                    throw new InvalidOperationException($"{Name}: 相机未初始化");

                try
                {
                    return _retryPipeline.Execute(() =>
                    {
                        ICogImage image = _acqFifo.Acquire(out int trigNum);
                        if (image == null)
                            throw new InvalidOperationException($"{Name}: 拍照返回空图像");
                        return image;
                    });
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"{Name}: 拍照失败，已重试 {MaxRetryCount} 次", ex);
                    throw;
                }
            }
        }

        public void Release()
        {
            try
            {
                _acqFifo?.Flush();
                _acqFifo = null;
                _isInitialized = false;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"{Name}: 释放失败", ex);
            }
        }

        public ICogImage Snap(double exposure)
        {
            lock (_snapLock)
            {
                if (_acqFifo == null)
                    throw new InvalidOperationException($"{Name}: 相机未初始化");

                try
                {
                    return _retryPipeline.Execute(() =>
                    {
                        _acqFifo.OwnedExposureParams.Exposure = exposure;
                        ICogImage image = _acqFifo.Acquire(out int trigNum);
                        if (image == null)
                            throw new InvalidOperationException($"{Name}: 拍照返回空图像");
                        return image;
                    });
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"{Name}: 拍照失败，已重试 {MaxRetryCount} 次", ex);
                    throw;
                }
            }
        }
    }
}