#define Showcase
#if MONO

#if Showcase
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
using shadcnui.GUIComponents.Data;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class ScreenshotUtility : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect controlRect = new Rect(20, 20, 450, 550);
        private bool showControls = true;
        private bool hideWhileCapturing = true;

        private string outputFolder = "Screenshots";
        private bool useTimestamp = false;
        private bool organizeByTheme = true;
        private int padding = 0;

        private bool isCapturing = false;
        private int currentTabIndex = 0;
        private int currentThemeIndex = 0;
        private bool captureAllThemes = false;
        private List<string> themesToCapture = new List<string>();

        private float tabSwitchDelay = 0.5f;
        private float menuOpenDelay = 0.3f;
        private float nextCaptureTime = 0f;

        private MonoBehaviour activeDemo;
        private string activeDemoName = "None";
        private List<TabInfo> detectedTabs = new List<TabInfo>();

        private string statusMessage = "Ready";
        private int totalCaptures = 0;
        private int capturedCount = 0;
        private Vector2 scrollPosition;
        private Vector2 tabListScroll;
        private bool showTabList = false;

        private enum CaptureMode
        {
            WindowOnly,
            FullScreen,
        }

        private CaptureMode captureMode = CaptureMode.WindowOnly;

        private string originalTheme;

        private class TabInfo
        {
            public string Name;
            public int Index;

            public TabInfo(string name, int index)
            {
                Name = name;
                Index = index;
            }
        }

        void Start()
        {
            guiHelper = new GUIHelper();
            EnsureOutputFolder();
            DetectActiveDemo();
        }

        void Update()
        {
            if (activeDemo == null && Time.frameCount % 60 == 0)
                DetectActiveDemo();
        }

        void OnGUI()
        {
            bool shouldShow = showControls && !(hideWhileCapturing && isCapturing);

            if (shouldShow)
            {
                GUI.skin.horizontalScrollbar = GUIStyle.none;
                GUI.skin.verticalScrollbar = GUIStyle.none;
                controlRect = GUI.Window(200, controlRect, DrawControlWindow, "Screenshot Utility");
            }

            if (isCapturing && Time.time >= nextCaptureTime)
                ProcessCaptureQueue();
        }

        void DrawControlWindow(int windowID)
        {
            guiHelper.UpdateGUI(showControls);
            if (!guiHelper.BeginGUI())
            {
                GUI.DragWindow();
                return;
            }

            scrollPosition = guiHelper.ScrollView(
                scrollPosition,
                () =>
                {
                    guiHelper.BeginVerticalGroup();
                    GUILayout.Space(10);
                    guiHelper.Label("Screenshot Utility", ControlVariant.Default);
                    guiHelper.MutedLabel("Capture demo screenshots automatically");
                    guiHelper.HorizontalSeparator();
                    GUILayout.Space(5);
                    DrawDemoInfo();
                    DrawSettings();
                    DrawCaptureStatus();
                    DrawCaptureButtons();
                    DrawFooter();
                },
                GUILayout.ExpandHeight(true)
            );

            guiHelper.EndGUI();
            GUI.DragWindow();
        }

        private void DrawDemoInfo()
        {
            guiHelper.BeginCard(400);
            guiHelper.CardContent(() =>
            {
                guiHelper.BeginHorizontalGroup();
                guiHelper.Label($"Demo: {activeDemoName}", ControlVariant.Default);
                GUILayout.FlexibleSpace();
                guiHelper.Label($"{detectedTabs.Count} tabs", ControlVariant.Muted);
                guiHelper.EndHorizontalGroup();

                if (activeDemo == null)
                {
                    GUILayout.Space(5);
                    guiHelper.Label("No demo detected", ControlVariant.Destructive);
                }
                else if (detectedTabs.Count > 0)
                {
                    GUILayout.Space(5);
                    if (guiHelper.Button(showTabList ? "Hide Tabs" : "Show Tabs", ControlVariant.Ghost, ControlSize.Small))
                        showTabList = !showTabList;

                    if (showTabList)
                    {
                        GUILayout.Space(5);
                        tabListScroll = GUILayout.BeginScrollView(tabListScroll, GUILayout.Height(100));
                        foreach (var tab in detectedTabs)
                            guiHelper.MutedLabel($"  {tab.Index + 1}. {tab.Name}");
                        GUILayout.EndScrollView();
                    }
                }
            });
            guiHelper.EndCard();

            GUILayout.Space(5);
            if (guiHelper.Button("Refresh Detection", ControlVariant.Outline, ControlSize.Small))
                DetectActiveDemo();
            GUILayout.Space(10);
        }

        private void DrawSettings()
        {
            guiHelper.HorizontalSeparator();
            GUILayout.Space(5);
            guiHelper.Label("Settings", ControlVariant.Default);
            GUILayout.Space(5);

            guiHelper.BeginHorizontalGroup();
            guiHelper.Label("Folder:", ControlVariant.Muted);
            outputFolder = GUILayout.TextField(outputFolder, GUILayout.Width(200));
            guiHelper.EndHorizontalGroup();

            GUILayout.Space(5);

            guiHelper.BeginHorizontalGroup();
            guiHelper.Label("Mode:", ControlVariant.Muted);
            if (guiHelper.Button("Window", captureMode == CaptureMode.WindowOnly ? ControlVariant.Default : ControlVariant.Ghost, ControlSize.Small))
                captureMode = CaptureMode.WindowOnly;
            if (guiHelper.Button("Full Screen", captureMode == CaptureMode.FullScreen ? ControlVariant.Default : ControlVariant.Ghost, ControlSize.Small))
                captureMode = CaptureMode.FullScreen;
            guiHelper.EndHorizontalGroup();

            if (captureMode == CaptureMode.WindowOnly)
            {
                GUILayout.Space(5);
                guiHelper.BeginHorizontalGroup();
                guiHelper.Label($"Padding: {padding}px", ControlVariant.Muted);
                GUILayout.FlexibleSpace();
                guiHelper.EndHorizontalGroup();
                padding = Mathf.RoundToInt(GUILayout.HorizontalSlider(padding, 0, 50, GUILayout.Width(200)));
            }

            GUILayout.Space(5);

            guiHelper.BeginHorizontalGroup();
            guiHelper.Label($"Tab delay: {tabSwitchDelay:F1}s", ControlVariant.Muted);
            GUILayout.FlexibleSpace();
            guiHelper.EndHorizontalGroup();
            tabSwitchDelay = GUILayout.HorizontalSlider(tabSwitchDelay, 0.2f, 2.0f, GUILayout.Width(200));

            guiHelper.BeginHorizontalGroup();
            guiHelper.Label($"Menu delay: {menuOpenDelay:F1}s", ControlVariant.Muted);
            GUILayout.FlexibleSpace();
            guiHelper.EndHorizontalGroup();
            menuOpenDelay = GUILayout.HorizontalSlider(menuOpenDelay, 0.1f, 1.0f, GUILayout.Width(200));

            GUILayout.Space(5);

            guiHelper.BeginHorizontalGroup();
            hideWhileCapturing = guiHelper.Checkbox("Hide while capturing", hideWhileCapturing);
            guiHelper.EndHorizontalGroup();

            guiHelper.BeginHorizontalGroup();
            useTimestamp = guiHelper.Checkbox("Add timestamp to filename", useTimestamp);
            guiHelper.EndHorizontalGroup();

            guiHelper.BeginHorizontalGroup();
            organizeByTheme = guiHelper.Checkbox("Organize by theme folder", organizeByTheme);
            guiHelper.EndHorizontalGroup();

            GUILayout.Space(10);
        }

        private void DrawCaptureStatus()
        {
            guiHelper.HorizontalSeparator();
            GUILayout.Space(5);
            guiHelper.Label("Status", ControlVariant.Default);
            GUILayout.Space(5);

            if (isCapturing)
            {
                string themeInfo = captureAllThemes ? $" (Theme {currentThemeIndex + 1}/{themesToCapture.Count})" : "";
                guiHelper.Label($"Capturing: {capturedCount}/{totalCaptures}{themeInfo}", ControlVariant.Default);
                guiHelper.Progress((float)capturedCount / Mathf.Max(1, totalCaptures), 380);

                GUILayout.Space(5);
                if (currentTabIndex < detectedTabs.Count)
                {
                    string currentTheme = captureAllThemes && currentThemeIndex < themesToCapture.Count ? themesToCapture[currentThemeIndex] : ThemeManager.Instance.CurrentTheme?.Name ?? "Unknown";
                    guiHelper.MutedLabel($"Current: {detectedTabs[currentTabIndex].Name} ({currentTheme})");
                }
            }
            else
            {
                guiHelper.Label(statusMessage, ControlVariant.Muted);
            }

            GUILayout.Space(10);
        }

        private void DrawCaptureButtons()
        {
            guiHelper.HorizontalSeparator();
            GUILayout.Space(5);

            bool canCapture = !isCapturing && activeDemo != null && detectedTabs.Count > 0;

            if (isCapturing)
            {
                if (guiHelper.Button("Stop Capture", ControlVariant.Destructive, ControlSize.Default))
                    StopCaptureSequence();
            }
            else
            {
                GUI.enabled = canCapture;

                if (guiHelper.Button("Capture Current Tab", ControlVariant.Secondary, ControlSize.Default))
                    CaptureSingleTab();

                GUILayout.Space(5);

                if (guiHelper.Button("Capture All Tabs", ControlVariant.Default, ControlSize.Default))
                    StartCaptureSequence(false);

                GUILayout.Space(5);

                int themeCount = ThemeManager.Instance.Themes?.Count ?? 0;
                if (guiHelper.Button($"Capture All Themes ({themeCount})", ControlVariant.Default, ControlSize.Default))
                    StartCaptureSequence(true);

                GUI.enabled = true;
            }

            GUILayout.Space(5);

            if (guiHelper.Button("Open Output Folder", ControlVariant.Outline, ControlSize.Small))
                OpenOutputFolder();

            GUILayout.Space(10);
        }

        private void DrawFooter()
        {
            guiHelper.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();
            if (guiHelper.Button("Close", ControlVariant.Ghost, ControlSize.Small))
                showControls = false;
            guiHelper.EndHorizontalGroup();
            GUILayout.EndVertical();
        }

        private void DetectActiveDemo()
        {
            activeDemo = null;
            detectedTabs.Clear();
            activeDemoName = "None";

            var demoTypes = new (Type type, string name, string tabsField, string currentField)[] { (typeof(FullDemo), "FullDemo", "demoTabs", "currentDemoTab"), (typeof(ComponentShowcase), "ComponentShowcase", "demoTabs", "currentDemoTab") };

            foreach (var (type, name, tabsField, currentField) in demoTypes)
            {
                var demo = FindFirstObjectByType(type) as MonoBehaviour;
                if (demo != null)
                {
                    activeDemo = demo;
                    activeDemoName = name;
                    ExtractTabs(demo, tabsField);
                    statusMessage = $"Found {name} with {detectedTabs.Count} tabs";
                    return;
                }
            }

            statusMessage = "No demo detected";
        }

        private void ExtractTabs(MonoBehaviour demo, string tabsFieldName)
        {
            detectedTabs.Clear();

            var field = demo.GetType().GetField(tabsFieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (field == null)
                return;

            var tabs = field.GetValue(demo) as TabConfig[];
            if (tabs == null)
                return;

            for (int i = 0; i < tabs.Length; i++)
                detectedTabs.Add(new TabInfo(tabs[i].Name, i));
        }

        private void StartCaptureSequence(bool allThemes)
        {
            if (activeDemo == null || detectedTabs.Count == 0)
            {
                statusMessage = "No demo or tabs to capture";
                return;
            }

            EnsureOutputFolder();

            captureAllThemes = allThemes;
            currentTabIndex = 0;
            currentThemeIndex = 0;
            capturedCount = 0;
            isCapturing = true;

            if (allThemes)
            {
                themesToCapture = ThemeManager.Instance.Themes.Keys.ToList();
                totalCaptures = detectedTabs.Count * themesToCapture.Count;
                originalTheme = ThemeManager.Instance.CurrentTheme?.Name ?? "Dark";
                ThemeManager.Instance.SetTheme(themesToCapture[0]);
            }
            else
            {
                themesToCapture.Clear();
                totalCaptures = detectedTabs.Count;
            }

            nextCaptureTime = Time.time + tabSwitchDelay;
            statusMessage = "Starting capture...";
        }

        private void StopCaptureSequence()
        {
            isCapturing = false;
            statusMessage = $"Stopped. Captured {capturedCount}/{totalCaptures}";

            if (captureAllThemes && !string.IsNullOrEmpty(originalTheme))
                ThemeManager.Instance.SetTheme(originalTheme);
        }

        private void ProcessCaptureQueue()
        {
            if (currentTabIndex >= detectedTabs.Count)
            {
                if (captureAllThemes && currentThemeIndex + 1 < themesToCapture.Count)
                {
                    currentThemeIndex++;
                    currentTabIndex = 0;
                    ThemeManager.Instance.SetTheme(themesToCapture[currentThemeIndex]);
                    nextCaptureTime = Time.time + tabSwitchDelay;
                    return;
                }

                isCapturing = false;
                statusMessage = $"Complete! {capturedCount} screenshots saved";

                if (captureAllThemes && !string.IsNullOrEmpty(originalTheme))
                    ThemeManager.Instance.SetTheme(originalTheme);

                return;
            }

            SwitchToTab(currentTabIndex);
            StartCoroutine(CaptureAfterDelay());
            nextCaptureTime = Time.time + tabSwitchDelay + menuOpenDelay + 0.5f;
        }

        private void SwitchToTab(int tabIndex)
        {
            if (activeDemo == null)
                return;

            string fieldName = activeDemoName == "FullDemo" ? "currentDemoTab" : "currentTab";
            var field = activeDemo.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            if (field != null)
                field.SetValue(activeDemo, tabIndex);
        }

        private void OpenAllMenus()
        {
            if (activeDemo == null)
                return;

            var guiHelperField = activeDemo.GetType().GetField("guiHelper", BindingFlags.NonPublic | BindingFlags.Instance);
            object helper = guiHelperField?.GetValue(activeDemo);

            SetFieldValue(activeDemo, "dropdownOpen", true);

            if (helper != null)
            {
                InvokeMethod(helper, "OpenSelect");
                InvokeMethod(helper, "OpenPopover");
                InvokeMethod(helper, "OpenDialog", "std_dlg");
            }
        }

        private void SetFieldValue(object obj, string fieldName, object value)
        {
            var field = obj?.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            field?.SetValue(obj, value);
        }

        private void InvokeMethod(object obj, string methodName, params object[] args)
        {
            var method = obj?.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            method?.Invoke(obj, args.Length > 0 ? args : null);
        }

        private IEnumerator CaptureAfterDelay()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.1f);

            OpenAllMenus();

            yield return new WaitForSeconds(menuOpenDelay);
            yield return new WaitForEndOfFrame();

            string tabName = detectedTabs[currentTabIndex].Name;
            string themeName = captureAllThemes && currentThemeIndex < themesToCapture.Count ? themesToCapture[currentThemeIndex] : ThemeManager.Instance.CurrentTheme?.Name ?? "Default";

            string fileName = BuildFileName(tabName, themeName, currentTabIndex + 1);
            string fullPath = BuildFilePath(fileName, themeName);

            EnsureDirectory(Path.GetDirectoryName(fullPath));

            Rect captureRect = GetCaptureRect();
            if (captureRect.width > 0 && captureRect.height > 0)
            {
                CaptureArea(captureRect, fullPath);
                capturedCount++;
                statusMessage = $"Captured: {tabName}";
            }

            currentTabIndex++;
        }

        private string BuildFileName(string tabName, string themeName, int index)
        {
            string sanitizedTab = SanitizeFileName(tabName);
            string timestamp = useTimestamp ? $"_{DateTime.Now:yyyyMMdd_HHmmss}" : "";

            if (captureAllThemes)
                return $"{activeDemoName}_{themeName}_{index:D2}_{sanitizedTab}{timestamp}.png";
            else
                return $"{activeDemoName}_{index:D2}_{sanitizedTab}{timestamp}.png";
        }

        private string BuildFilePath(string fileName, string themeName)
        {
            string basePath = Path.Combine(Application.dataPath, "..", outputFolder);

            if (captureAllThemes && organizeByTheme)
                return Path.Combine(basePath, themeName, fileName);
            else
                return Path.Combine(basePath, fileName);
        }

        private Rect GetCaptureRect()
        {
            if (captureMode == CaptureMode.FullScreen)
                return new Rect(0, 0, Screen.width, Screen.height);

            var field = activeDemo?.GetType().GetField("windowRect", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (field == null)
                return new Rect(0, 0, Screen.width, Screen.height);

            Rect windowRect = (Rect)field.GetValue(activeDemo);

            if (padding > 0)
            {
                windowRect.x -= padding;
                windowRect.y -= padding;
                windowRect.width += padding * 2;
                windowRect.height += padding * 2;
            }

            return windowRect;
        }

        private void CaptureArea(Rect rect, string filePath)
        {
            int x = Mathf.FloorToInt(rect.x);
            int y = Mathf.FloorToInt(Screen.height - rect.y - rect.height);
            int width = Mathf.FloorToInt(rect.width);
            int height = Mathf.FloorToInt(rect.height);

            x = Mathf.Max(0, x);
            y = Mathf.Max(0, y);
            width = Mathf.Min(width, Screen.width - x);
            height = Mathf.Min(height, Screen.height - y);

            if (width <= 0 || height <= 0)
                return;

            Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(x, y, width, height), 0, 0);
            screenshot.Apply();

            byte[] bytes = screenshot.EncodeToPNG();
            File.WriteAllBytes(filePath, bytes);

            Destroy(screenshot);
        }

        private void CaptureSingleTab()
        {
            if (activeDemo == null || detectedTabs.Count == 0)
            {
                statusMessage = "No demo or tabs detected";
                return;
            }

            StartCoroutine(CaptureSingleTabCoroutine());
        }

        private IEnumerator CaptureSingleTabCoroutine()
        {
            EnsureOutputFolder();

            string fieldName = activeDemoName == "FullDemo" ? "currentDemoTab" : "currentTab";
            var field = activeDemo.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            int tabIndex = field != null ? (int)field.GetValue(activeDemo) : 0;
            string tabName = tabIndex < detectedTabs.Count ? detectedTabs[tabIndex].Name : "Unknown";
            string themeName = ThemeManager.Instance.CurrentTheme?.Name ?? "Default";

            bool wasVisible = showControls;
            if (hideWhileCapturing)
                showControls = false;

            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.1f);

            OpenAllMenus();

            yield return new WaitForSeconds(menuOpenDelay);
            yield return new WaitForEndOfFrame();

            string fileName = BuildFileName(tabName, themeName, tabIndex + 1);
            string fullPath = BuildFilePath(fileName, themeName);

            EnsureDirectory(Path.GetDirectoryName(fullPath));

            Rect captureRect = GetCaptureRect();
            if (captureRect.width > 0 && captureRect.height > 0)
            {
                CaptureArea(captureRect, fullPath);
                statusMessage = $"Captured: {tabName}";
            }
            else
            {
                statusMessage = "Failed to get capture area";
            }

            if (hideWhileCapturing)
                showControls = wasVisible;
        }

        private void EnsureOutputFolder()
        {
            string fullPath = Path.Combine(Application.dataPath, "..", outputFolder);
            EnsureDirectory(fullPath);
        }

        private void EnsureDirectory(string path)
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private string SanitizeFileName(string fileName)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(c, '_');
            return fileName.Replace(' ', '_');
        }

        private void OpenOutputFolder()
        {
            string fullPath = Path.Combine(Application.dataPath, "..", outputFolder);

            if (Directory.Exists(fullPath))
                System.Diagnostics.Process.Start(fullPath);
            else
                statusMessage = "Output folder doesn't exist yet";
        }

        private new UnityEngine.Object FindFirstObjectByType(Type type)
        {
#pragma warning disable CS0618
            return FindObjectOfType(type);
#pragma warning restore CS0618
        }
    }
}
#endif
#endif
