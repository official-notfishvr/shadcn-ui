#define Showcase
#if MONO

#if Showcase
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using shadcnui.GUIComponents.Core;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class ScreenshotUtility : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect controlRect = new Rect(20, 20, 420, 450);
        private bool showControls = true;

        private string outputFolder = "Screenshots";
        private int screenshotWidth = 1920;
        private int screenshotHeight = 1080;
        private bool isCapturing = false;
        private int currentTabIndex = 0;
        private float captureDelay = 2.0f;
        private float nextCaptureTime = 0f;

        private MonoBehaviour activeDemo;
        private string activeDemoName = "None";
        private List<TabInfo> detectedTabs = new List<TabInfo>();

        private string statusMessage = "Ready to capture screenshots";
        private int totalTabs = 0;
        private int capturedCount = 0;
        private Vector2 scrollPosition;

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

            string fullPath = Path.Combine(Application.dataPath, "..", outputFolder);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            DetectActiveDemo();
        }

        void Update()
        {
            if (activeDemo == null && Time.frameCount % 60 == 0)
            {
                DetectActiveDemo();
            }
        }

        void OnGUI()
        {
            if (showControls)
            {
                // Todo: make the GUI auto do this
                GUI.skin.horizontalScrollbar = GUIStyle.none;
                GUI.skin.verticalScrollbar = GUIStyle.none;

                controlRect = GUI.Window(200, controlRect, DrawControlWindow, "Screenshot Utility");
            }

            if (isCapturing && Time.time >= nextCaptureTime)
            {
                CaptureCurrentTab();
            }
        }

        void DrawControlWindow(int windowID)
        {
            guiHelper.UpdateAnimations(showControls);
            if (guiHelper.BeginAnimatedGUI())
            {
                scrollPosition = guiHelper.DrawScrollView(
                    scrollPosition,
                    () =>
                    {
                        guiHelper.BeginVerticalGroup();
                        GUILayout.Space(10);

                        guiHelper.Label("Screenshot Utility", ControlVariant.Default);
                        guiHelper.MutedLabel("Capture all demo tabs automatically");
                        guiHelper.HorizontalSeparator();
                        GUILayout.Space(10);

                        guiHelper.Label("Active Demo", ControlVariant.Default);
                        GUILayout.Space(5);

                        guiHelper.BeginCard(320);
                        guiHelper.CardContent(() =>
                        {
                            guiHelper.Label($"Demo: {activeDemoName}", ControlVariant.Muted);
                            guiHelper.Label($"Tabs Found: {detectedTabs.Count}", ControlVariant.Muted);

                            if (activeDemo == null)
                            {
                                GUILayout.Space(5);
                                guiHelper.Label("No demo detected!", ControlVariant.Destructive);
                                guiHelper.MutedLabel("Load a demo first (FullDemo, DocsDemo, or ComponentShowcase)");
                            }
                            else if (detectedTabs.Count == 0)
                            {
                                GUILayout.Space(5);
                                guiHelper.Label("No tabs detected!", ControlVariant.Destructive);
                                guiHelper.MutedLabel("This demo may not have a tab system");
                            }
                        });
                        guiHelper.EndCard();

                        GUILayout.Space(5);

                        if (guiHelper.Button("Refresh Demo Detection", ControlVariant.Outline, ControlSize.Small))
                        {
                            DetectActiveDemo();
                        }

                        GUILayout.Space(10);
                        guiHelper.HorizontalSeparator();
                        GUILayout.Space(10);

                        guiHelper.Label("Settings", ControlVariant.Default);
                        GUILayout.Space(5);

                        guiHelper.BeginHorizontalGroup();
                        guiHelper.Label("Output Folder:", ControlVariant.Muted);
                        GUILayout.FlexibleSpace();
                        guiHelper.EndHorizontalGroup();
                        outputFolder = GUILayout.TextField(outputFolder, GUILayout.Width(300));

                        GUILayout.Space(5);

                        guiHelper.BeginHorizontalGroup();
                        guiHelper.Label($"Resolution: {screenshotWidth}x{screenshotHeight}", ControlVariant.Muted);
                        GUILayout.FlexibleSpace();
                        guiHelper.EndHorizontalGroup();

                        guiHelper.BeginHorizontalGroup();
                        if (guiHelper.Button("1920x1080", ControlVariant.Outline, ControlSize.Small))
                        {
                            screenshotWidth = 1920;
                            screenshotHeight = 1080;
                        }
                        if (guiHelper.Button("2560x1440", ControlVariant.Outline, ControlSize.Small))
                        {
                            screenshotWidth = 2560;
                            screenshotHeight = 1440;
                        }
                        if (guiHelper.Button("3840x2160", ControlVariant.Outline, ControlSize.Small))
                        {
                            screenshotWidth = 3840;
                            screenshotHeight = 2160;
                        }
                        guiHelper.EndHorizontalGroup();

                        GUILayout.Space(5);

                        guiHelper.BeginHorizontalGroup();
                        guiHelper.Label($"Delay: {captureDelay:F1}s", ControlVariant.Muted);
                        GUILayout.FlexibleSpace();
                        guiHelper.EndHorizontalGroup();
                        captureDelay = GUILayout.HorizontalSlider(captureDelay, 1.0f, 10.0f, GUILayout.Width(300));

                        GUILayout.Space(10);
                        guiHelper.HorizontalSeparator();
                        GUILayout.Space(10);

                        guiHelper.Label("Status", ControlVariant.Default);
                        GUILayout.Space(5);

                        if (isCapturing)
                        {
                            guiHelper.Label($"Capturing: {capturedCount}/{totalTabs}", ControlVariant.Default);
                            guiHelper.Progress((float)capturedCount / totalTabs, 300);
                            GUILayout.Space(5);
                            if (currentTabIndex < detectedTabs.Count)
                            {
                                guiHelper.MutedLabel($"Current: {detectedTabs[currentTabIndex].Name}");
                            }
                        }
                        else
                        {
                            guiHelper.Label(statusMessage, ControlVariant.Muted);
                        }

                        GUILayout.Space(10);
                        guiHelper.HorizontalSeparator();
                        GUILayout.Space(10);

                        GUI.enabled = !isCapturing && activeDemo != null && detectedTabs.Count > 0;
                        if (guiHelper.Button("Capture All Tabs", ControlVariant.Default, ControlSize.Default))
                        {
                            StartCaptureSequence();
                        }
                        GUI.enabled = true;

                        GUILayout.Space(5);

                        if (isCapturing)
                        {
                            if (guiHelper.Button("Stop Capture", ControlVariant.Destructive, ControlSize.Default))
                            {
                                StopCaptureSequence();
                            }
                        }
                        else
                        {
                            GUI.enabled = activeDemo != null && detectedTabs.Count > 0;
                            if (guiHelper.Button("Capture Current Tab", ControlVariant.Secondary, ControlSize.Default))
                            {
                                CaptureSingleTab();
                            }
                            GUI.enabled = true;
                        }

                        GUILayout.Space(5);

                        if (guiHelper.Button("Open Output Folder", ControlVariant.Outline, ControlSize.Default))
                        {
                            OpenOutputFolder();
                        }

                        GUILayout.Space(10);
                        guiHelper.HorizontalSeparator();

                        guiHelper.BeginHorizontalGroup();
                        GUILayout.FlexibleSpace();
                        if (guiHelper.Button("Close", ControlVariant.Ghost, ControlSize.Small))
                        {
                            showControls = false;
                        }
                        guiHelper.EndHorizontalGroup();

                        GUILayout.EndVertical();
                    },
                    GUILayout.ExpandHeight(true)
                );

                guiHelper.EndAnimatedGUI();
            }
            GUI.DragWindow();
        }

        private void DetectActiveDemo()
        {
            activeDemo = null;
            detectedTabs.Clear();
            activeDemoName = "None";

            var fullDemo = FindFirstObjectByType<FullDemo>();
            if (fullDemo != null)
            {
                activeDemo = fullDemo;
                activeDemoName = "FullDemo";
                ExtractTabs(fullDemo, "demoTabs", "currentDemoTab");
                statusMessage = $"Detected FullDemo with {detectedTabs.Count} tabs";
                Debug.Log(statusMessage);
                return;
            }

            var docsDemo = FindFirstObjectByType<DocsDemo>();
            if (docsDemo != null)
            {
                activeDemo = docsDemo;
                activeDemoName = "DocsDemo";
                ExtractTabs(docsDemo, "docTabs", "currentTab");
                statusMessage = $"Detected DocsDemo with {detectedTabs.Count} tabs";
                Debug.Log(statusMessage);
                return;
            }

            var componentShowcase = FindFirstObjectByType<ComponentShowcase>();
            if (componentShowcase != null)
            {
                activeDemo = componentShowcase;
                activeDemoName = "ComponentShowcase";
                ExtractTabs(componentShowcase, "showcaseTabs", "currentTab");
                statusMessage = $"Detected ComponentShowcase with {detectedTabs.Count} tabs";
                Debug.Log(statusMessage);
                return;
            }

            statusMessage = "No demo detected. Please load a demo first.";
            Debug.LogWarning(statusMessage);
        }

        private void ExtractTabs(MonoBehaviour demo, string tabsFieldName, string currentTabFieldName)
        {
            detectedTabs.Clear();

            var tabsField = demo.GetType().GetField(tabsFieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            if (tabsField != null)
            {
                var tabs = tabsField.GetValue(demo) as Tabs.TabConfig[];
                if (tabs != null)
                {
                    for (int i = 0; i < tabs.Length; i++)
                    {
                        detectedTabs.Add(new TabInfo(tabs[i].Name, i));
                    }
                    Debug.Log($"Extracted {detectedTabs.Count} tabs from {demo.GetType().Name}");
                }
                else
                {
                    Debug.LogWarning($"Tabs field '{tabsFieldName}' is not of type Tabs.TabConfig[]");
                }
            }
            else
            {
                Debug.LogWarning($"Could not find tabs field '{tabsFieldName}' in {demo.GetType().Name}");
            }
        }

        private void StartCaptureSequence()
        {
            if (activeDemo == null || detectedTabs.Count == 0)
            {
                statusMessage = "Error: No demo or tabs detected.";
                return;
            }

            totalTabs = detectedTabs.Count;
            currentTabIndex = 0;
            capturedCount = 0;
            isCapturing = true;
            nextCaptureTime = Time.time + captureDelay;
            statusMessage = "Starting capture sequence...";

            Debug.Log($"Starting screenshot capture of {totalTabs} tabs from {activeDemoName}");
        }

        private void StopCaptureSequence()
        {
            isCapturing = false;
            statusMessage = $"Capture stopped. Captured {capturedCount}/{totalTabs} tabs.";
            Debug.Log(statusMessage);
        }

        private void CaptureCurrentTab()
        {
            if (currentTabIndex >= totalTabs)
            {
                isCapturing = false;
                statusMessage = $"Capture complete! {capturedCount} screenshots saved to {outputFolder}/";
                Debug.Log(statusMessage);
                return;
            }

            nextCaptureTime = Time.time + captureDelay;

            SwitchToTab(currentTabIndex);

            StartCoroutine(CaptureAfterDelay());
        }

        private void SwitchToTab(int tabIndex)
        {
            if (activeDemo == null)
                return;

            string currentTabFieldName = "currentDemoTab";

            if (activeDemoName == "DocsDemo" || activeDemoName == "ComponentShowcase")
            {
                currentTabFieldName = "currentTab";
            }

            var currentTabField = activeDemo.GetType().GetField(currentTabFieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            if (currentTabField != null)
            {
                currentTabField.SetValue(activeDemo, tabIndex);
                Debug.Log($"Switched to tab {tabIndex}: {detectedTabs[tabIndex].Name}");
            }
            else
            {
                Debug.LogWarning($"Could not find field '{currentTabFieldName}' in {activeDemo.GetType().Name}");
            }
        }

        private IEnumerator CaptureAfterDelay()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForEndOfFrame();

            string tabName = detectedTabs[currentTabIndex].Name;
            int displayIndex = currentTabIndex + 1;
            string fileName = $"{activeDemoName}_{displayIndex:D2}_{SanitizeFileName(tabName)}.png";
            string fullPath = Path.Combine(Application.dataPath, "..", outputFolder, fileName);

            Rect windowRect = GetDemoWindowRect();
            if (windowRect.width > 0 && windowRect.height > 0)
            {
                CaptureWindowArea(windowRect, fullPath);
            }
            else
            {
                Debug.LogWarning("Could not get window rect, skipping screenshot");
            }

            capturedCount++;
            statusMessage = $"Captured: {tabName} ({capturedCount}/{totalTabs})";
            Debug.Log($"Screenshot saved: {fullPath}");

            currentTabIndex++;
        }

        private Rect GetDemoWindowRect()
        {
            if (activeDemo == null)
                return new Rect(0, 0, 0, 0);

            string windowRectFieldName = "windowRect";
            if (activeDemoName == "DocsDemo")
            {
                windowRectFieldName = "windowRect";
            }
            else if (activeDemoName == "FullDemo")
            {
                windowRectFieldName = "windowRect";
            }
            else if (activeDemoName == "ComponentShowcase")
            {
                windowRectFieldName = "windowRect";
            }

            var windowRectField = activeDemo.GetType().GetField(windowRectFieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            if (windowRectField != null)
            {
                return (Rect)windowRectField.GetValue(activeDemo);
            }

            return new Rect(0, 0, Screen.width, Screen.height);
        }

        private void CaptureWindowArea(Rect rect, string filePath)
        {
            int x = Mathf.FloorToInt(rect.x);
            int y = Mathf.FloorToInt(Screen.height - rect.y - rect.height);
            int width = Mathf.FloorToInt(rect.width);
            int height = Mathf.FloorToInt(rect.height);

            x = Mathf.Max(0, x);
            y = Mathf.Max(0, y);
            width = Mathf.Min(width, Screen.width - x);
            height = Mathf.Min(height, Screen.height - y);

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
                statusMessage = "Error: No demo or tabs detected.";
                return;
            }

            string currentTabFieldName = activeDemoName == "FullDemo" ? "currentDemoTab" : "currentTab";

            var currentTabField = activeDemo.GetType().GetField(currentTabFieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            int tabIndex = 0;
            if (currentTabField != null)
            {
                tabIndex = (int)currentTabField.GetValue(activeDemo);
            }

            string tabName = "Unknown";
            if (tabIndex >= 0 && tabIndex < detectedTabs.Count)
            {
                tabName = detectedTabs[tabIndex].Name;
            }

            int displayIndex = tabIndex + 1;
            string fileName = $"{activeDemoName}_{displayIndex:D2}_{SanitizeFileName(tabName)}.png";
            string fullPath = Path.Combine(Application.dataPath, "..", outputFolder, fileName);

            Rect windowRect = GetDemoWindowRect();
            if (windowRect.width > 0 && windowRect.height > 0)
            {
                CaptureWindowArea(windowRect, fullPath);
                statusMessage = $"Captured: {tabName}";
                Debug.Log($"Screenshot saved: {fullPath}");
            }
            else
            {
                statusMessage = "Error: Could not get window rect";
                Debug.LogWarning("Could not get window rect for single tab capture");
            }
        }

        private string SanitizeFileName(string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char c in invalidChars)
            {
                fileName = fileName.Replace(c, '_');
            }
            fileName = fileName.Replace(' ', '_');
            return fileName;
        }

        private void OpenOutputFolder()
        {
            string fullPath = Path.Combine(Application.dataPath, "..", outputFolder);

            if (Directory.Exists(fullPath))
            {
                System.Diagnostics.Process.Start(fullPath);
            }
            else
            {
                statusMessage = "Output folder does not exist yet.";
            }
        }
    }
}
#endif
#endif
