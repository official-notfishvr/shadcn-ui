using System;
using System.IO;
using UnityEngine;
#if IL2CPP_MELONLOADER
using MelonLoader;
#endif

namespace shadcnui.GUIComponents.Core
{
    public static class GUILogger
    {
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
        }

        private static LogLevel _minLogLevel = LogLevel.Warning;
        private static string _logFilePath;
        private static bool _fileLoggingEnabled = false;
        private static readonly object _fileLock = new object();

        public static void SetLogLevel(LogLevel level)
        {
            _minLogLevel = level;
        }

        public static void EnableFileLogging(string filePath = null)
        {
            if (filePath == null)
            {
                filePath = Path.Combine(Application.persistentDataPath, "logs", "GUILogger.log");
            }

            _logFilePath = filePath;
            string directory = Path.GetDirectoryName(_logFilePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _fileLoggingEnabled = true;
        }

        public static void DisableFileLogging()
        {
            _fileLoggingEnabled = false;
        }

        public static void LogDebug(string message, string component = "GUIHelper")
        {
            Log(LogLevel.Debug, message, component);
        }

        public static void LogInfo(string message, string component = "GUIHelper")
        {
            Log(LogLevel.Info, message, component);
        }

        public static void LogWarning(string message, string component = "GUIHelper")
        {
            Log(LogLevel.Warning, message, component);
        }

        public static void LogError(string message, string component = "GUIHelper")
        {
            Log(LogLevel.Error, message, component);
        }

        public static void LogException(Exception ex, string methodName = "", string component = "GUIHelper")
        {
            string message = $"Exception in {methodName}: {ex.Message}";
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                message += $"\nStack Trace: {ex.StackTrace}";
            }
            Log(LogLevel.Error, message, component);
        }

        private static void Log(LogLevel level, string message, string component)
        {
            if (level < _minLogLevel)
                return;

            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            string levelStr = level.ToString().ToUpper();
            string formattedMessage = $"[{timestamp}] [{levelStr}] [{component}] {message}";

#if IL2CPP_MELONLOADER
            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Info:
                    MelonLogger.Msg(formattedMessage);
                    break;
                case LogLevel.Warning:
                    MelonLogger.Warning(formattedMessage);
                    break;
                case LogLevel.Error:
                    MelonLogger.Error(formattedMessage);
                    break;
            }
#else
            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Info:
                    Debug.Log(formattedMessage);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(formattedMessage);
                    break;
                case LogLevel.Error:
                    Debug.LogError(formattedMessage);
                    break;
            }
#endif

            if (_fileLoggingEnabled && !string.IsNullOrEmpty(_logFilePath))
            {
                WriteToFile(formattedMessage);
            }
        }

        private static void WriteToFile(string message)
        {
            lock (_fileLock)
            {
                try
                {
                    File.AppendAllText(_logFilePath, message + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to write to log file: {ex.Message}");
                }
            }
        }
    }
}
