using System;
using UnityEngine;

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
        private static bool _enableUnityDebugLog = false;

        public static void SetLogLevel(LogLevel level)
        {
            _minLogLevel = level;
        }

        public static void SetUnityDebugLog(bool enabled)
        {
            _enableUnityDebugLog = enabled;
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

            if (_enableUnityDebugLog)
            {
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
            }
        }
    }
}
