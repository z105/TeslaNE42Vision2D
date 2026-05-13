using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TeslaNE42Vision2D.Entity;
using TeslaNE42Vision2D.Services;
using TeslaNE42Vision2D.Services.Camera;
using TeslaNE42Vision2D.Services.Vision;
using TeslaNE42Vision2D.Utils;
using TeslaNE42Vision2D.Views;

namespace TeslaNE42Vision2D
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SplashForm splash = null;
            List<ICameraService> cameras = null;
            Dictionary<string, IVisionService> visionServices = null;
            ClientDevice device = null;
            AppConfig config = null;

            try
            {
                // 显示加载等待窗口
                splash = new SplashForm();
                splash.Show();
                splash.UpdateStatus("正在初始化日志系统...");
                Application.DoEvents();

                // 初始化日志
                LogHelper.Initialize();
                splash.UpdateProgress(10);

                // 初始化基础服务（加载配置、数据库）
                splash.UpdateStatus("正在加载配置文件...");
                Application.DoEvents();
                RunDataService.Instance.Initialize();

                config = RunDataService.Instance.AppConfigService.Config;
                LogHelper.Info($"系统启动 | MachineID={config.MachineID} | Mock={config.UseMock}");
                splash.UpdateProgress(20);

                // 构建相机列表
                splash.UpdateStatus("正在创建相机服务...");
                Application.DoEvents();
                cameras = new List<ICameraService>();
                foreach (var camera in RunDataService.Instance.AppConfigService.Config.Cameras)
                {
                    if (config.UseMockCamera)
                    {
                        cameras.Add(new MockCameraService(config.MockImageFolder, camera.Name));
                    }
                    else
                    {
                        cameras.Add(new VisionProCameraService(camera.Sn, camera.Name));
                    }
                }
                splash.UpdateProgress(30);

                // 初始化相机
                splash.UpdateStatus("正在初始化相机...");
                Application.DoEvents();
                foreach (var cam in cameras)
                {
                    try
                    {
                        cam.Initialize();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Warning($"相机初始化失败: {ex.Message}");
                    }
                }
                splash.UpdateProgress(50);

                // 构建视觉服务
                splash.UpdateStatus("正在加载视觉工具...");
                Application.DoEvents();
                visionServices = new Dictionary<string, IVisionService>();

                foreach (var camera in RunDataService.Instance.AppConfigService.Config.Cameras)
                {
                    if (config.UseMock)
                    {
                        visionServices.Add(camera.Name, new MockVisionService());
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(camera.Name) || string.IsNullOrEmpty(camera.ToolBlockPath))
                        {
                            throw new Exception("Toolblock 不能为空! ");
                        }
                        else
                        {
                            var visionService = new VisionProVisionService(camera.ToolBlockPath);
                            visionService.Load();
                            visionServices.Add(camera.Name, visionService);
                        }
                    }
                }

                RunDataService.Instance.VisionServices = visionServices;

                splash.UpdateProgress(70);

                // 加载计算ToolBlock
                splash.UpdateStatus("正在加载计算工具...");
                Application.DoEvents();
                RunDataService.Instance.CalcToolBlock = CogSerializer.LoadObjectFromFile(RunDataService.Instance.AppConfigService.Config.CalcToolBlockPath) as CogToolBlock;
                splash.UpdateProgress(80);

                // 创建设备（TCP客户端 + 状态机）
                splash.UpdateStatus("正在创建通信设备...");
                Application.DoEvents();
                device = new ClientDevice(config.Ip, config.Port, config.MachineID);
                device.SetStateMachineServices(cameras, visionServices, RunDataService.Instance.CalibrationService);
                RunDataService.Instance.ClientDevice = device;
                splash.UpdateProgress(90);

                // 后台清理旧图片
                System.Threading.Tasks.Task.Run(() =>
                {
                    try { ImageSaveService.DeleteOldImages(config.ImageSavePath, config.ImageRetainDays); }
                    catch (Exception ex) { LogHelper.Warning($"清理旧图片失败: {ex.Message}"); }
                });

                splash.UpdateStatus("正在连接 PLC...");
                Application.DoEvents();

                // 启动后自动连接 PLC（后台线程）
                System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        LogHelper.Info("正在自动连接 PLC...");
                        bool connected = await device.ConnectAsync();
                        LogHelper.Info(connected ? "PLC 自动连接成功" : "PLC 自动连接失败，将自动重试...");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("PLC 自动连接异常", ex);
                    }
                });

                splash.UpdateProgress(100);
                splash.UpdateStatus("加载完成");
                Application.DoEvents();

                // 短暂延时让用户看到完成状态
                System.Threading.Thread.Sleep(300);

                // 关闭加载窗口
                splash.Close();
                splash.Dispose();
                splash = null;

                // 打开主窗口
                var mainForm = new MainForm();
                mainForm.Cameras = cameras;
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                LogHelper.Error("程序启动失败", ex);

                // 关闭加载窗口（如果还在显示）
                if (splash != null && !splash.IsDisposed)
                {
                    splash.Close();
                    splash.Dispose();
                }

                MessageBox.Show("程序启动失败:\n" + ex.Message, "严重错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                RunDataService.Instance.ClientDevice?.Dispose();
                LogHelper.Info("系统退出");
            }
        }
    }
}
