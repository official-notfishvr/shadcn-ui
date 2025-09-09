using UnityEngine;
using shadcnui;
using shadcnui.GUIComponents;
using System.Linq;

public class UI : MonoBehaviour
{
    private GUIHelper guiHelper;
    private Rect windowRect = new Rect(20, 20, 1450, 600);
    private bool showDemoWindow = false;

    private Vector2 scrollPosition;

    private bool checkboxValue = false;
    private bool switchValue = false;
    private float sliderValue = 50f;
    private string textAreaValue = "";
    private int selectedTab = 0;
    private int selectedToggle = 0;
    private bool isAlertDismissed = false;
    private float progressValue = 0.3f;
    private int glowButtonIndex = 0;
    private GUITabsComponents.TabConfig[] demoTabs;
    private int currentDemoTab = 0;

    void Start()
    {
        guiHelper = new GUIHelper();
        demoTabs = new GUITabsComponents.TabConfig[]
        {
            new GUITabsComponents.TabConfig("Alert", DrawAlertDemos),
            new GUITabsComponents.TabConfig("Avatar", DrawAvatarDemos),
            new GUITabsComponents.TabConfig("Badge", DrawBadgeDemos),
            new GUITabsComponents.TabConfig("Button", DrawButtonDemos),
            new GUITabsComponents.TabConfig("Card", DrawCardDemos),
            new GUITabsComponents.TabConfig("Checkbox", DrawCheckboxDemos),
            new GUITabsComponents.TabConfig("Input", DrawInputDemos),
            new GUITabsComponents.TabConfig("Label", DrawLabelDemos),
            new GUITabsComponents.TabConfig("Layout", DrawLayoutDemos),
            new GUITabsComponents.TabConfig("Progress", DrawProgressDemos),
            new GUITabsComponents.TabConfig("Separator", DrawSeparatorDemos),
            new GUITabsComponents.TabConfig("Skeleton", DrawSkeletonDemos),
            new GUITabsComponents.TabConfig("Slider", DrawSliderDemos),
            new GUITabsComponents.TabConfig("Switch", DrawSwitchDemos),
            new GUITabsComponents.TabConfig("Table", DrawTableDemos),
            new GUITabsComponents.TabConfig("Tabs", DrawTabsDemos),
            new GUITabsComponents.TabConfig("Text Area", DrawTextAreaDemos),
            new GUITabsComponents.TabConfig("Toggle", DrawToggleDemos),
            new GUITabsComponents.TabConfig("Visual", DrawVisualDemos)
        };
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 30), "Open Demo Window"))
        {
            showDemoWindow = !showDemoWindow;
        }

        if (showDemoWindow)
        {
            windowRect = GUI.Window(101, windowRect, (id) => DrawDemoWindow(id), "shadcn/ui Demo");
        }
    }

    void DrawDemoWindow(int windowID)
    {
        guiHelper.UpdateAnimations(showDemoWindow);
        if (guiHelper.BeginAnimatedGUI())
        {
            currentDemoTab = guiHelper.Tabs(demoTabs.Select(tab => tab.Name).ToArray(), currentDemoTab);

            guiHelper.BeginTabContent();
            scrollPosition = guiHelper.DrawScrollView(scrollPosition, windowRect.width - 20, windowRect.height - 40, () =>
            {
                GUILayout.BeginVertical(GUILayout.Width(windowRect.width - 20), GUILayout.ExpandHeight(true));
                if (currentDemoTab >= 0 && currentDemoTab < demoTabs.Length)
                {
                    demoTabs[currentDemoTab].Content?.Invoke();
                }
                GUILayout.EndVertical();
            });
            guiHelper.EndTabContent();
        }
        guiHelper.EndAnimatedGUI();
        GUI.DragWindow();
    }

    private string passwordValue = "password123";
    private string inputTextAreaValue = "This is a text area in Input Components.";
    private string glowInputFieldText = "Glow Input";
    private int glowInputFieldIdx = 0;
    private bool drawToggleValue = false;
    private bool drawCheckboxValue = false;
    private int selectionGridValue = 0;

    private bool outlineToggleValue = false;
    private bool smallToggleValue = false;
    private bool largeToggleValue = false;
    private bool[] multiToggleGroupValues = { false, true, false };
    private int intSliderValue = 50;
    private float visualProgressBarValue = 0.7f;
    private bool labelRectValue = false;
    private float progressRectValue = 0.5f;
    private bool separatorRectValue = false;
    private int selectedVerticalTab = 0;
    private string textAreaRectValue = "Text in Rect";
    private string outlineTextAreaValue = "Outline Text Area";
    private string ghostTextAreaValue = "Ghost Text Area";
    private string labeledTextAreaValue = "Labeled Text Area";
    private float resizableTextAreaHeight = 100f;
    private string resizableTextAreaValue = "Resizable Text Area";

    void DrawInputDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Input", LabelVariant.Default);
        guiHelper.MutedLabel("Displays a form input field or a component that takes user input.");
        guiHelper.HorizontalSeparator();

        guiHelper.DrawSectionHeader("Section Header");
        guiHelper.Label("Code: guiHelper.DrawSectionHeader(\"Section Header\");", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.RenderLabel("Rendered Label (width 150)", 150);
        guiHelper.Label("Code: guiHelper.RenderLabel(\"Rendered Label (width 150)\", 150);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        glowInputFieldText = guiHelper.RenderGlowInputField(glowInputFieldText, glowInputFieldIdx, "Enter text...", 200);
        guiHelper.Label($"Glow Input Value: {glowInputFieldText}");
        guiHelper.Label("Code: guiHelper.RenderGlowInputField(text, index, placeholder, width);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        passwordValue = guiHelper.DrawPasswordField(300, "Password", ref passwordValue);
        guiHelper.Label($"Password Value: {passwordValue}");
        guiHelper.Label("Code: guiHelper.DrawPasswordField(width, label, ref password);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.DrawTextArea(300, "Input Text Area", ref inputTextAreaValue, 200, 80);
        guiHelper.Label($"Input Text Area Value: {inputTextAreaValue}");
        guiHelper.Label("Code: guiHelper.DrawTextArea(width, label, ref text, maxLength, height);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }

    void DrawTabsDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        string[] tabNames = { "Account", "Password", "Notifications" };
        selectedTab = guiHelper.Tabs(tabNames, selectedTab);
        guiHelper.BeginTabContent();
        switch (selectedTab)
        {
            case 0:
                guiHelper.Label("Make changes to your account here.");
                break;
            case 1:
                guiHelper.Label("Change your password here.");
                break;
            case 2:
                guiHelper.Label("Manage your notification settings here.");
                break;
        }
        guiHelper.EndTabContent();
        guiHelper.Label("Code: selectedTab = guiHelper.Tabs(tabNames, selectedTab); ... guiHelper.BeginTabContent(); ... guiHelper.EndTabContent();", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Tabs with Content (using TabConfig)");
        GUITabsComponents.TabConfig[] tabConfigs = new GUITabsComponents.TabConfig[]
        {
            new GUITabsComponents.TabConfig("Tab A", () => guiHelper.Label("Content for Tab A.")),
            new GUITabsComponents.TabConfig("Tab B", () => guiHelper.Label("Content for Tab B.")),
            new GUITabsComponents.TabConfig("Tab C", () => guiHelper.Label("Content for Tab C."))
        };
        guiHelper.TabsWithContent(tabConfigs, selectedTab);
        guiHelper.Label("Code: guiHelper.TabsWithContent(tabConfigs, selectedTab);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Vertical Tabs");
        string[] verticalTabNames = { "Profile", "Settings", "Privacy" };
        selectedVerticalTab = guiHelper.VerticalTabs(verticalTabNames, selectedVerticalTab, tabWidth: 100);
        guiHelper.BeginTabContent();
        switch (selectedVerticalTab)
        {
            case 0:
                guiHelper.Label("Profile content.");
                break;
            case 1:
                guiHelper.Label("Settings content.");
                break;
            case 2:
                guiHelper.Label("Privacy content.");
                break;
        }
        guiHelper.EndTabContent();
        guiHelper.Label("Code: guiHelper.VerticalTabs(names, selected, tabWidth);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }

    void DrawTextAreaDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        textAreaValue = guiHelper.TextArea(textAreaValue, placeholder: "Type your message here.", minHeight: 100);
        guiHelper.Label("Code: textAreaValue = guiHelper.TextArea(value, placeholder, minHeight);");
        guiHelper.HorizontalSeparator();
        /*
        guiHelper.Label("Text Area in Rect");
        textAreaRectValue = guiHelper.TextArea(new Rect(10, 10, 300, 100), textAreaRectValue, placeholder: "Text in Rect");
        guiHelper.Label($"Text Area in Rect Value: {textAreaRectValue}");
        guiHelper.Label("Code: guiHelper.TextArea(Rect, text, placeholder);");
        guiHelper.HorizontalSeparator();
        */
        outlineTextAreaValue = guiHelper.OutlineTextArea(outlineTextAreaValue, placeholder: "Outline Text Area");
        guiHelper.Label($"Outline Text Area Value: {outlineTextAreaValue}");
        guiHelper.Label("Code: guiHelper.OutlineTextArea(text, placeholder);");
        guiHelper.HorizontalSeparator();

        ghostTextAreaValue = guiHelper.GhostTextArea(ghostTextAreaValue, placeholder: "Ghost Text Area");
        guiHelper.Label($"Ghost Text Area Value: {ghostTextAreaValue}");
        guiHelper.Label("Code: guiHelper.GhostTextArea(text, placeholder);");
        guiHelper.HorizontalSeparator();

        labeledTextAreaValue = guiHelper.LabeledTextArea("Your Message", labeledTextAreaValue, placeholder: "Type here...");
        guiHelper.Label($"Labeled Text Area Value: {labeledTextAreaValue}");
        guiHelper.Label("Code: guiHelper.LabeledTextArea(label, text, placeholder);");
        guiHelper.HorizontalSeparator();

        resizableTextAreaValue = guiHelper.ResizableTextArea(resizableTextAreaValue, ref resizableTextAreaHeight, placeholder: "Resize me!");
        guiHelper.Label($"Resizable Text Area Value: {resizableTextAreaValue} (Height: {resizableTextAreaHeight:F2})");
        guiHelper.Label("Code: guiHelper.ResizableTextArea(text, ref height, placeholder);");
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }




    void DrawAlertDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Alert", LabelVariant.Default);
        guiHelper.MutedLabel("Displays a callout for user attention.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Info", LabelVariant.Default);
        guiHelper.Alert("Heads up!", "You can use this component to show a message to the user.");
        guiHelper.Label("Code: guiHelper.Alert(\"Heads up!\", \"You can use this component to show a message to the user.\");", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Destructive", LabelVariant.Default);
        guiHelper.Alert("Error", "Your session has expired. Please log in again.", AlertVariant.Destructive);
        guiHelper.Label("Code: guiHelper.Alert(\"Error\", \"Your session has expired. Please log in again.\", AlertVariant.Destructive);");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("With Icon", LabelVariant.Default);
        guiHelper.Alert("Success", "Your profile has been updated successfully.", AlertVariant.Default, AlertType.Success, null);
        guiHelper.Label("Code: guiHelper.Alert(\"Success\", \"Your profile has been updated successfully.\", AlertVariant.Default, AlertType.Success, iconTexture);");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Dismissible", LabelVariant.Default);
        if (!isAlertDismissed)
        {
            //isAlertDismissed = guiHelper.DismissibleAlert("Warning", "This action cannot be undone.", AlertVariant.Default, AlertType.Warning, () => { isAlertDismissed = true; }, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        }
        else
        {
            guiHelper.Button("Reset Dismissible Alert", onClick: () => { isAlertDismissed = false; });
        }
        guiHelper.Label("Code: isAlertDismissed = guiHelper.DismissibleAlert(...);", LabelVariant.Muted);
        GUILayout.EndVertical();
    }

    void DrawAvatarDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Avatar", LabelVariant.Default);
        guiHelper.MutedLabel("An image element with a fallback for representing a user.");
        guiHelper.HorizontalSeparator();

        guiHelper.BeginHorizontalGroup();
        guiHelper.Avatar(null, "AV");
        guiHelper.Avatar(null, "SM", AvatarSize.Small);
        guiHelper.Avatar(null, "LG", AvatarSize.Large);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Avatar(texture, fallbackText, size, shape);");
        GUILayout.EndVertical();
    }

    void DrawBadgeDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Badge", LabelVariant.Default);
        guiHelper.MutedLabel("Displays a badge or a tag.");
        guiHelper.HorizontalSeparator();

        guiHelper.BeginHorizontalGroup();
        guiHelper.Badge("Default");
        guiHelper.Badge("Secondary", BadgeVariant.Secondary);
        guiHelper.Badge("Destructive", BadgeVariant.Destructive);
        guiHelper.Badge("Outline", BadgeVariant.Outline);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Badge(text, variant);");
        GUILayout.EndVertical();
    }

    void DrawButtonDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        guiHelper.Label("Variants");
        guiHelper.BeginHorizontalGroup();
        guiHelper.Button("Default");
        guiHelper.Button("Destructive", ButtonVariant.Destructive);
        guiHelper.Button("Outline", ButtonVariant.Outline);
        guiHelper.Button("Secondary", ButtonVariant.Secondary);
        guiHelper.Button("Ghost", ButtonVariant.Ghost);
        guiHelper.Button("Link", ButtonVariant.Link);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Button(label, variant, size, onClick, disabled);");

        guiHelper.Label("Sizes");
        guiHelper.BeginHorizontalGroup();
        guiHelper.Button("Default", ButtonVariant.Default, ButtonSize.Default);
        guiHelper.Button("Small", ButtonVariant.Default, ButtonSize.Small);
        guiHelper.Button("Large", ButtonVariant.Default, ButtonSize.Large);
        guiHelper.Button("Icon", ButtonVariant.Default, ButtonSize.Icon);
        guiHelper.EndHorizontalGroup();

        guiHelper.Label("With Icon");
        guiHelper.Button("Login");

        guiHelper.Label("Disabled");
        guiHelper.Button("Disabled", disabled: true);

        guiHelper.HorizontalSeparator();

        guiHelper.Label("Render Glow Button");
        guiHelper.RenderGlowButton("Glow Button", glowButtonIndex);
        guiHelper.Label("Code: guiHelper.RenderGlowButton(\"Glow Button\", glowButtonIndex);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Render Color Preset Button");
        guiHelper.RenderColorPresetButton("Red Button", Color.red);
        guiHelper.RenderColorPresetButton("Blue Button", Color.blue);
        guiHelper.Label("Code: guiHelper.RenderColorPresetButton(name, color);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Draw Button");
        guiHelper.DrawButton(200, "Simple Draw Button", () => Debug.Log("Simple Draw Button Clicked!"));
        guiHelper.Label("Code: guiHelper.DrawButton(width, text, onClick);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Draw Colored Button");
        guiHelper.DrawColoredButton(200, "Colored Draw Button", Color.magenta, () => Debug.Log("Colored Draw Button Clicked!"));
        guiHelper.Label("Code: guiHelper.DrawColoredButton(width, text, color, onClick);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Draw Fixed Button");
        guiHelper.DrawFixedButton("Fixed Button", 150, 40, () => Debug.Log("Fixed Button Clicked!"));
        guiHelper.Label("Code: guiHelper.DrawFixedButton(text, width, height, onClick);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        /*
        guiHelper.Label("Button with Rect (not visible directly)");
        guiHelper.Button(new Rect(10, 10, 100, 30), "Button in Rect");
        guiHelper.Label("Code: guiHelper.Button(new Rect(10, 10, 100, 30), \"Button in Rect\");", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        */
        guiHelper.Label("Specific Variant Buttons");
        guiHelper.BeginHorizontalGroup();
        guiHelper.DestructiveButton("Destructive", () => Debug.Log("Destructive Button Clicked!"));
        guiHelper.OutlineButton("Outline", () => Debug.Log("Outline Button Clicked!"));
        guiHelper.SecondaryButton("Secondary", () => Debug.Log("Secondary Button Clicked!"));
        guiHelper.GhostButton("Ghost", () => Debug.Log("Ghost Button Clicked!"));
        guiHelper.LinkButton("Link", () => Debug.Log("Link Button Clicked!"));
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.DestructiveButton(...); etc.", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Sized Variant Buttons");
        guiHelper.BeginHorizontalGroup();
        guiHelper.SmallButton("Small", () => Debug.Log("Small Button Clicked!"));
        guiHelper.LargeButton("Large", () => Debug.Log("Large Button Clicked!"));
        guiHelper.IconButton("Icon", () => Debug.Log("Icon Button Clicked!"));
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.SmallButton(...); etc.", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Draw Button Variant (direct call)");
        guiHelper.DrawButtonVariant("Direct Variant", ButtonVariant.Destructive, ButtonSize.Large);
        guiHelper.Label("Code: guiHelper.DrawButtonVariant(\"Direct Variant\", ButtonVariant.Destructive, ButtonSize.Large);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }

    void DrawCardDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        guiHelper.DrawCard("Create project", "Deploy your new project in one-click.",
            "This is the main content of the card where you can put any controls.",
            () => guiHelper.Button("Deploy"));

        guiHelper.Label("Code: guiHelper.DrawCard(title, description, content, footerAction);");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Simple Card");
        guiHelper.DrawSimpleCard("This is a simple card with just content.", 300, 100);
        guiHelper.Label("Code: guiHelper.DrawSimpleCard(content, width, height);");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Card with Sections");
        guiHelper.BeginCard(300, 200);
        guiHelper.BeginCardHeader();
        guiHelper.DrawCardTitle("Card Title");
        guiHelper.DrawCardDescription("Card Description");
        guiHelper.EndCardHeader();
        guiHelper.BeginCardContent();
        guiHelper.Label("Content goes here.");
        guiHelper.EndCardContent();
        guiHelper.BeginCardFooter();
        guiHelper.Button("Action");
        guiHelper.EndCardFooter();
        guiHelper.EndCard();
        guiHelper.Label("Code: guiHelper.BeginCard(); ... guiHelper.EndCard(); with sections.", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }

    void DrawCheckboxDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        checkboxValue = guiHelper.Checkbox("Accept terms and conditions", checkboxValue);
        guiHelper.Label($"The checkbox is {(checkboxValue ? "checked" : "unchecked")}");
        guiHelper.Label("Code: checkboxValue = guiHelper.Checkbox(label, value);");
        GUILayout.EndVertical();
    }

    void DrawLabelDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        guiHelper.Label("This is a default label.");
        guiHelper.SecondaryLabel("This is a secondary label.");
        guiHelper.MutedLabel("This is a muted label.");
        guiHelper.DestructiveLabel("This is a destructive label.");
        guiHelper.Label("Code: guiHelper.Label(text, variant);");
        guiHelper.HorizontalSeparator();
        /*
        guiHelper.Label(new Rect(10, 10, 200, 30), "Label in Rect");
        guiHelper.Label("Code: guiHelper.Label(Rect, text, variant, disabled);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        */
        GUILayout.EndVertical();
    }

    void DrawLayoutDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        guiHelper.Label("Horizontal Group");
        guiHelper.BeginHorizontalGroup();
        guiHelper.Button("One");
        guiHelper.Button("Two");
        guiHelper.Button("Three");
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.BeginHorizontalGroup(); ... guiHelper.EndHorizontalGroup();");

        guiHelper.Label("Vertical Group");
        guiHelper.BeginVerticalGroup();
        guiHelper.Checkbox("Option A", false);
        guiHelper.Checkbox("Option B", true);
        guiHelper.EndVerticalGroup();
        guiHelper.Label("Code: guiHelper.BeginVerticalGroup(); ... guiHelper.EndVerticalGroup();");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Add Space");
        guiHelper.Label("Text above space.");
        guiHelper.AddSpace(20);
        guiHelper.Label("Text below space (20 pixels).");
        guiHelper.Label("Code: guiHelper.AddSpace(pixels);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }

    void DrawProgressDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        guiHelper.Progress(progressValue, 300);
        guiHelper.DrawSlider(300, "Progress", ref progressValue, 0, 1);
        guiHelper.Label("Code: guiHelper.Progress(value, width);");
        guiHelper.HorizontalSeparator();
        /*
        guiHelper.Progress(new Rect(10, 10, 200, 20), progressRectValue);
        guiHelper.Label($"Progress in Rect Value: {progressRectValue:F2}");
        guiHelper.Label("Code: guiHelper.Progress(Rect, value);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        */
        guiHelper.LabeledProgress("Download", 0.75f, 300, showPercentage: true);
        guiHelper.Label("Code: guiHelper.LabeledProgress(label, value, width, showPercentage);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.CircularProgress(0.6f, 50);
        guiHelper.Label("Code: guiHelper.CircularProgress(value, size);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }

    void DrawSeparatorDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        guiHelper.Label("Above");
        guiHelper.HorizontalSeparator();
        guiHelper.Label("Below");
        guiHelper.Label("Code: guiHelper.HorizontalSeparator();", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Generic Separator (Horizontal, Decorative)");
        guiHelper.Separator(SeparatorOrientation.Horizontal, true);
        guiHelper.Label("Code: guiHelper.Separator(Horizontal, true);");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Vertical Separator");
        guiHelper.BeginHorizontalGroup();
        guiHelper.Label("Left");
        guiHelper.VerticalSeparator();
        guiHelper.Label("Right");
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.VerticalSeparator();", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        /*
        guiHelper.Label("Separator in Rect");
        guiHelper.Separator(new Rect(10, 10, 200, 2), SeparatorOrientation.Horizontal);
        guiHelper.Label("Code: guiHelper.Separator(Rect, Horizontal);");
        guiHelper.HorizontalSeparator();
        */
        guiHelper.Label("Separator with Spacing");
        guiHelper.Label("Text above spaced separator.");
        guiHelper.SeparatorWithSpacing(SeparatorOrientation.Horizontal, 10, 10);
        guiHelper.Label("Text below spaced separator.");
        guiHelper.Label("Code: guiHelper.SeparatorWithSpacing(Horizontal, 10, 10);");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Labeled Separator");
        guiHelper.LabeledSeparator("OR");
        guiHelper.Label("Code: guiHelper.LabeledSeparator(\"OR\");");
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }

    void DrawSkeletonDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        guiHelper.BeginHorizontalGroup();
        guiHelper.Skeleton(50, 50, SkeletonVariant.Circular);
        guiHelper.BeginVerticalGroup();
        guiHelper.Skeleton(200, 20, SkeletonVariant.Default, SkeletonSize.Default);
        guiHelper.Skeleton(150, 20, SkeletonVariant.Default, SkeletonSize.Default);
        guiHelper.EndVerticalGroup();
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Skeleton(width, height, variant);");
        GUILayout.EndVertical();
    }

    void DrawSliderDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        guiHelper.DrawSlider(300, "Volume", ref sliderValue, 0, 100);
        guiHelper.Label($"Current Value: {sliderValue:F2}");
        guiHelper.Label("Code: guiHelper.DrawSlider(width, label, ref value, min, max);");
        guiHelper.HorizontalSeparator();

        guiHelper.DrawIntSlider(300, "Integer Value", ref intSliderValue, 0, 100);
        guiHelper.Label($"Current Integer Value: {intSliderValue}");
        guiHelper.Label("Code: guiHelper.DrawIntSlider(width, label, ref value, min, max);");
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }

    void DrawSwitchDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        switchValue = guiHelper.Switch("Dark Mode", switchValue);
        guiHelper.Label($"Dark mode is {(switchValue ? "on" : "off")}");
        guiHelper.Label("Code: switchValue = guiHelper.Switch(label, value);");
        GUILayout.EndVertical();
    }

    void DrawTableDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        string[] headers = { "Invoice", "Status", "Method", "Amount" };
        string[,] data = {
            { "INV001", "Paid", "Credit Card", "$250.00" },
            { "INV002", "Pending", "PayPal", "$150.00" },
            { "INV003", "Unpaid", "Bank Transfer", "$350.00" }
        };
        guiHelper.Table(headers, data);
        guiHelper.Label("Code: guiHelper.Table(headers, data, variant, size);");
        GUILayout.EndVertical();
    }



    void DrawToggleDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        string[] toggleLabels = { "Left", "Center", "Right" };
        selectedToggle = guiHelper.ToggleGroup(toggleLabels, selectedToggle);
        guiHelper.Label($"Selected alignment: {toggleLabels[selectedToggle]}");
        guiHelper.Label("Code: selectedToggle = guiHelper.ToggleGroup(labels, selectedIndex);");
        guiHelper.HorizontalSeparator();

        guiHelper.DrawToggle(200, "Draw Toggle", ref drawToggleValue, (val) => drawToggleValue = val);
        guiHelper.Label($"Draw Toggle Value: {drawToggleValue}");
        guiHelper.Label("Code: guiHelper.DrawToggle(width, label, ref value, onToggle);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        drawCheckboxValue = guiHelper.DrawCheckbox(200, "Draw Checkbox", ref drawCheckboxValue);
        guiHelper.Label($"Draw Checkbox Value: {drawCheckboxValue}");
        guiHelper.Label("Code: guiHelper.DrawCheckbox(width, label, ref value);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        string[] selectionGridOptions = { "Option A", "Option B", "Option C" };
        selectionGridValue = guiHelper.DrawSelectionGrid(300, "Select an Option", selectionGridValue, selectionGridOptions, 3);
        guiHelper.Label($"Selected Grid Option: {selectionGridOptions[selectionGridValue]}");
        guiHelper.Label("Code: guiHelper.DrawSelectionGrid(width, label, selected, texts, xCount);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();



        outlineToggleValue = guiHelper.OutlineToggle("Outline Toggle", outlineToggleValue);
        guiHelper.Label($"Outline Toggle Value: {outlineToggleValue}");
        guiHelper.Label("Code: guiHelper.OutlineToggle(text, value);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        smallToggleValue = guiHelper.SmallToggle("Small Toggle", smallToggleValue);
        guiHelper.Label($"Small Toggle Value: {smallToggleValue}");
        guiHelper.Label("Code: guiHelper.SmallToggle(text, value);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        largeToggleValue = guiHelper.LargeToggle("Large Toggle", largeToggleValue);
        guiHelper.Label($"Large Toggle Value: {largeToggleValue}");
        guiHelper.Label("Code: guiHelper.LargeToggle(text, value);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Multi Toggle Group");
        guiHelper.Label("Multi Toggle Group");
        multiToggleGroupValues = guiHelper.MultiToggleGroup(new string[] { "Red", "Green", "Blue" }, multiToggleGroupValues);
        guiHelper.Label($"Multi Toggle Group Values: {string.Join(", ", multiToggleGroupValues)}");
        guiHelper.Label("Code: guiHelper.MultiToggleGroup(labels, values);");
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }

    void DrawVisualDemos()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        guiHelper.DrawProgressBar(300, "Loading Progress", visualProgressBarValue, Color.green);
        guiHelper.Label("Code: guiHelper.DrawProgressBar(width, label, progress, color);");
        guiHelper.HorizontalSeparator();

        guiHelper.DrawBox(300, "This is a custom drawn box.", 50);
        guiHelper.Label("Code: guiHelper.DrawBox(width, content, height);");
        guiHelper.HorizontalSeparator();

        guiHelper.DrawSeparator(300, 5);
        guiHelper.Label("Code: guiHelper.DrawSeparator(width, height);");
        guiHelper.HorizontalSeparator();

        guiHelper.RenderInstructions("Follow these instructions carefully.");
        guiHelper.Label("Code: guiHelper.RenderInstructions(text);");
        guiHelper.HorizontalSeparator();
        GUILayout.EndVertical();
    }
}