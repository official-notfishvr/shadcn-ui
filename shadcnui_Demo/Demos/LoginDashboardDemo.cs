using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class LoginDashboardDemo : MonoBehaviour
    {
        private GUIHelper _gui;
        private Rect _windowRect = new Rect(60f, 40f, 1180f, 780f);
        private Vector2 _dashboardScroll;

        private bool _isAuthenticated;
        private bool _rememberMe = true;
        private string _email = "captain@station.local";
        private string _password = "demo";
        private string _workspaceName = "Orbital Control";
        private string _status = "Awaiting authentication";

        private float _fleetReadiness = 0.82f;
        private float _signalIntegrity = 0.94f;
        private float _queueLoad = 0.61f;
        private float _automationCoverage = 0.73f;

        private readonly string[] _activityTimes = { "08:30", "09:10", "10:25", "11:05", "12:40" };
        private readonly string[] _activityTitles =
        {
            "Docking lane recalibrated",
            "Maintenance ticket approved",
            "Relay cluster warmed up",
            "Crew roster synced",
            "Telemetry export completed",
        };
        private readonly string[] _activityStates = { "Completed", "Pending", "Completed", "Completed", "Completed" };

        private void Start()
        {
            _gui = new GUIHelper();
        }

        private void OnDestroy()
        {
            _gui?.Cleanup();
        }

        private void OnGUI()
        {
            _windowRect = GUI.Window(205, _windowRect, DrawWindow, string.Empty);
            _gui?.DrawOverlays();
        }

        private void DrawWindow(int windowId)
        {
            _gui.UpdateGUI(true);
            if (!_gui.BeginGUI())
                return;

            DrawHeader();
            _gui.HorizontalSeparator();

            if (_isAuthenticated)
                DrawDashboard();
            else
                DrawLogin();

            _gui.EndGUI();
            GUI.DragWindow(new Rect(0f, 0f, _windowRect.width, 42f));
        }

        private void DrawHeader()
        {
            _gui.BeginHorizontalGroup();
            _gui.BeginVerticalGroup();
            _gui.Heading("Login To Dashboard");
            _gui.Caption("A compact authentication flow that opens into a control-room style dashboard.");
            _gui.EndVerticalGroup();

            GUILayout.FlexibleSpace();

            if (_isAuthenticated)
            {
                _gui.BeginVerticalGroup(GUILayout.Width(220f));
                _gui.Badge("Authenticated", ControlVariant.Secondary);
                _gui.MutedLabel(_workspaceName);
                _gui.EndVerticalGroup();
            }

            _gui.EndHorizontalGroup();
        }

        private void DrawLogin()
        {
            GUILayout.FlexibleSpace();
            _gui.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();

            _gui.BeginVerticalGroup(GUILayout.Width(420f));
            _gui.Heading("Welcome Back");
            _gui.Caption("Sign in to resume mission coordination, review health checks, and inspect live activity.");
            _gui.AddSpace(16f);

            _email = _gui.Input(_email, "captain@station.local", "Email");
            _password = _gui.Input(_password, "demo", "Password", sz: ControlSize.Default);
            _workspaceName = _gui.Input(_workspaceName, "Orbital Control", "Workspace");
            _rememberMe = _gui.Checkbox("Remember this operator", _rememberMe);

            _gui.AddSpace(12f);

            if (_gui.Button("Sign In", ControlVariant.Default, ControlSize.Large))
                SubmitLogin();

            _gui.AddSpace(8f);
            _gui.MutedLabel(_status);
            _gui.EndVerticalGroup();

            GUILayout.FlexibleSpace();
            _gui.EndHorizontalGroup();
            GUILayout.FlexibleSpace();
        }

        private void DrawDashboard()
        {
            _dashboardScroll = _gui.ScrollView(
                _dashboardScroll,
                () =>
                {
                    DrawDashboardTopBar();
                    _gui.AddSpace(14f);
                    DrawDashboardStats();
                    _gui.AddSpace(14f);
                    DrawDashboardMain();
                },
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true)
            );
        }

        private void DrawDashboardTopBar()
        {
            _gui.BeginHorizontalGroup();
            _gui.BeginVerticalGroup();
            _gui.Heading(_workspaceName);
            _gui.Caption("Operational dashboard with status summaries, tasks, and recent activity.");
            _gui.EndVerticalGroup();

            GUILayout.FlexibleSpace();

            if (_gui.Button("Sync", ControlVariant.Outline, ControlSize.Small))
                _gui.ShowSuccessToast("Sync Complete", "Telemetry and roster status refreshed.");

            if (_gui.Button("Create Alert", ControlVariant.Secondary, ControlSize.Small))
                _gui.ShowWarningToast("New Alert", "A caution marker was posted to the command feed.");

            if (_gui.Button("Log Out", ControlVariant.Ghost, ControlSize.Small))
                Logout();

            _gui.EndHorizontalGroup();
        }

        private void DrawDashboardStats()
        {
            _gui.BeginHorizontalGroup();
            _gui.StatCard("Fleet Readiness", "82%", "12 squadrons green", 250f);
            _gui.StatCard("Signal Integrity", "94%", "Relay network stable", 250f);
            _gui.StatCard("Queue Load", "61%", "14 work items active", 250f);
            _gui.StatCard("Automation", "73%", "3 pipelines degraded", 250f);
            _gui.EndHorizontalGroup();
        }

        private void DrawDashboardMain()
        {
            _gui.BeginHorizontalGroup();

            _gui.BeginVerticalGroup(GUILayout.Width(540f));
            DrawOperationsPanel();
            _gui.AddSpace(12f);
            DrawProgressPanel();
            _gui.EndVerticalGroup();

            _gui.AddSpace(16f);

            _gui.BeginVerticalGroup(GUILayout.Width(500f));
            DrawTasksPanel();
            _gui.AddSpace(12f);
            DrawActivityPanel();
            _gui.EndVerticalGroup();

            _gui.EndHorizontalGroup();
        }

        private void DrawOperationsPanel()
        {
            _gui.Heading("Operations");
            _gui.Caption("Current workspace posture and priority channels.");
            _gui.AddSpace(8f);
            _gui.KeyValueRow("Primary Region", "North Relay");
            _gui.KeyValueRow("Operator", _email);
            _gui.KeyValueRow("Incident Level", "Moderate");
            _gui.KeyValueRow("Auth Mode", _rememberMe ? "Remembered Session" : "Manual Session");
        }

        private void DrawProgressPanel()
        {
            _gui.Heading("System Health");
            _gui.Caption("Snapshot of the main command streams.");
            _gui.AddSpace(8f);
            _gui.LabeledProgress("Fleet Readiness", _fleetReadiness, width: 420f);
            _gui.LabeledProgress("Signal Integrity", _signalIntegrity, width: 420f);
            _gui.LabeledProgress("Queue Load", _queueLoad, width: 420f);
            _gui.LabeledProgress("Automation Coverage", _automationCoverage, width: 420f);
        }

        private void DrawTasksPanel()
        {
            _gui.Heading("Priority Tasks");
            _gui.Caption("Manual checkpoints that still need operator confirmation.");
            _gui.AddSpace(8f);
            DrawTaskRow("Approve outbound cargo window", "High");
            DrawTaskRow("Review overnight telemetry spikes", "Medium");
            DrawTaskRow("Confirm dock 3 maintenance lock", "High");
            DrawTaskRow("Publish roster update", "Low");
        }

        private void DrawTaskRow(string title, string priority)
        {
            _gui.BeginHorizontalGroup();
            _gui.Label(title);
            GUILayout.FlexibleSpace();
            _gui.Badge(priority, priority == "High" ? ControlVariant.Destructive : ControlVariant.Outline);
            _gui.EndHorizontalGroup();
        }

        private void DrawActivityPanel()
        {
            _gui.Heading("Recent Activity");
            _gui.Caption("Latest workspace events after sign-in.");
            _gui.AddSpace(8f);

            for (int i = 0; i < _activityTitles.Length; i++)
            {
                _gui.BeginHorizontalGroup();
                _gui.MutedLabel(_activityTimes[i]);
                _gui.AddSpace(8f);
                _gui.Label(_activityTitles[i]);
                GUILayout.FlexibleSpace();
                _gui.Badge(_activityStates[i], ControlVariant.Secondary);
                _gui.EndHorizontalGroup();
            }
        }

        private void SubmitLogin()
        {
            if (string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_password))
            {
                _status = "Email and password are required.";
                _gui.ShowToast("Sign In Failed", "Enter both email and password before continuing.", ToastVariant.Error);
                return;
            }

            _isAuthenticated = true;
            _status = $"Signed in to {_workspaceName}";
            _dashboardScroll = Vector2.zero;
            _gui.ShowSuccessToast("Welcome Back", $"Connected to {_workspaceName}.");
        }

        private void Logout()
        {
            _isAuthenticated = false;
            _status = "Session cleared";
            _password = string.Empty;
            _dashboardScroll = Vector2.zero;
            _gui.DismissAllToasts();
        }
    }
}
