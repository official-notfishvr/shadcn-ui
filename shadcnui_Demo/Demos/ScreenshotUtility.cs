#if MONO
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class ScreenshotUtility : MonoBehaviour
    {
        private static readonly string[] FullDemoTabs = { "Overview", "Controls", "Inputs", "Display", "Layout", "Data", "Overlay" };
        private static readonly string[] FullDemoOldTabs =
        {
            "Button",
            "Badge",
            "Input",
            "Toggle",
            "Checkbox",
            "Switch",
            "TextArea",
            "Avatar",
            "Card",
            "Progress",
            "Separator",
            "Label",
            "Dialog",
            "Select",
            "DropdownMenu",
            "Popover",
            "Tabs",
            "MenuBar",
            "Chart",
            "Table",
            "Toast",
            "Tooltip",
            "Slider",
            "Layout",
        };

        private GUIHelper _gui;
        private FullDemo _fullDemo;
        private FullDemo_old _fullDemoOld;
        private string[] _tabNames = FullDemoTabs;

        private Rect _windowRect = new(20f, 20f, 420f, 560f);
        private Vector2 _scroll;
        private const float DragHandleHeight = 28f;
        private const float WindowChromePadding = 24f;

        private bool _showWindow = true;
        private bool _hideWhileCapturing = false;
        private bool _appendTimestamp = false;
        private bool _keepGifFrames = false;

        private string _outputFolder = "Screenshots\\FullDemo";
        private string _ffmpegPath = "ffmpeg";
        private int _padding = 8;
        private int _showcaseMargin = 24;
        private int _gifFps = 12;
        private int _holdFrames = 4;
        private float _settleDelay = 0.08f;
        private float _scrollDuration = 1.15f;

        private bool _isCapturing;
        private bool _cancelRequested;
        private int _progressTotal;
        private int _progressCurrent;
        private int _savedCount;
        private string _activeJobLabel = string.Empty;
        private string _status = "Ready";
        private Coroutine _captureRoutine;

        private const string ActiveTabField = "_activeTab";
        private const string WindowRectField = "_windowRect";
        private const string GuiField = "_gui";
        private const string ScrollField = "_scroll";
        private const string LastScrollViewportHeightField = "_lastScrollViewportHeight";
        private const string LastScrollContentHeightField = "_lastScrollContentHeight";
        private const string ShowDialogField = "_showDialog";
        private const string ConfirmDeployField = "_confirmDeploy";
        private const string OldActiveTabField = "currentDemoTab";
        private const string OldWindowRectField = "windowRect";
        private const string OldShowWindowField = "showDemoWindow";
        private const string OldScrollField = "scrollPosition";
        private const string OldGuiField = "guiHelper";
        private const string OldDropdownOpenField = "dropdownOpen";
        private const bool EnableCaptureLogging = true;

        private const string LegacySelectId = "select";
        private const string LocationSelectId = "location_select";
        private const string DropdownId = "full_demo_dropdown";
        private const string MeetingPickerId = "meeting_picker";
        private const string ShipPickerId = "ship_picker";
        private const string RangePickerId = "maintenance_range";

        private sealed class CaptureJob
        {
            public int TabIndex;
            public bool Animated;

            public CaptureJob(int tabIndex, bool animated)
            {
                TabIndex = tabIndex;
                Animated = animated;
            }
        }

        private sealed class CapturePlan
        {
            public float HeroScrollY;
            public string HeroPreviewState;
            public float ScrollTargetY;
            public bool UseMeasuredMaxScroll;
            public string[] PreviewStates;

            public CapturePlan(float heroScrollY, string heroPreviewState, float scrollTargetY, bool useMeasuredMaxScroll, params string[] previewStates)
            {
                HeroScrollY = heroScrollY;
                HeroPreviewState = heroPreviewState ?? string.Empty;
                ScrollTargetY = scrollTargetY;
                UseMeasuredMaxScroll = useMeasuredMaxScroll;
                PreviewStates = previewStates ?? Array.Empty<string>();
            }
        }

        private sealed class FrameCounter
        {
            public int Value;
        }

        private void Start()
        {
            _gui = new GUIHelper();
            EnsureOutputFolder();
            RefreshFullDemo();
        }

        private void Update()
        {
            if (((_fullDemo == null || !_fullDemo) && (_fullDemoOld == null || !_fullDemoOld)) && Time.frameCount % 120 == 0)
                RefreshFullDemo();
        }

        private void OnGUI()
        {
            if (!_showWindow || (_hideWhileCapturing && _isCapturing))
                return;

            _windowRect = GUI.Window(9999, _windowRect, DrawWindow, "Demo Capture");
        }

        private void DrawWindow(int id)
        {
            _gui.UpdateGUI(_showWindow);
            if (!_gui.BeginGUI())
            {
                GUI.DragWindow();
                return;
            }

            _scroll = _gui.ScrollView(
                _scroll,
                () =>
                {
                    DrawTargetSection();
                    _gui.HorizontalSeparator();
                    DrawSettingsSection();
                    _gui.HorizontalSeparator();
                    DrawStatusSection();
                    _gui.HorizontalSeparator();
                    DrawActionsSection();
                },
                GUILayout.Height(GetScrollViewportHeight()),
                GUILayout.ExpandWidth(true)
            );

            _gui.EndGUI();
            GUI.DragWindow(new Rect(0f, 0f, _windowRect.width, DragHandleHeight));
        }

        private float GetScrollViewportHeight()
        {
            return Mathf.Max(160f, _windowRect.height - DragHandleHeight - WindowChromePadding);
        }

        private void DrawTargetSection()
        {
            _gui.Label("Target", ControlVariant.Default);

            if ((_fullDemo == null || !_fullDemo) && (_fullDemoOld == null || !_fullDemoOld))
            {
                _gui.Label("No demo found", ControlVariant.Destructive);
                if (_gui.Button("Refresh", ControlVariant.Outline, ControlSize.Small))
                    RefreshFullDemo();
                return;
            }

            _gui.Label(_fullDemo != null && _fullDemo ? "Attached to FullDemo" : "Attached to FullDemo_old", ControlVariant.Default);
            _gui.MutedLabel($"{GetCurrentTabName()} tab active");

            _gui.BeginHorizontalGroup();
            for (int i = 0; i < Mathf.Min(_tabNames.Length, 8); i++)
            {
                if (_gui.Button(_tabNames[i], GetCurrentTabIndex() == i ? ControlVariant.Default : ControlVariant.Ghost, ControlSize.Small))
                    JumpToTab(i);
            }
            _gui.EndHorizontalGroup();

            if (_tabNames.Length > 8)
                _gui.MutedLabel($"{_tabNames.Length} tabs available");

            if (_gui.Button("Refresh", ControlVariant.Outline, ControlSize.Small))
                RefreshFullDemo();
        }

        private void DrawSettingsSection()
        {
            _gui.Label("Settings", ControlVariant.Default);

            _gui.BeginHorizontalGroup();
            _gui.Label("Folder:", ControlVariant.Muted);
            _outputFolder = GUILayout.TextField(_outputFolder, GUILayout.Width(220));
            _gui.EndHorizontalGroup();

            _gui.BeginHorizontalGroup();
            _gui.Label("ffmpeg:", ControlVariant.Muted);
            _ffmpegPath = GUILayout.TextField(_ffmpegPath, GUILayout.Width(220));
            _gui.EndHorizontalGroup();

            DrawSlider("Padding", ref _padding, 0, 24, "px");
            DrawSlider("Showcase Margin", ref _showcaseMargin, 0, 80, "px");
            DrawSlider("GIF FPS", ref _gifFps, 6, 24, string.Empty);
            DrawSlider("Hold Frames", ref _holdFrames, 2, 12, string.Empty);
            DrawSlider("Settle Delay", ref _settleDelay, 0.02f, 0.25f, "s");
            DrawSlider("Scroll Duration", ref _scrollDuration, 0.35f, 2.25f, "s");

            _hideWhileCapturing = _gui.Checkbox("Hide utility while capturing", _hideWhileCapturing);
            _appendTimestamp = _gui.Checkbox("Append timestamp to filenames", _appendTimestamp);
            _keepGifFrames = _gui.Checkbox("Keep GIF frame folders", _keepGifFrames);
        }

        private void DrawStatusSection()
        {
            _gui.Label("Status", ControlVariant.Default);

            if (_isCapturing)
            {
                _gui.Label($"{_progressCurrent}/{Mathf.Max(1, _progressTotal)} completed", ControlVariant.Default);
                _gui.Progress(_progressCurrent / (float)Mathf.Max(1, _progressTotal), 360f);
                _gui.MutedLabel(string.IsNullOrWhiteSpace(_activeJobLabel) ? "Running capture..." : _activeJobLabel);
            }
            else
            {
                _gui.Label(_status, ControlVariant.Muted);
            }
        }

        private void DrawActionsSection()
        {
            bool hasDemo = (_fullDemo != null && _fullDemo) || (_fullDemoOld != null && _fullDemoOld);
            bool canRun = hasDemo && !_isCapturing;

            if (_isCapturing)
            {
                if (_gui.Button("Stop", ControlVariant.Destructive, ControlSize.Default))
                    StopCapture();
            }
            else
            {
                GUI.enabled = canRun;

                if (_gui.Button("Current PNG", ControlVariant.Secondary, ControlSize.Default))
                    StartJobs(new List<CaptureJob> { new CaptureJob(GetCurrentTabIndex(), false) });

                GUILayout.Space(5f);

                if (_gui.Button("All PNGs", ControlVariant.Default, ControlSize.Default))
                    StartJobs(BuildJobs(false));

                GUILayout.Space(5f);

                if (_gui.Button("Current GIF", ControlVariant.Outline, ControlSize.Default))
                    StartJobs(new List<CaptureJob> { new CaptureJob(GetCurrentTabIndex(), true) });

                GUILayout.Space(5f);

                if (_gui.Button("All GIFs", ControlVariant.Outline, ControlSize.Default))
                    StartJobs(BuildJobs(true));

                GUI.enabled = true;
            }

            GUILayout.Space(6f);

            _gui.BeginHorizontalGroup();
            if (_gui.Button("Open Folder", ControlVariant.Outline, ControlSize.Small))
                OpenOutputFolder();
            GUILayout.FlexibleSpace();
            if (_gui.Button("Close", ControlVariant.Ghost, ControlSize.Small))
                _showWindow = false;
            _gui.EndHorizontalGroup();
        }

        private void DrawSlider(string label, ref int value, int min, int max, string suffix)
        {
            _gui.BeginHorizontalGroup();
            _gui.Label($"{label}: {value}{suffix}", ControlVariant.Muted);
            GUILayout.FlexibleSpace();
            _gui.EndHorizontalGroup();
            value = Mathf.RoundToInt(GUILayout.HorizontalSlider(value, min, max, GUILayout.Width(220)));
        }

        private void DrawSlider(string label, ref float value, float min, float max, string suffix)
        {
            _gui.BeginHorizontalGroup();
            _gui.Label($"{label}: {value:F2}{suffix}", ControlVariant.Muted);
            GUILayout.FlexibleSpace();
            _gui.EndHorizontalGroup();
            value = GUILayout.HorizontalSlider(value, min, max, GUILayout.Width(220));
        }

        private void RefreshFullDemo()
        {
            _fullDemo = UnityEngine.Object.FindFirstObjectByType<FullDemo>();
            _fullDemoOld = _fullDemo == null ? UnityEngine.Object.FindFirstObjectByType<FullDemo_old>() : null;
            _tabNames = _fullDemo != null ? FullDemoTabs : (_fullDemoOld != null ? FullDemoOldTabs : FullDemoTabs);
            _status = _fullDemo != null ? "FullDemo detected" : (_fullDemoOld != null ? "FullDemo_old detected" : "No demo found");
        }

        private void JumpToTab(int tabIndex)
        {
            if (!EnsureFullDemo())
                return;

            PrepareDemoForTab(tabIndex);
            _status = $"Showing {GetTabName(tabIndex)}";
        }

        private void StartJobs(List<CaptureJob> jobs)
        {
            if (_isCapturing)
                return;

            if (!EnsureFullDemo())
                return;

            EnsureOutputFolder();
            _cancelRequested = false;
            _progressCurrent = 0;
            _progressTotal = jobs.Count;
            _savedCount = 0;
            _activeJobLabel = string.Empty;
            _status = jobs.Count == 1 ? "Starting capture..." : $"Starting {jobs.Count} captures...";
            LogCapture($"StartJobs count={jobs.Count} currentTab={GetCurrentTabIndex()} currentTabName={GetCurrentTabName()}");
            _captureRoutine = StartCoroutine(RunJobs(jobs));
        }

        private IEnumerator RunJobs(List<CaptureJob> jobs)
        {
            _isCapturing = true;
            bool originalWindowVisible = _showWindow;

            if (_hideWhileCapturing)
                _showWindow = false;

            try
            {
                for (int i = 0; i < jobs.Count; i++)
                {
                    if (_cancelRequested)
                        break;

                    if (!EnsureFullDemo())
                        break;

                    CaptureJob job = jobs[i];
                    _activeJobLabel = $"{(job.Animated ? "GIF" : "PNG")} - {GetTabName(job.TabIndex)}";

                    if (job.Animated)
                        yield return CaptureGif(job.TabIndex);
                    else
                        yield return CaptureStill(job.TabIndex);

                    _progressCurrent = i + 1;
                }
            }
            finally
            {
                CleanupDemoState();
                _captureRoutine = null;
                _isCapturing = false;
                _activeJobLabel = string.Empty;

                if (_hideWhileCapturing)
                    _showWindow = originalWindowVisible;
            }

            if (_cancelRequested)
                _status = $"Stopped after {_savedCount}/{_progressTotal}";
            else if (_savedCount > 0)
                _status = $"Saved {_savedCount}/{_progressTotal}";
            else if (string.IsNullOrWhiteSpace(_status))
                _status = "Nothing captured";
        }

        private IEnumerator CaptureStill(int tabIndex)
        {
            PrepareDemoForTab(tabIndex);
            yield return ApplyPreviewState(tabIndex, 0f, string.Empty);

            CapturePlan plan = ResolvePlan(GetPlan(tabIndex));

            yield return ApplyPreviewState(tabIndex, plan.HeroScrollY, plan.HeroPreviewState);

            string filePath = BuildOutputPath(tabIndex, false);
            Rect captureRect = GetCaptureRect();
            CaptureRegion(captureRect, filePath);

            if (File.Exists(filePath))
            {
                _savedCount++;
                _status = $"Saved {Path.GetFileName(filePath)}";
            }
            else
            {
                _status = $"Failed to save {Path.GetFileName(filePath)}";
            }
        }

        private IEnumerator CaptureGif(int tabIndex)
        {
            PrepareDemoForTab(tabIndex);
            yield return ApplyPreviewState(tabIndex, 0f, string.Empty);

            string gifPath = BuildOutputPath(tabIndex, true);
            string framesDir = BuildFrameDirectory(tabIndex);
            Rect captureRect = GetCaptureRect();
            CapturePlan plan = ResolvePlan(GetPlan(tabIndex));
            var frameCounter = new FrameCounter();

            RecreateDirectory(framesDir);

            yield return CaptureHoldFrames(tabIndex, captureRect, framesDir, frameCounter, 0f, string.Empty, _holdFrames);

            if (plan.ScrollTargetY > 0f)
                yield return CaptureScrollFrames(tabIndex, captureRect, framesDir, frameCounter, 0f, plan.ScrollTargetY);

            yield return CaptureHoldFrames(tabIndex, captureRect, framesDir, frameCounter, plan.HeroScrollY, plan.HeroPreviewState, _holdFrames);

            for (int i = 0; i < plan.PreviewStates.Length; i++)
            {
                if (_cancelRequested)
                    yield break;

                string previewState = plan.PreviewStates[i];
                if (string.Equals(previewState, plan.HeroPreviewState, StringComparison.Ordinal))
                    continue;

                yield return CaptureHoldFrames(tabIndex, captureRect, framesDir, frameCounter, plan.HeroScrollY, previewState, _holdFrames + 2);
            }

            if (_cancelRequested)
                yield break;

            string finalPreviewState = plan.PreviewStates.Length > 0 ? plan.PreviewStates[plan.PreviewStates.Length - 1] : plan.HeroPreviewState;
            int finalHoldFrames = Mathf.Max(_holdFrames * 3, _gifFps);
            yield return CaptureHoldFrames(tabIndex, captureRect, framesDir, frameCounter, plan.HeroScrollY, finalPreviewState, finalHoldFrames);

            bool gifCreated = TryCreateGif(framesDir, gifPath);
            if (gifCreated)
            {
                _savedCount++;
                _status = $"Saved {Path.GetFileName(gifPath)}";
                if (!_keepGifFrames)
                    DeleteDirectory(framesDir);
            }
            else
            {
                _status = $"Frames saved in {Path.GetFileName(framesDir)}";
            }
        }

        private IEnumerator CaptureScrollFrames(int tabIndex, Rect captureRect, string framesDir, FrameCounter frameCounter, float fromY, float toY)
        {
            int scrollFrames = Mathf.Max(8, Mathf.RoundToInt(_gifFps * Mathf.Max(0.35f, _scrollDuration)));
            for (int i = 0; i < scrollFrames; i++)
            {
                if (_cancelRequested)
                    yield break;

                float t = scrollFrames <= 1 ? 1f : i / (float)(scrollFrames - 1);
                float scrollY = Mathf.Lerp(fromY, toY, t);
                yield return CaptureFrame(tabIndex, captureRect, framesDir, frameCounter, scrollY, string.Empty);
            }
        }

        private IEnumerator CaptureHoldFrames(int tabIndex, Rect captureRect, string framesDir, FrameCounter frameCounter, float scrollY, string previewState, int frameCount)
        {
            for (int i = 0; i < frameCount; i++)
            {
                if (_cancelRequested)
                    yield break;

                yield return CaptureFrame(tabIndex, captureRect, framesDir, frameCounter, scrollY, previewState);
            }
        }

        private IEnumerator CaptureFrame(int tabIndex, Rect captureRect, string framesDir, FrameCounter frameCounter, float scrollY, string previewState)
        {
            yield return ApplyPreviewState(tabIndex, scrollY, previewState);
            string framePath = Path.Combine(framesDir, $"frame_{frameCounter.Value:D4}.png");
            CaptureRegion(captureRect, framePath);
            frameCounter.Value++;
        }

        private IEnumerator ApplyPreviewState(int tabIndex, float scrollY, string previewState)
        {
            SetActiveTab(tabIndex);
            ResizeDemoWindow();
            CloseTransientUi();
            LogCapture($"ApplyPreviewState tab={tabIndex} name={GetTabName(tabIndex)} requestedScroll={scrollY:F2} preview='{previewState ?? string.Empty}' before={DescribeCurrentScrollState()}");
            if (_fullDemo != null && _fullDemo)
                _fullDemo.SetScreenshotPreview(scrollY, previewState);
            else if (_fullDemoOld != null && _fullDemoOld)
                SetFieldValue(_fullDemoOld, OldScrollField, new Vector2(0f, Mathf.Max(0f, scrollY)));

            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(_settleDelay);
            yield return new WaitForEndOfFrame();
            LogCapture($"ApplyPreviewState settled tab={tabIndex} after={DescribeCurrentScrollState()}");
        }

        private void PrepareDemoForTab(int tabIndex)
        {
            SetActiveTab(tabIndex);
            ResizeDemoWindow();
            CleanupDemoState();
            if (_fullDemo != null && _fullDemo)
                _fullDemo.SetScreenshotPreview(0f, string.Empty);
            else if (_fullDemoOld != null && _fullDemoOld)
                SetFieldValue(_fullDemoOld, OldScrollField, Vector2.zero);
        }

        private void CleanupDemoState()
        {
            if ((_fullDemo == null || !_fullDemo) && (_fullDemoOld == null || !_fullDemoOld))
                return;

            CloseTransientUi();
            if (_fullDemo != null && _fullDemo)
                _fullDemo.ClearScreenshotPreview();
        }

        private void CloseTransientUi()
        {
            if ((_fullDemo == null || !_fullDemo) && (_fullDemoOld == null || !_fullDemoOld))
                return;

            if (_fullDemo != null && _fullDemo)
            {
                _fullDemo.ClearScreenshotPreview();
                SetFieldValue(_fullDemo, ShowDialogField, false);
                SetFieldValue(_fullDemo, ConfirmDeployField, false);
            }
            else
            {
                SetFieldValue(_fullDemoOld, OldDropdownOpenField, false);
                SetFieldValue(_fullDemoOld, OldScrollField, Vector2.zero);
                SetFieldValue(_fullDemoOld, OldShowWindowField, true);
            }

            GUIHelper helper = GetFullDemoHelper();
            if (helper == null)
                return;

            InvokeHelper(helper, "CloseDialog");
            InvokeHelper(helper, "ClosePopover");
            InvokeHelper(helper, "DismissAllToasts", false);
            InvokeHelper(helper, "CloseSelect");
            InvokeHelper(helper, "CloseSelect", LegacySelectId);
            InvokeHelper(helper, "CloseSelect", LocationSelectId);
            InvokeHelper(helper, "CloseDropdownMenu", DropdownId);
            InvokeHelper(helper, "CloseDatePicker", MeetingPickerId);
            InvokeHelper(helper, "CloseDatePicker", ShipPickerId);
            InvokeHelper(helper, "CloseDatePicker", RangePickerId);
        }

        private bool EnsureFullDemo()
        {
            if ((_fullDemo != null && _fullDemo) || (_fullDemoOld != null && _fullDemoOld))
                return true;

            RefreshFullDemo();
            if ((_fullDemo == null || !_fullDemo) && (_fullDemoOld == null || !_fullDemoOld))
            {
                _status = "No demo found";
                return false;
            }

            return true;
        }

        private bool EnsureFullDemoSilently()
        {
            if ((_fullDemo != null && _fullDemo) || (_fullDemoOld != null && _fullDemoOld))
                return true;

            _fullDemo = UnityEngine.Object.FindFirstObjectByType<FullDemo>();
            _fullDemoOld = _fullDemo == null ? UnityEngine.Object.FindFirstObjectByType<FullDemo_old>() : null;
            _tabNames = _fullDemo != null ? FullDemoTabs : (_fullDemoOld != null ? FullDemoOldTabs : FullDemoTabs);
            return _fullDemo != null || _fullDemoOld != null;
        }

        private void StopCapture()
        {
            _cancelRequested = true;
            _status = "Stopping...";
        }

        private List<CaptureJob> BuildJobs(bool animated)
        {
            var jobs = new List<CaptureJob>(_tabNames.Length);
            for (int i = 0; i < _tabNames.Length; i++)
                jobs.Add(new CaptureJob(i, animated));
            return jobs;
        }

        private CapturePlan ResolvePlan(CapturePlan plan)
        {
            float measuredMaxScroll = _fullDemo != null && _fullDemo ? Mathf.Max(0f, _fullDemo.GetScreenshotMaxScroll()) : (_fullDemoOld != null && _fullDemoOld ? Mathf.Max(0f, _fullDemoOld.GetScreenshotMaxScroll()) : 0f);
            LogCapture($"ResolvePlan tab={GetCurrentTabIndex()} name={GetCurrentTabName()} measuredMaxScroll={measuredMaxScroll:F2} rawPlan={(plan == null ? "<null>" : $"hero={plan.HeroScrollY:F2},target={plan.ScrollTargetY:F2},useMeasured={plan.UseMeasuredMaxScroll},states={plan.PreviewStates.Length}")}");

            if (plan == null)
            {
                CapturePlan fallbackPlan = measuredMaxScroll > 0f ? new CapturePlan(measuredMaxScroll, string.Empty, measuredMaxScroll, true) : new CapturePlan(0f, string.Empty, 0f, false);
                LogResolvedPlan("null-plan fallback", measuredMaxScroll, fallbackPlan);
                return fallbackPlan;
            }

            if (!plan.UseMeasuredMaxScroll)
            {
                bool needsAutomaticScroll = plan.HeroScrollY <= 0f && plan.ScrollTargetY <= 0f && measuredMaxScroll > 0f;
                if (!needsAutomaticScroll)
                {
                    LogResolvedPlan("explicit plan", measuredMaxScroll, plan);
                    return plan;
                }

                CapturePlan autoPlan = new CapturePlan(measuredMaxScroll, plan.HeroPreviewState, measuredMaxScroll, true, plan.PreviewStates);
                LogResolvedPlan("auto-measured fallback", measuredMaxScroll, autoPlan);
                return autoPlan;
            }

            float resolvedScroll = measuredMaxScroll > 0f ? measuredMaxScroll : Mathf.Max(plan.HeroScrollY, plan.ScrollTargetY);
            CapturePlan resolvedPlan = new CapturePlan(resolvedScroll, plan.HeroPreviewState, resolvedScroll, true, plan.PreviewStates);
            LogResolvedPlan("measured plan", measuredMaxScroll, resolvedPlan);
            return resolvedPlan;
        }

        private CapturePlan GetPlan(int tabIndex)
        {
            if (_fullDemoOld != null && _fullDemoOld)
            {
                return tabIndex switch
                {
                    2 => new CapturePlan(220f, string.Empty, 220f, true),
                    6 => new CapturePlan(320f, string.Empty, 320f, true),
                    12 => new CapturePlan(520f, string.Empty, 520f, true),
                    13 => new CapturePlan(220f, string.Empty, 220f, true),
                    14 => new CapturePlan(220f, string.Empty, 220f, true),
                    15 => new CapturePlan(220f, string.Empty, 220f, true),
                    18 => new CapturePlan(260f, string.Empty, 260f, true),
                    19 => new CapturePlan(420f, string.Empty, 420f, true),
                    20 => new CapturePlan(220f, string.Empty, 220f, true),
                    22 => new CapturePlan(340f, string.Empty, 340f, true),
                    23 => new CapturePlan(420f, string.Empty, 420f, true),
                    _ => new CapturePlan(0f, string.Empty, 0f, false),
                };
            }

            return tabIndex switch
            {
                0 => new CapturePlan(112f, string.Empty, 160f, false),
                1 => new CapturePlan(148f, string.Empty, 220f, false),
                2 => new CapturePlan(360f, "inputs_select", 360f, true, "inputs_select", "inputs_dropdown"),
                3 => new CapturePlan(140f, string.Empty, 220f, false),
                4 => new CapturePlan(520f, string.Empty, 520f, true),
                5 => new CapturePlan(720f, string.Empty, 720f, true),
                6 => new CapturePlan(240f, "overlay_dialog", 240f, true, "overlay_dialog", "overlay_popover", "overlay_toasts"),
                _ => new CapturePlan(0f, string.Empty, 0f, false),
            };
        }

        private string GetCurrentTabName() => GetTabName(GetCurrentTabIndex());

        private string GetTabName(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= _tabNames.Length)
                return "Unknown";

            return _tabNames[tabIndex];
        }

        private int GetCurrentTabIndex()
        {
            if (!EnsureFullDemoSilently())
                return 0;

            object value = _fullDemo != null && _fullDemo ? GetFieldValue(_fullDemo, ActiveTabField) : GetFieldValue(_fullDemoOld, OldActiveTabField);
            return value is int tabIndex ? Mathf.Clamp(tabIndex, 0, _tabNames.Length - 1) : 0;
        }

        private void SetActiveTab(int tabIndex)
        {
            if (_fullDemo != null && _fullDemo)
                SetFieldValue(_fullDemo, ActiveTabField, Mathf.Clamp(tabIndex, 0, _tabNames.Length - 1));
            else if (_fullDemoOld != null && _fullDemoOld)
            {
                SetFieldValue(_fullDemoOld, OldActiveTabField, Mathf.Clamp(tabIndex, 0, _tabNames.Length - 1));
                SetFieldValue(_fullDemoOld, OldShowWindowField, true);
            }
        }

        private void ResizeDemoWindow()
        {
            if (_fullDemo != null && _fullDemo)
                SetFieldValue(_fullDemo, WindowRectField, GetShowcaseRect());
            else if (_fullDemoOld != null && _fullDemoOld)
            {
                SetFieldValue(_fullDemoOld, OldWindowRectField, GetShowcaseRect());
                SetFieldValue(_fullDemoOld, OldShowWindowField, true);
            }
        }

        private Rect GetShowcaseRect()
        {
            float margin = Mathf.Max(0f, _showcaseMargin);
            float width = Mathf.Max(960f, Screen.width - (margin * 2f));
            float height = Mathf.Max(720f, Screen.height - (margin * 2f));
            float x = Mathf.Max(0f, (Screen.width - width) * 0.5f);
            float y = Mathf.Max(0f, (Screen.height - height) * 0.5f);
            return new Rect(x, y, width, height);
        }

        private Rect GetCaptureRect()
        {
            object value = _fullDemo != null && _fullDemo ? GetFieldValue(_fullDemo, WindowRectField) : GetFieldValue(_fullDemoOld, OldWindowRectField);
            Rect rect = value is Rect currentRect ? currentRect : GetShowcaseRect();

            if (_padding <= 0)
                return rect;

            rect.x -= _padding;
            rect.y -= _padding;
            rect.width += _padding * 2f;
            rect.height += _padding * 2f;
            return rect;
        }

        private GUIHelper GetFullDemoHelper()
        {
            object value = _fullDemo != null && _fullDemo ? GetFieldValue(_fullDemo, GuiField) : GetFieldValue(_fullDemoOld, OldGuiField);
            return value as GUIHelper;
        }

        private object GetFieldValue(object instance, string fieldName)
        {
            if (instance == null || string.IsNullOrWhiteSpace(fieldName))
                return null;

            FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return field?.GetValue(instance);
        }

        private void SetFieldValue(object instance, string fieldName, object value)
        {
            if (instance == null || string.IsNullOrWhiteSpace(fieldName))
                return;

            FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            field?.SetValue(instance, value);
        }

        private void InvokeHelper(object instance, string methodName, params object[] args)
        {
            if (instance == null || string.IsNullOrWhiteSpace(methodName))
                return;

            object[] provided = args ?? Array.Empty<object>();
            MethodInfo[] methods = instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo method = methods[i];
                if (!string.Equals(method.Name, methodName, StringComparison.Ordinal))
                    continue;

                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length < provided.Length)
                    continue;

                bool compatible = true;
                for (int j = 0; j < provided.Length; j++)
                {
                    object arg = provided[j];
                    if (arg == null)
                        continue;

                    Type expectedType = parameters[j].ParameterType;
                    if (expectedType.IsInstanceOfType(arg))
                        continue;

                    if (expectedType.IsEnum && arg is int)
                        continue;

                    compatible = false;
                    break;
                }

                if (!compatible)
                    continue;

                for (int j = provided.Length; j < parameters.Length; j++)
                {
                    if (!parameters[j].IsOptional)
                    {
                        compatible = false;
                        break;
                    }
                }

                if (!compatible)
                    continue;

                object[] invokeArgs = new object[parameters.Length];
                for (int j = 0; j < provided.Length; j++)
                    invokeArgs[j] = provided[j];
                for (int j = provided.Length; j < parameters.Length; j++)
                    invokeArgs[j] = Type.Missing;

                method.Invoke(instance, invokeArgs);
                return;
            }
        }

        private string BuildOutputPath(int tabIndex, bool animated)
        {
            string suffix = animated ? ".gif" : ".png";
            string timestamp = _appendTimestamp ? $"_{DateTime.Now:yyyyMMdd_HHmmss}" : string.Empty;
            string prefix = _fullDemo != null && _fullDemo ? "FullDemo" : "FullDemo_old";
            string fileName = $"{prefix}_{tabIndex + 1:D2}_{SanitizeFileName(GetTabName(tabIndex))}{timestamp}{suffix}";
            return Path.Combine(GetOutputFolderPath(), fileName);
        }

        private string BuildFrameDirectory(int tabIndex)
        {
            string timestamp = _appendTimestamp ? $"_{DateTime.Now:yyyyMMdd_HHmmss}" : string.Empty;
            string prefix = _fullDemo != null && _fullDemo ? "FullDemo" : "FullDemo_old";
            string folderName = $"{prefix}_{tabIndex + 1:D2}_{SanitizeFileName(GetTabName(tabIndex))}{timestamp}_frames";
            return Path.Combine(GetOutputFolderPath(), folderName);
        }

        private string GetOutputFolderPath() => Path.Combine(Application.dataPath, "..", _outputFolder);

        private bool TryCreateGif(string framesDir, string gifPath)
        {
            string ffmpegExe = ResolveFfmpegPath();
            if (string.IsNullOrWhiteSpace(ffmpegExe))
            {
                _status = "ffmpeg path is invalid";
                return false;
            }

            string palettePath = Path.Combine(framesDir, "palette.png");
            string inputPattern = Path.Combine(framesDir, "frame_%04d.png");

            bool paletteCreated = RunProcess(ffmpegExe, $"-y -framerate {_gifFps} -i \"{inputPattern}\" -vf \"palettegen=reserve_transparent=0\" \"{palettePath}\"");
            if (!paletteCreated || !File.Exists(palettePath))
                return false;

            bool gifCreated = RunProcess(ffmpegExe, $"-y -framerate {_gifFps} -i \"{inputPattern}\" -i \"{palettePath}\" -lavfi \"paletteuse=dither=bayer:bayer_scale=5\" -loop 0 \"{gifPath}\"");
            return gifCreated && File.Exists(gifPath);
        }

        private string ResolveFfmpegPath()
        {
            if (string.IsNullOrWhiteSpace(_ffmpegPath))
                return null;

            if (File.Exists(_ffmpegPath))
                return _ffmpegPath;

            return string.Equals(_ffmpegPath, "ffmpeg", StringComparison.OrdinalIgnoreCase) ? _ffmpegPath : null;
        }

        private bool RunProcess(string exePath, string arguments)
        {
            try
            {
                var errorOutput = new StringBuilder();

                using var process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                };
                process.ErrorDataReceived += (_, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                        errorOutput.AppendLine(e.Data);
                };

                process.Start();
                process.BeginErrorReadLine();

                if (!process.WaitForExit(120000))
                {
                    _status = "ffmpeg timed out";
                    try
                    {
                        process.Kill();
                    }
                    catch { }

                    UnityEngine.Debug.LogError($"ffmpeg timed out: {exePath} {arguments}");
                    return false;
                }

                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    _status = $"ffmpeg failed ({process.ExitCode})";
                    if (errorOutput.Length > 0)
                        UnityEngine.Debug.LogError(errorOutput.ToString());
                }

                return process.ExitCode == 0;
            }
            catch (Exception ex)
            {
                _status = $"Process failed: {ex.GetType().Name}";
                UnityEngine.Debug.LogException(ex);
                return false;
            }
        }

        private void CaptureRegion(Rect rect, string filePath)
        {
            int x = Mathf.FloorToInt(Mathf.Max(0f, rect.x));
            int y = Mathf.FloorToInt(Mathf.Max(0f, Screen.height - rect.y - rect.height));
            int width = Mathf.FloorToInt(Mathf.Min(rect.width, Screen.width - x));
            int height = Mathf.FloorToInt(Mathf.Min(rect.height, Screen.height - y));

            if (width <= 0 || height <= 0)
                return;

            Texture2D fullTexture = ScreenCapture.CaptureScreenshotAsTexture();
            if (fullTexture == null)
            {
                _status = "Failed to capture screenshot";
                return;
            }

            width = Mathf.Min(width, fullTexture.width - x);
            height = Mathf.Min(height, fullTexture.height - y);
            if (width <= 0 || height <= 0)
            {
                Destroy(fullTexture);
                return;
            }

            Texture2D cropped = new Texture2D(width, height, TextureFormat.RGB24, false);
            cropped.SetPixels(fullTexture.GetPixels(x, y, width, height));
            cropped.Apply();

            File.WriteAllBytes(filePath, cropped.EncodeToPNG());

            Destroy(fullTexture);
            Destroy(cropped);
        }

        private void EnsureOutputFolder() => EnsureDirectory(GetOutputFolderPath());

        private void EnsureDirectory(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && !Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void RecreateDirectory(string path)
        {
            DeleteDirectory(path);
            Directory.CreateDirectory(path);
        }

        private void DeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
            }
        }

        private string SanitizeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "unnamed";

            foreach (char invalid in Path.GetInvalidFileNameChars())
                value = value.Replace(invalid, '_');

            return value.Replace(' ', '_');
        }

        private void OpenOutputFolder()
        {
            string folder = GetOutputFolderPath();
            EnsureDirectory(folder);
            Process.Start(new ProcessStartInfo { FileName = folder, UseShellExecute = true });
        }

        private void LogResolvedPlan(string reason, float measuredMaxScroll, CapturePlan plan)
        {
            LogCapture($"ResolvedPlan reason={reason} measuredMaxScroll={measuredMaxScroll:F2} hero={plan.HeroScrollY:F2} target={plan.ScrollTargetY:F2} useMeasured={plan.UseMeasuredMaxScroll} states={plan.PreviewStates.Length}");
        }

        private void LogCapture(string message)
        {
            if (!EnableCaptureLogging)
                return;

            UnityEngine.Debug.Log($"[ScreenshotUtility] {message}");
        }

        private string DescribeCurrentScrollState()
        {
            if (_fullDemo != null && _fullDemo)
            {
                Vector2 currentScroll = GetFieldValue(_fullDemo, ScrollField) is Vector2 scroll ? scroll : Vector2.zero;
                float contentHeight = GetFieldValue(_fullDemo, LastScrollContentHeightField) is float content ? content : -1f;
                float viewportHeight = GetFieldValue(_fullDemo, LastScrollViewportHeightField) is float viewport ? viewport : -1f;
                float maxScroll = _fullDemo.GetScreenshotMaxScroll();
                return $"FullDemo scroll=({currentScroll.x:F2},{currentScroll.y:F2}) content={contentHeight:F2} viewport={viewportHeight:F2} max={maxScroll:F2}";
            }

            if (_fullDemoOld != null && _fullDemoOld)
            {
                Vector2 currentScroll = GetFieldValue(_fullDemoOld, OldScrollField) is Vector2 scroll ? scroll : Vector2.zero;
                float maxScroll = _fullDemoOld.GetScreenshotMaxScroll();
                return $"FullDemo_old scroll=({currentScroll.x:F2},{currentScroll.y:F2}) max={maxScroll:F2}";
            }

            return "No demo attached";
        }
    }
}
#endif
