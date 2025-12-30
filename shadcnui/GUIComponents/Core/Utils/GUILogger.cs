using System;
using System.IO;
using UnityEngine;
#if IL2CPP_MELONLOADER
using MelonLoader;
#endif

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

        private static LogLevel _minLogLevel = LogLevel.Trace;
        private static string _logFilePath;
        private static bool _fileLoggingEnabled = false;
        private static readonly object _fileLock = new object();
        private static int _maxLogFileSizeBytes = 5 * 1024 * 1024;
        private static int _maxLogFileCount = 5;
        private static System.Collections.Generic.Queue<string> _logBuffer = new System.Collections.Generic.Queue<string>();
        private static int _bufferFlushThreshold = 10;

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

        public static void LogTrace(string message, string component = "GUIHelper")
        {
            Log(LogLevel.Trace, message, component);
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

#if IL2CPP_MELONLOADER || IL2CPP_MELONLOADER_PRE157
            switch (level)
            {
                case LogLevel.Trace:
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
                case LogLevel.Trace:
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
                    _logBuffer.Enqueue(message);
                    if (_logBuffer.Count >= _bufferFlushThreshold)
                    {
                        FlushLogBufferInternal();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to write to log file: {ex.Message}");
                }
            }
        }

        public static void FlushLogBuffer()
        {
            lock (_fileLock)
            {
                FlushLogBufferInternal();
            }
        }

        private static void FlushLogBufferInternal()
        {
            if (_logBuffer.Count == 0)
                return;
            try
            {
                string batch = string.Join(Environment.NewLine, _logBuffer);
                RotateLogFilesIfNeeded();
                File.AppendAllText(_logFilePath, batch + Environment.NewLine);
                _logBuffer.Clear();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to flush log buffer: {ex.Message}");
            }
        }

        private static void RotateLogFilesIfNeeded()
        {
            if (!_fileLoggingEnabled || string.IsNullOrEmpty(_logFilePath))
                return;

            FileInfo logFile = new FileInfo(_logFilePath);
            if (!logFile.Exists || logFile.Length < _maxLogFileSizeBytes)
                return;

            for (int i = _maxLogFileCount - 1; i >= 1; i--)
            {
                string oldPath = _logFilePath + "." + i;
                string newPath = _logFilePath + "." + (i + 1);
                if (File.Exists(oldPath))
                {
                    if (i == _maxLogFileCount - 1)
                        File.Delete(oldPath);
                    else
                        File.Move(oldPath, newPath);
                }
            }
            File.Move(_logFilePath, _logFilePath + ".1");
        }
    }
}
