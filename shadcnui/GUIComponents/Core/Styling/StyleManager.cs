using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core.Styling
{
    #region Enums
    public enum ControlVariant
    {
        Default,
        Secondary,
        Destructive,
        Outline,
        Ghost,
        Link,
        Muted,
    }

    public enum ControlSize
    {
        Default,
        Small,
        Large,
        Icon,
        Mini,
    }

    public enum SeparatorOrientation
    {
        Horizontal,
        Vertical,
    }

    public enum AvatarShape
    {
        Circle,
        Square,
        Rounded,
    }

    public enum StyleComponentType
    {
        Button,
        Toggle,
        Input,
        Label,
        PasswordField,
        TextArea,
        ProgressBar,
        Separator,
        TabsList,
        TabsTrigger,
        TabsContent,
        Checkbox,
        Switch,
        Badge,
        Avatar,
        Table,
        TableRow,
        TableHeader,
        TableCell,
        Dialog,
        Chart,
        ChartAxis,
        MenuBar,
        MenuBarItem,
        MenuDropdown,
        SelectContent,
        SelectItem,
        DropdownMenu,
        DropdownMenuItem,
        Popover,
        AnimatedBox,
        SectionHeader,
        Card,
        CardHeader,
        CardTitle,
        CardDescription,
        CardContent,
        CardFooter,
        Toast,
        ToastTitle,
        ToastDescription,
        Tooltip,
        SliderTrack,
        SliderThumb,
        SliderFill,
        Navigation,
    }

    public readonly struct StyleKey : IEquatable<StyleKey>
    {
        public readonly StyleComponentType Type;
        public readonly ControlVariant Variant;
        public readonly ControlSize Size;
        public readonly int State;

        public StyleKey(StyleComponentType type, ControlVariant variant, ControlSize size, int state = 0)
        {
            Type = type;
            Variant = variant;
            Size = size;
            State = state;
        }

        public bool Equals(StyleKey other) => Type == other.Type && Variant == other.Variant && Size == other.Size && State == other.State;

        public override bool Equals(object obj) => obj is StyleKey other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)Type;
                hashCode = (hashCode * 397) ^ (int)Variant;
                hashCode = (hashCode * 397) ^ (int)Size;
                hashCode = (hashCode * 397) ^ State;
                return hashCode;
            }
        }
    }
    #endregion

    public partial class StyleManager
    {
        #region Fields
        private readonly GUIHelper _guiHelper;
        private bool _isInitialized;
        public Font CustomFont { get; set; }
        public StyleRegistry Registry { get; }
        public TextureManager Textures { get; }

        private Theme _cachedTheme;
        private readonly Dictionary<float, int> _scaledRadiusCache = new();
        private readonly Dictionary<float, int> _scaledSpacingCache = new();
        private readonly Dictionary<float, int> _scaledHeightCache = new();
        private float _lastUiScale = -1f;
        #endregion

        #region Texture Accessors
        private Texture2D InputFocusedTexture => Textures?.InputFocused;
        private Texture2D SeparatorTexture => Textures?.Separator;
        private Texture2D TableCellTexture => Textures?.TableCell;
        private Texture2D TableHeaderTexture => Textures?.TableHeader;
        private Texture2D TableRowTexture => Textures?.TableRow;
        private Texture2D TableRowAlternateTexture => Textures?.TableRowAlternate;
        private Texture2D TransparentTexture => Textures?.Transparent;
        private Texture2D TabsActiveTexture => Textures?.TabsActive;
        private Texture2D ChartContainerTexture => Textures?.ChartContainer;
        #endregion

        #region Base GUIStyles
        private GUIStyle _baseButtonStyle;
        private GUIStyle _baseToggleStyle;
        private GUIStyle _checkboxStyle;
        private GUIStyle _checkboxSolidStyle;
        private GUIStyle _baseSwitchStyle;
        private GUIStyle _baseInputStyle;
        private GUIStyle _baseLabelStyle;
        private GUIStyle _baseBadgeStyle;
        private GUIStyle _baseTableStyle;
        private GUIStyle _tableCellStyle;
        private GUIStyle _cardStyle;
        private GUIStyle _tabsListStyle;
        private GUIStyle _tabsTriggerStyle;
        private GUIStyle _chartContainerStyle;
        private GUIStyle _dialogContentStyle;
        private GUIStyle _dropdownContentStyle;
        private GUIStyle _dropdownItemStyle;
        private GUIStyle _menuBarStyle;
        private GUIStyle _progressBarStyle;
        private GUIStyle _separatorStyle;
        private GUIStyle _avatarStyle;
        private GUIStyle _baseSliderStyle;
        private GUIStyle _tableHeaderStyle;
        private GUIStyle _tableRowStyle;
        private GUIStyle _tableRowAlternateStyle;
        private GUIStyle _navigationStyle;
        internal GUIStyle AnimatedBoxStyle;
        #endregion

        #region Style Cache
        private readonly Dictionary<StyleKey, CachedStyle> _styleCache = new();
        private const int MAX_CACHE_SIZE = 500;
        private const float CACHE_PRUNE_THRESHOLD = 5f;

        private struct CachedStyle
        {
            public GUIStyle Style;
            public float LastAccessTime;
        }

        private void PruneOldCacheEntries()
        {
            if (_styleCache.Count < MAX_CACHE_SIZE * 0.9f)
                return;

            var toRemove = new List<StyleKey>();
            var currentTime = Time.realtimeSinceStartup;

            foreach (var kvp in _styleCache)
            {
                if (currentTime - kvp.Value.LastAccessTime > CACHE_PRUNE_THRESHOLD)
                {
                    toRemove.Add(kvp.Key);
                }
            }

            foreach (var key in toRemove)
            {
                _styleCache.Remove(key);
            }
        }

        private void TouchCacheEntry(StyleKey key)
        {
            if (_styleCache.TryGetValue(key, out var cached))
            {
                cached.LastAccessTime = Time.realtimeSinceStartup;
                _styleCache[key] = cached;
            }
        }

        private void AddToCache(StyleKey key, GUIStyle style)
        {
            PruneOldCacheEntries();
            _styleCache[key] = new CachedStyle { Style = style, LastAccessTime = Time.realtimeSinceStartup };
        }
        #endregion

        #region Constructor
        public StyleManager(GUIHelper helper)
        {
            try
            {
                _guiHelper = helper ?? throw new ArgumentNullException(nameof(helper));
                Registry = new StyleRegistry();
                Textures = new TextureManager(helper);
                _cachedTheme = ThemeManager.Instance.CurrentTheme;

                ThemeManager.Instance.OnThemeChanged += OnThemeChanged;

                GUILogger.LogInfo("StyleManager initialized", "StyleManager.Constructor");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "Constructor", "StyleManager");
                throw;
            }
        }
        #endregion

        #region Core Methods
        public Theme GetTheme() => _cachedTheme ??= ThemeManager.Instance.CurrentTheme;

        private void OnThemeChanged()
        {
            try
            {
                GUILogger.LogInfo("Theme changed, refreshing styles", "StyleManager.OnThemeChanged");
                _cachedTheme = ThemeManager.Instance.CurrentTheme;
                _styleCache.Clear();
                Textures?.DestroyAllTextures();

                if (_isInitialized)
                {
                    Textures?.CreateAllTextures();
                    AllStyles();
                }
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "OnThemeChanged", "StyleManager");
            }
        }

        public void InitializeGUI()
        {
            if (_isInitialized)
            {
                GUILogger.LogWarning("StyleManager already initialized", "StyleManager.InitializeGUI");
                return;
            }

            try
            {
                GUILogger.LogInfo("Starting StyleManager initialization", "StyleManager.InitializeGUI");

                _styleCache.Clear();
                Registry.Clear();

                if (Textures == null)
                {
                    GUILogger.LogError("TextureManager is null", "StyleManager.InitializeGUI");
                    return;
                }

                Textures.CreateAllTextures();
                GUILogger.LogInfo("Textures created", "StyleManager.InitializeGUI");

                AllStyles();
                GUILogger.LogInfo("Styles created", "StyleManager.InitializeGUI");

                _isInitialized = true;
                GUILogger.LogInfo("StyleManager initialization complete", "StyleManager.InitializeGUI");
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "InitializeGUI", "StyleManager");
                _isInitialized = false;
            }
        }
        #endregion

        #region Scale Caching
        private void InvalidateScaleCaches()
        {
            if (Mathf.Abs(_guiHelper.uiScale - _lastUiScale) > 0.001f)
            {
                _scaledRadiusCache.Clear();
                _scaledSpacingCache.Clear();
                _scaledHeightCache.Clear();
                _lastUiScale = _guiHelper.uiScale;
            }
        }

        public int GetScaledBorderRadius(float radius)
        {
            InvalidateScaleCaches();

            if (_scaledRadiusCache.TryGetValue(radius, out var cached))
                return cached;

            int scaled = Mathf.RoundToInt(radius * _guiHelper.uiScale);
            _scaledRadiusCache[radius] = scaled;
            return scaled;
        }

        public int GetScaledSpacing(float spacing)
        {
            InvalidateScaleCaches();

            if (_scaledSpacingCache.TryGetValue(spacing, out var cached))
                return cached;

            int scaled = Mathf.RoundToInt(spacing * _guiHelper.uiScale);
            _scaledSpacingCache[spacing] = scaled;
            return scaled;
        }

        public int GetScaledHeight(float height)
        {
            InvalidateScaleCaches();

            if (_scaledHeightCache.TryGetValue(height, out var cached))
                return cached;

            int scaled = Mathf.RoundToInt(height * _guiHelper.uiScale);
            _scaledHeightCache[height] = scaled;
            return scaled;
        }

        public int GetScaledFontSize(float scale = 1.0f) => Mathf.RoundToInt(_guiHelper.fontSize * scale * _guiHelper.uiScale);

        public RectOffset GetSpacingOffset(float horizontal = 8f, float vertical = 8f)
        {
            int h = GetScaledSpacing(horizontal);
            int v = GetScaledSpacing(vertical);
            return new UnityHelpers.RectOffset(h, h, v, v);
        }

        #endregion

        #region Texture Creation
        public Texture2D CreateSolidTexture(Color color) => Textures.GenerateSolid(color);

        public Texture2D CreateTexture(int width, int height, int radius, Color color) => Textures.GenerateShape(width, height, radius, color, color, Color.clear, 0);

        public Texture2D CreateTexture(int width, int height, int radius, Color color, Color color2) => Textures.GenerateShape(width, height, radius, color, color2, Color.clear, 0);

        public Texture2D CreateBorderTexture(int width, int height, int radius, Color fillColor, Color borderColor, float borderThickness = 1f) => Textures.GenerateShape(width, height, radius, fillColor, fillColor, borderColor, borderThickness);

        public Texture2D CreateBorderTexture(Color borderColor, int thickness) => Textures.GenerateShape(thickness * 4, thickness * 4, 0, Color.clear, Color.clear, borderColor, thickness);

        public Texture2D CreateTexture(int width, int height, int radius, Color ringColor, float thickness = 2f) => Textures.GenerateShape(width, height, radius, Color.clear, Color.clear, ringColor, thickness);

        public Texture2D CreateTexture(int width, int height, int radius, Color topColor, Color bottomColor, float shadowIntensity = 0.12f, int shadowBlur = 10) => Textures.GenerateShape(width, height, radius, topColor, bottomColor, Color.clear, 0, shadowIntensity, shadowBlur);

        public Texture2D CreateOutlineTexture(int width, int height, int radius, Color borderColor, float thickness = 1f) => Textures.GenerateShape(width, height, radius, Color.clear, Color.clear, borderColor, thickness);

        private Texture2D CreateBorderTexture(int width, int height, int borderThickness, Color borderColor, Color fillColor) => Textures.GenerateShape(width, height, 0, fillColor, fillColor, borderColor, borderThickness);

        public Texture2D CreateAvatarTexture(int size, int radius, Color backgroundColor, Color borderColor, float borderThickness, bool withShadow = true) => Textures.GenerateAvatarTexture(size, radius, backgroundColor, borderColor, borderThickness, withShadow);

        public Texture2D CreateStatusIndicator(int size, bool isOnline) => Textures.GenerateStatusIndicator(size, isOnline);
        #endregion

        #region Cleanup
        public void Cleanup()
        {
            Textures?.Cleanup();
            _styleCache.Clear();
            _scaledRadiusCache.Clear();
            _scaledSpacingCache.Clear();
            _scaledHeightCache.Clear();
        }
        #endregion

        #region Style Health Monitoring
        private bool _stylesCorruption;
        private float _lastScanTime;
        private float _lastRefreshTime;
        private readonly float _scanInterval = 5f;
        private readonly Dictionary<string, int> _styleHealthChecks = new();
        private List<System.Reflection.FieldInfo> _monitoredStyleFields;

        private static int GetStyleHealth(GUIStyle style)
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
                hash = hash * 31 + style.normal.textColor.GetHashCode();
                return hash;
            }
        }

        public void MarkStylesCorruption() => _stylesCorruption = true;

        public void RefreshStylesIfCorruption()
        {
            if (!_stylesCorruption)
                return;
            if (Time.realtimeSinceStartup - _lastRefreshTime < 1.0f)
                return;

            _lastRefreshTime = Time.realtimeSinceStartup;

            try
            {
                GUILogger.LogWarning("Style corruption detected, refreshing styles", "StyleManager.RefreshStylesIfCorruption");

                _isInitialized = false;
                InitializeGUI();

                EnsureStyleFieldsInitialized();
                UpdateStyleHealthChecks();

                _stylesCorruption = false;
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, "RefreshStylesIfCorruption", "StyleManager");
            }
        }

        private void EnsureStyleFieldsInitialized()
        {
            if (_monitoredStyleFields?.Count > 0)
                return;

            _monitoredStyleFields = new List<System.Reflection.FieldInfo>();
            var fields = GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(GUIStyle))
                {
                    _monitoredStyleFields.Add(field);
                }
            }
        }

        private void UpdateStyleHealthChecks()
        {
            _styleHealthChecks.Clear();

            foreach (var field in _monitoredStyleFields)
            {
                if (field.GetValue(this) is GUIStyle style && style != null)
                {
                    _styleHealthChecks[field.Name] = GetStyleHealth(style);
                }
            }
        }

        public bool ScanForCorruption()
        {
            if (Time.realtimeSinceStartup - _lastScanTime < _scanInterval)
                return false;

            _lastScanTime = Time.realtimeSinceStartup;

            EnsureStyleFieldsInitialized();

            if (_monitoredStyleFields.Count == 0)
            {
                UpdateStyleHealthChecks();
                return false;
            }

            foreach (var field in _monitoredStyleFields)
            {
                if (field.GetValue(this) is not GUIStyle style || style == null)
                    continue;

                string styleName = field.Name;
                int currentHealth = GetStyleHealth(style);

                if (_styleHealthChecks.TryGetValue(styleName, out int previousHealth) && previousHealth != currentHealth)
                {
                    GUILogger.LogWarning($"Style corruption detected in '{styleName}'", "StyleManager.ScanForCorruption");
                    _stylesCorruption = true;
                    RefreshStylesIfCorruption();
                    return true;
                }

                _styleHealthChecks[styleName] = currentHealth;
            }

            return false;
        }
        #endregion
    }
}
