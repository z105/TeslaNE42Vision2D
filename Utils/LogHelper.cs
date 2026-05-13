using Serilog;
using System;

namespace TeslaNE42Vision2D.Utils
{
    public static class LogHelper
    {
        private static ILogger _logger;

        public static void Initialize()
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        }

        public static void Info(string message)
        {
            _logger?.Information(message);
        }

        public static void Warning(string message)
        {
            _logger?.Warning(message);
        }

        public static void Error(string message, Exception ex = null)
        {
            if (ex != null)
                _logger?.Error(ex, message);
            else
                _logger?.Error(message);
        }

        public static void Debug(string message)
        {
            _logger?.Debug(message);
        }

        public static void CloseAndFlush()
        {
            Log.CloseAndFlush();
        }
    }
}
