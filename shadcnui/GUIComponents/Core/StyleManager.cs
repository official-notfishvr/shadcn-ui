using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
using Object = UnityEngine.Object;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core
{
    #region Enums
    public enum ButtonVariant
    {
        Default,
        Destructive,
        Outline,
        Secondary,
        Ghost,
        Link,
    }

    public enum ButtonSize
    {
        Default,
        Small,
        Large,
        Icon,
    }

    public enum ToggleVariant
    {
        Default,
        Outline,
    }

    public enum ToggleSize
    {
        Default,
        Small,
        Large,
    }

    public enum InputVariant
    {
        Default,
        Outline,
        Ghost,
    }

    public enum LabelVariant
    {
        Default,
        Secondary,
        Muted,
        Destructive,
    }

    public enum SeparatorOrientation
    {
        Horizontal,
        Vertical,
    }

    public enum TextAreaVariant
    {
        Default,
        Outline,
        Ghost,
    }

    public enum CheckboxVariant
    {
        Default,
        Outline,
        Ghost,
    }

    public enum CheckboxSize
    {
        Default,
        Small,
        Large,
    }

    public enum SwitchVariant
    {
        Default,
        Outline,
        Ghost,
    }

    public enum SwitchSize
    {
        Default,
        Small,
        Large,
    }

    public enum BadgeVariant
    {
        Default,
        Secondary,
        Destructive,
        Outline,
    }

    public enum BadgeSize
    {
        Default,
        Small,
        Large,
    }

    public enum AvatarSize
    {
        Small,
        Default,
        Large,
    }

    public enum AvatarShape
    {
        Circle,
        Square,
        Rounded,
    }

    public enum TableVariant
    {
        Default,
        Striped,
        Bordered,
        Hover,
    }

    public enum TableSize
    {
        Small,
        Default,
        Large,
    }

    public enum CalendarVariant
    {
        Default,
    }

    public enum CalendarSize
    {
        Default,
        Small,
        Large,
    }

    public enum DropdownMenuVariant
    {
        Default,
    }

    public enum DropdownMenuSize
    {
        Default,
        Small,
        Large,
    }

    public enum SelectVariant
    {
        Default,
    }

    public enum SelectSize
    {
        Default,
        Small,
        Large,
    }

    public enum ChartVariant
    {
        Default,
    }

    public enum ChartSize
    {
        Default,
        Small,
        Large,
    }
    #endregion

    public class StyleManager
    {
        #region Fields
        private GUIHelper guiHelper;
        private bool isInitialized = false;
        public Font customFont;
        #endregion

        #region Texture Fields
        private Texture2D gradientTexture;
        private Texture2D glowTexture;
        private Texture2D particleTexture;
        private Texture2D cardBackgroundTexture;
        private Texture2D outlineTexture;
        private Texture2D transparentTexture;
        private Texture2D inputBackgroundTexture;
        private Texture2D inputFocusedTexture;
        private Texture2D progressBarBackgroundTexture;
        private Texture2D progressBarFillTexture;
        private Texture2D separatorTexture;
        private Texture2D tabsBackgroundTexture;
        private Texture2D tabsActiveTexture;
        private Texture2D checkboxTexture;
        private Texture2D checkboxCheckedTexture;
        private Texture2D switchTexture;
        private Texture2D switchOnTexture;
        private Texture2D switchOffTexture;
        private Texture2D badgeTexture;
        private Texture2D avatarTexture;
        private Texture2D tableTexture;
        private Texture2D tableHeaderTexture;
        private Texture2D tableCellTexture;
        private Texture2D calendarBackgroundTexture;
        private Texture2D calendarHeaderTexture;
        private Texture2D calendarDayTexture;
        private Texture2D calendarDaySelectedTexture;
        private Texture2D dropdownMenuContentTexture;
        private Texture2D popoverContentTexture;
        private Texture2D scrollAreaThumbTexture;
        private Texture2D scrollAreaTrackTexture;
        private Texture2D selectTriggerTexture;
        private Texture2D selectContentTexture;
        private Texture2D chartContainerTexture;
        private Texture2D chartGridTexture;
        private Texture2D chartAxisTexture;
        #endregion

        #region GUIStyle - Animated
        private GUIStyle animatedBoxStyle;
        private GUIStyle animatedButtonStyle;
        private GUIStyle animatedInputStyle;
        private GUIStyle glowLabelStyle;
        private GUIStyle titleStyle;
        private GUIStyle colorPresetStyle;
        private GUIStyle sectionHeaderStyle;
        #endregion

        #region GUIStyle - Card
        private GUIStyle cardStyle;
        private GUIStyle cardHeaderStyle;
        private GUIStyle cardTitleStyle;
        private GUIStyle cardDescriptionStyle;
        private GUIStyle cardContentStyle;
        private GUIStyle cardFooterStyle;
        #endregion

        #region GUIStyle - Button
        private GUIStyle buttonDefaultStyle;
        private GUIStyle buttonDestructiveStyle;
        private GUIStyle buttonOutlineStyle;
        private GUIStyle buttonSecondaryStyle;
        private GUIStyle buttonGhostStyle;
        private GUIStyle buttonLinkStyle;
        #endregion

        #region GUIStyle - Toggle
        private GUIStyle toggleDefaultStyle;
        private GUIStyle toggleOutlineStyle;
        #endregion

        #region GUIStyle - Input
        private GUIStyle inputDefaultStyle;
        private GUIStyle inputOutlineStyle;
        private GUIStyle inputGhostStyle;
        private GUIStyle inputFocusedStyle;
        private GUIStyle inputDisabledStyle;
        private GUIStyle passwordFieldStyle;
        #endregion

        #region GUIStyle - Label
        private GUIStyle labelDefaultStyle;
        private GUIStyle labelSecondaryStyle;
        private GUIStyle labelMutedStyle;
        private GUIStyle labelDestructiveStyle;
        #endregion

        #region GUIStyle - TextArea
        private GUIStyle textAreaStyle;
        private GUIStyle textAreaFocusedStyle;
        private GUIStyle textAreaOutlineStyle;
        private GUIStyle textAreaGhostStyle;
        #endregion

        #region GUIStyle - Progress
        private GUIStyle progressBarStyle;
        private GUIStyle progressBarBackgroundStyle;
        private GUIStyle progressBarFillStyle;
        #endregion

        #region GUIStyle - Separator
        private GUIStyle separatorHorizontalStyle;
        private GUIStyle separatorVerticalStyle;
        #endregion

        #region GUIStyle - Tabs
        private GUIStyle tabsListStyle;
        private GUIStyle tabsTriggerStyle;
        private GUIStyle tabsTriggerActiveStyle;
        private GUIStyle tabsContentStyle;
        #endregion

        #region GUIStyle - Checkbox
        private GUIStyle checkboxDefaultStyle;
        private GUIStyle checkboxOutlineStyle;
        private GUIStyle checkboxGhostStyle;
        #endregion

        #region GUIStyle - Switch
        private GUIStyle switchDefaultStyle;
        private GUIStyle switchOutlineStyle;
        private GUIStyle switchGhostStyle;
        #endregion

        #region GUIStyle - Badge
        private GUIStyle badgeDefaultStyle;
        private GUIStyle badgeSecondaryStyle;
        private GUIStyle badgeDestructiveStyle;
        private GUIStyle badgeOutlineStyle;
        #endregion

        #region GUIStyle - Avatar
        private GUIStyle avatarStyle;
        #endregion

        #region GUIStyle - Table
        private GUIStyle tableStyle;
        private GUIStyle tableHeaderStyle;
        private GUIStyle tableCellStyle;
        private GUIStyle tableStripedStyle;
        private GUIStyle tableBorderedStyle;
        private GUIStyle tableHoverStyle;
        #endregion

        #region GUIStyle - Calendar
        private GUIStyle calendarStyle;
        private GUIStyle calendarHeaderStyle;
        private GUIStyle calendarTitleStyle;
        private GUIStyle calendarWeekdayStyle;
        private GUIStyle calendarDayStyle;
        private GUIStyle calendarDaySelectedStyle;
        private GUIStyle calendarDayOutsideMonthStyle;
        private GUIStyle calendarDayTodayStyle;
        private GUIStyle calendarDayInRangeStyle;
        #endregion

        #region GUIStyle - DatePicker
        private GUIStyle datePickerStyle;
        private GUIStyle datePickerHeaderStyle;
        private GUIStyle datePickerTitleStyle;
        private GUIStyle datePickerWeekdayStyle;
        private GUIStyle datePickerDayStyle;
        private GUIStyle datePickerDaySelectedStyle;
        private GUIStyle datePickerDayOutsideMonthStyle;
        private GUIStyle datePickerDayTodayStyle;
        #endregion

        #region GUIStyle - Dialog
        private GUIStyle dialogOverlayStyle;
        private GUIStyle dialogContentStyle;
        private GUIStyle dialogTitleStyle;
        private GUIStyle dialogDescriptionStyle;
        #endregion

        #region GUIStyle - DropdownMenu
        private GUIStyle dropdownMenuContentStyle;
        private GUIStyle dropdownMenuItemStyle;
        private GUIStyle dropdownMenuSeparatorStyle;
        private GUIStyle dropdownMenuHeaderStyle;
        #endregion

        #region GUIStyle - Popover
        private GUIStyle popoverContentStyle;
        #endregion

        #region GUIStyle - Select
        private GUIStyle selectTriggerStyle;
        private GUIStyle selectContentStyle;
        private GUIStyle selectItemStyle;
        #endregion

        #region GUIStyle - Chart
        private GUIStyle chartContainerStyle;
        private GUIStyle chartContentStyle;
        private GUIStyle chartAxisStyle;
        #endregion

        #region GUIStyle - MenuBar
        private GUIStyle menuBarStyle;
        private GUIStyle menuBarItemStyle;
        private GUIStyle menuDropdownStyle;
        #endregion

        #region Style Caches
        private Dictionary<(ButtonVariant, ButtonSize), GUIStyle> buttonStyleCache = new();
        private Dictionary<(ToggleVariant, ToggleSize), GUIStyle> toggleStyleCache = new();
        private Dictionary<(InputVariant, bool, bool), GUIStyle> inputStyleCache = new();
        private Dictionary<LabelVariant, GUIStyle> labelStyleCache = new();
        private Dictionary<(TextAreaVariant, bool), GUIStyle> textAreaStyleCache = new();
        private Dictionary<(CheckboxVariant, CheckboxSize), GUIStyle> checkboxStyleCache = new();
        private Dictionary<(SwitchVariant, SwitchSize), GUIStyle> switchStyleCache = new();
        private Dictionary<(BadgeVariant, BadgeSize), GUIStyle> badgeStyleCache = new();
        private Dictionary<(AvatarSize, AvatarShape), GUIStyle> avatarStyleCache = new();
        private Dictionary<(TableVariant, TableSize), GUIStyle> tableStyleCache = new();
        private Dictionary<(CalendarVariant, CalendarSize), GUIStyle> calendarStyleCache = new();
        private Dictionary<(DropdownMenuVariant, DropdownMenuSize), GUIStyle> dropdownMenuStyleCache = new();
        private Dictionary<(SelectVariant, SelectSize), GUIStyle> selectStyleCache = new();
        private Dictionary<(ChartVariant, ChartSize), GUIStyle> chartStyleCache = new();
        #endregion

        #region Texture Caches
        private Dictionary<Color, Texture2D> solidColorTextureCache = new();
        private Dictionary<(Color, Color), Texture2D> outlineButtonTextureCache = new();
        private Dictionary<Color, Texture2D> outlineTextureCache = new();
        #endregion

        #region Constructor
        public StyleManager(GUIHelper helper)
        {
            try
            {
                guiHelper = helper ?? throw new ArgumentNullException(nameof(helper));
                GUILogger.LogInfo("StyleManager initialized successfully", "StyleManager.Constructor");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Constructor", "StyleManager");
                throw;
            }
        }
        #endregion

        #region Core Methods
        public Theme GetTheme() => ThemeManager.Instance.CurrentTheme;

        public void SetCustomFont(byte[] fontData, string fontName = "CustomFont.ttf")
        {
            try
            {
                if (fontData == null || fontData.Length == 0)
                {
                    GUILogger.LogWarning("Font data is null or empty", "StyleManager.SetCustomFont");
                    return;
                }

                string tempPath = Path.Combine(Application.temporaryCachePath, fontName);
                File.WriteAllBytes(tempPath, fontData);

#if IL2CPP_MELONLOADER_PRE57
                GUILogger.LogWarning("IL2CPP: Cannot load dynamic fonts from file easily", "StyleManager.SetCustomFont");
#else
                UnityHelpers.Font loadedFont = new UnityHelpers.Font(tempPath);
                if (loadedFont != null)
                {
                    customFont = loadedFont;
                    GUILogger.LogInfo($"Custom font '{fontName}' loaded successfully", "StyleManager.SetCustomFont");
                }
                else
                {
                    GUILogger.LogWarning($"Failed to create font from bytes for '{fontName}'", "StyleManager.SetCustomFont");
                }

                try
                {
                    File.Delete(tempPath);
                }
                catch (Exception deleteEx)
                {
                    GUILogger.LogWarning($"Failed to delete temporary font file: {deleteEx.Message}", "StyleManager.SetCustomFont");
                }
#endif
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetCustomFont", "StyleManager");
            }
        }

        public void InitializeGUI()
        {
            try
            {
                if (isInitialized)
                {
                    GUILogger.LogWarning("StyleManager already initialized", "StyleManager.InitializeGUI");
                    return;
                }

                GUILogger.LogInfo("Starting StyleManager initialization", "StyleManager.InitializeGUI");
                CreateCustomTextures();
                SetupAllStyles();
                isInitialized = true;
                GUILogger.LogInfo("StyleManager initialization completed successfully", "StyleManager.InitializeGUI");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "InitializeGUI", "StyleManager");
                isInitialized = false;
            }
        }

        internal void SetupAllStyles()
        {
            try
            {
                SetupAnimatedStyles();
                SetupCardStyles();
                SetupButtonVariantStyles();
                SetupToggleVariantStyles();
                SetupInputVariantStyles();
                SetupLabelVariantStyles();
                SetupProgressBarStyles();
                SetupSeparatorStyles();
                SetupTabsStyles();
                SetupTextAreaVariantStyles();
                SetupCheckboxStyles();
                SetupSwitchStyles();
                SetupBadgeStyles();
                SetupAvatarStyles();
                SetupTableStyles();
                SetupCalendarStyles();
                SetupDropdownMenuStyles();
                SetupPopoverStyles();
                SetupSelectStyles();
                SetupDatePickerStyles();
                SetupDialogStyles();
                SetupChartStyles();
                SetupMenuBarStyles();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupAllStyles", "StyleManager");
            }
        }
        #endregion

        #region Fix GUI Styles
        private bool _stylesCorruption = false;
        private float _lastScanTime = 0f;
        private float _scanInterval = 5f;
        private Dictionary<string, int> _styleHealthChecks = new Dictionary<string, int>();

        private readonly string[] _stylesToMonitor = new[]
        {
            "tabsTriggerActive",
            "buttonDefault",
            "buttonDestructive",
            "buttonOutline",
            "buttonSecondary",
            "buttonGhost",
            "buttonLink",
            "toggleDefault",
            "toggleOutline",
            "inputDefault",
            "inputOutline",
            "inputGhost",
            "inputFocused",
            "inputDisabled",
            "labelDefault",
            "labelSecondary",
            "labelMuted",
            "labelDestructive",
            "cardStyle",
            "cardHeader",
            "cardTitle",
            "cardDescription",
            "cardContent",
            "cardFooter",
            "progressBar",
            "progressBarBg",
            "progressBarFill",
            "checkboxDefault",
            "checkboxOutline",
            "checkboxGhost",
            "switchDefault",
            "switchOutline",
            "switchGhost",
            "badgeDefault",
            "badgeSecondary",
            "badgeDestructive",
            "badgeOutline",
            "separator",
            "separatorVertical",
            "textArea",
            "textAreaFocused",
            "textAreaOutline",
            "textAreaGhost",
            "passwordField",
            "tabsList",
            "tabsTrigger",
            "tabsContent",
            "table",
            "tableHeader",
            "tableCell",
            "tableStriped",
            "tableBordered",
            "tableHover",
            "calendar",
            "calendarHeader",
            "calendarTitle",
            "calendarWeekday",
            "calendarDay",
            "calendarDaySelected",
            "calendarDayOutsideMonth",
            "calendarDayToday",
            "calendarDayInRange",
            "dialogOverlay",
            "dialogContent",
            "dialogTitle",
            "dialogDescription",
            "dropdownMenuContent",
            "dropdownMenuItem",
            "dropdownMenuSeparator",
            "dropdownMenuHeader",
            "popover",
            "selectTrigger",
            "selectContent",
            "selectItem",
            "chartContainer",
            "chartContent",
            "chartAxis",
            "menuBar",
            "menuBarItem",
            "menuDropdown",
            "animatedBox",
            "animatedButton",
            "animatedInput",
            "glowLabel",
            "title",
            "colorPreset",
            "sectionHeader",
            "avatar",
            "datePickerStyle",
            "datePickerHeader",
            "datePickerTitle",
            "datePickerWeekday",
            "datePickerDay",
            "datePickerDaySelected",
            "datePickerDayOutsideMonth",
            "datePickerDayToday",
        };

        private GUIStyle GetStyleByName(string name) =>
            name switch
            {
                "tabsTriggerActive" => tabsTriggerActiveStyle,
                "buttonDefault" => buttonDefaultStyle,
                "buttonDestructive" => buttonDestructiveStyle,
                "buttonOutline" => buttonOutlineStyle,
                "buttonSecondary" => buttonSecondaryStyle,
                "buttonGhost" => buttonGhostStyle,
                "buttonLink" => buttonLinkStyle,
                "toggleDefault" => toggleDefaultStyle,
                "toggleOutline" => toggleOutlineStyle,
                "inputDefault" => inputDefaultStyle,
                "inputOutline" => inputOutlineStyle,
                "inputGhost" => inputGhostStyle,
                "inputFocused" => inputFocusedStyle,
                "inputDisabled" => inputDisabledStyle,
                "labelDefault" => labelDefaultStyle,
                "labelSecondary" => labelSecondaryStyle,
                "labelMuted" => labelMutedStyle,
                "labelDestructive" => labelDestructiveStyle,
                "cardStyle" => cardStyle,
                "cardHeader" => cardHeaderStyle,
                "cardTitle" => cardTitleStyle,
                "cardDescription" => cardDescriptionStyle,
                "cardContent" => cardContentStyle,
                "cardFooter" => cardFooterStyle,
                "progressBar" => progressBarStyle,
                "progressBarBg" => progressBarBackgroundStyle,
                "progressBarFill" => progressBarFillStyle,
                "checkboxDefault" => checkboxDefaultStyle,
                "checkboxOutline" => checkboxOutlineStyle,
                "checkboxGhost" => checkboxGhostStyle,
                "switchDefault" => switchDefaultStyle,
                "switchOutline" => switchOutlineStyle,
                "switchGhost" => switchGhostStyle,
                "badgeDefault" => badgeDefaultStyle,
                "badgeSecondary" => badgeSecondaryStyle,
                "badgeDestructive" => badgeDestructiveStyle,
                "badgeOutline" => badgeOutlineStyle,
                "separator" => separatorHorizontalStyle,
                "separatorVertical" => separatorVerticalStyle,
                "textArea" => textAreaStyle,
                "textAreaFocused" => textAreaFocusedStyle,
                "textAreaOutline" => textAreaOutlineStyle,
                "textAreaGhost" => textAreaGhostStyle,
                "passwordField" => passwordFieldStyle,
                "tabsList" => tabsListStyle,
                "tabsTrigger" => tabsTriggerStyle,
                "tabsContent" => tabsContentStyle,
                "table" => tableStyle,
                "tableHeader" => tableHeaderStyle,
                "tableCell" => tableCellStyle,
                "tableStriped" => tableStripedStyle,
                "tableBordered" => tableBorderedStyle,
                "tableHover" => tableHoverStyle,
                "calendar" => calendarStyle,
                "calendarHeader" => calendarHeaderStyle,
                "calendarTitle" => calendarTitleStyle,
                "calendarWeekday" => calendarWeekdayStyle,
                "calendarDay" => calendarDayStyle,
                "calendarDaySelected" => calendarDaySelectedStyle,
                "calendarDayOutsideMonth" => calendarDayOutsideMonthStyle,
                "calendarDayToday" => calendarDayTodayStyle,
                "calendarDayInRange" => calendarDayInRangeStyle,
                "dialogOverlay" => dialogOverlayStyle,
                "dialogContent" => dialogContentStyle,
                "dialogTitle" => dialogTitleStyle,
                "dialogDescription" => dialogDescriptionStyle,
                "dropdownMenuContent" => dropdownMenuContentStyle,
                "dropdownMenuItem" => dropdownMenuItemStyle,
                "dropdownMenuSeparator" => dropdownMenuSeparatorStyle,
                "dropdownMenuHeader" => dropdownMenuHeaderStyle,
                "popover" => popoverContentStyle,
                "selectTrigger" => selectTriggerStyle,
                "selectContent" => selectContentStyle,
                "selectItem" => selectItemStyle,
                "chartContainer" => chartContainerStyle,
                "chartContent" => chartContentStyle,
                "chartAxis" => chartAxisStyle,
                "menuBar" => menuBarStyle,
                "menuBarItem" => menuBarItemStyle,
                "menuDropdown" => menuDropdownStyle,
                "animatedBox" => animatedBoxStyle,
                "animatedButton" => animatedButtonStyle,
                "animatedInput" => animatedInputStyle,
                "glowLabel" => glowLabelStyle,
                "title" => titleStyle,
                "colorPreset" => colorPresetStyle,
                "sectionHeader" => sectionHeaderStyle,
                "avatar" => avatarStyle,
                "datePickerStyle" => datePickerStyle,
                "datePickerHeader" => datePickerHeaderStyle,
                "datePickerTitle" => datePickerTitleStyle,
                "datePickerWeekday" => datePickerWeekdayStyle,
                "datePickerDay" => datePickerDayStyle,
                "datePickerDaySelected" => datePickerDaySelectedStyle,
                "datePickerDayOutsideMonth" => datePickerDayOutsideMonthStyle,
                "datePickerDayToday" => datePickerDayTodayStyle,
                _ => null,
            };

        private int GetStyleHealth(GUIStyle style)
        {
            if (style == null)
                return 0;
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + (style.normal.background?.GetInstanceID() ?? 0);
                hash = hash * 31 + (style.hover.background?.GetInstanceID() ?? 0);
                hash = hash * 31 + (style.active.background?.GetInstanceID() ?? 0);
                hash = hash * 31 + style.fontSize;
                hash = hash * 31 + style.padding.left + style.padding.right + style.padding.top + style.padding.bottom;
                hash = hash * 31 + (style.normal.textColor.GetHashCode());
                return hash;
            }
        }

        public void MarkStylesCorruption()
        {
            _stylesCorruption = true;
        }

        public void RefreshStylesIfCorruption()
        {
            if (!_stylesCorruption)
                return;

            try
            {
                buttonStyleCache.Clear();
                toggleStyleCache.Clear();
                inputStyleCache.Clear();
                labelStyleCache.Clear();
                textAreaStyleCache.Clear();
                checkboxStyleCache.Clear();
                switchStyleCache.Clear();
                badgeStyleCache.Clear();
                avatarStyleCache.Clear();
                tableStyleCache.Clear();
                calendarStyleCache.Clear();
                dropdownMenuStyleCache.Clear();
                selectStyleCache.Clear();
                chartStyleCache.Clear();

                SetupAnimatedStyles();
                SetupCardStyles();
                SetupButtonVariantStyles();
                SetupToggleVariantStyles();
                SetupInputVariantStyles();
                SetupLabelVariantStyles();
                SetupProgressBarStyles();
                SetupSeparatorStyles();
                SetupTabsStyles();
                SetupTextAreaVariantStyles();
                SetupCheckboxStyles();
                SetupSwitchStyles();
                SetupBadgeStyles();
                SetupAvatarStyles();
                SetupTableStyles();
                SetupCalendarStyles();
                SetupDropdownMenuStyles();
                SetupPopoverStyles();
                SetupSelectStyles();
                SetupDatePickerStyles();
                SetupDialogStyles();
                SetupChartStyles();
                SetupMenuBarStyles();

                _styleHealthChecks.Clear();
                foreach (var styleName in _stylesToMonitor)
                {
                    _styleHealthChecks[styleName] = GetStyleHealth(GetStyleByName(styleName));
                }

                _stylesCorruption = false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "RefreshStylesIfCorruption", "StyleManager");
            }
        }

        public bool ScanForCorruption()
        {
            if (Time.realtimeSinceStartup - _lastScanTime < _scanInterval)
                return false;

            _lastScanTime = Time.realtimeSinceStartup;

            foreach (var styleName in _stylesToMonitor)
            {
                int currentHealth = GetStyleHealth(GetStyleByName(styleName));

                if (_styleHealthChecks.ContainsKey(styleName) && _styleHealthChecks[styleName] != currentHealth)
                {
                    _styleHealthChecks[styleName] = currentHealth;
                    return true;
                }

                _styleHealthChecks[styleName] = currentHealth;
            }

            return false;
        }
        #endregion

        #region Design Helpers
        public int GetScaledBorderRadius(float radius) => Mathf.RoundToInt(radius * guiHelper.uiScale);

        public int GetScaledSpacing(float spacing) => Mathf.RoundToInt(spacing * guiHelper.uiScale);

        public int GetScaledHeight(float height) => Mathf.RoundToInt(height * guiHelper.uiScale);

        public int GetScaledFontSize(float scale = 1.0f) => Mathf.RoundToInt(guiHelper.fontSize * scale * guiHelper.uiScale);

        public RectOffset GetSpacingOffset(float horizontal = 8f, float vertical = 8f)
        {
            int h = GetScaledSpacing(horizontal);
            int v = GetScaledSpacing(vertical);
            return new UnityHelpers.RectOffset(h, h, v, v);
        }

        public RectOffset GetBorderOffset(float radius = 6f)
        {
            int r = GetScaledBorderRadius(radius);
            return new UnityHelpers.RectOffset(r, r, r, r);
        }

        public Color GetHoverColor(Color baseColor, bool isDark = true) => isDark ? Color.Lerp(baseColor, Color.white, 0.15f) : Color.Lerp(baseColor, Color.black, 0.08f);

        public Color GetFocusColor(Color baseColor) => Color.Lerp(baseColor, ThemeManager.Instance.CurrentTheme.Accent, 0.25f);

        public Color GetOverlayColor() => ThemeManager.Instance.CurrentTheme.Overlay;

        public Color GetShadowColor() => ThemeManager.Instance.CurrentTheme.Shadow;

        public Color GetBorderColor() => ThemeManager.Instance.CurrentTheme.Border;
        #endregion

        #region Texture Creation
        public Texture2D CreateBorderTexture(Color borderColor, int thickness)
        {
            int size = thickness * 2 + 2;
            var texture = new Texture2D(size, size);
            for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                bool isBorder = x < thickness || y < thickness || x >= size - thickness || y >= size - thickness;
                texture.SetPixel(x, y, isBorder ? borderColor : Color.clear);
            }
            texture.Apply();
            return texture;
        }

        private Texture2D CreateGlowTexture(Color glowColor, int size)
        {
            var texture = new Texture2D(size, size);
            Vector2 center = new Vector2(size / 2f, size / 2f);
            float maxDist = size / 2f;
            for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                float t = Mathf.Clamp01(distance / maxDist);
                float alpha = 1f - (t * t * (3f - 2f * t));
                texture.SetPixel(x, y, new Color(glowColor.r, glowColor.g, glowColor.b, alpha * 0.6f));
            }
            texture.Apply();
            return texture;
        }

        private Texture2D CreateShadowTexture(int width, int height, float intensity = 0.22f, float blur = 3.5f)
        {
            var texture = new Texture2D(width, height);
            Color themeShadow = ThemeManager.Instance.CurrentTheme.Shadow;
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                float edgeDistanceX = Mathf.Min(x, width - x - 1);
                float edgeDistanceY = Mathf.Min(y, height - y - 1);
                float edgeDistance = Mathf.Min(edgeDistanceX, edgeDistanceY);
                float alpha = Mathf.Clamp01(edgeDistance / blur);
                alpha = (1f - alpha) * intensity;
                texture.SetPixel(x, y, new Color(themeShadow.r, themeShadow.g, themeShadow.b, alpha));
            }
            texture.Apply();
            return texture;
        }

        private Texture2D CreateBottomBorderTexture(int width, int height, int borderThickness, Color borderColor, Color fillColor)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                bool isBorder = y < borderThickness ? false : (y >= height - borderThickness);
                if (isBorder)
                {
                    texture.SetPixel(x, y, borderColor);
                }
                else
                {
                    texture.SetPixel(x, y, fillColor);
                }
            }
            texture.Apply();
            return texture;
        }

        public Texture2D CreateSolidTexture(Color color)
        {
            if (solidColorTextureCache.TryGetValue(color, out var cachedTexture))
                return cachedTexture;
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            solidColorTextureCache[color] = texture;
            return texture;
        }

        public Texture2D CreateRoundedRectTexture(int width, int height, int radius, Color color) => CreateGradientRoundedRectTexture(width, height, radius, color, color);

        public Texture2D CreateGradientRoundedRectTexture(int width, int height, int radius, Color topColor, Color bottomColor)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isCornerArea = (x < radius && y < radius) || (x > width - radius && y < radius) || (x < radius && y > height - radius) || (x > width - radius && y > height - radius);
                    if (isCornerArea)
                    {
                        Vector2 cornerCenter = GetCornerCenter(x, y, width, height, radius);
                        if (Vector2.Distance(new Vector2(x, y), cornerCenter) > radius)
                        {
                            texture.SetPixel(x, y, Color.clear);
                            continue;
                        }
                    }
                    float t = (float)y / (Mathf.Max(1, height - 1));
                    texture.SetPixel(x, y, Color.Lerp(bottomColor, topColor, t));
                }
            }
            texture.Apply();
            return texture;
        }

        public Texture2D CreateRoundedRectWithShadowTexture(int width, int height, int radius, Color fillColor, float shadowIntensity = 0.08f, int shadowBlur = 6) => CreateGradientRoundedRectWithShadowTexture(width, height, radius, fillColor, fillColor, shadowIntensity, shadowBlur);

        public Texture2D CreateGradientRoundedRectWithShadowTexture(int width, int height, int radius, Color topColor, Color bottomColor, float shadowIntensity = 0.08f, int shadowBlur = 6)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bool isCornerArea = (x < radius && y < radius) || (x > width - radius && y < radius) || (x < radius && y > height - radius) || (x > width - radius && y > height - radius);
                    if (isCornerArea)
                    {
                        Vector2 cornerCenter = GetCornerCenter(x, y, width, height, radius);
                        if (Vector2.Distance(new Vector2(x, y), cornerCenter) > radius)
                        {
                            texture.SetPixel(x, y, Color.clear);
                            continue;
                        }
                    }
                    float t = (float)y / (Mathf.Max(1, height - 1));
                    texture.SetPixel(x, y, Color.Lerp(bottomColor, topColor, t));
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int dx = Mathf.Min(x, width - 1 - x);
                    int dy = Mathf.Min(y, height - 1 - y);
                    int d = Mathf.Min(dx, dy);
                    if (d < shadowBlur)
                    {
                        Color basePx = texture.GetPixel(x, y);
                        float t = 1f - (d / (float)shadowBlur);
                        float a = t * shadowIntensity;
                        Color shadow = new Color(0f, 0f, 0f, a);
                        float outA = shadow.a + basePx.a * (1f - shadow.a);
                        if (outA <= 0f)
                        {
                            texture.SetPixel(x, y, basePx);
                        }
                        else
                        {
                            float outR = (shadow.r * shadow.a + basePx.r * basePx.a * (1f - shadow.a)) / outA;
                            float outG = (shadow.g * shadow.a + basePx.g * basePx.a * (1f - shadow.a)) / outA;
                            float outB = (shadow.b * shadow.a + basePx.b * basePx.a * (1f - shadow.a)) / outA;
                            texture.SetPixel(x, y, new Color(outR, outG, outB, outA));
                        }
                    }
                }
            }
            texture.Apply();
            return texture;
        }

        private Vector2 GetCornerCenter(int x, int y, int width, int height, int radius)
        {
            if (x < radius && y < radius)
                return new Vector2(radius, radius);
            if (x > width - radius && y < radius)
                return new Vector2(width - radius, radius);
            if (x < radius && y > height - radius)
                return new Vector2(radius, height - radius);
            return new Vector2(width - radius, height - radius);
        }

        public Texture2D CreateOutlineButtonTexture(Color backgroundColor, Color borderColor)
        {
            if (outlineButtonTextureCache.TryGetValue((backgroundColor, borderColor), out var cachedTexture))
                return cachedTexture;
            int size = 32;
            Texture2D texture = new Texture2D(size, size);
            for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                bool isBorder = x == 0 || y == 0 || x == size - 1 || y == size - 1;
                texture.SetPixel(x, y, isBorder ? borderColor : backgroundColor);
            }
            texture.Apply();
            outlineButtonTextureCache[(backgroundColor, borderColor)] = texture;
            return texture;
        }

        private Texture2D CreateOutlineTexture()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            var keyColor = theme.Border;
            if (outlineTextureCache.TryGetValue(keyColor, out var cachedTexture))
                return cachedTexture;
            Texture2D texture = new Texture2D(4, 4);
            for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
            {
                bool isBorder = x == 0 || y == 0 || x == 3 || y == 3;
                texture.SetPixel(x, y, isBorder ? keyColor : Color.clear);
            }
            texture.Apply();
            outlineTextureCache[keyColor] = texture;
            return texture;
        }

        private void CreateCustomTextures()
        {
            try
            {
                GUILogger.LogInfo("Creating custom textures", "StyleManager.CreateCustomTextures");
                var theme = ThemeManager.Instance.CurrentTheme;

                gradientTexture = new Texture2D(1, 100);
                for (int i = 0; i < 100; i++)
                {
                    float t = i / 99f;
                    gradientTexture.SetPixel(0, i, Color.Lerp(theme.Base, theme.Secondary, t));
                }
                gradientTexture.Apply();

                cardBackgroundTexture = CreateGradientRoundedRectWithShadowTexture(128, 128, 12, Color.Lerp(theme.Secondary, theme.Base, 0.05f), theme.Secondary, 0.1f, 8);
                inputBackgroundTexture = CreateGradientRoundedRectTexture(128, 128, 6, theme.Base, Color.Lerp(theme.Base, theme.Secondary, 0.05f));
                inputFocusedTexture = CreateGradientRoundedRectTexture(128, 128, 6, theme.Base, Color.Lerp(theme.Base, theme.Secondary, 0.05f));
                outlineTexture = CreateRoundedRectTexture(128, 128, 6, theme.Border);
                transparentTexture = CreateSolidTexture(Color.clear);
                glowTexture = CreateGlowTexture(theme.Accent, 32);
                particleTexture = CreateSolidTexture(Color.Lerp(theme.Accent, theme.Text, 0.5f));
                progressBarBackgroundTexture = CreateGradientRoundedRectTexture(128, 128, 6, theme.Secondary, Color.Lerp(theme.Secondary, Color.black, 0.1f));
                progressBarFillTexture = CreateGradientRoundedRectTexture(128, 128, 6, Color.Lerp(theme.Accent, Color.white, 0.1f), theme.Accent);
                separatorTexture = CreateGradientRoundedRectTexture(128, 2, 0, theme.Border, Color.Lerp(theme.Border, Color.black, 0.1f));
                tabsBackgroundTexture = CreateRoundedRectTexture(128, 128, 6, theme.Secondary);
                tabsActiveTexture = CreateGradientRoundedRectTexture(128, 128, 6, theme.Base, Color.Lerp(theme.Base, theme.Secondary, 0.05f));
                checkboxTexture = CreateGradientRoundedRectTexture(16, 16, 4, theme.Secondary, Color.Lerp(theme.Secondary, Color.black, 0.1f));
                checkboxCheckedTexture = CreateGradientRoundedRectTexture(16, 16, 4, Color.Lerp(theme.Accent, Color.white, 0.1f), theme.Accent);
                switchTexture = CreateGradientRoundedRectTexture(32, 16, 8, theme.Secondary, Color.Lerp(theme.Secondary, Color.black, 0.1f));
                switchOnTexture = CreateGradientRoundedRectTexture(32, 16, 8, Color.Lerp(theme.Accent, Color.white, 0.1f), theme.Accent);
                switchOffTexture = CreateGradientRoundedRectTexture(32, 16, 8, theme.Secondary, Color.Lerp(theme.Secondary, Color.black, 0.1f));
                badgeTexture = CreateGradientRoundedRectTexture(64, 24, 12, Color.Lerp(theme.Elevated, Color.white, 0.05f), theme.Elevated);
                avatarTexture = CreateRoundedRectTexture(40, 40, 20, theme.Secondary);
                tableTexture = CreateGradientRoundedRectTexture(128, 128, 0, theme.Base, Color.Lerp(theme.Base, theme.Secondary, 0.02f));
                tableHeaderTexture = CreateSolidTexture(theme.Secondary);
                tableCellTexture = CreateSolidTexture(theme.Base);
                calendarBackgroundTexture = CreateGradientRoundedRectTexture(256, 256, 6, Color.Lerp(theme.Secondary, theme.Base, 0.05f), theme.Secondary);
                calendarHeaderTexture = CreateSolidTexture(theme.Secondary);
                calendarDayTexture = CreateRoundedRectTexture(32, 32, 4, theme.Secondary);
                calendarDaySelectedTexture = CreateGradientRoundedRectTexture(32, 32, 4, Color.Lerp(theme.Accent, Color.white, 0.1f), theme.Accent);
                dropdownMenuContentTexture = CreateGradientRoundedRectWithShadowTexture(128, 128, 6, Color.Lerp(theme.Secondary, theme.Base, 0.05f), theme.Secondary, 0.1f, 6);
                popoverContentTexture = CreateGradientRoundedRectWithShadowTexture(128, 128, 6, Color.Lerp(theme.Base, theme.Secondary, 0.05f), theme.Base, 0.1f, 6);
                scrollAreaThumbTexture = CreateRoundedRectTexture(8, 8, 4, theme.Border);
                scrollAreaTrackTexture = CreateRoundedRectTexture(8, 8, 4, theme.Secondary);
                selectTriggerTexture = CreateGradientRoundedRectTexture(128, 40, 6, theme.Base, Color.Lerp(theme.Base, theme.Secondary, 0.05f));
                selectContentTexture = CreateGradientRoundedRectWithShadowTexture(128, 128, 6, Color.Lerp(theme.Secondary, theme.Base, 0.05f), theme.Secondary, 0.1f, 6);
                chartContainerTexture = CreateGradientRoundedRectWithShadowTexture(256, 256, 8, Color.Lerp(theme.Secondary, theme.Base, 0.05f), theme.Secondary, 0.08f, 6);
                chartGridTexture = CreateGradientRoundedRectTexture(128, 2, 0, theme.Border, Color.Lerp(theme.Border, Color.clear, 0.5f));
                chartAxisTexture = CreateSolidTexture(theme.Muted);

                GUILogger.LogInfo("Custom textures created successfully", "StyleManager.CreateCustomTextures");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "CreateCustomTextures", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Animated
        private void SetupAnimatedStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                animatedBoxStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                animatedBoxStyle.normal.background = gradientTexture;
                animatedBoxStyle.border = new UnityHelpers.RectOffset(guiHelper.cornerRadius, guiHelper.cornerRadius, guiHelper.cornerRadius, 5);
                animatedBoxStyle.padding = new UnityHelpers.RectOffset(15, 15, 15, 15);
                animatedButtonStyle = CreateStyleWithFont(GUI.skin.button, guiHelper.fontSize, FontStyle.Bold);
                animatedButtonStyle.alignment = TextAnchor.MiddleCenter;
                animatedButtonStyle.normal.textColor = Color.Lerp(Color.white, theme.Accent, 0.4f);
                animatedButtonStyle.hover.textColor = theme.Accent;
                colorPresetStyle = CreateStyleWithFont(GUI.skin.button, Mathf.RoundToInt(guiHelper.fontSize * 0.9f), FontStyle.Bold);
                colorPresetStyle.alignment = TextAnchor.MiddleCenter;
                animatedInputStyle = CreateStyleWithFont(GUI.skin.textField, guiHelper.fontSize + 1);
                animatedInputStyle.padding = new UnityHelpers.RectOffset(8, 8, 4, 4);
                animatedInputStyle.normal.textColor = theme.Text;
                animatedInputStyle.focused.textColor = theme.Accent;
                glowLabelStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize);
                glowLabelStyle.normal.textColor = Color.Lerp(theme.Text, theme.Accent, 0.25f);
                titleStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize + 4, FontStyle.Bold);
                titleStyle.alignment = TextAnchor.MiddleCenter;
                titleStyle.normal.textColor = theme.Accent;
                sectionHeaderStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize + 2, FontStyle.Bold);
                sectionHeaderStyle.normal.textColor = Color.Lerp(theme.Accent, theme.Text, 0.4f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupAnimatedStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Card
        private void SetupCardStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                cardStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                int cardRadius = GetScaledBorderRadius(12f);
                cardStyle.normal.background = CreateRoundedRectWithShadowTexture(256, 256, cardRadius, theme.Secondary, 0.2f, 12);
                cardStyle.border = new UnityHelpers.RectOffset(12, 12, 12, 12);
                cardStyle.padding = GetSpacingOffset(28f, 28f);
                cardStyle.margin = GetSpacingOffset(8f, 8f);
                cardHeaderStyle = new UnityHelpers.GUIStyle();
                cardHeaderStyle.padding = GetSpacingOffset(0f, 20f);
                cardTitleStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.75f), FontStyle.Bold);
                cardTitleStyle.normal.textColor = theme.Text;
                cardTitleStyle.wordWrap = true;
                cardDescriptionStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.95f));
                cardDescriptionStyle.normal.textColor = theme.Muted;
                cardDescriptionStyle.wordWrap = true;
                cardDescriptionStyle.margin = GetSpacingOffset(0f, 6f);
                cardContentStyle = new UnityHelpers.GUIStyle();
                cardContentStyle.padding = GetSpacingOffset(0f, 12f);
                cardFooterStyle = new UnityHelpers.GUIStyle();
                cardFooterStyle.padding = GetSpacingOffset(0f, 20f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCardStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Button
        private void SetupButtonVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                buttonDefaultStyle = CreateButtonStyle(theme.Secondary, theme.ButtonPrimaryFg, GetHoverColor(theme.Secondary, true), theme.ButtonPrimaryFg);
                buttonDestructiveStyle = CreateButtonStyle(theme.ButtonDestructiveBg, theme.ButtonDestructiveFg, Color.Lerp(theme.ButtonDestructiveBg, Color.black, 0.15f), theme.ButtonDestructiveFg);
                buttonOutlineStyle = CreateButtonStyle(theme.Border, theme.ButtonOutlineFg, Color.Lerp(theme.Border, Color.black, 0.05f), theme.ButtonOutlineFg);
                buttonSecondaryStyle = CreateButtonStyle(theme.Secondary, theme.ButtonSecondaryFg, GetHoverColor(theme.Secondary, true), theme.ButtonSecondaryFg);
                buttonGhostStyle = CreateButtonStyle(transparentTexture, theme.ButtonGhostFg, theme.Secondary, theme.ButtonGhostFg);
                buttonLinkStyle = CreateLinkButtonStyle(theme);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupButtonVariantStyles", "StyleManager");
            }
        }

        private GUIStyle CreateButtonStyle(Color normalBg, Color normalFg, Color activeBg, Color activeFg)
        {
            GUIStyle style = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f), FontStyle.Bold);
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = GetSpacingOffset(16f, 8f);
            style.border = GetBorderOffset(6f);
            style.fixedHeight = GetScaledHeight(36f);
            int btnTexH = GetScaledHeight(36f);
            int btnRadius = GetScaledBorderRadius(6f);

            style.normal.background = CreateGradientRoundedRectTexture(128, btnTexH, btnRadius, normalBg, Color.Lerp(normalBg, Color.black, 0.05f));
            style.hover.background = CreateGradientRoundedRectTexture(128, btnTexH, btnRadius, GetHoverColor(normalBg, false), Color.Lerp(GetHoverColor(normalBg, false), Color.black, 0.05f));
            style.normal.textColor = normalFg;
            style.hover.textColor = normalFg;
            style.active.background = CreateGradientRoundedRectTexture(128, btnTexH, btnRadius, activeBg, Color.Lerp(activeBg, Color.black, 0.1f));
            style.active.textColor = activeFg;
            style.focused.background = CreateGradientRoundedRectTexture(128, btnTexH, btnRadius, Color.Lerp(normalBg, ThemeManager.Instance.CurrentTheme.Accent, 0.1f), Color.Lerp(normalBg, ThemeManager.Instance.CurrentTheme.Accent, 0.15f));
            style.focused.textColor = normalFg;
            return style;
        }

        private GUIStyle CreateButtonStyle(Texture2D normalBg, Color normalFg, Color hoverBg, Color activeFg)
        {
            GUIStyle style = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f), FontStyle.Bold);
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = GetSpacingOffset(16f, 8f);
            style.border = GetBorderOffset(6f);
            style.fixedHeight = GetScaledHeight(36f);
            int btnTexH = GetScaledHeight(36f);
            int btnRadius = GetScaledBorderRadius(6f);
            style.normal.background = normalBg;
            style.hover.background = CreateRoundedRectTexture(128, btnTexH, btnRadius, hoverBg);
            style.normal.textColor = normalFg;
            style.hover.textColor = normalFg;
            style.active.background = CreateRoundedRectTexture(128, btnTexH, btnRadius, hoverBg);
            style.active.textColor = activeFg;
            style.focused.background = normalBg;
            style.focused.textColor = normalFg;
            return style;
        }

        private GUIStyle CreateLinkButtonStyle(Theme theme)
        {
            GUIStyle style = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f));
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = GetSpacingOffset(0f, 2f);
            style.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
            style.normal.background = transparentTexture;
            style.hover.background = transparentTexture;
            style.normal.textColor = theme.ButtonLinkColor;
            style.hover.textColor = Color.Lerp(theme.ButtonLinkColor, Color.white, 0.2f);
            style.active.background = transparentTexture;
            style.active.textColor = Color.Lerp(theme.ButtonLinkColor, Color.black, 0.25f);
            return style;
        }
        #endregion

        #region Style Setup - Toggle
        private void SetupToggleVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                toggleDefaultStyle = CreateToggleStyle(theme.Secondary, theme.Text, theme.Accent, theme.Text);
                toggleOutlineStyle = new UnityHelpers.GUIStyle(toggleDefaultStyle);
                if (customFont != null)
                    toggleOutlineStyle.font = customFont;
                toggleOutlineStyle.normal.background = CreateOutlineButtonTexture(Color.Lerp(theme.Base, Color.black, 0.15f), theme.Accent);
                toggleOutlineStyle.border = new UnityHelpers.RectOffset(2, 2, 2, 2);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupToggleVariantStyles", "StyleManager");
            }
        }

        private GUIStyle CreateToggleStyle(Color normalBg, Color normalFg, Color onBg, Color onFg)
        {
            GUIStyle style = CreateStyleWithFont(GUI.skin.button, guiHelper.fontSize);
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = normalFg;
            int radius = GetScaledBorderRadius(4f);
            style.normal.background = CreateGradientRoundedRectTexture(32, 32, radius, normalBg, Color.Lerp(normalBg, Color.black, 0.05f));
            style.hover.background = CreateGradientRoundedRectTexture(32, 32, radius, ThemeManager.Instance.CurrentTheme.Secondary, Color.Lerp(ThemeManager.Instance.CurrentTheme.Secondary, Color.white, 0.05f));
            style.hover.textColor = ThemeManager.Instance.CurrentTheme.Text;
            style.active.background = CreateGradientRoundedRectTexture(32, 32, radius, Color.Lerp(normalBg, Color.black, 0.15f), Color.Lerp(normalBg, Color.black, 0.2f));
            style.active.textColor = Color.Lerp(normalFg, Color.white, 0.5f);
            style.onNormal.background = CreateGradientRoundedRectTexture(32, 32, radius, onBg, Color.Lerp(onBg, Color.black, 0.05f));
            style.onNormal.textColor = onFg;
            style.onHover.background = CreateGradientRoundedRectTexture(32, 32, radius, Color.Lerp(onBg, Color.white, 0.12f), Color.Lerp(onBg, Color.white, 0.05f));
            style.onHover.textColor = onFg;
            style.onActive.background = CreateGradientRoundedRectTexture(32, 32, radius, Color.Lerp(onBg, Color.black, 0.15f), Color.Lerp(onBg, Color.black, 0.2f));
            style.onActive.textColor = onFg;
            style.border = GetBorderOffset(4f);
            return style;
        }
        #endregion

        #region Style Setup - Input
        private void SetupInputVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                inputDefaultStyle = CreateInputStyle(inputBackgroundTexture, theme.Text, theme.Text);
                inputDefaultStyle.normal.background = CreateRoundedRectTexture(128, 40, 8, theme.Secondary);
                inputDefaultStyle.focused.background = CreateRoundedRectTexture(128, 40, 8, theme.Secondary);
                inputOutlineStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    inputOutlineStyle.font = customFont;
                inputOutlineStyle.normal.background = CreateRoundedRectTexture(128, 40, 6, theme.Base);
                inputOutlineStyle.focused.background = CreateRoundedRectTexture(128, 40, 6, theme.Base);
                inputGhostStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    inputGhostStyle.font = customFont;
                inputGhostStyle.normal.background = transparentTexture;
                inputGhostStyle.focused.background = transparentTexture;
                inputFocusedStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    inputFocusedStyle.font = customFont;
                inputFocusedStyle.normal.background = inputFocusedTexture;
                inputFocusedStyle.border = new UnityHelpers.RectOffset(2, 2, 2, 2);
                inputDisabledStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    inputDisabledStyle.font = customFont;
                inputDisabledStyle.normal.textColor = theme.Muted;
                inputDisabledStyle.normal.background = CreateRoundedRectTexture(128, 40, 8, theme.Secondary);
                passwordFieldStyle = CreateStyleWithFont(inputDefaultStyle, guiHelper.fontSize + 2);
                textAreaStyle = CreateStyleWithFont(inputDefaultStyle, guiHelper.fontSize);
                textAreaStyle.wordWrap = true;
                textAreaStyle.stretchHeight = true;
                textAreaStyle.padding = new UnityHelpers.RectOffset(12, 12, 8, 8);
                textAreaFocusedStyle = new UnityHelpers.GUIStyle(inputDefaultStyle);
                if (customFont != null)
                    textAreaFocusedStyle.font = customFont;
                textAreaFocusedStyle.normal.background = inputFocusedTexture;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupInputVariantStyles", "StyleManager");
            }
        }

        private GUIStyle CreateInputStyle(Texture2D background, Color textColor, Color focusedColor)
        {
            GUIStyle style = CreateStyleWithFont(GUI.skin.textField, GetScaledFontSize(0.875f));
            style.padding = GetSpacingOffset(12f, 8f);
            style.margin = GetSpacingOffset(0f, 4f);
            style.border = GetBorderOffset(6f);
            style.fixedHeight = GetScaledHeight(36f);
            style.normal.background = background;
            style.normal.textColor = textColor;
            style.hover.background = background;
            style.hover.textColor = textColor;
            style.focused.background = inputFocusedTexture;
            style.focused.textColor = focusedColor;
            int inpRadius = GetScaledBorderRadius(6f);
            var theme = ThemeManager.Instance.CurrentTheme;
            Color focusTint = Color.Lerp(theme.Accent, theme.Overlay, 0.1f);
            style.onFocused.background = CreateGradientRoundedRectTexture(128, GetScaledHeight(36f), inpRadius, focusTint, Color.Lerp(focusTint, Color.black, 0.05f));
            return style;
        }
        #endregion

        #region Style Setup - Label
        private void SetupLabelVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                labelDefaultStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.0f), FontStyle.Normal);
                labelDefaultStyle.normal.textColor = theme.Text;
                labelDefaultStyle.padding = GetSpacingOffset(0f, 3f);
                labelDefaultStyle.wordWrap = true;
                labelDefaultStyle.alignment = TextAnchor.UpperLeft;
                labelSecondaryStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.9f));
                labelSecondaryStyle.normal.textColor = Color.Lerp(theme.Text, theme.Muted, 0.4f);
                labelSecondaryStyle.wordWrap = true;
                labelMutedStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.85f));
                labelMutedStyle.normal.textColor = theme.Muted;
                labelMutedStyle.wordWrap = true;
                labelDestructiveStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.0f), FontStyle.Bold);
                labelDestructiveStyle.normal.textColor = theme.Destructive;
                labelDestructiveStyle.wordWrap = true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupLabelVariantStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Progress
        private void SetupProgressBarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                progressBarStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                int progRadius = GetScaledBorderRadius(8f);
                progressBarStyle.normal.background = CreateRoundedRectTexture(256, 12, progRadius, theme.Secondary);
                progressBarStyle.border = GetBorderOffset(8f);
                progressBarStyle.padding = GetSpacingOffset(0f, 0f);
                progressBarStyle.margin = GetSpacingOffset(0f, 4f);
                progressBarStyle.fixedHeight = GetScaledHeight(12f);
                progressBarBackgroundStyle = new UnityHelpers.GUIStyle();
                progressBarBackgroundStyle.normal.background = CreateRoundedRectTexture(256, 12, progRadius, theme.Secondary);
                progressBarBackgroundStyle.fixedHeight = GetScaledHeight(12f);
                progressBarFillStyle = new UnityHelpers.GUIStyle();
                progressBarFillStyle.normal.background = CreateRoundedRectTexture(256, 12, progRadius, theme.Accent);
                progressBarFillStyle.fixedHeight = GetScaledHeight(12f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupProgressBarStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Separator
        private void SetupSeparatorStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                separatorHorizontalStyle = new UnityHelpers.GUIStyle();
                separatorHorizontalStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Border, Color.black, 0.1f));
                separatorHorizontalStyle.fixedHeight = Mathf.RoundToInt(1.5f * guiHelper.uiScale);
                separatorHorizontalStyle.stretchWidth = true;
                separatorHorizontalStyle.margin = GetSpacingOffset(0f, 6f);
                separatorVerticalStyle = new UnityHelpers.GUIStyle();
                separatorVerticalStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Border, Color.black, 0.1f));
                separatorVerticalStyle.fixedWidth = Mathf.RoundToInt(1.5f * guiHelper.uiScale);
                separatorVerticalStyle.stretchHeight = true;
                separatorVerticalStyle.margin = GetSpacingOffset(6f, 0f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSeparatorStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Tabs
        private void SetupTabsStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                tabsListStyle = new UnityHelpers.GUIStyle();
                tabsListStyle.normal.background = CreateGradientRoundedRectTexture(128, 128, 6, theme.TabsBg, Color.Lerp(theme.TabsBg, Color.black, 0.05f));
                tabsListStyle.border = GetBorderOffset(0f);
                tabsListStyle.padding = GetSpacingOffset(6f, 6f);
                tabsListStyle.margin = GetSpacingOffset(0f, 0f);
                tabsTriggerStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.9f), FontStyle.Bold);
                tabsTriggerStyle.alignment = TextAnchor.MiddleCenter;
                tabsTriggerStyle.padding = GetSpacingOffset(14f, 6f);
                tabsTriggerStyle.border = GetBorderOffset(0f);
                tabsTriggerStyle.normal.background = transparentTexture;
                tabsTriggerStyle.normal.textColor = Color.Lerp(theme.Muted, Color.black, 0.2f);
                tabsTriggerStyle.hover.background = CreateSolidTexture(new Color(theme.Accent.r, theme.Accent.g, theme.Accent.b, 0.08f));
                tabsTriggerStyle.active.background = transparentTexture;
                tabsTriggerActiveStyle = new UnityHelpers.GUIStyle(tabsTriggerStyle);
                if (customFont != null)
                    tabsTriggerActiveStyle.font = customFont;

                tabsTriggerActiveStyle.normal.background = CreateBottomBorderTexture(128, GetScaledHeight(40f), Mathf.Max(3, Mathf.RoundToInt(3f * guiHelper.uiScale)), theme.Accent, theme.Base);
                tabsTriggerActiveStyle.normal.textColor = theme.TabsTriggerActiveFg;
                tabsTriggerActiveStyle.hover.background = tabsTriggerActiveStyle.normal.background;
                tabsTriggerActiveStyle.active.background = tabsTriggerActiveStyle.normal.background;
                tabsContentStyle = new UnityHelpers.GUIStyle();
                tabsContentStyle.padding = GetSpacingOffset(18f, 18f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTabsStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - TextArea
        private void SetupTextAreaVariantStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                textAreaStyle = CreateStyleWithFont(GUI.skin.textArea, GetScaledFontSize(0.875f));
                textAreaStyle.padding = GetSpacingOffset(12f, 8f);
                textAreaStyle.border = GetBorderOffset(6f);
                textAreaStyle.normal.background = CreateRoundedRectTexture(128, 80, 8, theme.Secondary);
                textAreaStyle.normal.textColor = theme.Text;
                textAreaStyle.focused.background = CreateRoundedRectTexture(128, 80, 8, Color.Lerp(theme.Secondary, theme.Accent, 0.1f));
                textAreaStyle.focused.textColor = theme.Text;
                textAreaStyle.hover.background = CreateRoundedRectTexture(128, 80, 8, Color.Lerp(theme.Secondary, Color.white, 0.08f));
                textAreaStyle.wordWrap = true;
                textAreaStyle.stretchHeight = true;
                textAreaFocusedStyle = new UnityHelpers.GUIStyle(textAreaStyle);
                textAreaFocusedStyle.normal.background = CreateRoundedRectTexture(128, 80, 8, Color.Lerp(theme.Secondary, theme.Accent, 0.1f));
                textAreaFocusedStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                textAreaOutlineStyle = new UnityHelpers.GUIStyle(textAreaStyle);
                textAreaOutlineStyle.normal.background = CreateOutlineTexture();
                textAreaOutlineStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.black, 0.15f));
                textAreaGhostStyle = new UnityHelpers.GUIStyle(textAreaStyle);
                textAreaGhostStyle.normal.background = transparentTexture;
                textAreaGhostStyle.focused.background = CreateSolidTexture(Color.Lerp(theme.Secondary, Color.black, 0.08f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTextAreaVariantStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Checkbox
        private void SetupCheckboxStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                checkboxDefaultStyle = new UnityHelpers.GUIStyle(GUI.skin.toggle);
                if (customFont != null)
                    checkboxDefaultStyle.font = customFont;
                checkboxDefaultStyle.fontSize = GetScaledFontSize(0.9f);
                int cbRadius = GetScaledBorderRadius(6f);
                checkboxDefaultStyle.normal.background = CreateRoundedRectTexture(20, 20, cbRadius, theme.Secondary);
                checkboxDefaultStyle.normal.textColor = theme.Text;
                checkboxDefaultStyle.hover.background = CreateRoundedRectTexture(20, 20, cbRadius, Color.Lerp(theme.Secondary, Color.white, 0.15f));
                checkboxDefaultStyle.hover.textColor = theme.Text;
                checkboxDefaultStyle.active.background = CreateRoundedRectTexture(20, 20, cbRadius, Color.Lerp(theme.Secondary, Color.black, 0.1f));
                checkboxDefaultStyle.active.textColor = theme.Text;
                checkboxDefaultStyle.onNormal.background = CreateRoundedRectTexture(20, 20, cbRadius, theme.Accent);
                checkboxDefaultStyle.onNormal.textColor = Color.white;
                checkboxDefaultStyle.onHover.background = CreateRoundedRectTexture(20, 20, cbRadius, Color.Lerp(theme.Accent, Color.white, 0.12f));
                checkboxDefaultStyle.onHover.textColor = Color.white;
                checkboxDefaultStyle.onActive.background = CreateRoundedRectTexture(20, 20, cbRadius, Color.Lerp(theme.Accent, Color.black, 0.1f));
                checkboxDefaultStyle.onActive.textColor = Color.white;
                checkboxDefaultStyle.border = GetBorderOffset(6f);
                checkboxDefaultStyle.padding = GetSpacingOffset(8f, 0f);
                checkboxOutlineStyle = new UnityHelpers.GUIStyle(checkboxDefaultStyle);
                if (customFont != null)
                    checkboxOutlineStyle.font = customFont;
                checkboxOutlineStyle.normal.background = CreateOutlineTexture();
                checkboxOutlineStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.white, 0.12f));
                checkboxOutlineStyle.hover.textColor = Color.white;
                checkboxOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.black, 0.15f));
                checkboxOutlineStyle.active.textColor = Color.white;
                checkboxOutlineStyle.onNormal.background = CreateOutlineButtonTexture(theme.Accent, theme.Accent);
                checkboxOutlineStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.white, 0.12f));
                checkboxOutlineStyle.onHover.textColor = Color.white;
                checkboxOutlineStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.black, 0.15f));
                checkboxOutlineStyle.onActive.textColor = Color.white;
                checkboxGhostStyle = new UnityHelpers.GUIStyle(checkboxDefaultStyle);
                if (customFont != null)
                    checkboxGhostStyle.font = customFont;
                checkboxGhostStyle.normal.background = transparentTexture;
                checkboxGhostStyle.normal.textColor = theme.Text;
                checkboxGhostStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Base, Color.black, 0.12f));
                checkboxGhostStyle.hover.textColor = theme.Text;
                checkboxGhostStyle.active.background = CreateSolidTexture(Color.Lerp(theme.Base, Color.black, 0.2f));
                checkboxGhostStyle.active.textColor = theme.Text;
                checkboxGhostStyle.onNormal.background = CreateSolidTexture(theme.Accent);
                checkboxGhostStyle.onNormal.textColor = Color.white;
                checkboxGhostStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.white, 0.12f));
                checkboxGhostStyle.onHover.textColor = Color.white;
                checkboxGhostStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.black, 0.15f));
                checkboxGhostStyle.onActive.textColor = Color.white;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCheckboxStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Switch
        private void SetupSwitchStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                switchDefaultStyle = new UnityHelpers.GUIStyle(GUI.skin.toggle);
                if (customFont != null)
                    switchDefaultStyle.font = customFont;
                switchDefaultStyle.fontSize = GetScaledFontSize(0.875f);
                switchDefaultStyle.normal.background = switchOffTexture;
                switchDefaultStyle.normal.textColor = theme.Text;
                switchDefaultStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Secondary, Color.white, 0.12f));
                switchDefaultStyle.hover.textColor = theme.Text;
                switchDefaultStyle.active.background = CreateSolidTexture(Color.Lerp(theme.Secondary, Color.black, 0.15f));
                switchDefaultStyle.active.textColor = theme.Text;
                switchDefaultStyle.onNormal.background = switchOnTexture;
                switchDefaultStyle.onNormal.textColor = theme.Text;
                switchDefaultStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.white, 0.12f));
                switchDefaultStyle.onHover.textColor = theme.Text;
                switchDefaultStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.black, 0.15f));
                switchDefaultStyle.onActive.textColor = theme.Text;
                switchDefaultStyle.border = GetBorderOffset(6f);
                switchOutlineStyle = new UnityHelpers.GUIStyle(switchDefaultStyle);
                if (customFont != null)
                    switchOutlineStyle.font = customFont;
                switchOutlineStyle.normal.background = CreateOutlineTexture();
                switchOutlineStyle.normal.textColor = theme.Text;
                switchOutlineStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.white, 0.12f));
                switchOutlineStyle.hover.textColor = theme.Text;
                switchOutlineStyle.active.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.black, 0.15f));
                switchOutlineStyle.active.textColor = theme.Text;
                switchOutlineStyle.onNormal.background = CreateOutlineButtonTexture(theme.Accent, theme.Accent);
                switchOutlineStyle.onNormal.textColor = theme.Text;
                switchOutlineStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.white, 0.12f));
                switchOutlineStyle.onHover.textColor = theme.Text;
                switchOutlineStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.black, 0.15f));
                switchOutlineStyle.onActive.textColor = theme.Text;
                switchGhostStyle = new UnityHelpers.GUIStyle(switchDefaultStyle);
                if (customFont != null)
                    switchGhostStyle.font = customFont;
                switchGhostStyle.normal.background = transparentTexture;
                switchGhostStyle.normal.textColor = theme.Text;
                switchGhostStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Base, Color.black, 0.12f));
                switchGhostStyle.hover.textColor = theme.Text;
                switchGhostStyle.active.background = CreateSolidTexture(Color.Lerp(theme.Base, Color.black, 0.2f));
                switchGhostStyle.active.textColor = theme.Text;
                switchGhostStyle.onNormal.background = CreateSolidTexture(theme.Accent);
                switchGhostStyle.onNormal.textColor = theme.Text;
                switchGhostStyle.onHover.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.white, 0.12f));
                switchGhostStyle.onHover.textColor = theme.Text;
                switchGhostStyle.onActive.background = CreateSolidTexture(Color.Lerp(theme.Accent, Color.black, 0.15f));
                switchGhostStyle.onActive.textColor = theme.Text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSwitchStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Badge
        private void SetupBadgeStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                badgeDefaultStyle = CreateStyleWithFont(GUI.skin.box, GetScaledFontSize(0.8f), FontStyle.Bold);
                badgeDefaultStyle.normal.background = CreateRoundedRectTexture(128, 24, 12, theme.Elevated);
                badgeDefaultStyle.normal.textColor = theme.Text;
                badgeDefaultStyle.border = GetBorderOffset(12f);
                badgeDefaultStyle.padding = GetSpacingOffset(10f, 4f);
                badgeDefaultStyle.alignment = TextAnchor.MiddleCenter;
                badgeSecondaryStyle = new UnityHelpers.GUIStyle(badgeDefaultStyle);
                if (customFont != null)
                    badgeSecondaryStyle.font = customFont;
                badgeSecondaryStyle.normal.background = CreateGradientRoundedRectTexture(128, 24, 12, theme.Secondary, Color.Lerp(theme.Secondary, Color.black, 0.05f));
                badgeDestructiveStyle = new UnityHelpers.GUIStyle(badgeDefaultStyle);
                if (customFont != null)
                    badgeDestructiveStyle.font = customFont;
                badgeDestructiveStyle.normal.background = CreateGradientRoundedRectTexture(128, 24, 12, theme.Destructive, Color.Lerp(theme.Destructive, Color.black, 0.1f));
                badgeOutlineStyle = new UnityHelpers.GUIStyle(badgeDefaultStyle);
                if (customFont != null)
                    badgeOutlineStyle.font = customFont;
                badgeOutlineStyle.normal.background = CreateOutlineTexture();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupBadgeStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Avatar
        private void SetupAvatarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                int defaultAvatarSize = GetScaledHeight(40f);
                avatarStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                avatarStyle.normal.background = CreateSolidTexture(theme.Secondary);
                avatarStyle.alignment = TextAnchor.MiddleCenter;
                avatarStyle.fixedWidth = defaultAvatarSize;
                avatarStyle.fixedHeight = defaultAvatarSize;
                avatarStyle.border = GetAvatarBorder(AvatarShape.Circle, AvatarSize.Default);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupAvatarStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Table
        private void SetupTableStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                tableStyle = new UnityHelpers.GUIStyle();
                tableStyle.normal.background = CreateSolidTexture(theme.Secondary);
                tableStyle.border = GetBorderOffset(1f);
                tableStyle.padding = GetSpacingOffset(0f, 0f);
                tableStyle.margin = GetSpacingOffset(0f, 0f);
                tableHeaderStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.9f), FontStyle.Bold);
                tableHeaderStyle.normal.background = CreateGradientRoundedRectTexture(128, 40, 0, Color.Lerp(theme.Secondary, theme.Accent, 0.15f), Color.Lerp(theme.Secondary, theme.Accent, 0.1f));
                tableHeaderStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.2f));
                tableHeaderStyle.active.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.25f));
                tableHeaderStyle.normal.textColor = theme.Text;
                tableHeaderStyle.hover.textColor = theme.Text;
                tableHeaderStyle.active.textColor = theme.Text;
                tableHeaderStyle.padding = GetSpacingOffset(14f, 14f);
                tableHeaderStyle.alignment = TextAnchor.MiddleLeft;
                tableHeaderStyle.border = GetBorderOffset(0f);
                tableCellStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.95f));
                tableCellStyle.normal.background = CreateSolidTexture(theme.Secondary);
                tableCellStyle.normal.textColor = theme.Text;
                tableCellStyle.padding = GetSpacingOffset(14f, 14f);
                tableCellStyle.alignment = TextAnchor.MiddleLeft;
                tableCellStyle.wordWrap = false;
                tableCellStyle.clipping = TextClipping.Clip;
                tableStripedStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableStripedStyle.font = customFont;
                tableStripedStyle.normal.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.08f));
                tableBorderedStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableBorderedStyle.font = customFont;
                tableBorderedStyle.normal.background = CreateBorderTexture(theme.Border, 1);
                tableHoverStyle = new UnityHelpers.GUIStyle(tableCellStyle);
                if (customFont != null)
                    tableHoverStyle.font = customFont;
                tableHoverStyle.normal.background = CreateSolidTexture(theme.Secondary);
                tableHoverStyle.normal.textColor = theme.Text;
                tableHoverStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.12f));
                tableHoverStyle.hover.textColor = theme.Text;
                tableHoverStyle.active.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.12f));
                tableHoverStyle.active.textColor = theme.Text;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupTableStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Calendar
        private void SetupCalendarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                calendarStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                calendarStyle.normal.background = CreateSolidTexture(theme.Secondary);
                calendarStyle.border = GetBorderOffset(4f);
                calendarStyle.padding = GetSpacingOffset(12f, 12f);
                calendarHeaderStyle = new UnityHelpers.GUIStyle();
                calendarHeaderStyle.padding = GetSpacingOffset(0f, 4f);
                calendarTitleStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.125f), FontStyle.Bold);
                calendarTitleStyle.normal.textColor = theme.Text;
                calendarTitleStyle.alignment = TextAnchor.MiddleCenter;
                calendarWeekdayStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.875f));
                calendarWeekdayStyle.normal.textColor = Color.Lerp(theme.Text, Color.black, 0.35f);
                calendarWeekdayStyle.alignment = TextAnchor.MiddleCenter;
                calendarDayStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(1.0f));
                calendarDayStyle.normal.textColor = theme.Text;
                calendarDayStyle.normal.background = CreateSolidTexture(theme.Secondary);
                calendarDayStyle.hover.background = CreateGradientRoundedRectTexture(32, 32, 4, Color.Lerp(theme.Secondary, theme.Accent, 0.12f), Color.Lerp(theme.Secondary, theme.Accent, 0.08f));
                calendarDaySelectedStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDaySelectedStyle.font = customFont;
                calendarDaySelectedStyle.normal.background = CreateSolidTexture(theme.Accent);
                calendarDaySelectedStyle.normal.textColor = theme.Text;
                calendarDayOutsideMonthStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayOutsideMonthStyle.font = customFont;
                calendarDayOutsideMonthStyle.normal.textColor = Color.Lerp(theme.Text, Color.black, 0.5f);
                calendarDayTodayStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayTodayStyle.font = customFont;
                calendarDayTodayStyle.normal.background = CreateOutlineButtonTexture(theme.Secondary, theme.Accent);
                calendarDayInRangeStyle = new UnityHelpers.GUIStyle(calendarDayStyle);
                if (customFont != null)
                    calendarDayInRangeStyle.font = customFont;
                calendarDayInRangeStyle.normal.background = CreateSolidTexture(new Color(theme.Accent.r, theme.Accent.g, theme.Accent.b, 0.55f));
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupCalendarStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - DropdownMenu
        private void SetupDropdownMenuStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                dropdownMenuContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                int dropdownRadius = GetScaledBorderRadius(8f);
                dropdownMenuContentStyle.normal.background = CreateRoundedRectWithShadowTexture(256, 256, dropdownRadius, theme.Secondary, 0.18f, 10);
                dropdownMenuContentStyle.border = GetBorderOffset(8f);
                dropdownMenuContentStyle.padding = GetSpacingOffset(6f, 6f);
                dropdownMenuItemStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.9f));
                dropdownMenuItemStyle.alignment = TextAnchor.MiddleLeft;
                dropdownMenuItemStyle.normal.background = transparentTexture;
                dropdownMenuItemStyle.normal.textColor = theme.Text;
                dropdownMenuItemStyle.hover.background = CreateGradientRoundedRectTexture(128, 32, 6, Color.Lerp(theme.Accent, Color.white, 0.15f), Color.Lerp(theme.Accent, Color.white, 0.1f));
                dropdownMenuItemStyle.hover.textColor = theme.Accent;
                dropdownMenuItemStyle.active.background = CreateGradientRoundedRectTexture(128, 32, 6, Color.Lerp(theme.Accent, Color.black, 0.1f), Color.Lerp(theme.Accent, Color.black, 0.15f));
                dropdownMenuItemStyle.active.textColor = theme.Accent;
                dropdownMenuItemStyle.padding = GetSpacingOffset(14f, 6f);
                dropdownMenuSeparatorStyle = new UnityHelpers.GUIStyle();
                dropdownMenuSeparatorStyle.normal.background = CreateSolidTexture(theme.Border);
                dropdownMenuSeparatorStyle.fixedHeight = 1;
                dropdownMenuSeparatorStyle.margin = GetSpacingOffset(6f, 6f);
                dropdownMenuHeaderStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.8f), FontStyle.Bold);
                dropdownMenuHeaderStyle.normal.textColor = theme.Muted;
                dropdownMenuHeaderStyle.padding = GetSpacingOffset(14f, 6f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDropdownMenuStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Popover
        private void SetupPopoverStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                popoverContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                popoverContentStyle.normal.background = CreateSolidTexture(theme.Secondary);
                popoverContentStyle.border = GetBorderOffset(6f);
                popoverContentStyle.padding = GetSpacingOffset(12f, 12f);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupPopoverStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Select
        private void SetupSelectStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                float scaledFontSize = guiHelper.fontSize * guiHelper.uiScale;
                int borderRadius = Mathf.RoundToInt(guiHelper.cornerRadius * guiHelper.uiScale);
                selectTriggerStyle = CreateStyleWithFont(GUI.skin.button, Mathf.RoundToInt(scaledFontSize));
                selectTriggerStyle.alignment = TextAnchor.MiddleLeft;
                selectTriggerStyle.normal.background = selectTriggerTexture;
                selectTriggerStyle.normal.textColor = theme.Text;
                selectTriggerStyle.hover.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(theme.Base, theme.Accent, 0.12f), Color.Lerp(theme.Base, theme.Accent, 0.08f));
                selectTriggerStyle.active.background = CreateGradientRoundedRectTexture(128, 40, 6, Color.Lerp(theme.Base, theme.Accent, 0.25f), Color.Lerp(theme.Base, theme.Accent, 0.2f));
                selectTriggerStyle.padding = new UnityHelpers.RectOffset(10, 10, 5, 5);
                selectTriggerStyle.border = new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                selectContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                selectContentStyle.normal.background = CreateSolidTexture(theme.Secondary);
                selectContentStyle.border = new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
                selectContentStyle.padding = new UnityHelpers.RectOffset(5, 5, 5, 5);
                selectItemStyle = CreateStyleWithFont(GUI.skin.button, Mathf.RoundToInt(scaledFontSize));
                selectItemStyle.alignment = TextAnchor.MiddleLeft;
                selectItemStyle.normal.background = transparentTexture;
                selectItemStyle.normal.textColor = theme.Text;
                selectItemStyle.hover.background = CreateSolidTexture(GetHoverColor(theme.Base));
                selectItemStyle.hover.textColor = theme.Text;
                selectItemStyle.active.background = CreateSolidTexture(GetHoverColor(theme.Base, true));
                selectItemStyle.active.textColor = theme.Text;
                selectItemStyle.padding = new UnityHelpers.RectOffset(10, 10, 5, 5);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupSelectStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - DatePicker
        private void SetupDatePickerStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                datePickerStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                datePickerStyle.normal.background = CreateSolidTexture(theme.Secondary);
                datePickerStyle.border = new UnityHelpers.RectOffset(4, 4, 4, 4);
                datePickerStyle.padding = new UnityHelpers.RectOffset(10, 10, 10, 10);
                datePickerHeaderStyle = new UnityHelpers.GUIStyle();
                datePickerHeaderStyle.padding = new UnityHelpers.RectOffset(0, 0, 5, 5);
                datePickerTitleStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize + 2, FontStyle.Bold);
                datePickerTitleStyle.normal.textColor = theme.Text;
                datePickerTitleStyle.alignment = TextAnchor.MiddleCenter;
                datePickerWeekdayStyle = CreateStyleWithFont(GUI.skin.label, guiHelper.fontSize - 1);
                datePickerWeekdayStyle.normal.textColor = Color.Lerp(theme.Text, Color.black, 0.35f);
                datePickerWeekdayStyle.alignment = TextAnchor.MiddleCenter;
                datePickerDayStyle = CreateStyleWithFont(GUI.skin.button, guiHelper.fontSize);
                datePickerDayStyle.normal.textColor = theme.Text;
                datePickerDayStyle.normal.background = CreateSolidTexture(theme.Secondary);
                datePickerDayStyle.hover.background = CreateSolidTexture(Color.Lerp(theme.Secondary, theme.Accent, 0.12f));
                datePickerDaySelectedStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDaySelectedStyle.font = customFont;
                datePickerDaySelectedStyle.normal.background = CreateSolidTexture(theme.Accent);
                datePickerDaySelectedStyle.normal.textColor = theme.Text;
                datePickerDayOutsideMonthStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDayOutsideMonthStyle.font = customFont;
                datePickerDayOutsideMonthStyle.normal.textColor = Color.Lerp(theme.Text, Color.black, 0.5f);
                datePickerDayTodayStyle = new UnityHelpers.GUIStyle(datePickerDayStyle);
                if (customFont != null)
                    datePickerDayTodayStyle.font = customFont;
                datePickerDayTodayStyle.normal.background = CreateOutlineButtonTexture(theme.Secondary, theme.Accent);
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDatePickerStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Dialog
        private void SetupDialogStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                dialogOverlayStyle = new UnityHelpers.GUIStyle();
                dialogOverlayStyle.normal.background = CreateSolidTexture(new Color(0, 0, 0, 0.85f));
                dialogContentStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                dialogContentStyle.normal.background = CreateRoundedRectWithShadowTexture(512, 512, 12, theme.Secondary, 0.18f, 12);
                dialogContentStyle.border = new UnityHelpers.RectOffset(12, 12, 12, 12);
                dialogContentStyle.padding = new UnityHelpers.RectOffset(24, 24, 24, 24);
                dialogTitleStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.25f), FontStyle.Bold);
                dialogTitleStyle.normal.textColor = theme.Text;
                dialogTitleStyle.wordWrap = true;
                dialogDescriptionStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(1.0f));
                dialogDescriptionStyle.normal.textColor = theme.Muted;
                dialogDescriptionStyle.wordWrap = true;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupDialogStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - Chart
        private void SetupChartStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                chartContainerStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                chartContainerStyle.normal.background = chartContainerTexture;
                chartContainerStyle.border = GetBorderOffset(12f);
                chartContainerStyle.padding = GetSpacingOffset(16f, 16f);
                chartContainerStyle.margin = GetSpacingOffset(0f, 0f);
                chartContentStyle = new UnityHelpers.GUIStyle();
                chartContentStyle.normal.background = transparentTexture;
                chartContentStyle.padding = GetSpacingOffset(0f, 0f);
                chartContentStyle.margin = GetSpacingOffset(0f, 0f);
                chartAxisStyle = CreateStyleWithFont(GUI.skin.label, GetScaledFontSize(0.75f));
                chartAxisStyle.normal.textColor = theme.Muted;
                chartAxisStyle.alignment = TextAnchor.MiddleCenter;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupChartStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Setup - MenuBar
        private void SetupMenuBarStyles()
        {
            try
            {
                var theme = ThemeManager.Instance.CurrentTheme;
                menuBarStyle = new UnityHelpers.GUIStyle();
                menuBarStyle.padding = GetSpacingOffset(4f, 0f);
                menuBarStyle.margin = GetSpacingOffset(0f, 0f);
                menuBarStyle.fixedHeight = GetScaledHeight(40f);
                menuBarStyle.stretchWidth = false;
                menuBarItemStyle = CreateStyleWithFont(GUI.skin.button, GetScaledFontSize(0.875f));
                menuBarItemStyle.alignment = TextAnchor.MiddleLeft;
                menuBarItemStyle.padding = GetSpacingOffset(12f, 8f);
                menuBarItemStyle.margin = GetSpacingOffset(0f, 0f);
                menuBarItemStyle.border = new UnityHelpers.RectOffset(0, 0, 0, 0);
                menuBarItemStyle.normal.background = transparentTexture;
                menuBarItemStyle.normal.textColor = theme.Text;
                menuBarItemStyle.hover.background = CreateSolidTexture(GetHoverColor(theme.Base));
                menuBarItemStyle.hover.textColor = theme.Text;
                menuBarItemStyle.active.background = CreateSolidTexture(GetHoverColor(theme.Base, true));
                menuBarItemStyle.active.textColor = theme.Text;
                menuBarItemStyle.stretchWidth = false;
                menuDropdownStyle = new UnityHelpers.GUIStyle(GUI.skin.box);
                menuDropdownStyle.normal.background = CreateGradientRoundedRectWithShadowTexture(200, 200, 4, theme.Secondary, theme.Secondary, 0.1f, 6);
                menuDropdownStyle.border = GetBorderOffset(4f);
                menuDropdownStyle.padding = GetSpacingOffset(4f, 4f);
                menuDropdownStyle.margin = GetSpacingOffset(0f, 0f);
                menuDropdownStyle.fixedWidth = 200;
                menuDropdownStyle.stretchWidth = false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "SetupMenuBarStyles", "StyleManager");
            }
        }
        #endregion

        #region Style Getters - Public Only
        public GUIStyle GetAnimatedBoxStyle() => animatedBoxStyle ?? GUI.skin.box;

        public GUIStyle GetSectionHeaderStyle() => sectionHeaderStyle ?? GUI.skin.label;

        public GUIStyle GetCardStyle() => cardStyle ?? GUI.skin.box;

        public GUIStyle GetCardHeaderStyle() => cardHeaderStyle ?? GUIStyle.none;

        public GUIStyle GetCardTitleStyle() => cardTitleStyle ?? GUI.skin.label;

        public GUIStyle GetCardDescriptionStyle() => cardDescriptionStyle ?? GUI.skin.label;

        public GUIStyle GetCardContentStyle() => cardContentStyle ?? GUIStyle.none;

        public GUIStyle GetCardFooterStyle() => cardFooterStyle ?? GUIStyle.none;

        public GUIStyle GetButtonStyle(ButtonVariant variant, ButtonSize size)
        {
            if (buttonStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            var baseStyle = variant switch
            {
                ButtonVariant.Destructive => buttonDestructiveStyle,
                ButtonVariant.Outline => buttonOutlineStyle,
                ButtonVariant.Secondary => buttonSecondaryStyle,
                ButtonVariant.Ghost => buttonGhostStyle,
                ButtonVariant.Link => buttonLinkStyle,
                _ => buttonDefaultStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.button);
            ApplyButtonSizing(sizedStyle, size);
            buttonStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplyButtonSizing(GUIStyle style, ButtonSize size)
        {
            switch (size)
            {
                case ButtonSize.Small:
                    style.fontSize = GetScaledFontSize(0.75f);
                    style.padding = GetSpacingOffset(12f, 2f);
                    style.fixedHeight = GetScaledHeight(32f);
                    style.border = GetBorderOffset(6f);
                    break;

                case ButtonSize.Large:
                    style.fontSize = GetScaledFontSize(1.0f);
                    style.padding = GetSpacingOffset(24f, 12f);
                    style.fixedHeight = GetScaledHeight(44f);
                    style.border = GetBorderOffset(6f);
                    break;

                case ButtonSize.Icon:
                    style.fontSize = GetScaledFontSize(1.0f);
                    style.padding = GetSpacingOffset(0f, 0f);
                    int iconSize = GetScaledHeight(36f);
                    style.fixedWidth = iconSize;
                    style.fixedHeight = iconSize;
                    style.border = GetBorderOffset(6f);
                    break;

                case ButtonSize.Default:
                    style.fontSize = GetScaledFontSize(0.875f);
                    style.padding = GetSpacingOffset(16f, 8f);
                    style.fixedHeight = GetScaledHeight(36f);
                    style.border = GetBorderOffset(6f);
                    break;
            }
        }

        public GUIStyle GetToggleStyle(ToggleVariant variant, ToggleSize size)
        {
            if (toggleStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                ToggleVariant.Outline => toggleOutlineStyle,
                ToggleVariant.Default => toggleDefaultStyle,
                _ => toggleDefaultStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.button);
            ApplyToggleSizing(sizedStyle, size);
            toggleStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplyToggleSizing(GUIStyle style, ToggleSize size)
        {
            switch (size)
            {
                case ToggleSize.Small:
                    style.fontSize = GetScaledFontSize(0.75f);
                    style.padding = GetSpacingOffset(6f, 2f);
                    break;
                case ToggleSize.Large:
                    style.fontSize = GetScaledFontSize(1.25f);
                    style.padding = GetSpacingOffset(10f, 6f);
                    break;
                case ToggleSize.Default:
                    style.fontSize = GetScaledFontSize(1.0f);
                    style.padding = GetSpacingOffset(8f, 4f);
                    break;
            }
        }

        public GUIStyle GetInputStyle(InputVariant variant, bool focused = false, bool disabled = false)
        {
            if (inputStyleCache.TryGetValue((variant, focused, disabled), out var cachedStyle))
                return cachedStyle;
            GUIStyle style =
                disabled ? (inputDisabledStyle ?? GUI.skin.textField)
                : focused ? (inputFocusedStyle ?? GUI.skin.textField)
                : variant switch
                {
                    InputVariant.Outline => inputOutlineStyle ?? GUI.skin.textField,
                    InputVariant.Ghost => inputGhostStyle ?? GUI.skin.textField,
                    InputVariant.Default => inputDefaultStyle ?? GUI.skin.textField,
                    _ => inputDefaultStyle ?? GUI.skin.textField,
                };
            inputStyleCache[(variant, focused, disabled)] = style;
            return style;
        }

        public GUIStyle GetLabelStyle(LabelVariant variant)
        {
            if (labelStyleCache.TryGetValue(variant, out var cachedStyle))
                return cachedStyle;
            GUIStyle style = variant switch
            {
                LabelVariant.Secondary => labelSecondaryStyle ?? GUI.skin.label,
                LabelVariant.Muted => labelMutedStyle ?? GUI.skin.label,
                LabelVariant.Destructive => labelDestructiveStyle ?? GUI.skin.label,
                LabelVariant.Default => labelDefaultStyle ?? GUI.skin.label,
                _ => labelDefaultStyle ?? GUI.skin.label,
            };
            labelStyleCache[variant] = style;
            return style;
        }

        public GUIStyle GetPasswordFieldStyle() => passwordFieldStyle ?? GUI.skin.textField;

        public GUIStyle GetTextAreaStyle(TextAreaVariant variant = TextAreaVariant.Default, bool focused = false)
        {
            if (textAreaStyleCache.TryGetValue((variant, focused), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                TextAreaVariant.Outline => textAreaOutlineStyle ?? textAreaStyle ?? GUI.skin.textArea,
                TextAreaVariant.Ghost => textAreaGhostStyle ?? textAreaStyle ?? GUI.skin.textArea,
                TextAreaVariant.Default => textAreaStyle ?? GUI.skin.textArea,
                _ => textAreaStyle ?? GUI.skin.textArea,
            };
            GUIStyle style = focused ? (textAreaFocusedStyle ?? baseStyle) : baseStyle;
            textAreaStyleCache[(variant, focused)] = style;
            return style;
        }

        public GUIStyle GetProgressBarStyle() => progressBarStyle ?? GUI.skin.box;

        public GUIStyle GetProgressBarBackgroundStyle() => progressBarBackgroundStyle ?? GUI.skin.box;

        public GUIStyle GetProgressBarFillStyle() => progressBarFillStyle ?? GUI.skin.box;

        public GUIStyle GetSeparatorStyle(SeparatorOrientation orientation) =>
            orientation switch
            {
                SeparatorOrientation.Horizontal => separatorHorizontalStyle ?? GUI.skin.box,
                SeparatorOrientation.Vertical => separatorVerticalStyle ?? GUI.skin.box,
                _ => separatorHorizontalStyle ?? GUI.skin.box,
            };

        public GUIStyle GetTabsListStyle() => tabsListStyle ?? GUI.skin.box;

        public GUIStyle GetTabsTriggerStyle(bool active = false) => active ? (tabsTriggerActiveStyle ?? tabsTriggerStyle ?? GUI.skin.button) : (tabsTriggerStyle ?? GUI.skin.button);

        public GUIStyle GetTabsContentStyle() => tabsContentStyle ?? GUIStyle.none;

        public GUIStyle GetCheckboxStyle(CheckboxVariant variant, CheckboxSize size)
        {
            if (checkboxStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                CheckboxVariant.Outline => checkboxOutlineStyle,
                CheckboxVariant.Ghost => checkboxGhostStyle,
                CheckboxVariant.Default => checkboxDefaultStyle,
                _ => checkboxDefaultStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.toggle);
            ApplyCheckboxSizing(sizedStyle, size);
            checkboxStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplyCheckboxSizing(GUIStyle style, CheckboxSize size)
        {
            switch (size)
            {
                case CheckboxSize.Small:
                    style.fontSize = GetScaledFontSize(0.75f);
                    style.padding = GetSpacingOffset(6f, 0f);
                    break;
                case CheckboxSize.Large:
                    style.fontSize = GetScaledFontSize(1.25f);
                    style.padding = GetSpacingOffset(10f, 0f);
                    break;
                case CheckboxSize.Default:
                    style.fontSize = GetScaledFontSize(0.9f);
                    style.padding = GetSpacingOffset(8f, 0f);
                    break;
            }
        }

        public GUIStyle GetSwitchStyle(SwitchVariant variant, SwitchSize size)
        {
            if (switchStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                SwitchVariant.Outline => switchOutlineStyle,
                SwitchVariant.Ghost => switchGhostStyle,
                SwitchVariant.Default => switchDefaultStyle,
                _ => switchDefaultStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.toggle);
            ApplySwitchSizing(sizedStyle, size);
            switchStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplySwitchSizing(GUIStyle style, SwitchSize size)
        {
            switch (size)
            {
                case SwitchSize.Small:
                    style.fontSize = GetScaledFontSize(0.75f);
                    style.padding = GetSpacingOffset(6f, 2f);
                    break;
                case SwitchSize.Large:
                    style.fontSize = GetScaledFontSize(1.25f);
                    style.padding = GetSpacingOffset(10f, 6f);
                    break;
                case SwitchSize.Default:
                    style.fontSize = GetScaledFontSize(0.875f);
                    style.padding = GetSpacingOffset(8f, 4f);
                    break;
            }
        }

        public GUIStyle GetBadgeStyle(BadgeVariant variant, BadgeSize size)
        {
            if (badgeStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                BadgeVariant.Secondary => badgeSecondaryStyle,
                BadgeVariant.Destructive => badgeDestructiveStyle,
                BadgeVariant.Outline => badgeOutlineStyle,
                BadgeVariant.Default => badgeDefaultStyle,
                _ => badgeDefaultStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.box);
            ApplyBadgeSizing(sizedStyle, size);
            badgeStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplyBadgeSizing(GUIStyle style, BadgeSize size)
        {
            switch (size)
            {
                case BadgeSize.Small:
                    style.fontSize = GetScaledFontSize(0.65f);
                    style.padding = GetSpacingOffset(8f, 2f);
                    break;
                case BadgeSize.Large:
                    style.fontSize = GetScaledFontSize(1.0f);
                    style.padding = GetSpacingOffset(12f, 6f);
                    break;
                case BadgeSize.Default:
                    style.fontSize = GetScaledFontSize(0.8f);
                    style.padding = GetSpacingOffset(10f, 4f);
                    break;
            }
        }

        public GUIStyle GetAvatarStyle(AvatarSize size, AvatarShape shape)
        {
            if (avatarStyleCache.TryGetValue((size, shape), out var cachedStyle))
                return cachedStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(avatarStyle ?? GUI.skin.box);
            int avatarSizeValue = size switch
            {
                AvatarSize.Small => GetScaledHeight(32f),
                AvatarSize.Large => GetScaledHeight(48f),
                AvatarSize.Default => GetScaledHeight(40f),
                _ => GetScaledHeight(40f),
            };
            sizedStyle.fixedWidth = avatarSizeValue;
            sizedStyle.fixedHeight = avatarSizeValue;
            sizedStyle.border = GetAvatarBorder(shape, size);
            avatarStyleCache[(size, shape)] = sizedStyle;
            return sizedStyle;
        }

        public RectOffset GetAvatarBorder(AvatarShape shape, AvatarSize size)
        {
            int borderRadius = shape switch
            {
                AvatarShape.Circle => Mathf.RoundToInt(50 * guiHelper.uiScale),
                AvatarShape.Rounded => GetScaledBorderRadius(8f),
                AvatarShape.Square => 0,
                _ => 0,
            };
            return new UnityHelpers.RectOffset(borderRadius, borderRadius, borderRadius, borderRadius);
        }

        public float GetStatusIndicatorSize(AvatarSize size) =>
            size switch
            {
                AvatarSize.Small => 6f,
                AvatarSize.Large => 12f,
                AvatarSize.Default => 8f,
                _ => 8f,
            };

        public GUIStyle GetTableStyle(TableVariant variant, TableSize size)
        {
            if (tableStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                TableVariant.Striped => tableStripedStyle,
                TableVariant.Bordered => tableBorderedStyle,
                TableVariant.Hover => tableHoverStyle,
                TableVariant.Default => tableStyle,
                _ => tableStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.box);
            ApplyTableSizing(sizedStyle, size);
            tableStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplyTableSizing(GUIStyle style, TableSize size)
        {
            switch (size)
            {
                case TableSize.Small:
                    style.fontSize = GetScaledFontSize(0.9f);
                    style.padding = GetSpacingOffset(8f, 8f);
                    break;
                case TableSize.Large:
                    style.fontSize = GetScaledFontSize(1.1f);
                    style.padding = GetSpacingOffset(20f, 20f);
                    break;
                case TableSize.Default:
                    style.fontSize = GetScaledFontSize(0.95f);
                    style.padding = GetSpacingOffset(14f, 14f);
                    break;
            }
        }

        public GUIStyle GetTableHeaderStyle(TableVariant variant, TableSize size) => tableHeaderStyle ?? GUI.skin.label;

        public GUIStyle GetTableCellStyle(TableVariant variant, TableSize size) => tableCellStyle ?? GUI.skin.label;

        public GUIStyle GetCalendarStyle(CalendarVariant variant, CalendarSize size)
        {
            if (calendarStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                CalendarVariant.Default => calendarStyle,
                _ => calendarStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.box);
            ApplyCalendarSizing(sizedStyle, size, variant);
            calendarStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplyCalendarSizing(GUIStyle style, CalendarSize size, CalendarVariant variant)
        {
            switch (variant)
            {
                case CalendarVariant.Default:
                    break;
            }
            switch (size)
            {
                case CalendarSize.Small:
                    style.padding = GetSpacingOffset(8f, 8f);
                    break;
                case CalendarSize.Large:
                    style.padding = GetSpacingOffset(16f, 16f);
                    break;
                case CalendarSize.Default:
                    style.padding = GetSpacingOffset(12f, 12f);
                    break;
            }
        }

        public GUIStyle GetCalendarWeekdayStyle() => calendarWeekdayStyle ?? GUI.skin.label;

        public GUIStyle GetCalendarDayStyle() => calendarDayStyle ?? GUI.skin.button;

        public GUIStyle GetCalendarDaySelectedStyle() => calendarDaySelectedStyle ?? GUI.skin.button;

        public GUIStyle GetCalendarDayOutsideMonthStyle() => calendarDayOutsideMonthStyle ?? GUI.skin.button;

        public GUIStyle GetCalendarDayTodayStyle() => calendarDayTodayStyle ?? GUI.skin.button;

        public GUIStyle GetCalendarDayInRangeStyle() => calendarDayInRangeStyle ?? GUI.skin.button;

        public GUIStyle GetDatePickerStyle(CalendarVariant variant, CalendarSize size)
        {
            if (calendarStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle baseStyle = variant switch
            {
                CalendarVariant.Default => datePickerStyle,
                _ => datePickerStyle,
            };
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(baseStyle ?? GUI.skin.box);
            ApplyDatePickerSizing(sizedStyle, size, variant);
            calendarStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplyDatePickerSizing(GUIStyle style, CalendarSize size, CalendarVariant variant)
        {
            switch (variant)
            {
                case CalendarVariant.Default:
                    break;
            }
            switch (size)
            {
                case CalendarSize.Small:
                    style.padding = GetSpacingOffset(8f, 8f);
                    break;
                case CalendarSize.Large:
                    style.padding = GetSpacingOffset(16f, 16f);
                    break;
                case CalendarSize.Default:
                    style.padding = GetSpacingOffset(10f, 10f);
                    break;
            }
        }

        public GUIStyle GetDatePickerTitleStyle() => datePickerTitleStyle ?? GUI.skin.label;

        public GUIStyle GetDatePickerWeekdayStyle() => datePickerWeekdayStyle ?? GUI.skin.label;

        public GUIStyle GetDatePickerDayStyle() => datePickerDayStyle ?? GUI.skin.button;

        public GUIStyle GetDatePickerDaySelectedStyle() => datePickerDaySelectedStyle ?? GUI.skin.button;

        public GUIStyle GetDatePickerDayOutsideMonthStyle() => datePickerDayOutsideMonthStyle ?? GUI.skin.button;

        public GUIStyle GetDatePickerDayTodayStyle() => datePickerDayTodayStyle ?? GUI.skin.button;

        public GUIStyle GetDialogContentStyle() => dialogContentStyle ?? GUI.skin.box;

        public GUIStyle GetDialogTitleStyle() => dialogTitleStyle ?? GUI.skin.label;

        public GUIStyle GetDialogDescriptionStyle() => dialogDescriptionStyle ?? GUI.skin.label;

        public GUIStyle GetDropdownMenuItemStyle() => dropdownMenuItemStyle ?? GUI.skin.button;

        public GUIStyle GetDropdownMenuSeparatorStyle() => dropdownMenuSeparatorStyle ?? GUIStyle.none;

        public GUIStyle GetDropdownMenuHeaderStyle() => dropdownMenuHeaderStyle ?? GUI.skin.label;

        public GUIStyle GetDropdownMenuStyle(DropdownMenuVariant variant, DropdownMenuSize size)
        {
            if (dropdownMenuStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(dropdownMenuContentStyle ?? GUI.skin.box);
            ApplyDropdownMenuSizing(sizedStyle, size, variant);
            dropdownMenuStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplyDropdownMenuSizing(GUIStyle style, DropdownMenuSize size, DropdownMenuVariant variant)
        {
            switch (size)
            {
                case DropdownMenuSize.Small:
                    style.padding = GetSpacingOffset(4f, 4f);
                    break;
                case DropdownMenuSize.Large:
                    style.padding = GetSpacingOffset(8f, 8f);
                    break;
                case DropdownMenuSize.Default:
                    style.padding = GetSpacingOffset(6f, 6f);
                    break;
            }
            switch (variant)
            {
                case DropdownMenuVariant.Default:
                    break;
            }
        }

        public GUIStyle GetPopoverContentStyle() => popoverContentStyle ?? GUI.skin.box;

        public GUIStyle GetSelectTriggerStyle() => selectTriggerStyle ?? GUI.skin.button;

        public GUIStyle GetSelectItemStyle() => selectItemStyle ?? GUI.skin.button;

        public GUIStyle GetSelectStyle(SelectVariant variant, SelectSize size)
        {
            if (selectStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(selectContentStyle ?? GUI.skin.box);
            ApplySelectSizing(sizedStyle, size, variant);
            selectStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplySelectSizing(GUIStyle style, SelectSize size, SelectVariant variant)
        {
            switch (size)
            {
                case SelectSize.Small:
                    style.fontSize = GetScaledFontSize(0.8f);
                    style.padding = GetSpacingOffset(8f, 6f);
                    break;
                case SelectSize.Large:
                    style.fontSize = GetScaledFontSize(1.1f);
                    style.padding = GetSpacingOffset(12f, 10f);
                    break;
                case SelectSize.Default:
                    style.fontSize = GetScaledFontSize(0.95f);
                    style.padding = GetSpacingOffset(10f, 8f);
                    break;
            }
            switch (variant)
            {
                case SelectVariant.Default:
                    break;
            }
        }

        public GUIStyle GetChartStyle(ChartVariant variant, ChartSize size)
        {
            if (chartStyleCache.TryGetValue((variant, size), out var cachedStyle))
                return cachedStyle;
            GUIStyle sizedStyle = new UnityHelpers.GUIStyle(chartContainerStyle ?? GUI.skin.box);
            ApplyChartSizing(sizedStyle, size, variant);
            chartStyleCache[(variant, size)] = sizedStyle;
            return sizedStyle;
        }

        private void ApplyChartSizing(GUIStyle style, ChartSize size, ChartVariant variant)
        {
            switch (size)
            {
                case ChartSize.Small:
                    style.padding = GetSpacingOffset(12f, 12f);
                    break;
                case ChartSize.Large:
                    style.padding = GetSpacingOffset(20f, 20f);
                    break;
                case ChartSize.Default:
                    style.padding = GetSpacingOffset(16f, 16f);
                    break;
            }
            switch (variant)
            {
                case ChartVariant.Default:
                    break;
            }
        }

        public GUIStyle GetChartAxisStyle() => chartAxisStyle ?? GUI.skin.label;

        public GUIStyle GetMenuBarStyle() => menuBarStyle ?? GUIStyle.none;

        public GUIStyle GetMenuBarItemStyle() => menuBarItemStyle ?? GUI.skin.button;

        public GUIStyle GetMenuDropdownStyle() => menuDropdownStyle ?? GUI.skin.box;

        public Texture2D GetParticleTexture() => particleTexture;

        #endregion

        #region Helpers
        private GUIStyle CreateStyleWithFont(GUIStyle baseStyle, int fontSize, FontStyle fontStyle = FontStyle.Normal)
        {
            GUIStyle style = new UnityHelpers.GUIStyle(baseStyle);
            if (customFont != null)
                style.font = customFont;
            style.fontSize = fontSize;
            style.fontStyle = fontStyle;
            return style;
        }
        #endregion

        #region Cleanup
        public void Cleanup()
        {
            DestroyTexture(gradientTexture);
            DestroyTexture(glowTexture);
            DestroyTexture(particleTexture);
            DestroyTexture(cardBackgroundTexture);
            DestroyTexture(outlineTexture);
            DestroyTexture(transparentTexture);
            DestroyTexture(inputBackgroundTexture);
            DestroyTexture(inputFocusedTexture);
            DestroyTexture(progressBarBackgroundTexture);
            DestroyTexture(progressBarFillTexture);
            DestroyTexture(separatorTexture);
            DestroyTexture(tabsBackgroundTexture);
            DestroyTexture(tabsActiveTexture);
            DestroyTexture(checkboxTexture);
            DestroyTexture(checkboxCheckedTexture);
            DestroyTexture(switchTexture);
            DestroyTexture(switchOnTexture);
            DestroyTexture(switchOffTexture);
            DestroyTexture(badgeTexture);
            DestroyTexture(avatarTexture);
            DestroyTexture(tableTexture);
            DestroyTexture(tableHeaderTexture);
            DestroyTexture(tableCellTexture);
            DestroyTexture(calendarBackgroundTexture);
            DestroyTexture(calendarHeaderTexture);
            DestroyTexture(calendarDayTexture);
            DestroyTexture(calendarDaySelectedTexture);
            DestroyTexture(dropdownMenuContentTexture);
            DestroyTexture(popoverContentTexture);
            DestroyTexture(scrollAreaThumbTexture);
            DestroyTexture(scrollAreaTrackTexture);
            DestroyTexture(selectTriggerTexture);
            DestroyTexture(selectContentTexture);
            DestroyTexture(chartContainerTexture);
            DestroyTexture(chartGridTexture);
            DestroyTexture(chartAxisTexture);
            ClearTextureCache(solidColorTextureCache);
            ClearTextureCache(outlineButtonTextureCache);
            ClearTextureCache(outlineTextureCache);
        }

        private void DestroyTexture(Texture2D texture)
        {
            if (texture)
                Object.Destroy(texture);
        }

        private void ClearTextureCache<K>(Dictionary<K, Texture2D> cache)
        {
            foreach (var texture in cache.Values)
                if (texture)
                    Object.Destroy(texture);
            cache.Clear();
        }
        #endregion
    }
}
