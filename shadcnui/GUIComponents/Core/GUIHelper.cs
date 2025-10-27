using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Controls;
using shadcnui.GUIComponents.Data;
using shadcnui.GUIComponents.Display;
using shadcnui.GUIComponents.Layout;
using UnityEngine;
using static shadcnui.GUIComponents.Layout.Tabs;
using Input = shadcnui.GUIComponents.Controls.Input;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core
{
    public class GUIHelper
    {
        private readonly Dictionary<Type, BaseComponent> _componentInstances;
        private readonly IComponentResolver _componentResolver;

        #region Internal Settings
        internal bool animationsEnabled = false;
        internal float animationSpeed = 12f;
        internal bool glowEffectsEnabled = true;
        internal bool particleEffectsEnabled = false;
        internal float backgroundAlpha = 0.9f;
        internal int fontSize = 12;
        internal int cornerRadius = 14;
        internal bool hoverEffectsEnabled = true;
        internal bool fadeTransitionsEnabled = true;
        internal float glowIntensity = 16.5f;
        internal bool smoothAnimationsEnabled = true;
        public float uiScale = 1f;
        #endregion

        #region Animation State
        private float menuAlpha = 0f;
        private float menuScale = 0.8f;
        private float titleGlow = 0f;
        private int hoveredButton = -1;
        private float[] buttonGlowEffects = new float[20];
        private float backgroundPulse = 0f;
        private Vector2 mousePos;
        private float particleTime = 0f;
        private float[] inputFieldGlow = new float[10];
        private int focusedField = -1;
        #endregion

        #region Component Instances
        private StyleManager styleManager;
        private AnimationManager animationManager;
        private shadcnui.GUIComponents.Layout.Layout layoutComponents;
        #endregion

        private bool initialized = false;

        public GUIHelper()
        {
            _componentInstances = new Dictionary<Type, BaseComponent>();
            _componentResolver = new ComponentResolver(_componentInstances);
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            try
            {
                GUILogger.LogInfo("Initializing GUIHelper components", "GUIHelper.InitializeComponents");

                styleManager = new StyleManager(this);
                animationManager = new AnimationManager(this);

                _componentInstances[typeof(Input)] = new Input(this);
                _componentInstances[typeof(TextArea)] = new TextArea(this);
                _componentInstances[typeof(Checkbox)] = new Checkbox(this);
                _componentInstances[typeof(Switch)] = new Switch(this);
                _componentInstances[typeof(Slider)] = new Slider(this);
                _componentInstances[typeof(Button)] = new Button(this);
                _componentInstances[typeof(Toggle)] = new Toggle(this);

                layoutComponents = new shadcnui.GUIComponents.Layout.Layout(this);
                _componentInstances[typeof(Card)] = new Card(this);
                _componentInstances[typeof(Separator)] = new Separator(this);
                _componentInstances[typeof(Label)] = new Label(this);
                _componentInstances[typeof(Tabs)] = new Tabs(this);
                _componentInstances[typeof(MenuBar)] = new MenuBar(this);
                _componentInstances[typeof(Progress)] = new Progress(this);
                _componentInstances[typeof(Badge)] = new Badge(this);
                _componentInstances[typeof(Table)] = new Table(this);
                _componentInstances[typeof(shadcnui.GUIComponents.Display.Avatar)] = new shadcnui.GUIComponents.Display.Avatar(this);
                _componentInstances[typeof(Chart)] = new Chart(this);
                _componentInstances[typeof(DataTable)] = new DataTable(this);
                _componentInstances[typeof(Calendar)] = new Calendar(this);
                _componentInstances[typeof(Select)] = new Select(this);
                _componentInstances[typeof(DatePicker)] = new DatePicker(this);
                _componentInstances[typeof(Dialog)] = new Dialog(this);
                _componentInstances[typeof(DropdownMenu)] = new DropdownMenu(this);
                _componentInstances[typeof(Popover)] = new Popover(this);

                GUILogger.LogInfo("GUIHelper components initialized successfully", "GUIHelper.InitializeComponents");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "InitializeComponents", "GUIHelper");
                styleManager = null;
                animationManager = null;
            }
        }

        private T GetComponent<T>()
            where T : BaseComponent
        {
            try
            {
                return _componentResolver.GetComponent<T>();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, $"GetComponent<{typeof(T).Name}>", "GUIHelper");
                return null;
            }
        }

        public StyleManager GetStyleManager() => styleManager;

        public Calendar GetCalendarComponents() => GetComponent<Calendar>();

        public Chart GetChartComponents() => GetComponent<Chart>();

        public Settings CreateSetting()
        {
            try
            {
                return new Settings(this);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "CreateSetting", "GUIHelper");
                return null;
            }
        }

        public void UpdateAnimations(bool isOpen)
        {
            try
            {
                if (!animationsEnabled)
                {
                    menuAlpha = 1f;
                    menuScale = uiScale;
                    titleGlow = 1f;
                    backgroundPulse = backgroundAlpha;
                    hoveredButton = -1;
                    focusedField = -1;
                    particleTime = 0f;
                    return;
                }
                animationManager?.UpdateAnimations(isOpen, ref menuAlpha, ref menuScale, ref titleGlow, ref backgroundPulse, ref hoveredButton, buttonGlowEffects, inputFieldGlow, ref focusedField, ref particleTime, ref mousePos);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "UpdateAnimations", "GUIHelper");
            }
        }

        public bool BeginAnimatedGUI()
        {
            try
            {
                if (!initialized && styleManager != null)
                {
                    styleManager.InitializeGUI();
                    initialized = true;
                }

                float currentMenuAlpha = menuAlpha;
                float currentBackgroundPulse = backgroundPulse;
                Vector2 currentMousePos = mousePos;

                float screenWidth = Screen.width * 2f;
                float screenHeight = Screen.height * 2f;
                float offsetX = (screenWidth - Screen.width) / 2f;
                float offsetY = (screenHeight - Screen.height) / 2f;

                Rect scaledBackgroundRect = new Rect(-offsetX, -offsetY, screenWidth, screenHeight);

                float targetPulse = animationsEnabled ? currentBackgroundPulse : 1f;
                float smoothPulse = Mathf.Lerp(targetPulse, currentBackgroundPulse, Time.deltaTime * animationSpeed);
                float bgAlpha = Mathf.Clamp(currentMenuAlpha * backgroundAlpha * smoothPulse, 0.05f, 1f);

                Color baseCol = ThemeManager.Instance.CurrentTheme.PrimaryColor;
                Color backgroundColor = new Color(baseCol.r, baseCol.g, baseCol.b, bgAlpha);

                GUI.color = backgroundColor;
                GUI.DrawTexture(scaledBackgroundRect, Texture2D.whiteTexture);
                GUI.color = Color.white;

                return animationManager?.BeginAnimatedGUI(currentMenuAlpha, currentBackgroundPulse, currentMousePos) ?? true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "BeginAnimatedGUI", "GUIHelper");
                return true;
            }
        }

        public void EndAnimatedGUI()
        {
            try
            {
                animationManager?.EndAnimatedGUI();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "EndAnimatedGUI", "GUIHelper");
            }
        }

        public void Cleanup()
        {
            try
            {
                GUILogger.LogInfo("Starting GUIHelper cleanup", "GUIHelper.Cleanup");
                styleManager?.Cleanup();
                animationManager?.Cleanup();
                GUILogger.LogInfo("GUIHelper cleanup completed", "GUIHelper.Cleanup");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Cleanup", "GUIHelper");
            }
        }

        internal float GetMenuAlpha() => menuAlpha;

        internal Vector2 GetMousePos() => mousePos;

        internal float GetParticleTime() => particleTime;

        #region Input Components
        public void DrawSectionHeader(string title) => TryExecute(() => GetComponent<Input>()?.DrawSectionHeader(title), "DrawSectionHeader");

        public void RenderLabel(string text, int width = -1) => TryExecute(() => GetComponent<Input>()?.RenderLabel(text, width), "RenderLabel");

        public string DrawPasswordField(float windowWidth, string label, ref string password, char maskChar = '*')
        {
            try
            {
                return GetComponent<Input>()?.DrawPasswordField(windowWidth, label, ref password, maskChar) ?? password;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawPasswordField", "GUIHelper");
                return password;
            }
        }

        public void DrawTextArea(float windowWidth, string label, ref string text, int maxLength, float height = 60f)
        {
            try
            {
                GetComponent<Input>()?.DrawTextArea(windowWidth, label, ref text, maxLength, height);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawTextArea", "GUIHelper");
            }
        }
        #endregion

        #region Button Components
        public bool DrawButtonVariant(string text, ButtonVariant variant, ButtonSize size) =>
            TryExecute(
                () =>
                {
                    GUIStyle btnStyle = styleManager.GetButtonStyle(variant, size);
                    GUILayoutOption[] options = GetButtonSizeOptions(size);
                    return GUILayout.Button(text ?? "Button", btnStyle ?? GUI.skin.button, options);
                },
                "DrawButtonVariant"
            );

        public bool DrawButton(float windowWidth, string text, Action onClick, float opacity = 1f) =>
            TryExecute(() => GetComponent<Button>()?.DrawButton(text, ButtonVariant.Default, ButtonSize.Default, onClick, false, opacity, new GUILayoutOption[] { GUILayout.Width(windowWidth) }) ?? false, "DrawButton");

        public bool DrawFixedButton(string text, float width, float height, Action onClick = null, float opacity = 1f) =>
            TryExecute(() => GetComponent<Button>()?.DrawButton(text, ButtonVariant.Default, ButtonSize.Default, onClick, false, opacity, new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) }) ?? false, "DrawFixedButton");

        public bool Button(string text, ButtonVariant variant = ButtonVariant.Default, ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Button>()?.DrawButton(text, variant, size, onClick, disabled, opacity, options ?? Array.Empty<GUILayoutOption>()) ?? false, "Button");

        public bool DestructiveButton(string text, Action onClick, ButtonSize size = ButtonSize.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, ButtonVariant.Destructive, size, onClick, false, opacity, options);

        public bool OutlineButton(string text, Action onClick, ButtonSize size = ButtonSize.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, ButtonVariant.Outline, size, onClick, false, opacity, options);

        public bool SecondaryButton(string text, Action onClick, ButtonSize size = ButtonSize.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, ButtonVariant.Secondary, size, onClick, false, opacity, options);

        public bool GhostButton(string text, Action onClick, ButtonSize size = ButtonSize.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, ButtonVariant.Ghost, size, onClick, false, opacity, options);

        public bool LinkButton(string text, Action onClick, ButtonSize size = ButtonSize.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, ButtonVariant.Link, size, onClick, false, opacity, options);

        public bool SmallButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, variant, ButtonSize.Small, onClick, false, opacity, options);

        public bool LargeButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, variant, ButtonSize.Large, onClick, false, opacity, options);

        public bool IconButton(string text, Action onClick, ButtonVariant variant = ButtonVariant.Default, float opacity = 1f, params GUILayoutOption[] options) => Button(text, variant, ButtonSize.Icon, onClick, false, opacity, options);

        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f) => TryExecute(() => GetComponent<Button>()?.ButtonGroup(drawButtons, horizontal, spacing), "ButtonGroup");

        private GUILayoutOption[] GetButtonSizeOptions(ButtonSize size) =>
            size switch
            {
                ButtonSize.Small => new GUILayoutOption[] { GUILayout.Height(24 * uiScale) },
                ButtonSize.Large => new GUILayoutOption[] { GUILayout.Height(40 * uiScale) },
                ButtonSize.Icon => new GUILayoutOption[] { GUILayout.Width(36 * uiScale), GUILayout.Height(36 * uiScale) },
                _ => new GUILayoutOption[] { GUILayout.Height(30 * uiScale) },
            };
        #endregion

        #region Toggle Components
        public bool Toggle(string text, bool value, ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    bool newValue = GetComponent<Toggle>()?.DrawToggle(text, value, variant, size, onToggle, disabled, options) ?? value;
                    if (newValue != value)
                        onToggle?.Invoke(newValue);
                    return newValue;
                },
                "Toggle"
            );

        public bool Toggle(Rect rect, string text, bool value, ToggleVariant variant = ToggleVariant.Default, ToggleSize size = ToggleSize.Default, Action<bool> onToggle = null, bool disabled = false) =>
            TryExecute(() => GetComponent<Toggle>()?.DrawToggle(rect, text, value, variant, size, onToggle, disabled) ?? value, "Toggle");

        public bool OutlineToggle(string text, bool value, Action<bool> onToggle = null, ToggleSize size = ToggleSize.Default, params GUILayoutOption[] options) => Toggle(text, value, ToggleVariant.Outline, size, onToggle, false, options);

        public bool SmallToggle(string text, bool value, Action<bool> onToggle = null, ToggleVariant variant = ToggleVariant.Default, params GUILayoutOption[] options) => Toggle(text, value, variant, ToggleSize.Small, onToggle, false, options);

        public bool LargeToggle(string text, bool value, Action<bool> onToggle = null, ToggleVariant variant = ToggleVariant.Default, params GUILayoutOption[] options) => Toggle(text, value, variant, ToggleSize.Large, onToggle, false, options);
        #endregion

        #region Card Components
        public void BeginCard(float width = -1, float height = -1) => TryExecute(() => GetComponent<Card>()?.BeginCard(width, height), "BeginCard");

        public void EndCard() => TryExecute(() => GetComponent<Card>()?.EndCard(), "EndCard");

        public void DrawCard(string title, string description, string content, Action footerContent = null, float width = -1, float height = -1) => TryExecute(() => GetComponent<Card>()?.DrawCard(title, description, content, footerContent, width, height), "DrawCard");

        public void DrawCardWithImage(Texture2D image, string title, string description, string content, Action footerContent = null, float width = -1, float height = -1) =>
            TryExecute(() => GetComponent<Card>()?.DrawCardWithImage(image, title, description, content, footerContent, width, height), "DrawCardWithImage");

        public void DrawCardWithAvatar(Texture2D avatar, string title, string subtitle, string content, Action footerContent = null, float width = -1, float height = -1) =>
            TryExecute(() => GetComponent<Card>()?.DrawCardWithAvatar(avatar, title, subtitle, content, footerContent, width, height), "DrawCardWithAvatar");

        public void DrawCardWithHeader(string title, string description, Action header, string content, Action footerContent = null, float width = -1, float height = -1) =>
            TryExecute(() => GetComponent<Card>()?.DrawCardWithHeader(title, description, header, content, footerContent, width, height), "DrawCardWithHeader");

        public void DrawSimpleCard(string content, float width = -1, float height = -1) => TryExecute(() => GetComponent<Card>()?.DrawSimpleCard(content, width, height), "DrawSimpleCard");

        public void CardHeader(Action content) => GetComponent<Card>()?.CardHeader(content);

        public void CardTitle(string title) => GetComponent<Card>()?.CardTitle(title);

        public void CardDescription(string description) => GetComponent<Card>()?.CardDescription(description);

        public void CardContent(Action content) => GetComponent<Card>()?.CardContent(content);

        public void CardFooter(Action content) => GetComponent<Card>()?.CardFooter(content);
        #endregion

        #region Slider Components
        public void DrawSlider(float windowWidth, string label, ref float value, float minValue, float maxValue)
        {
            try
            {
                GetComponent<Slider>()?.DrawSlider(windowWidth, label, ref value, minValue, maxValue);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawSlider", "GUIHelper");
            }
        }

        public void DrawIntSlider(float windowWidth, string label, ref int value, int minValue, int maxValue)
        {
            try
            {
                GetComponent<Slider>()?.DrawIntSlider(windowWidth, label, ref value, minValue, maxValue);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawIntSlider", "GUIHelper");
            }
        }
        #endregion

        #region Layout Components

        public Vector2 DrawScrollView(Vector2 scrollPosition, Action drawContent, params GUILayoutOption[] options) => TryExecute(() => layoutComponents?.DrawScrollView(scrollPosition, drawContent, options) ?? scrollPosition, "DrawScrollView");

        public void BeginHorizontalGroup() => TryExecute(() => layoutComponents?.BeginHorizontalGroup(), "BeginHorizontalGroup");

        public void EndHorizontalGroup() => TryExecute(() => layoutComponents?.EndHorizontalGroup(), "EndHorizontalGroup");

        public void BeginVerticalGroup(params GUILayoutOption[] options) => TryExecute(() => layoutComponents?.BeginVerticalGroup(options), "BeginVerticalGroup");

        public void EndVerticalGroup() => TryExecute(() => layoutComponents?.EndVerticalGroup(), "EndVerticalGroup");

        public void AddSpace(float pixels) => TryExecute(() => layoutComponents?.AddSpace(pixels), "AddSpace");
        #endregion

        #region Label Components
        public void Label(string text, LabelVariant variant = LabelVariant.Default, bool disabled = false, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Label>()?.DrawLabel(text, variant, disabled, options), "Label");

        public void Label(Rect rect, string text, LabelVariant variant = LabelVariant.Default, bool disabled = false) => TryExecute(() => GetComponent<Label>()?.DrawLabel(rect, text, variant, disabled), "Label");

        public void SecondaryLabel(string text, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Label>()?.SecondaryLabel(text, options), "SecondaryLabel");

        public void MutedLabel(string text, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Label>()?.MutedLabel(text, options), "MutedLabel");

        public void DestructiveLabel(string text, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Label>()?.DestructiveLabel(text, options), "DestructiveLabel");
        #endregion

        #region Progress Components
        public void Progress(float value, float width = -1, float height = -1, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Progress>()?.DrawProgress(value, width, height, options), "Progress");

        public void Progress(Rect rect, float value) => TryExecute(() => GetComponent<Progress>()?.DrawProgress(rect, value), "Progress");

        public void LabeledProgress(string label, float value, float width = -1, float height = -1, bool showPercentage = true, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Progress>()?.LabeledProgress(label, value, width, height, showPercentage, options), "LabeledProgress");

        public void CircularProgress(float value, float size = 32f, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Progress>()?.CircularProgress(value, size, options), "CircularProgress");
        #endregion

        #region Avatar Components
        public void Avatar(Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.DrawAvatar(image, fallbackText, size, shape, options), "Avatar");

        public void Avatar(Rect rect, Texture2D image, string fallbackText, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle) => TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.DrawAvatar(rect, image, fallbackText, size, shape), "Avatar");

        public void AvatarWithStatus(Texture2D image, string fallbackText, bool isOnline, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.AvatarWithStatus(image, fallbackText, isOnline, size, shape, options), "AvatarWithStatus");

        public void AvatarWithName(Texture2D image, string fallbackText, string name, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, bool showNameBelow = false, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.AvatarWithName(image, fallbackText, name, size, shape, showNameBelow, options), "AvatarWithName");

        public void AvatarWithBorder(Texture2D image, string fallbackText, Color borderColor, AvatarSize size = AvatarSize.Default, AvatarShape shape = AvatarShape.Circle, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<shadcnui.GUIComponents.Display.Avatar>()?.AvatarWithBorder(image, fallbackText, borderColor, size, shape, options), "AvatarWithBorder");
        #endregion

        #region Table Components
        public void Table(string[] headers, string[,] data, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Table>()?.DrawTable(headers, data, variant, size, options), "Table");

        public void Table(Rect rect, string[] headers, string[,] data, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default) => TryExecute(() => GetComponent<Table>()?.DrawTable(rect, headers, data, variant, size), "Table");

        public void SortableTable(string[] headers, string[,] data, ref int[] sortColumns, ref bool[] sortAscending, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<int, bool> onSort = null, params GUILayoutOption[] options)
        {
            try
            {
                GetComponent<Table>()?.SortableTable(headers, data, ref sortColumns, ref sortAscending, variant, size, onSort, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SortableTable", "GUIHelper");
            }
        }

        public void SelectableTable(string[] headers, string[,] data, ref bool[] selectedRows, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<int, bool> onSelectionChange = null, params GUILayoutOption[] options)
        {
            try
            {
                GetComponent<Table>()?.SelectableTable(headers, data, ref selectedRows, variant, size, onSelectionChange, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SelectableTable", "GUIHelper");
            }
        }

        public void CustomTable(string[] headers, object[,] data, Action<object, int, int> cellRenderer, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Table>()?.CustomTable(headers, data, cellRenderer, variant, size, options), "CustomTable");

        public void PaginatedTable(string[] headers, string[,] data, ref int currentPage, int pageSize, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<int> onPageChange = null, params GUILayoutOption[] options)
        {
            try
            {
                GetComponent<Table>()?.PaginatedTable(headers, data, ref currentPage, pageSize, variant, size, onPageChange, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "PaginatedTable", "GUIHelper");
            }
        }

        public void SearchableTable(string[] headers, string[,] data, ref string searchQuery, ref string[,] filteredData, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, Action<string> onSearch = null, params GUILayoutOption[] options)
        {
            try
            {
                GetComponent<Table>()?.SearchableTable(headers, data, ref searchQuery, ref filteredData, variant, size, onSearch, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SearchableTable", "GUIHelper");
            }
        }

        public void ResizableTable(string[] headers, string[,] data, ref float[] columnWidths, TableVariant variant = TableVariant.Default, TableSize size = TableSize.Default, params GUILayoutOption[] options)
        {
            try
            {
                GetComponent<Table>()?.ResizableTable(headers, data, ref columnWidths, variant, size, options);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "ResizableTable", "GUIHelper");
            }
        }

        #endregion

        #region Calendar Components
        public void Calendar() => TryExecute(() => GetComponent<Calendar>()?.DrawCalendar(), "Calendar");
        #endregion

        #region DropdownMenu Components
        public void DropdownMenu(DropdownMenuConfig config) => TryExecute(() => GetComponent<DropdownMenu>()?.Draw(config), "DropdownMenu");
        #endregion

        #region Popover Components
        public void Popover(Action content) => TryExecute(() => GetComponent<Popover>()?.DrawPopover(content), "Popover");

        public void OpenPopover() => GetComponent<Popover>()?.Open();

        public void ClosePopover() => GetComponent<Popover>()?.Close();

        public bool IsPopoverOpen() => GetComponent<Popover>()?.IsOpen ?? false;
        #endregion

        #region Select Components
        public int Select(string[] items, int selectedIndex) => TryExecute(() => GetComponent<Select>()?.DrawSelect(items, selectedIndex) ?? selectedIndex, "Select");

        public void OpenSelect() => GetComponent<Select>()?.Open();

        public void CloseSelect() => GetComponent<Select>()?.Close();

        public bool IsSelectOpen() => GetComponent<Select>()?.IsOpen ?? false;
        #endregion

        #region Dialog Components
        public void DrawDialog(string dialogId, Action content, float width = 400, float height = 300) => TryExecute(() => GetComponent<Dialog>()?.DrawDialog(dialogId, content, width, height), "DrawDialog");

        public void DrawDialog(string dialogId, string title, string description, Action content, Action footer = null, float width = 400, float height = 300) => TryExecute(() => GetComponent<Dialog>()?.DrawDialog(dialogId, title, description, content, footer, width, height), "DrawDialog");

        public void OpenDialog(string dialogId) => GetComponent<Dialog>()?.Open(dialogId);

        public void CloseDialog() => GetComponent<Dialog>()?.Close();

        public bool IsDialogOpen() => GetComponent<Dialog>()?.IsOpen ?? false;

        public bool DrawDialogTrigger(string label, ButtonVariant variant = ButtonVariant.Default, ButtonSize size = ButtonSize.Default)
        {
            try
            {
                return GetComponent<Dialog>()?.DrawDialogTrigger(label, variant, size) ?? false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "DrawDialogTrigger", "GUIHelper");
                return false;
            }
        }

        public void DrawDialogHeader(string title, string description = null) => TryExecute(() => GetComponent<Dialog>()?.DrawDialogHeader(title, description), "DrawDialogHeader");

        public void DrawDialogContent(Action content) => TryExecute(() => GetComponent<Dialog>()?.DrawDialogContent(content), "DrawDialogContent");

        public void DrawDialogFooter(Action footer) => TryExecute(() => GetComponent<Dialog>()?.DrawDialogFooter(footer), "DrawDialogFooter");
        #endregion

        #region DatePicker Components
        public DateTime? DatePicker(string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options) => TryExecute(() => GetComponent<DatePicker>()?.DrawDatePicker(placeholder, selectedDate, id, options) ?? selectedDate, "DatePicker");

        public DateTime? DatePickerWithLabel(string label, string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<DatePicker>()?.DrawDatePickerWithLabel(label, placeholder, selectedDate, id, options) ?? selectedDate, "DatePickerWithLabel");

        public DateTime? DateRangePicker(string placeholder, DateTime? startDate, DateTime? endDate, string id = "daterange", params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<DatePicker>()?.DrawDateRangePicker(placeholder, startDate, endDate, id, options) ?? startDate, "DateRangePicker");

        public void CloseDatePicker(string id) => TryExecute(() => GetComponent<DatePicker>()?.CloseDatePicker(id), "CloseDatePicker");

        public bool IsDatePickerOpen(string id) => TryExecute(() => GetComponent<DatePicker>()?.IsDatePickerOpen(id) ?? false, "IsDatePickerOpen");

        #endregion

        #region DataTable Components
        public void DrawDataTable(string id, List<DataTableColumn> columns, List<DataTableRow> data, bool showPagination = true, bool showSearch = true, bool showSelection = true, bool showColumnToggle = false, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<DataTable>()?.DrawDataTable(id, columns, data, showPagination, showSearch, showSelection, showColumnToggle, options), "DrawDataTable");

        public DataTableColumn CreateDataTableColumn(string id, string header, string accessorKey = null) => new DataTableColumn(id, header, accessorKey);

        public DataTableRow CreateDataTableRow(string id) => new DataTableRow(id);

        public List<string> GetSelectedRows(string tableId) => TryExecute(() => GetComponent<DataTable>()?.GetSelectedRows(tableId) ?? new List<string>(), "GetSelectedRows");

        public void ClearSelection(string tableId) => TryExecute(() => GetComponent<DataTable>()?.ClearSelection(tableId), "ClearSelection");
        #endregion

        #region Chart Components
        public void Chart(ChartConfig config) => TryExecute(() => GetComponent<Chart>()?.DrawChart(config), "Chart");

        public void Chart(List<ChartSeries> chartSeries, ChartType chartType, Vector2 size, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    var config = new ChartConfig(chartSeries, chartType) { Size = size, Options = options };
                    GetComponent<Chart>()?.DrawChart(config);
                },
                "Chart"
            );
        #endregion

        #region Separator Components
        public void Separator(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, bool decorative = true, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Separator>()?.DrawSeparator(orientation, decorative, options), "Separator");

        public void HorizontalSeparator(params GUILayoutOption[] options) => TryExecute(() => GetComponent<Separator>()?.HorizontalSeparator(options), "HorizontalSeparator");

        public void VerticalSeparator(params GUILayoutOption[] options) => TryExecute(() => GetComponent<Separator>()?.VerticalSeparator(options), "VerticalSeparator");

        public void Separator(Rect rect, SeparatorOrientation orientation = SeparatorOrientation.Horizontal) => TryExecute(() => GetComponent<Separator>()?.DrawSeparator(rect, orientation), "Separator");

        public void SeparatorWithSpacing(SeparatorOrientation orientation = SeparatorOrientation.Horizontal, float spacingBefore = 8f, float spacingAfter = 8f, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Separator>()?.SeparatorWithSpacing(orientation, spacingBefore, spacingAfter, options), "SeparatorWithSpacing");

        public void LabeledSeparator(string text, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Separator>()?.LabeledSeparator(text, options), "LabeledSeparator");
        #endregion

        #region Tabs Components
        public int DrawTabs(string[] tabNames, int selectedIndex, Action<int> onTabChange, int maxLines = 1, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    var config = new Tabs.TabsConfig(tabNames, selectedIndex)
                    {
                        OnTabChange = onTabChange,
                        MaxLines = maxLines,
                        Options = options,
                    };

                    var result = GetComponent<Tabs>()?.Draw(config);
                    return result ?? selectedIndex;
                },
                "DrawTabs"
            );

        public int DrawTabs(string[] tabNames, int selectedIndex, Action content, int maxLines = 1, TabPosition position = TabPosition.Top, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    var config = new Tabs.TabsConfig(tabNames, selectedIndex)
                    {
                        Content = content,
                        MaxLines = maxLines,
                        Position = position,
                        Options = options,
                    };

                    var result = GetComponent<Tabs>()?.Draw(config);
                    return result ?? selectedIndex;
                },
                "DrawTabs"
            );

        public void BeginTabContent() => TryExecute(() => GetComponent<Tabs>()?.BeginTabContent(), "BeginTabContent");

        public void EndTabContent() => TryExecute(() => GetComponent<Tabs>()?.EndTabContent(), "EndTabContent");

        public int TabsWithContent(Tabs.TabConfig[] tabConfigs, int selectedIndex, Action<int> onTabChange = null, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    if (tabConfigs == null || tabConfigs.Length == 0)
                        return selectedIndex;

                    var tabNames = new string[tabConfigs.Length];
                    for (var i = 0; i < tabConfigs.Length; i++)
                        tabNames[i] = tabConfigs[i].Name;

                    var config = new Tabs.TabsConfig(tabNames, selectedIndex)
                    {
                        OnTabChange = onTabChange,
                        MaxLines = 1,
                        Position = Tabs.TabPosition.Top,
                        Options = options,
                    };

                    var newSelectedIndex = GetComponent<Tabs>()?.Draw(config) ?? selectedIndex;

                    if (newSelectedIndex >= 0 && newSelectedIndex < tabConfigs.Length)
                    {
                        BeginTabContent();
                        tabConfigs[newSelectedIndex].Content?.Invoke();
                        EndTabContent();
                    }

                    return newSelectedIndex;
                },
                "TabsWithContent"
            );

        public int VerticalTabs(string[] tabNames, int selectedIndex, Action content, float tabWidth = 120f, int maxLines = 1, TabSide side = TabSide.Left, params GUILayoutOption[] options) =>
            TryExecute(
                () =>
                {
                    var position = side == Tabs.TabSide.Left ? Tabs.TabPosition.Left : Tabs.TabPosition.Right;
                    var config = new Tabs.TabsConfig(tabNames, selectedIndex)
                    {
                        Content = content,
                        TabWidth = tabWidth,
                        MaxLines = maxLines,
                        Side = side,
                        Position = position,
                        Options = options,
                    };
                    return GetComponent<Tabs>()?.Draw(config) ?? selectedIndex;
                },
                "VerticalTabs"
            );
        #endregion

        #region TextArea Components
        public string TextArea(string text, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<TextArea>()?.DrawTextArea(text, variant, placeholder, disabled, minHeight, maxLength, options) ?? text, "TextArea");

        public string TextArea(Rect rect, string text, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, int maxLength = -1) =>
            TryExecute(() => GetComponent<TextArea>()?.DrawTextArea(rect, text, variant, placeholder, disabled, maxLength) ?? text, "TextArea");

        public string OutlineTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<TextArea>()?.OutlineTextArea(text, placeholder, disabled, minHeight, maxLength, options) ?? text, "OutlineTextArea");

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<TextArea>()?.GhostTextArea(text, placeholder, disabled, minHeight, maxLength, options) ?? text, "GhostTextArea");

        public string LabeledTextArea(string label, string text, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, bool showCharCount = true, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<TextArea>()?.LabeledTextArea(label, text, variant, placeholder, disabled, minHeight, maxLength, showCharCount, options) ?? text, "LabeledTextArea");

        public string ResizableTextArea(string text, ref float height, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, float maxHeight = 300f, int maxLength = -1, params GUILayoutOption[] options)
        {
            try
            {
                return GetComponent<TextArea>()?.ResizableTextArea(text, ref height, variant, placeholder, disabled, minHeight, maxHeight, maxLength, options) ?? text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "ResizableTextArea", "GUIHelper");
                return text;
            }
        }

        #endregion

        #region Checkbox Components
        public bool Checkbox(string text, bool value, CheckboxVariant variant = CheckboxVariant.Default, CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Checkbox>()?.DrawCheckbox(text, value, variant, size, onToggle, disabled, options) ?? value, "Checkbox");

        public bool Checkbox(Rect rect, string text, bool value, CheckboxVariant variant = CheckboxVariant.Default, CheckboxSize size = CheckboxSize.Default, Action<bool> onToggle = null, bool disabled = false) =>
            TryExecute(() => GetComponent<Checkbox>()?.DrawCheckbox(rect, text, value, variant, size, onToggle, disabled) ?? value, "Checkbox");

        #endregion

        #region Switch Components

        public bool Switch(string text, bool value, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Switch>()?.DrawSwitch(text, value, variant, size, onToggle, disabled, options) ?? value, "Switch");

        public bool Switch(Rect rect, string text, bool value, SwitchVariant variant = SwitchVariant.Default, SwitchSize size = SwitchSize.Default, Action<bool> onToggle = null, bool disabled = false) =>
            TryExecute(() => GetComponent<Switch>()?.DrawSwitch(rect, text, value, variant, size, onToggle, disabled) ?? false, "RoundedBadge");

        #endregion

        #region Badge Components
        public void Badge(string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.DrawBadge(text, variant, size, options), "Badge");

        public void Badge(Rect rect, string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default) => TryExecute(() => GetComponent<Badge>()?.DrawBadge(rect, text, variant, size), "Badge");

        public void BadgeWithIcon(string text, Texture2D icon, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.BadgeWithIcon(text, icon, variant, size, options), "BadgeWithIcon");

        public void CountBadge(int count, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, int maxCount = 99, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.CountBadge(count, variant, size, maxCount, options), "CountBadge");

        public void StatusBadge(string text, bool isActive, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.StatusBadge(text, isActive, variant, size, options), "StatusBadge");

        public void ProgressBadge(string text, float progress, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.ProgressBadge(text, progress, variant, size, options), "ProgressBadge");

        public void AnimatedBadge(string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, params GUILayoutOption[] options) => TryExecute(() => GetComponent<Badge>()?.AnimatedBadge(text, variant, size, options), "AnimatedBadge");

        public void RoundedBadge(string text, BadgeVariant variant = BadgeVariant.Default, BadgeSize size = BadgeSize.Default, float cornerRadius = 12f, params GUILayoutOption[] options) =>
            TryExecute(() => GetComponent<Badge>()?.RoundedBadge(text, variant, size, cornerRadius, options), "RoundedBadge");
        #endregion

        #region MenuBar Components
        public void MenuBar(MenuBar.MenuBarConfig config) => TryExecute(() => GetComponent<MenuBar>()?.Draw(config), "MenuBar");

        public void MenuBar(List<MenuBar.MenuItem> items, params GUILayoutOption[] options)
        {
            try
            {
                var config = new MenuBar.MenuBarConfig(items) { Options = options };
                GetComponent<MenuBar>()?.Draw(config);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "MenuBar", "GUIHelper");
            }
        }
        #endregion

        private void TryExecute(Action action, string methodName)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, methodName, "GUIHelper");
            }
        }

        private T TryExecute<T>(Func<T> func, string methodName)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, methodName, "GUIHelper");
                return default;
            }
        }
    }

    internal interface IComponentResolver
    {
        T GetComponent<T>()
            where T : BaseComponent;
    }

    internal class ComponentResolver : IComponentResolver
    {
        private readonly Dictionary<Type, BaseComponent> _componentInstances;

        public ComponentResolver(Dictionary<Type, BaseComponent> componentInstances)
        {
            _componentInstances = componentInstances;
        }

        public T GetComponent<T>()
            where T : BaseComponent
        {
            if (_componentInstances.TryGetValue(typeof(T), out BaseComponent component))
            {
                return (T)component;
            }
            GUILogger.LogWarning($"Component of type {typeof(T).Name} not found.", "ComponentResolver.GetComponent<T>");
            return null;
        }
    }
}
