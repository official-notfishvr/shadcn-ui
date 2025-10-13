using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Controls;
using shadcnui.GUIComponents.Core;
using shadcnui.GUIComponents.Data;
using shadcnui.GUIComponents.Display;
using shadcnui.GUIComponents.Layout;
using UnityEngine;

namespace shadcnui_testing.Menu
{
    public class ComponentShowcase : MonoBehaviour
    {
        private GUIHelper guiHelper;
        private Rect windowRect = new Rect(20, 20, 1600, 750);
        private bool showDemoWindow = false;
        private Vector2 scrollPosition;

        private int currentDemoTab = 0;
        private Tabs.TabConfig[] demoTabs;

        private float masterVolume = 50f;
        private string searchQuery = "";
        private bool showAdvanced = false;

        private bool opt1 = false;
        private bool opt2 = true;
        private bool opt3 = false;

        private bool chk1 = false;
        private bool chk2 = true;

        private bool sw1 = false;
        private bool sw2 = true;

        private float brightness = 75f;

        private bool compactView = false;
        private bool emailNotif = true;
        private bool pushNotif = true;
        private bool smsNotif = false;
        private bool publicProfile = false;
        private bool twoFactor = true;
        private bool dataCollection = true;
        private bool betaFeatures = false;
        private bool debugMode = false;

        void Start()
        {
            guiHelper = new GUIHelper();

            demoTabs = new Tabs.TabConfig[]
            {
                new Tabs.TabConfig("Dashboard", DrawDashboard),
                new Tabs.TabConfig("Components", DrawComponentShowcase),
                new Tabs.TabConfig("Forms", DrawFormsDemo),
                new Tabs.TabConfig("Tables", DrawTablesDemo),
                new Tabs.TabConfig("Settings", DrawSettingsDemo),
                new Tabs.TabConfig("Gallery", DrawGalleryDemo),
            };
        }

        void OnGUI()
        {
            GUI.skin.horizontalScrollbar = GUIStyle.none;
            GUI.skin.verticalScrollbar = GUIStyle.none;

            if (GUI.Button(new Rect(10, 10, 150, 30), "Open Demo Window"))
            {
                showDemoWindow = !showDemoWindow;
            }

            if (showDemoWindow)
            {
                windowRect = GUI.Window(101, windowRect, (GUI.WindowFunction)DrawDemoWindow, "ShadcnUI Component Showcase");
            }
        }

        void DrawDemoWindow(int windowID)
        {
            guiHelper.UpdateAnimations(showDemoWindow);
            if (guiHelper.BeginAnimatedGUI())
            {
                currentDemoTab = guiHelper.DrawTabs(
                    demoTabs.Select(tab => tab.Name).ToArray(),
                    currentDemoTab,
                    () =>
                    {
                        scrollPosition = guiHelper.DrawScrollView(scrollPosition, DrawCurrentTabContent, GUILayout.Height(650));
                    },
                    maxLines: 1,
                    position: Tabs.TabPosition.Top
                );

                guiHelper.EndAnimatedGUI();
            }
            GUI.DragWindow();
        }

        void DrawCurrentTabContent()
        {
            guiHelper.BeginVerticalGroup();
            if (currentDemoTab >= 0 && currentDemoTab < demoTabs.Length)
            {
                demoTabs[currentDemoTab].Content?.Invoke();
            }
            GUILayout.EndVertical();
        }

        void DrawDashboard()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Dashboard Overview", LabelVariant.Default);
            guiHelper.MutedLabel("Real-time analytics and system metrics");
            guiHelper.HorizontalSeparator();

            guiHelper.BeginHorizontalGroup();

            DrawStatCard("Total Users", "12,459", "+8.2%", "green");
            GUILayout.Space(12);
            DrawStatCard("Revenue", "$45,231.89", "+20.1%", "blue");
            GUILayout.Space(12);
            DrawStatCard("Conversions", "3,462", "+5.4%", "cyan");
            GUILayout.Space(12);
            DrawStatCard("Growth Rate", "24.5%", "+12.3%", "magenta");

            guiHelper.EndHorizontalGroup();
            guiHelper.HorizontalSeparator();

