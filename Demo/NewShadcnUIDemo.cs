using UnityEngine;
using ShadcnUI;
using ShadcnUI.Core.Styling;
using BepInEx;

namespace ShadcnUI.Demo;

[System.ComponentModel.Description("fgdsfds")]
[BepInPlugin("3", "3", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    public static Plugin instance;
    public static bool FirstLaunch;

    private void Awake()
    {
        instance = this;
        GorillaTagger.OnPlayerSpawned(LoadMenu);
    }

    private static void LoadMenu()
    {
        GameObject Loader = new GameObject("Loader");
        Loader.AddComponent<NewShadcnUIDemo>();
        UnityEngine.Object.DontDestroyOnLoad(Loader);
    }
}

/// <summary>
/// Demo showcasing the new ShadcnUI library components in a standalone window.
/// </summary>
public class NewShadcnUIDemo : MonoBehaviour
{
    private Rect _windowRect = new Rect(50, 50, 600, 700);
    private string _username = "";
    private string _password = "";
    private string _bio = "";
    private bool _rememberMe = false;
    private bool _notifications = true;
    private int _selectedTab = 0;
    private int _selectedNav = 0;
    private int _selectedOption = 0;
    private float _volume = 50f;
    private bool _showDialog = false;
    private bool _showWindow = true;
    
    private readonly string[] _tabs = { "Account", "Settings", "Notifications" };
    private readonly string[] _navItems = { "Home", "Profile", "Settings", "Help" };
    private readonly string[] _options = { "Option A", "Option B", "Option C" };

    void OnGUI()
    {
        if (!_showWindow) return;
        
        _windowRect = GUILayout.Window(0, _windowRect, DrawWindow, "ShadcnUI Demo");
    }

    void DrawWindow(int windowID)
    {
        // Draw window background - flat color, no rounded corners
        var theme = Shadcn.CurrentTheme;
        var bgTex = RenderHelpers.CreateSolidTexture(theme.Background);
        GUI.DrawTexture(new Rect(0, 0, _windowRect.width, _windowRect.height), bgTex);
        
        Shadcn.BeginGUI();
        
        // Close button
        Shadcn.BeginHorizontal();
        Shadcn.FlexibleSpace();
        if (Shadcn.Button("X", null, new ButtonVariants(ControlVariant.Ghost)))
            _showWindow = false;
        Shadcn.EndHorizontal();
        
        // Title
        Shadcn.Title("ShadcnUI Demo");
        Shadcn.Space(20);
        
        // Theme switcher
        Shadcn.BeginHorizontal();
        Shadcn.Label("Theme:");
        if (Shadcn.Button("Dark", null, new ButtonVariants(Shadcn.CurrentTheme.Name == "dark" ? ControlVariant.Primary : ControlVariant.Ghost)))
            Shadcn.Services.ThemeManager.SetTheme(ShadcnUI.Core.Theming.Theme.Dark);
        if (Shadcn.Button("Light", null, new ButtonVariants(Shadcn.CurrentTheme.Name == "light" ? ControlVariant.Primary : ControlVariant.Ghost)))
            Shadcn.Services.ThemeManager.SetTheme(ShadcnUI.Core.Theming.Theme.Light);
        if (Shadcn.Button("Zinc", null, new ButtonVariants(Shadcn.CurrentTheme.Name == "zinc" ? ControlVariant.Primary : ControlVariant.Ghost)))
            Shadcn.Services.ThemeManager.SetTheme(ShadcnUI.Core.Theming.Theme.Zinc);
        Shadcn.EndHorizontal();
        
        Shadcn.Space(20);
        
        // Tabs
        Shadcn.Tabs(_tabs, ref _selectedTab, content =>
        {
            switch (content)
            {
                case 0: // Account tab
                    RenderAccountTab();
                    break;
                case 1: // Settings tab
                    RenderSettingsTab();
                    break;
                case 2: // Notifications tab
                    RenderNotificationsTab();
                    break;
            }
        });
        
        // Dialog demo
        if (_showDialog)
        {
            Shadcn.OpenDialog("demo-dialog");
            Shadcn.ConfirmDialog("demo-dialog", 
                "Confirm Action", 
                "Are you sure you want to proceed?",
                () => Debug.Log("Confirmed!"),
                () => _showDialog = false);
        }
        
        // Render tooltip last
        Shadcn.RenderTooltip();
        Shadcn.EndGUI();
        
        GUI.DragWindow();
    }

    private void RenderAccountTab()
    {
        Shadcn.Card(() =>
        {
            Shadcn.CardHeader("Login Information");
            Shadcn.CardContent(() =>
            {
                _username = Shadcn.Input("Username", _username, "Enter username");
                Shadcn.Space(10);
                _password = Shadcn.Password("Password", _password);
                Shadcn.Space(10);
                Shadcn.Checkbox("Remember me", ref _rememberMe);
            });
            Shadcn.CardFooter(() =>
            {
                Shadcn.FlexibleSpace();
                if (Shadcn.Button("Reset", null, new ButtonVariants(ControlVariant.Ghost)))
                {
                    _username = "";
                    _password = "";
                }
                Shadcn.Space(10);
                if (Shadcn.Button("Login", () => Debug.Log($"Login: {_username}"), new ButtonVariants(ControlVariant.Primary)))
                {
                    // Login action
                }
            });
        });
        
        Shadcn.Space(20);
        
        // Bio card
        Shadcn.Card(() =>
        {
            Shadcn.CardHeader("About");
            Shadcn.CardContent(() =>
            {
                _bio = Shadcn.TextArea("Bio", _bio, "Tell us about yourself...", 100f);
            });
        });
    }

    private void RenderSettingsTab()
    {
        Shadcn.Card(() =>
        {
            Shadcn.CardHeader("Preferences");
            Shadcn.CardContent(() =>
            {
                // Select dropdown
                Shadcn.Label("Choose Option");
                _selectedOption = Shadcn.Select(_options, _selectedOption);
                
                Shadcn.Space(15);
                
                // Slider
                Shadcn.Slider("Volume", ref _volume, 0f, 100f, onChange: null, disabled: false, showValue: true);
                
                Shadcn.Space(15);
                
                // Switch
                Shadcn.Switch("Enable Notifications", ref _notifications);
            });
        });
        
        Shadcn.Space(20);
        
        // Alert examples
        Shadcn.SectionHeader("Alerts");
        Shadcn.DefaultAlert("Info", "This is a default alert message.");
        Shadcn.Space(10);
        Shadcn.SuccessAlert("Success", "Your changes have been saved!");
        Shadcn.Space(10);
        Shadcn.DestructiveAlert("Error", "Something went wrong.");
    }

    private void RenderNotificationsTab()
    {
        Shadcn.Card(() =>
        {
            Shadcn.CardHeader("Recent Activity");
            Shadcn.CardContent(() =>
            {
                // Badge examples
                Shadcn.BeginHorizontal();
                Shadcn.PrimaryBadge("New");
                Shadcn.Space(5);
                Shadcn.SecondaryBadge("Updated");
                Shadcn.Space(5);
                Shadcn.DestructiveBadge("Urgent");
                Shadcn.Space(5);
                Shadcn.OutlineBadge("Draft");
                Shadcn.EndHorizontal();
                
                Shadcn.Space(20);
                
                // Progress bars
                Shadcn.SectionHeader("Progress");
                Shadcn.PrimaryProgress(75f, 100f, "Uploading...");
                Shadcn.SecondaryProgress(30f, 100f, "Processing...");
                
                Shadcn.Space(20);
                
                // Button variants
                Shadcn.SectionHeader("Button Variants");
                Shadcn.BeginHorizontal();
                if (Shadcn.Button("Primary", null, new ButtonVariants(ControlVariant.Primary)))
                    Debug.Log("Primary clicked");
                if (Shadcn.Button("Secondary", null, new ButtonVariants(ControlVariant.Secondary)))
                    Debug.Log("Secondary clicked");
                if (Shadcn.Button("Destructive", null, new ButtonVariants(ControlVariant.Destructive)))
                    Debug.Log("Destructive clicked");
                if (Shadcn.Button("Ghost", null, new ButtonVariants(ControlVariant.Ghost)))
                    Debug.Log("Ghost clicked");
                if (Shadcn.Button("Outline", null, new ButtonVariants(ControlVariant.Outline)))
                    Debug.Log("Outline clicked");
                Shadcn.EndHorizontal();
                
                Shadcn.Space(20);
                
                // Dialog trigger
                if (Shadcn.Button("Open Dialog", () => _showDialog = true, new ButtonVariants(ControlVariant.Primary)))
                {
                    _showDialog = true;
                }
            });
        });
    }
}
