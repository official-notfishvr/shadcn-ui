using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui_Demo.Menu
{
    public class ShadcnDocsHomeDemo : MonoBehaviour
    {
        private GUIHelper _gui;
        private Rect _windowRect = new Rect(8f, 8f, 1560f, 780f);

        private string _nameOnCard = "John Doe";
        private string _cardNumber = "1234 5678 9012 3456";
        private string _cvv = "123";
        private string _comments = string.Empty;
        private string _topUrl = "https://";
        private string _search = string.Empty;
        private string _messageInput = string.Empty;
        private string _exampleUrl = "https:// example.com";
        private string _chatPrompt = string.Empty;
        private string _gpuCount = "8";

        private bool _sameAsShipping = true;
        private bool _wallpaperTinting;
        private bool _agreeToTerms = true;

        private float _priceLower = 320f;
        private float _priceUpper = 680f;

        private const float PaymentFieldHeight = 30f;
        private int _surveyIndex;
        private int _computeIndex;
        private int _monthIndex = 4;
        private int _yearIndex;

        private readonly string[] _months = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        private readonly string[] _years = { "YYYY", "2026", "2027", "2028", "2029", "2030" };
        private readonly string[] _computeOptions = { "Kubernetes", "Virtual Machine" };
        private readonly string[] _surveyOptions = { "Social Media", "Search Engine", "Referral", "Other" };

        private ComponentAppearance _cardAppearance;
        private ComponentAppearance _selectedCardAppearance;
        private ComponentAppearance _mutedCardAppearance;
        private ComponentAppearance _inputAppearance;
        private ComponentAppearance _lightButtonAppearance;
        private ComponentAppearance _pillAppearance;
        private ComponentAppearance _pillActiveAppearance;
        private ComponentAppearance _chipActiveAppearance;
        private ComponentAppearance _inlineBadgeAppearance;

        private void Start()
        {
            _gui = new GUIHelper();
            _gui.SetTheme("Dark");

            _cardAppearance = Surface("#121214", "#2a2a2e", 18f);
            _selectedCardAppearance = Surface("#1b1b1f", "#52525b", 18f);
            _mutedCardAppearance = Surface("#09090b", "#2a2a2e", 18f);
            _inputAppearance = Surface("#121214", "#2a2a2e", 12f);
            _lightButtonAppearance = new ComponentAppearance
            {
                BackgroundColor = Theme.Hex("#f4f4f5"),
                ForegroundColor = Theme.Hex("#09090b"),
                BorderColor = Color.clear,
                BorderRadius = 10f,
                BorderThickness = 0f,
            };
            _pillAppearance = Surface("#121214", "#2a2a2e", 999f);
            _pillActiveAppearance = new ComponentAppearance
            {
                BackgroundColor = Theme.Hex("#f4f4f5"),
                ForegroundColor = Theme.Hex("#09090b"),
                BorderColor = Color.clear,
                BorderRadius = 999f,
                BorderThickness = 0f,
            };
            _chipActiveAppearance = Surface("#1c1c20", "#2a2a2e", 999f);
            _inlineBadgeAppearance = Surface("#18181b", "#2a2a2e", 999f);
        }

        private void OnDestroy()
        {
            _gui?.Cleanup();
        }

        private void OnGUI()
        {
            _windowRect = GUI.Window(407, _windowRect, (GUI.WindowFunction)DrawWindow, string.Empty);
            _gui?.DrawOverlays();
        }

        private void DrawWindow(int id)
        {
            _gui.UpdateGUI(true);
            if (!_gui.BeginGUI())
                return;

            _gui.BeginHorizontalGroup();
            DrawPaymentColumn();
            _gui.AddSpace(16f);
            DrawCenterLeftColumn();
            _gui.AddSpace(16f);
            DrawCenterRightColumn();
            _gui.AddSpace(16f);
            DrawRightColumn();
            _gui.EndHorizontalGroup();

            _gui.EndGUI();
            GUI.DragWindow(new Rect(0f, 0f, _windowRect.width, 18f));
        }

        private void DrawPaymentColumn()
        {
            _gui.BeginVerticalGroup(GUILayout.Width(342f));
            _gui.BeginCard(342f, 688f, ControlVariant.Default, ControlSize.Default, _cardAppearance);
            _gui.CardHeader(() =>
            {
                _gui.Heading("Payment Method");
                _gui.Caption("All transactions are secure and encrypted");
            });
            _gui.CardContent(() =>
            {
                _nameOnCard = _gui.Input(
                    new InputConfig
                    {
                        Label = "Name on Card",
                        Value = _nameOnCard,
                        Size = ControlSize.Mini,
                        Height = PaymentFieldHeight,
                        Appearance = _inputAppearance,
                    }
                );
                _gui.AddSpace(10f);

                _gui.BeginHorizontalGroup();
                _gui.BeginVerticalGroup(GUILayout.Width(190f));
                _gui.Label("Card Number");
                _cardNumber = _gui.Input(
                    new InputConfig
                    {
                        Value = _cardNumber,
                        Size = ControlSize.Mini,
                        Height = PaymentFieldHeight,
                        Appearance = _inputAppearance,
                        LayoutOptions = new[] { GUILayout.Width(190f) },
                    }
                );
                _gui.EndVerticalGroup();
                _gui.AddSpace(8f);
                _gui.BeginVerticalGroup(GUILayout.Width(88f));
                _gui.Label("CVV");
                _cvv = _gui.Input(
                    new InputConfig
                    {
                        Value = _cvv,
                        Size = ControlSize.Mini,
                        Height = PaymentFieldHeight,
                        Appearance = _inputAppearance,
                        LayoutOptions = new[] { GUILayout.Width(88f) },
                    }
                );
                _gui.EndVerticalGroup();
                _gui.EndHorizontalGroup();
                _gui.Caption("Enter your 16-digit number.");
                _gui.AddSpace(10f);

                _gui.BeginHorizontalGroup();
                _gui.BeginVerticalGroup(GUILayout.Width(140f));
                _gui.Label("Month");
                _monthIndex = _gui.Select(
                    new SelectConfig
                    {
                        Id = "payment_expiry_month",
                        Options = ToOptions(_months),
                        SelectedIndex = _monthIndex,
                        Size = ControlSize.Mini,
                        MaxHeight = 180f,
                        Appearance = _inputAppearance,
                        LayoutOptions = new[] { GUILayout.Width(140f) },
                    }
                );
                _gui.EndVerticalGroup();
                _gui.AddSpace(8f);
                _gui.BeginVerticalGroup(GUILayout.Width(140f));
                _gui.Label("Year");
                _yearIndex = _gui.Select(
                    new SelectConfig
                    {
                        Id = "payment_expiry_year",
                        Options = ToOptions(_years),
                        SelectedIndex = _yearIndex,
                        Size = ControlSize.Mini,
                        MaxHeight = 180f,
                        Appearance = _inputAppearance,
                        LayoutOptions = new[] { GUILayout.Width(140f) },
                    }
                );
                _gui.EndVerticalGroup();
                _gui.EndHorizontalGroup();

                _gui.AddSpace(10f);
                _gui.HorizontalSeparator();
                _gui.AddSpace(14f);
                _gui.Label("Billing Address");
                _gui.Caption("The billing address associated with your");
                _gui.Caption("payment method");
                _gui.AddSpace(8f);
                _sameAsShipping = _gui.Checkbox(new CheckboxConfig { Label = "Same as shipping address", Value = _sameAsShipping });
                _gui.AddSpace(12f);
                _gui.HorizontalSeparator();
                _gui.AddSpace(14f);

                _gui.Label("Comments");
                _gui.AddSpace(8f);
                _comments = _gui.TextArea(
                    new TextAreaConfig
                    {
                        Value = _comments,
                        Placeholder = "Add any additional comments",
                        MinHeight = 62f,
                        ShowCharCount = false,
                        Appearance = _inputAppearance,
                    }
                );
            });
            _gui.CardFooter(() =>
            {
                _gui.Button("Submit", ControlVariant.Default, ControlSize.Small, appearance: _lightButtonAppearance);
                _gui.Button("Cancel", ControlVariant.Outline, ControlSize.Small, appearance: _pillAppearance);
            });
            _gui.EndCard();
            _gui.EndVerticalGroup();
        }

        private void DrawCenterLeftColumn()
        {
            _gui.BeginVerticalGroup(GUILayout.Width(342f));

            _gui.BeginCard(342f, 222f, ControlVariant.Default, ControlSize.Default, _mutedCardAppearance);
            _gui.CardContent(() =>
            {
                _gui.AddSpace(8f);
                _gui.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();
                _gui.AvatarGroup(new List<(Texture2D img, string fallback)> { (null, "C"), (null, "L"), (null, "V") }, ControlSize.Default, 3);
                GUILayout.FlexibleSpace();
                _gui.EndHorizontalGroup();
                _gui.AddSpace(24f);
                _gui.Heading("No Team Members");
                _gui.Caption("Invite your team to collaborate on this project.");
                _gui.AddSpace(16f);
                _gui.Button("+ Invite Members", ControlVariant.Default, ControlSize.Small, appearance: _lightButtonAppearance);
            });
            _gui.EndCard();

            _gui.AddSpace(16f);
            _gui.BeginHorizontalGroup();
            DrawPill("Syncing", true, 74f);
            DrawPill("Updating", false, 86f);
            DrawPill("Loading", false, 76f);
            _gui.EndHorizontalGroup();

            _gui.AddSpace(16f);
            _messageInput = _gui.Input(
                new InputConfig
                {
                    Value = _messageInput,
                    Placeholder = "Send a message...",
                    Appearance = _inputAppearance,
                }
            );

            _gui.AddSpace(22f);
            _gui.Label("Price Range");
            _gui.Caption("Set your budget range ($200 - 800).");
            Vector2 range = _gui.RangeSlider(_priceLower, _priceUpper, 200f, 800f, 10f, showValue: false, appearance: null, opts: new[] { GUILayout.Width(342f) });
            _priceLower = range.x;
            _priceUpper = range.y;

            _gui.AddSpace(16f);
            _gui.BeginHorizontalGroup();
            _search = _gui.Input(
                new InputConfig
                {
                    Value = _search,
                    Placeholder = "Search...",
                    Appearance = _inputAppearance,
                    LayoutOptions = new[] { GUILayout.Width(238f) },
                }
            );
            _gui.Label("12 results", ControlVariant.Muted, opts: new[] { GUILayout.Width(80f) });
            _gui.EndHorizontalGroup();

            _gui.AddSpace(16f);
            _exampleUrl = _gui.Input(
                new InputConfig
                {
                    Value = _exampleUrl,
                    Placeholder = "https:// example.com",
                    Appearance = _inputAppearance,
                }
            );

            _gui.AddSpace(16f);
            _gui.BeginCard(342f, 102f, ControlVariant.Default, ControlSize.Default, _cardAppearance);
            _gui.CardContent(() =>
            {
                _chatPrompt = _gui.TextArea(
                    new TextAreaConfig
                    {
                        Value = _chatPrompt,
                        Placeholder = "Ask, Search or Chat...",
                        MinHeight = 44f,
                        ShowCharCount = false,
                        Variant = ControlVariant.Ghost,
                    }
                );
                _gui.BeginHorizontalGroup();
                _gui.Label("Auto", ControlVariant.Muted);
                GUILayout.FlexibleSpace();
                _gui.Label("52% used", ControlVariant.Muted);
                _gui.Button("↑", ControlVariant.Default, ControlSize.Icon, appearance: _lightButtonAppearance);
                _gui.EndHorizontalGroup();
            });
            _gui.EndCard();

            _gui.AddSpace(16f);
            _gui.Input(new InputConfig { Value = "@shadcn", Appearance = _inputAppearance });
            _gui.EndVerticalGroup();
        }

        private void DrawCenterRightColumn()
        {
            _gui.BeginVerticalGroup(GUILayout.Width(342f));
            _topUrl = _gui.Input(new InputConfig { Value = _topUrl, Appearance = _inputAppearance });
            _gui.AddSpace(14f);

            _gui.BeginCard(342f, 64f, ControlVariant.Default, ControlSize.Default, _cardAppearance);
            _gui.CardContent(() =>
            {
                _gui.BeginHorizontalGroup();
                _gui.BeginVerticalGroup();
                _gui.Label("Two-factor authentication");
                _gui.Caption("Verify via email or phone number.");
                _gui.EndVerticalGroup();
                GUILayout.FlexibleSpace();
                _gui.Button("Enable", ControlVariant.Default, ControlSize.Small, appearance: _lightButtonAppearance);
                _gui.EndHorizontalGroup();
            });
            _gui.EndCard();

            _gui.AddSpace(12f);
            _gui.BeginCard(342f, 40f, ControlVariant.Default, ControlSize.Default, _cardAppearance);
            _gui.CardContent(() =>
            {
                _gui.BeginHorizontalGroup();
                _gui.Label("Your profile has been verified.");
                GUILayout.FlexibleSpace();
                _gui.Label(">");
                _gui.EndHorizontalGroup();
            });
            _gui.EndCard();

            _gui.AddSpace(24f);
            _gui.LabeledSeparator("Appearance Settings");
            _gui.AddSpace(18f);

            _gui.Label("Compute Environment");
            _gui.Caption("Select the compute environment for your cluster.");
            _gui.AddSpace(12f);

            DrawComputeCard(0, "Kubernetes", "Run GPU workloads on a K8s configured", "cluster. This is the default.");
            _gui.AddSpace(12f);
            DrawComputeCard(1, "Virtual Machine", "Access a VM configured cluster to run", "workloads. (Coming soon)");

            _gui.AddSpace(16f);
            _gui.HorizontalSeparator();
            _gui.AddSpace(14f);
            _gui.BeginHorizontalGroup();
            _gui.BeginVerticalGroup();
            _gui.Label("Number of GPUs");
            _gui.Caption("You can add more later.");
            _gui.EndVerticalGroup();
            GUILayout.FlexibleSpace();
            _gpuCount = _gui.Input(
                new InputConfig
                {
                    Value = _gpuCount,
                    Appearance = _inputAppearance,
                    LayoutOptions = new[] { GUILayout.Width(110f) },
                }
            );
            _gui.EndHorizontalGroup();

            _gui.AddSpace(14f);
            _gui.HorizontalSeparator();
            _gui.AddSpace(14f);
            _gui.BeginHorizontalGroup();
            _gui.BeginVerticalGroup();
            _gui.Label("Wallpaper Tinting");
            _gui.Caption("Allow the wallpaper to be tinted.");
            _gui.EndVerticalGroup();
            GUILayout.FlexibleSpace();
            _wallpaperTinting = _gui.Switch(new SwitchConfig { Label = string.Empty, Value = _wallpaperTinting });
            _gui.EndHorizontalGroup();
            _gui.EndVerticalGroup();
        }

        private void DrawRightColumn()
        {
            _gui.BeginVerticalGroup(GUILayout.Width(344f));
            _gui.BeginCard(344f, 160f, ControlVariant.Default, ControlSize.Default, _cardAppearance);
            _gui.CardContent(() =>
            {
                _gui.Badge("@ Add context", ControlVariant.Outline, ControlSize.Small, _inlineBadgeAppearance);
                _gui.AddSpace(18f);
                _gui.Caption("Ask, search, or make anything...");
                GUILayout.FlexibleSpace();
                _gui.BeginHorizontalGroup();
                _gui.Label("Auto", ControlVariant.Muted);
                _gui.AddSpace(10f);
                _gui.Label("All Sources", ControlVariant.Muted);
                GUILayout.FlexibleSpace();
                _gui.Button("↑", ControlVariant.Default, ControlSize.Icon, appearance: _lightButtonAppearance);
                _gui.EndHorizontalGroup();
            });
            _gui.EndCard();

            _gui.AddSpace(18f);
            _gui.BeginHorizontalGroup();
            _gui.Button("←", ControlVariant.Outline, ControlSize.Icon, appearance: _pillAppearance);
            _gui.Button("Archive", ControlVariant.Outline, ControlSize.Small, appearance: _pillAppearance);
            _gui.Button("Report", ControlVariant.Outline, ControlSize.Small, appearance: _pillAppearance);
            _gui.Button("Snooze", ControlVariant.Outline, ControlSize.Small, appearance: _pillAppearance);
            _gui.Button("...", ControlVariant.Outline, ControlSize.Icon, appearance: _pillAppearance);
            _gui.EndHorizontalGroup();

            _gui.AddSpace(18f);
            _agreeToTerms = _gui.Checkbox(
                new CheckboxConfig
                {
                    Label = "I agree to the terms and conditions",
                    Value = _agreeToTerms,
                    Appearance = _cardAppearance,
                }
            );

            _gui.AddSpace(18f);
            _gui.BeginHorizontalGroup();
            DrawPill("1", false, 28f);
            DrawPill("2", false, 28f);
            DrawPill("3", true, 28f);
            DrawPill("←", false, 28f);
            DrawPill("→", false, 28f);
            GUILayout.FlexibleSpace();
            _gui.Button("Copilot", ControlVariant.Outline, ControlSize.Small, appearance: _pillAppearance);
            _gui.EndHorizontalGroup();

            _gui.AddSpace(18f);
            _gui.BeginCard(344f, 170f, ControlVariant.Default, ControlSize.Default, _cardAppearance);
            _gui.CardHeader(() =>
            {
                _gui.Heading("How did you hear about us?");
                _gui.Caption("Select the option that best describes how you...");
            });
            _gui.CardContent(() =>
            {
                _gui.BeginHorizontalGroup();
                DrawSurveyChip("Social Media", 0, 124f);
                _gui.AddSpace(8f);
                DrawSurveyChip("Search Engine", 1, 118f);
                _gui.EndHorizontalGroup();
                _gui.AddSpace(8f);
                _gui.BeginHorizontalGroup();
                DrawSurveyChip("Referral", 2, 88f);
                _gui.AddSpace(8f);
                DrawSurveyChip("Other", 3, 76f);
                _gui.EndHorizontalGroup();
            });
            _gui.EndCard();

            _gui.AddSpace(24f);
            _gui.BeginCard(344f, 186f, ControlVariant.Default, ControlSize.Default, _mutedCardAppearance);
            _gui.CardContent(() =>
            {
                _gui.AddSpace(24f);
                _gui.ProgressBadge(" ", 0.5f, ControlVariant.Secondary);
                _gui.AddSpace(20f);
                _gui.Heading("Processing your request");
                _gui.Caption("Please wait while we process your");
                _gui.Caption("request. Do not refresh the page.");
                _gui.AddSpace(20f);
                _gui.Button("Cancel", ControlVariant.Outline, ControlSize.Small, appearance: _pillAppearance);
            });
            _gui.EndCard();
            _gui.EndVerticalGroup();
        }

        private void DrawComputeCard(int index, string title, string lineOne, string lineTwo)
        {
            ComponentAppearance appearance = _computeIndex == index ? _selectedCardAppearance : _cardAppearance;
            _gui.BeginCard(342f, 84f, ControlVariant.Default, ControlSize.Default, appearance);
            _gui.CardContent(() =>
            {
                _gui.BeginHorizontalGroup();
                _gui.BeginVerticalGroup();
                _gui.Label(title);
                _gui.Caption(lineOne);
                _gui.Caption(lineTwo);
                _gui.EndVerticalGroup();
                GUILayout.FlexibleSpace();
                if (_gui.Button(_computeIndex == index ? "Selected" : "Select", ControlVariant.Outline, ControlSize.Small, appearance: _pillAppearance))
                    _computeIndex = index;
                _gui.EndHorizontalGroup();
            });
            _gui.EndCard();
        }

        private void DrawSurveyChip(string text, int index, float width)
        {
            bool active = _surveyIndex == index;
            ComponentAppearance appearance = active ? _chipActiveAppearance : _pillAppearance;
            if (_gui.Button(text, ControlVariant.Outline, ControlSize.Small, appearance: appearance, opts: new[] { GUILayout.Width(width) }))
                _surveyIndex = index;
        }

        private void DrawPill(string text, bool active, float width)
        {
            _gui.Button(text, ControlVariant.Outline, ControlSize.Small, appearance: active ? _pillActiveAppearance : _pillAppearance, opts: new[] { GUILayout.Width(width) });
            _gui.AddSpace(6f);
        }

        private static SelectOption[] ToOptions(string[] items)
        {
            var options = new SelectOption[items.Length];
            for (int i = 0; i < items.Length; i++)
                options[i] = new SelectOption(items[i], items[i]);
            return options;
        }

        private static ComponentAppearance Surface(string fill, string border, float radius)
        {
            return new ComponentAppearance
            {
                BackgroundColor = Theme.Hex(fill),
                BorderColor = Theme.Hex(border),
                BorderRadius = radius,
                BorderThickness = 1f,
            };
        }
    }
}