            guiHelper.Label("System Status", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.StatusBadge("Server", true);
            GUILayout.Space(12);
            guiHelper.StatusBadge("Database", true);
            GUILayout.Space(12);
            guiHelper.StatusBadge("Cache", false);
            guiHelper.EndHorizontalGroup();

            guiHelper.HorizontalSeparator();

            guiHelper.Label("Quick Actions", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            if (guiHelper.Button("Generate Report", ButtonVariant.Default, ButtonSize.Small))
                Debug.Log("Report generated");
            GUILayout.Space(8);
            if (guiHelper.Button("Export Data", ButtonVariant.Outline, ButtonSize.Small))
                Debug.Log("Data exported");
            GUILayout.Space(8);
            if (guiHelper.Button("Settings", ButtonVariant.Ghost, ButtonSize.Small))
                Debug.Log("Settings opened");
            guiHelper.EndHorizontalGroup();

            guiHelper.EndVerticalGroup();
        }

        void DrawStatCard(string title, string value, string change, string color)
        {
            guiHelper.BeginCard(180, 120);
            guiHelper.CardContent(() =>
            {
                guiHelper.Label(title, LabelVariant.Muted);
                guiHelper.Label(value, LabelVariant.Default);
                guiHelper.BeginHorizontalGroup();
                guiHelper.Badge(change, GetBadgeVariantFromColor(color), BadgeSize.Small);
                guiHelper.EndHorizontalGroup();
            });
            guiHelper.EndCard();
        }

        BadgeVariant GetBadgeVariantFromColor(string color)
        {
            return color switch
            {
                "green" => BadgeVariant.Default,
                "blue" => BadgeVariant.Secondary,
                "cyan" => BadgeVariant.Outline,
                _ => BadgeVariant.Secondary,
            };
        }

        void DrawComponentShowcase()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Component Library", LabelVariant.Default);
            guiHelper.MutedLabel("Explore all available UI components");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Button Variants & Sizes", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Button("Default", ButtonVariant.Default);
            guiHelper.Button("Secondary", ButtonVariant.Secondary);
            guiHelper.Button("Destructive", ButtonVariant.Destructive);
            guiHelper.Button("Outline", ButtonVariant.Outline);
            guiHelper.EndHorizontalGroup();

            guiHelper.BeginHorizontalGroup();
            guiHelper.Button("Large", ButtonVariant.Default, ButtonSize.Large);
            guiHelper.Button("Small", ButtonVariant.Default, ButtonSize.Small);
            guiHelper.Button("Icon", ButtonVariant.Ghost, ButtonSize.Icon);
            guiHelper.EndHorizontalGroup();
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Badges & Status Indicators", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Badge("Default");
            guiHelper.Badge("Secondary", BadgeVariant.Secondary);
            guiHelper.Badge("Destructive", BadgeVariant.Destructive);
            guiHelper.Badge("Outline", BadgeVariant.Outline);
            guiHelper.CountBadge(99, maxCount: 99);
            guiHelper.EndHorizontalGroup();
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Progress Indicators", LabelVariant.Default);
            guiHelper.LabeledProgress("Upload", 0.65f, 400, showPercentage: true);
            guiHelper.LabeledProgress("Download", 0.35f, 400, showPercentage: true);
            guiHelper.BeginHorizontalGroup();
            guiHelper.CircularProgress(0.3f, 40);
            guiHelper.CircularProgress(0.6f, 40);
            guiHelper.CircularProgress(0.9f, 40);
            guiHelper.EndHorizontalGroup();
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Separators", LabelVariant.Default);
            guiHelper.HorizontalSeparator();
            guiHelper.LabeledSeparator("OR");
            guiHelper.Separator(SeparatorOrientation.Horizontal, true);

            guiHelper.EndVerticalGroup();
        }

        void DrawFormsDemo()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Form Elements", LabelVariant.Default);
            guiHelper.MutedLabel("Interactive form components and controls");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Text Input", LabelVariant.Default);
            searchQuery = guiHelper.TextArea(searchQuery, placeholder: "Search users...", minHeight: 60);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Toggle Controls", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            opt1 = guiHelper.Toggle("Option 1", opt1);
            opt2 = guiHelper.Toggle("Option 2", opt2);
            opt3 = guiHelper.Toggle("Option 3", opt3);
            guiHelper.EndHorizontalGroup();
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Checkboxes", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            chk1 = guiHelper.Checkbox("Receive emails", chk1);
            chk2 = guiHelper.Checkbox("Enable notifications", chk2);
            guiHelper.EndHorizontalGroup();
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Switches", LabelVariant.Default);
            sw1 = guiHelper.Switch("Two-Factor Auth", sw1);
            sw2 = guiHelper.Switch("Public Profile", sw2);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Sliders", LabelVariant.Default);
            guiHelper.DrawSlider(400, "Master Volume", ref masterVolume, 0, 100);
            guiHelper.DrawSlider(400, "Brightness", ref brightness, 0, 100);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Actions", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();
            guiHelper.Button("Save Changes", ButtonVariant.Default);
            GUILayout.Space(8);
            guiHelper.Button("Cancel", ButtonVariant.Outline);
            guiHelper.EndHorizontalGroup();

            guiHelper.EndVerticalGroup();
        }

