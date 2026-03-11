#if MONO
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class ScreenshotUtility : MonoBehaviour
    {
        private GUIHelper _gui;
        private Rect _windowRect = new Rect(20, 20, 400, 600);
        private Vector2 _scrollPos;

        private bool _showWindow = true;
        private bool _hideWhileCapturing = false;
        private bool _openOverlaysBeforeCapture = false;

        private string _outputFolder = "Screenshots";
        private bool _useTimestamp = false;
        private int _padding = 4;
        private float _tabDelay = 0.4f;
        private float _overlayDelay = 0.3f;

        private bool _isCapturing = false;
        private int _currentTab = 0;
        private int _currentTheme = 0;
        private int _capturedCount = 0;
        private int _totalCaptures = 0;
        private string _status = "Ready";
        private float _nextActionTime = 0f;

        private List<DemoInfo> _detectedDemos = new List<DemoInfo>();
        private DemoInfo _activeDemo;
        private List<string> _themes = new List<string>();
        private string _originalTheme;
        private bool _captureAllThemes = false;

        private CaptureMode _captureMode = CaptureMode.WindowOnly;

        private enum CaptureMode
        {
            WindowOnly,
            FullScreen,
        }

        private class DemoInfo
        {
            public MonoBehaviour Instance;
            public string Name;
            public string[] TabNames;
            public string TabField;
            public string IndexField;
            public string WindowRectField;
            public string WindowVisibleField;

            public DemoInfo(MonoBehaviour instance, string name, string[] tabs, string tabField, string indexField, string windowRectField, string windowVisibleField = null)
            {
                Instance = instance;
                Name = name;
                TabNames = tabs;
                TabField = tabField;
                IndexField = indexField;
                WindowRectField = windowRectField;
                WindowVisibleField = windowVisibleField;
            }
        }

        void Start()
        {
            _gui = new GUIHelper();
            EnsureOutputFolder();
            DetectDemos();
        }

        void Update()
        {
            if (!_isCapturing && Time.frameCount % 120 == 0)
                DetectDemos();
        }

        void OnGUI()
        {
            if (!_showWindow || (_hideWhileCapturing && _isCapturing))
                return;

            _windowRect = GUI.Window(9999, _windowRect, DrawWindow, "Screenshot Utility");

            if (_isCapturing && Time.time >= _nextActionTime)
                ProcessCaptureQueue();
        }

        void DrawWindow(int id)
        {
            _gui.UpdateGUI(_showWindow);
            if (!_gui.BeginGUI())
            {
                GUI.DragWindow();
                return;
            }

            _scrollPos = _gui.ScrollView(
                _scrollPos,
                () =>
                {
                    DrawDemoSelector();
                    _gui.HorizontalSeparator();
                    DrawSettings();
                    _gui.HorizontalSeparator();
                    DrawStatus();
                    _gui.HorizontalSeparator();
                    DrawActions();
                },
                GUILayout.ExpandHeight(true)
            );

            _gui.EndGUI();
            GUI.DragWindow();
        }

        void DrawDemoSelector()
        {
            _gui.Label("Target Demo", ControlVariant.Default);

            if (_detectedDemos.Count == 0)
            {
                _gui.Label("No demos detected", ControlVariant.Destructive);
                if (_gui.Button("Scan Again", ControlVariant.Outline, ControlSize.Small))
                    DetectDemos();
                return;
            }

            for (int i = 0; i < _detectedDemos.Count; i++)
            {
                var demo = _detectedDemos[i];
                bool isActive = _activeDemo == demo;

                _gui.BeginHorizontalGroup();
                if (_gui.Button(demo.Name, isActive ? ControlVariant.Default : ControlVariant.Ghost, ControlSize.Small))
                    _activeDemo = demo;

                _gui.Label($"{demo.TabNames?.Length ?? 0} tabs", ControlVariant.Muted);
                _gui.EndHorizontalGroup();
            }

            if (_activeDemo != null)
            {
                _gui.MutedLabel($"Selected: {_activeDemo.Name}");
                if (_activeDemo.TabNames != null && _activeDemo.TabNames.Length > 0)
                {
                    _gui.BeginHorizontalGroup();
                    for (int i = 0; i < Math.Min(_activeDemo.TabNames.Length, 6); i++)
                    {
                        _gui.Badge(_activeDemo.TabNames[i], ControlVariant.Outline, ControlSize.Small);
                    }
                    if (_activeDemo.TabNames.Length > 6)
                        _gui.Label($"+{_activeDemo.TabNames.Length - 6} more", ControlVariant.Muted);
                    _gui.EndHorizontalGroup();
                }
            }
        }

        void DrawSettings()
        {
            _gui.Label("Settings", ControlVariant.Default);

            _gui.BeginHorizontalGroup();
            _gui.Label("Folder:", ControlVariant.Muted);
            _outputFolder = GUILayout.TextField(_outputFolder, GUILayout.Width(180));
            _gui.EndHorizontalGroup();

            _gui.BeginHorizontalGroup();
            _gui.Label("Mode:", ControlVariant.Muted);
            if (_gui.Button("Window", _captureMode == CaptureMode.WindowOnly ? ControlVariant.Default : ControlVariant.Ghost, ControlSize.Small))
                _captureMode = CaptureMode.WindowOnly;
            if (_gui.Button("Screen", _captureMode == CaptureMode.FullScreen ? ControlVariant.Default : ControlVariant.Ghost, ControlSize.Small))
                _captureMode = CaptureMode.FullScreen;
            _gui.EndHorizontalGroup();

            if (_captureMode == CaptureMode.WindowOnly)
            {
                _gui.BeginHorizontalGroup();
                _gui.Label($"Padding: {_padding}px", ControlVariant.Muted);
                GUILayout.FlexibleSpace();
                _gui.EndHorizontalGroup();
                _padding = Mathf.RoundToInt(GUILayout.HorizontalSlider(_padding, 0, 50, GUILayout.Width(200)));
            }

            _gui.BeginHorizontalGroup();
            _gui.Label($"Tab Delay: {_tabDelay:F1}s", ControlVariant.Muted);
            GUILayout.FlexibleSpace();
            _gui.EndHorizontalGroup();
            _tabDelay = GUILayout.HorizontalSlider(_tabDelay, 0.1f, 1.0f, GUILayout.Width(200));

            _gui.BeginHorizontalGroup();
            _gui.Label($"Overlay Delay: {_overlayDelay:F1}s", ControlVariant.Muted);
            GUILayout.FlexibleSpace();
            _gui.EndHorizontalGroup();
            _overlayDelay = GUILayout.HorizontalSlider(_overlayDelay, 0.1f, 1.0f, GUILayout.Width(200));

            _hideWhileCapturing = _gui.Checkbox("Hide utility while capturing", _hideWhileCapturing);
            _openOverlaysBeforeCapture = _gui.Checkbox("Open dialogs/dropdowns before capture", _openOverlaysBeforeCapture);
            _useTimestamp = _gui.Checkbox("Add timestamp to filenames", _useTimestamp);
        }

        void DrawStatus()
        {
            _gui.Label("Status", ControlVariant.Default);

            if (_isCapturing)
            {
                string themeInfo = _captureAllThemes ? $" (Theme {_currentTheme + 1}/{_themes.Count})" : "";
                _gui.Label($"Progress: {_capturedCount}/{_totalCaptures}{themeInfo}", ControlVariant.Default);
                _gui.Progress((float)_capturedCount / Mathf.Max(1, _totalCaptures), 360);

                if (_activeDemo != null && _currentTab < (_activeDemo.TabNames?.Length ?? 0))
                {
                    string themeName = _captureAllThemes && _currentTheme < _themes.Count ? _themes[_currentTheme] : ThemeManager.Instance.CurrentTheme?.Name ?? "Default";
                    _gui.MutedLabel($"Capturing: {_activeDemo.TabNames[_currentTab]} ({themeName})");
                }
            }
            else
            {
                _gui.Label(_status, ControlVariant.Muted);
            }
        }

        void DrawActions()
        {
            bool canCapture = !_isCapturing && _activeDemo != null && (_activeDemo.TabNames?.Length ?? 0) > 0;

            if (_isCapturing)
            {
                if (_gui.Button("Stop Capture", ControlVariant.Destructive, ControlSize.Default))
                    StopCapture();
            }
            else
            {
                GUI.enabled = canCapture;

                if (_gui.Button("Capture Current Tab", ControlVariant.Secondary, ControlSize.Default))
                    CaptureSingle();

                GUILayout.Space(5);

                if (_gui.Button("Capture All Tabs", ControlVariant.Default, ControlSize.Default))
                    StartCapture(false);

                GUILayout.Space(5);

                int themeCount = ThemeManager.Instance.Themes?.Count ?? 0;
                if (_gui.Button($"All Tabs & Themes ({themeCount})", ControlVariant.Default, ControlSize.Default))
                    StartCapture(true);

                GUI.enabled = true;
            }

            GUILayout.Space(5);

            _gui.BeginHorizontalGroup();
            if (_gui.Button("Open Folder", ControlVariant.Outline, ControlSize.Small))
                OpenOutputFolder();
            GUILayout.FlexibleSpace();
            if (_gui.Button("Close", ControlVariant.Ghost, ControlSize.Small))
                _showWindow = false;
            _gui.EndHorizontalGroup();
        }

        void DetectDemos()
        {
            _detectedDemos.Clear();

            DetectDemo<FullDemo>("FullDemo", "_tabs", "_activeTab", "_windowRect");
            DetectDemo<FullDemo_old>("FullDemo_old", "demoTabs", "currentDemoTab", "windowRect", "showDemoWindow");

            if (_activeDemo == null || (_activeDemo.Instance == null && _detectedDemos.Count > 0))
                _activeDemo = _detectedDemos.FirstOrDefault();

            _status = _detectedDemos.Count > 0 ? $"Found {_detectedDemos.Count} demo(s)" : "No demos detected";
        }

        void DetectDemo<T>(string name, string tabField, string indexField, string windowRectField, string windowVisibleField = null) where T : MonoBehaviour
        {
            var instance = FindFirstObjectByType<T>();
            if (instance == null) return;

            string[] tabNames = ExtractTabNames(instance, tabField);
            var demo = new DemoInfo(instance, name, tabNames, tabField, indexField, windowRectField, windowVisibleField);
            _detectedDemos.Add(demo);
        }

        string[] ExtractTabNames(MonoBehaviour instance, string tabFieldName)
        {
            if (instance == null) return Array.Empty<string>();

            FieldInfo field = instance.GetType().GetField(tabFieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null) return Array.Empty<string>();

            object value = field.GetValue(instance);

            if (value is string[] strArray)
                return strArray;

            if (value is Array array)
            {
                var names = new List<string>();
                for (int i = 0; i < array.Length; i++)
                {
                    object item = array.GetValue(i);
                    if (item == null) continue;

                    PropertyInfo nameProp = item.GetType().GetProperty("Name");
                    if (nameProp != null)
                    {
                        object nameValue = nameProp.GetValue(item);
                        if (nameValue != null)
                            names.Add(nameValue.ToString());
                    }
                    else
                    {
                        FieldInfo nameField = item.GetType().GetField("Name");
                        if (nameField != null)
                        {
                            object nameValue = nameField.GetValue(item);
                            if (nameValue != null)
                                names.Add(nameValue.ToString());
                        }
                    }
                }
                return names.ToArray();
            }

            return Array.Empty<string>();
        }

        void StartCapture(bool allThemes)
        {
            if (_activeDemo == null || (_activeDemo.TabNames?.Length ?? 0) == 0)
            {
                _status = "No demo or tabs to capture";
                return;
            }

            EnsureOutputFolder();

            _captureAllThemes = allThemes;
            _currentTab = 0;
            _currentTheme = 0;
            _capturedCount = 0;
            _isCapturing = true;

            if (allThemes)
            {
                _themes = ThemeManager.Instance.Themes?.Keys?.ToList() ?? new List<string>();
                _totalCaptures = (_activeDemo.TabNames?.Length ?? 0) * _themes.Count;
                _originalTheme = ThemeManager.Instance.CurrentTheme?.Name ?? "Dark";
                if (_themes.Count > 0)
                    ThemeManager.Instance.SetTheme(_themes[0]);
            }
            else
            {
                _themes.Clear();
                _totalCaptures = _activeDemo.TabNames?.Length ?? 0;
            }

            _nextActionTime = Time.time + _tabDelay;
            _status = "Starting capture...";

            EnsureDemoWindowVisible();
        }

        void StopCapture()
        {
            _isCapturing = false;
            _status = $"Stopped. Captured {_capturedCount}/{_totalCaptures}";

            if (_captureAllThemes && !string.IsNullOrEmpty(_originalTheme))
                ThemeManager.Instance.SetTheme(_originalTheme);
        }

        void ProcessCaptureQueue()
        {
            if (_activeDemo == null || _activeDemo.Instance == null)
            {
                StopCapture();
                return;
            }

            int tabCount = _activeDemo.TabNames?.Length ?? 0;

            if (_currentTab >= tabCount)
            {
                if (_captureAllThemes && _currentTheme + 1 < _themes.Count)
                {
                    _currentTheme++;
                    _currentTab = 0;
                    ThemeManager.Instance.SetTheme(_themes[_currentTheme]);
                    _nextActionTime = Time.time + _tabDelay;
                    return;
                }

                _isCapturing = false;
                _status = $"Complete! {_capturedCount} screenshots saved";

                if (_captureAllThemes && !string.IsNullOrEmpty(_originalTheme))
                    ThemeManager.Instance.SetTheme(_originalTheme);

                return;
            }

            SetTabIndex(_currentTab);
            StartCoroutine(CaptureWithDelay());
            _nextActionTime = Time.time + _tabDelay + _overlayDelay + 0.3f;
        }

        void SetTabIndex(int index)
        {
            if (_activeDemo?.Instance == null) return;

            FieldInfo field = _activeDemo.Instance.GetType().GetField(_activeDemo.IndexField, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            field?.SetValue(_activeDemo.Instance, index);
        }

        void EnsureDemoWindowVisible()
        {
            if (_activeDemo?.Instance == null || string.IsNullOrEmpty(_activeDemo.WindowVisibleField)) return;

            FieldInfo field = _activeDemo.Instance.GetType().GetField(_activeDemo.WindowVisibleField, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field?.FieldType == typeof(bool))
                field.SetValue(_activeDemo.Instance, true);
        }

        void OpenOverlays()
        {
            if (_activeDemo?.Instance == null || !_openOverlaysBeforeCapture) return;

            SetFieldValue(_activeDemo.Instance, "dropdownOpen", true);
            SetFieldValue(_activeDemo.Instance, "_showDropdown", true);
            SetFieldValue(_activeDemo.Instance, "_showDialog", true);
            SetFieldValue(_activeDemo.Instance, "_showSelect", true);

            InvokeMethod(_activeDemo.Instance, "OpenSelect");
            InvokeMethod(_activeDemo.Instance, "OpenPopover");
            InvokeMethod(_activeDemo.Instance, "OpenDialog", "std_dlg");

            FieldInfo helperField = _activeDemo.Instance.GetType().GetField("guiHelper", BindingFlags.NonPublic | BindingFlags.Instance);
            object helper = helperField?.GetValue(_activeDemo.Instance);

            helperField = _activeDemo.Instance.GetType().GetField("_gui", BindingFlags.NonPublic | BindingFlags.Instance);
            helper = helper ?? helperField?.GetValue(_activeDemo.Instance);

            if (helper != null)
            {
                InvokeMethod(helper, "OpenSelect");
                InvokeMethod(helper, "OpenPopover");
                InvokeMethod(helper, "OpenDialog", "std_dlg");
            }
        }

        void SetFieldValue(object obj, string fieldName, object value)
        {
            if (obj == null) return;
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            field?.SetValue(obj, value);
        }

        void InvokeMethod(object obj, string methodName, params object[] args)
        {
            if (obj == null) return;
            MethodInfo method = obj.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            method?.Invoke(obj, args.Length > 0 ? args : null);
        }

        IEnumerator CaptureWithDelay()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.1f);

            OpenOverlays();

            yield return new WaitForSeconds(_overlayDelay);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.05f);

            string tabName = _currentTab < (_activeDemo.TabNames?.Length ?? 0) ? _activeDemo.TabNames[_currentTab] : "Unknown";
            string themeName = _captureAllThemes && _currentTheme < _themes.Count ? _themes[_currentTheme] : ThemeManager.Instance.CurrentTheme?.Name ?? "Default";

            string fileName = BuildFileName(tabName, themeName, _currentTab + 1);
            string filePath = BuildFilePath(fileName, themeName);

            EnsureDirectory(Path.GetDirectoryName(filePath));

            Rect captureRect = GetCaptureRect();
            if (captureRect.width > 0 && captureRect.height > 0)
            {
                CaptureRegion(captureRect, filePath);
                _capturedCount++;
                _status = $"Captured: {tabName}";
            }
            else
            {
                _status = $"Bad rect for {tabName}: {captureRect}";
            }

            _currentTab++;
            yield break;
        }

        void CaptureSingle()
        {
            if (_activeDemo == null)
            {
                _status = "ERROR: _activeDemo is null";
                return;
            }
            if (_activeDemo.Instance == null)
            {
                _status = "ERROR: _activeDemo.Instance is null";
                return;
            }
            if (_activeDemo.TabNames == null || _activeDemo.TabNames.Length == 0)
            {
                _status = "ERROR: No tabs detected";
                return;
            }

            _status = "Starting capture coroutine...";
            StartCoroutine(CaptureSingleCoroutine());
        }

        IEnumerator CaptureSingleCoroutine()
        {
            EnsureOutputFolder();
            EnsureDemoWindowVisible();

            int currentIndex = GetTabIndex();
            string tabName = currentIndex >= 0 && currentIndex < (_activeDemo.TabNames?.Length ?? 0) ? _activeDemo.TabNames[currentIndex] : "Unknown";
            string themeName = ThemeManager.Instance.CurrentTheme?.Name ?? "Default";

            _status = $"Capturing {tabName}...";

            bool wasVisible = _showWindow;
            if (_hideWhileCapturing)
                _showWindow = false;

            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.1f);

            OpenOverlays();

            yield return new WaitForSeconds(_overlayDelay);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.05f);

            string fileName = BuildFileName(tabName, themeName, currentIndex + 1);
            string filePath = BuildFilePath(fileName, themeName);

            EnsureDirectory(Path.GetDirectoryName(filePath));

            Rect captureRect = GetCaptureRect();

            if (captureRect.width > 0 && captureRect.height > 0)
            {
                CaptureRegion(captureRect, filePath);
                _status = $"Saved: {fileName}";
            }
            else
            {
                _status = $"Bad capture rect: {captureRect}";
            }

            if (_hideWhileCapturing)
                _showWindow = wasVisible;

            yield break;
        }

        int GetTabIndex()
        {
            if (_activeDemo?.Instance == null) return 0;

            FieldInfo field = _activeDemo.Instance.GetType().GetField(_activeDemo.IndexField, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null) return 0;

            object value = field.GetValue(_activeDemo.Instance);
            return value is int i ? i : 0;
        }

        string BuildFileName(string tabName, string themeName, int index)
        {
            string sanitizedTab = SanitizeFileName(tabName);
            string timestamp = _useTimestamp ? $"_{DateTime.Now:yyyyMMdd_HHmmss}" : "";

            if (_captureAllThemes)
                return $"{_activeDemo.Name}_{themeName}_{index:D2}_{sanitizedTab}{timestamp}.png";
            else
                return $"{_activeDemo.Name}_{index:D2}_{sanitizedTab}{timestamp}.png";
        }

        string BuildFilePath(string fileName, string themeName)
        {
            string basePath = Path.Combine(Application.dataPath, "..", _outputFolder);
            return Path.Combine(basePath, fileName);
        }

        Rect GetCaptureRect()
        {
            if (_captureMode == CaptureMode.FullScreen)
                return new Rect(0, 0, Screen.width, Screen.height);

            if (_activeDemo?.Instance == null)
            {
                _status = "ERROR: No demo instance for capture rect";
                return new Rect(0, 0, Screen.width, Screen.height);
            }

            FieldInfo field = _activeDemo.Instance.GetType().GetField(_activeDemo.WindowRectField, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null)
            {
                _status = $"ERROR: Field '{_activeDemo.WindowRectField}' not found on {_activeDemo.Name}";
                return new Rect(0, 0, Screen.width, Screen.height);
            }

            if (field.FieldType != typeof(Rect))
            {
                _status = $"ERROR: Field '{_activeDemo.WindowRectField}' is not a Rect";
                return new Rect(0, 0, Screen.width, Screen.height);
            }

            Rect rect = (Rect)field.GetValue(_activeDemo.Instance);

            if (_padding > 0)
            {
                rect.x -= _padding;
                rect.y -= _padding;
                rect.width += _padding * 2;
                rect.height += _padding * 2;
            }

            return rect;
        }

        void CaptureRegion(Rect rect, string filePath)
        {
            int x = Mathf.FloorToInt(Mathf.Max(0, rect.x));
            int y = Mathf.FloorToInt(Mathf.Max(0, Screen.height - rect.y - rect.height));
            int width = Mathf.FloorToInt(Mathf.Min(rect.width, Screen.width - x));
            int height = Mathf.FloorToInt(Mathf.Min(rect.height, Screen.height - y));

            if (width <= 0 || height <= 0)
                return;

            Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(x, y, width, height), 0, 0);
            screenshot.Apply();

            byte[] bytes = screenshot.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);
            Destroy(screenshot);
        }

        void EnsureOutputFolder()
        {
            string path = Path.Combine(Application.dataPath, "..", _outputFolder);
            EnsureDirectory(path);
        }

        void EnsureDirectory(string path)
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        string SanitizeFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "unnamed";
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Replace(' ', '_');
        }

        void OpenOutputFolder()
        {
            string path = Path.Combine(Application.dataPath, "..", _outputFolder);
            if (Directory.Exists(path))
                System.Diagnostics.Process.Start(path);
            else
                _status = "Folder doesn't exist yet";
        }

        T FindFirstObjectByType<T>() where T : MonoBehaviour
        {
#pragma warning disable CS0618
            return UnityEngine.Object.FindObjectOfType<T>();
#pragma warning restore CS0618
        }
    }
}
#endif
