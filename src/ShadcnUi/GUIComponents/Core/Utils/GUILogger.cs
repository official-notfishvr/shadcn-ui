using System;
using System.IO;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Utils
{
    public static class GUILogger
    {
        public enum LogLevel
        {
            Trace,
            Debug,
            Info,
            Warning,
            Error,
        }

        private static readonly object _fileLock = new();
        private static string _logFilePath;
        private static LogLevel _minimumLevel = LogLevel.Warning;

        public static void SetLogLevel(LogLevel level) => _minimumLevel = level;

        public static void EnableFileLogging(string filePath = null)
        {
            _logFilePath = string.IsNullOrWhiteSpace(filePath) ? Path.Combine(Application.persistentDataPath, "shadcnui.log") : filePath;

            var directory = Path.GetDirectoryName(_logFilePath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public static void DisableFileLogging() => _logFilePath = null;

        public static void LogTrace(string message, string component = "GUI") => Log(LogLevel.Trace, message, component);

        public static void LogDebug(string message, string component = "GUI") => Log(LogLevel.Debug, message, component);

        public static void LogInfo(string message, string component = "GUI") => Log(LogLevel.Info, message, component);

        public static void LogWarning(string message, string component = "GUI") => Log(LogLevel.Warning, message, component);

        public static void LogError(string message, string component = "GUI") => Log(LogLevel.Error, message, component);

        public static void LogException(Exception exception, string methodName = "", string component = "GUI")
        {
            if (exception == null)
                return;

            var message = string.IsNullOrWhiteSpace(methodName) ? exception.ToString() : $"[{methodName}] {exception}";

            Log(LogLevel.Error, message, component);
        }

        private static void Log(LogLevel level, string message, string component)
        {
            if (level < _minimumLevel)
                return;

            var line = $"[{DateTime.Now:HH:mm:ss}] [{level}] [{component}] {message}";

            switch (level)
            {
                case LogLevel.Warning:
                    Debug.LogWarning(line);
                    break;
                case LogLevel.Error:
                    Debug.LogError(line);
                    break;
                default:
                    Debug.Log(line);
                    break;
            }

            if (string.IsNullOrWhiteSpace(_logFilePath))
                return;

            lock (_fileLock)
            {
                try
                {
                    File.AppendAllText(_logFilePath, line + Environment.NewLine);
                }
                catch { }
            }
        }
    }
}