        void DrawTablesDemo()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Data Tables & Lists", LabelVariant.Default);
            guiHelper.MutedLabel("Display structured data and collections");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Product Inventory", LabelVariant.Default);
            string[] headers = { "SKU", "Product", "Category", "Stock", "Price", "Status" };
            string[,] data = new string[,]
            {
                { "SK001", "Laptop Pro", "Electronics", "24", "$1,299", "In Stock" },
                { "SK002", "Wireless Mouse", "Electronics", "156", "$49", "In Stock" },
                { "SK003", "USB-C Cable", "Accessories", "0", "$12", "Out of Stock" },
                { "SK004", "Monitor 4K", "Electronics", "8", "$599", "Low Stock" },
                { "SK005", "Keyboard RGB", "Electronics", "42", "$129", "In Stock" },
            };
            guiHelper.Table(headers, data, TableVariant.Striped);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Task List", LabelVariant.Default);
            guiHelper.BeginVerticalGroup();
            DrawTaskItem("Complete project documentation", true);
            DrawTaskItem("Review pull requests", true);
            DrawTaskItem("Update dependencies", false);
            DrawTaskItem("Fix critical bugs", false);
            guiHelper.EndVerticalGroup();

            guiHelper.EndVerticalGroup();
        }

        void DrawTaskItem(string task, bool completed)
        {
            guiHelper.BeginHorizontalGroup();
            guiHelper.Checkbox("", completed);
            GUILayout.Space(8);
            if (completed)
                guiHelper.MutedLabel(task);
            else
                guiHelper.Label(task);
            guiHelper.EndHorizontalGroup();
        }

        void DrawSettingsDemo()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Settings & Preferences", LabelVariant.Default);
            guiHelper.MutedLabel("Application configuration and user preferences");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Display", LabelVariant.Default);
            compactView = guiHelper.Switch("Compact View", compactView);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Notifications", LabelVariant.Default);
            emailNotif = guiHelper.Checkbox("Email Notifications", emailNotif);
            pushNotif = guiHelper.Checkbox("Push Notifications", pushNotif);
            smsNotif = guiHelper.Checkbox("SMS Alerts", smsNotif);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Privacy & Security", LabelVariant.Default);
            publicProfile = guiHelper.Switch("Public Profile", publicProfile);
            twoFactor = guiHelper.Switch("Two-Factor Authentication", twoFactor);
            dataCollection = guiHelper.Switch("Allow Data Collection", dataCollection);
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Advanced", LabelVariant.Default);
            showAdvanced = guiHelper.Toggle("Show Advanced Options", showAdvanced);
            if (showAdvanced)
            {
                guiHelper.MutedLabel("Advanced options are now visible");
                betaFeatures = guiHelper.Switch("Beta Features", betaFeatures);
                debugMode = guiHelper.Switch("Debug Mode", debugMode);
            }
            guiHelper.HorizontalSeparator();

            guiHelper.BeginHorizontalGroup();
            guiHelper.Button("Save Settings", ButtonVariant.Default);
            GUILayout.Space(8);
            guiHelper.Button("Reset to Defaults", ButtonVariant.Outline);
            guiHelper.EndHorizontalGroup();

            guiHelper.EndVerticalGroup();
        }

        void DrawGalleryDemo()
        {
            guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            guiHelper.Label("Component Gallery", LabelVariant.Default);
            guiHelper.MutedLabel("Visual showcase of styled components");
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Feature Cards", LabelVariant.Default);
            guiHelper.BeginHorizontalGroup();

            DrawFeatureCard("Fast", "Lightning quick performance");
            GUILayout.Space(12);
            DrawFeatureCard("Reliable", "Enterprise-grade stability");
            GUILayout.Space(12);
            DrawFeatureCard("Flexible", "Highly customizable UI");

            guiHelper.EndHorizontalGroup();
            guiHelper.HorizontalSeparator();

            guiHelper.Label("All Badge Variants", LabelVariant.Default);
            guiHelper.BeginVerticalGroup();

            guiHelper.BeginHorizontalGroup();
            guiHelper.Badge("Default", BadgeVariant.Default);
            guiHelper.Badge("Secondary", BadgeVariant.Secondary);
            guiHelper.Badge("Destructive", BadgeVariant.Destructive);
            guiHelper.Badge("Outline", BadgeVariant.Outline);
            guiHelper.EndHorizontalGroup();

            guiHelper.BeginHorizontalGroup();
            guiHelper.AnimatedBadge("Loading");
            GUILayout.Space(8);
            guiHelper.ProgressBadge("Uploading", 0.75f);
            GUILayout.Space(8);
            guiHelper.CountBadge(5);
            guiHelper.EndHorizontalGroup();

            guiHelper.EndVerticalGroup();
            guiHelper.HorizontalSeparator();

            guiHelper.Label("Text Variants", LabelVariant.Default);
            guiHelper.Label("Default text styling");
            guiHelper.SecondaryLabel("Secondary text styling");
            guiHelper.MutedLabel("Muted text styling");
            guiHelper.DestructiveLabel("Destructive text styling");

            guiHelper.EndVerticalGroup();
        }

        void DrawFeatureCard(string title, string description)
        {
            guiHelper.BeginCard(180, 120);
            guiHelper.CardContent(() =>
            {
                guiHelper.Label(title, LabelVariant.Default);
                guiHelper.MutedLabel(description);
                GUILayout.Space(12);
                guiHelper.Button("Learn More", ButtonVariant.Ghost, ButtonSize.Small);
            });
            guiHelper.EndCard();
        }
    }
}
