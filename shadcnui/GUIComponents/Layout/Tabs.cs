using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    #region Enums and Structs
    public enum TabSide
    {
        Left,
        Right,
    }

    public enum TabPosition
    {
        Top,
        Bottom,
        Left,
        Right,
    }

    public enum IndicatorStyle
    {
        Underline,
        Background,
        Border,
        Pill,
    }

    public struct TabConfig
    {
        public string Name;
        public Action Content;
        public bool Disabled;
        public Texture2D Icon;
        public bool Closable;

        public TabConfig(string name, Action content, bool disabled = false, Texture2D icon = null, bool closable = false)
        {
            Name = name;
            Content = content;
            Disabled = disabled;
            Icon = icon;
            Closable = closable;
        }
    }

    public struct PillTextureKey : IEquatable<PillTextureKey>
    {
        public int Width;
        public int Height;
        public int Radius;
        public Color Color;

        public PillTextureKey(int width, int height, int radius, Color color)
        {
            Width = width;
            Height = height;
            Radius = radius;
            Color = color;
        }

        public override bool Equals(object obj)
        {
            return obj is PillTextureKey key && Equals(key);
        }

        public bool Equals(PillTextureKey other)
        {
            return Width == other.Width && Height == other.Height && Radius == other.Radius && Color.Equals(other.Color);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                hashCode = (hashCode * 397) ^ Radius.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                return hashCode;
            }
        }
    }
    #endregion

    public class TabItem
    {
        public string Id { get; }
        public string Name { get; set; }
        public Action Content { get; set; }
        public bool Disabled { get; set; }
        public Texture2D Icon { get; set; }
        public bool Closable { get; set; }
        public object UserData { get; set; }

        public TabItem(string name, Action content, bool disabled = false, Texture2D icon = null, bool closable = false)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Content = content;
            Disabled = disabled;
            Icon = icon;
            Closable = closable;
        }
    }

    public class Tabs : BaseComponent
    {
        #region Constants
        private const float CLOSE_BUTTON_HIT_AREA = 20f;
        private const float CLOSE_BUTTON_ICON_SIZE = 12f;
        private const float CLOSE_BUTTON_FONT_SIZE = 14f;
        private const float TAB_INDICATOR_HEIGHT = 2f;
        private const float TAB_BORDER_WIDTH = 2f;
        private const float TAB_HEIGHT = 36f;
        #endregion

        #region Fields
        private int _pendingCloseIndex = -1;
        private Action<int> _pendingCloseCallback;
        private Vector2 _tabScrollPosition = Vector2.zero;
        private Dictionary<PillTextureKey, Texture2D> _pillTextureCache = new Dictionary<PillTextureKey, Texture2D>();
        private PillTextureKey _lastPillKey;
        private Texture2D _lastPillTexture;
        #endregion

        #region Constructor
        public Tabs(GUIHelper helper)
            : base(helper) { }
        #endregion

        #region Public Drawing API
        public int Draw(TabsConfig config)
        {
            if (config.TabNames == null || config.TabNames.Length == 0)
            {
                config.Content?.Invoke();
                return config.SelectedIndex;
            }

            ProcessPendingClose(config);
            return DrawTabs(config);
        }

        public int DrawWithAutoClose(ref string[] tabNames, ref bool[] closableTabs, int selectedIndex, Action content = null, Action<int> onTabChange = null)
        {
            if (tabNames == null || tabNames.Length == 0)
            {
                content?.Invoke();
                return selectedIndex;
            }

            var config = new TabsConfig(tabNames, selectedIndex)
            {
                ClosableTabs = closableTabs,
                Content = content,
                OnTabChange = onTabChange,
            };

            return HandleAutoClose(ref tabNames, ref closableTabs, ref selectedIndex, config);
        }

        #endregion

        #region Core Drawing Logic
        private int DrawTabs(TabsConfig config)
        {
            var selectedIndex = Mathf.Clamp(config.SelectedIndex, 0, config.TabNames.Length - 1);
            var newSelectedIndex = selectedIndex;

            switch (config.Position)
            {
                case TabPosition.Top:
                    newSelectedIndex = DrawMultiLineTabs(config, selectedIndex, false);
                    RenderTabContent(config, newSelectedIndex);
                    break;
                case TabPosition.Bottom:
                    RenderTabContent(config, newSelectedIndex);
                    newSelectedIndex = DrawMultiLineTabs(config, selectedIndex, false);
                    break;
                case TabPosition.Left:
                    newSelectedIndex = DrawVerticalTabsWithContent(config, selectedIndex, false);
                    break;
                case TabPosition.Right:
                    newSelectedIndex = DrawVerticalTabsWithContent(config, selectedIndex, true);
                    break;
            }

            return newSelectedIndex;
        }

        private int DrawVerticalTabsWithContent(TabsConfig config, int selectedIndex, bool tabsOnRight)
        {
            var newSelectedIndex = selectedIndex;
            var mainHorizontalStarted = false;

            try
            {
                layoutComponents.BeginHorizontalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                mainHorizontalStarted = true;

                if (tabsOnRight)
                {
                    RenderTabContent(config, newSelectedIndex);
                    newSelectedIndex = DrawMultiLineTabs(config, selectedIndex, true);
                }
                else
                {
                    newSelectedIndex = DrawMultiLineTabs(config, selectedIndex, true);
                    RenderTabContent(config, newSelectedIndex);
                }
            }
            finally
            {
                if (mainHorizontalStarted)
                {
                    layoutComponents.EndHorizontalGroup();
                }
            }

            return newSelectedIndex;
        }

        private int DrawMultiLineTabs(TabsConfig config, int selectedIndex, bool isVertical)
        {
            var tabCount = config.TabNames.Length;
            var localStyleManager = guiHelper.GetStyleManager();

            if (config.MaxLines <= 1)
            {
                return DrawSingleLineTabs(config, localStyleManager, selectedIndex, isVertical);
            }

            var tabsPerLine = (int)Mathf.Ceil((float)tabCount / config.MaxLines);
            if (isVertical)
            {
                return DrawVerticalTabColumns(config, localStyleManager, selectedIndex, tabsPerLine);
            }
            return DrawMultiLineHorizontalTabs(config, localStyleManager, selectedIndex, tabsPerLine);
        }

        private int DrawSingleLineTabs(TabsConfig config, StyleManager localStyleManager, int selectedIndex, bool isVertical)
        {
            var newSelectedIndex = selectedIndex;

            if (config.EnableOverflowScroll)
            {
                if (isVertical)
                {
                    _tabScrollPosition = GUILayout.BeginScrollView(_tabScrollPosition, GUILayout.Width(config.TabWidth * guiHelper.uiScale), GUILayout.ExpandHeight(true));
                }
                else
                {
                    _tabScrollPosition = GUILayout.BeginScrollView(_tabScrollPosition, GUIStyle.none, GUIStyle.none, GUILayout.Height(TAB_HEIGHT * guiHelper.uiScale + 4));
                }
            }

            try
            {
                if (isVertical)
                {
                    layoutComponents.BeginVerticalGroup(localStyleManager.GetTabsListStyle(), GUILayout.Width(config.TabWidth * guiHelper.uiScale));

                    for (var i = 0; i < config.TabNames.Length; i++)
                    {
                        newSelectedIndex = DrawSingleTab(config, localStyleManager, i, selectedIndex, newSelectedIndex, true);

                        if (i < config.TabNames.Length - 1)
                        {
                            layoutComponents.AddSpace((int)(DesignTokens.Spacing.XXS * guiHelper.uiScale));
                        }
                    }
                }
                else
                {
                    layoutComponents.BeginHorizontalGroup(localStyleManager.GetTabsListStyle());

                    for (var i = 0; i < config.TabNames.Length; i++)
                    {
                        newSelectedIndex = DrawSingleTab(config, localStyleManager, i, selectedIndex, newSelectedIndex);

                        if (i < config.TabNames.Length - 1)
                        {
                            layoutComponents.AddSpace((int)(DesignTokens.Spacing.XXS * guiHelper.uiScale));
                        }
                    }
                }
            }
            finally
            {
                if (isVertical)
                {
                    layoutComponents.EndVerticalGroup();
                }
                else
                {
                    layoutComponents.EndHorizontalGroup();
                }

                if (config.EnableOverflowScroll)
                {
                    GUILayout.EndScrollView();
                }
            }

            return newSelectedIndex;
        }

        private int DrawMultiLineHorizontalTabs(TabsConfig config, StyleManager localStyleManager, int selectedIndex, int tabsPerLine)
        {
            var newSelectedIndex = selectedIndex;
            var totalLines = Mathf.Min(config.MaxLines, (int)Mathf.Ceil((float)config.TabNames.Length / tabsPerLine));

            if (config.EnableOverflowScroll)
            {
                var totalHeight = TAB_HEIGHT * guiHelper.uiScale * totalLines + DesignTokens.Spacing.XXS * guiHelper.uiScale * (totalLines - 1) + 4;
                _tabScrollPosition = GUILayout.BeginScrollView(_tabScrollPosition, GUIStyle.none, GUIStyle.none, GUILayout.Height(totalHeight));
            }

            try
            {
                layoutComponents.BeginVerticalGroup(localStyleManager.GetTabsListStyle());

                for (var line = 0; line < totalLines; line++)
                {
                    layoutComponents.BeginHorizontalGroup();

                    for (var i = line * tabsPerLine; i < (line + 1) * tabsPerLine && i < config.TabNames.Length; i++)
                    {
                        newSelectedIndex = DrawSingleTab(config, localStyleManager, i, selectedIndex, newSelectedIndex);

                        if (i < (line + 1) * tabsPerLine - 1 && i < config.TabNames.Length - 1)
                        {
                            layoutComponents.AddSpace((int)(DesignTokens.Spacing.XXS * guiHelper.uiScale));
                        }
                    }

                    layoutComponents.EndHorizontalGroup();

                    if (line < totalLines - 1)
                    {
                        layoutComponents.AddSpace((int)(DesignTokens.Spacing.XXS * guiHelper.uiScale));
                    }
                }
            }
            finally
            {
                layoutComponents.EndVerticalGroup();

                if (config.EnableOverflowScroll)
                {
                    GUILayout.EndScrollView();
                }
            }

            return newSelectedIndex;
        }

        private int DrawVerticalTabColumns(TabsConfig config, StyleManager localStyleManager, int selectedIndex, int tabsPerColumn)
        {
            var newSelectedIndex = selectedIndex;
            var totalColumns = Mathf.Min(config.MaxLines, (int)Mathf.Ceil((float)config.TabNames.Length / tabsPerColumn));

            if (config.EnableOverflowScroll)
            {
                var totalWidth = config.TabWidth * guiHelper.uiScale * totalColumns + DesignTokens.Spacing.XXS * guiHelper.uiScale * (totalColumns - 1) + 4;
                _tabScrollPosition = GUILayout.BeginScrollView(_tabScrollPosition, GUILayout.Width(totalWidth), GUILayout.ExpandHeight(true));
            }

            try
            {
                layoutComponents.BeginHorizontalGroup(localStyleManager.GetTabsListStyle());

                for (var col = 0; col < totalColumns; col++)
                {
                    layoutComponents.BeginVerticalGroup(GUILayout.Width(config.TabWidth * guiHelper.uiScale));

                    for (var i = col * tabsPerColumn; i < (col + 1) * tabsPerColumn && i < config.TabNames.Length; i++)
                    {
                        newSelectedIndex = DrawSingleTab(config, localStyleManager, i, selectedIndex, newSelectedIndex, true);

                        if (i < (col + 1) * tabsPerColumn - 1 && i < config.TabNames.Length - 1)
                        {
                            layoutComponents.AddSpace((int)(DesignTokens.Spacing.XXS * guiHelper.uiScale));
                        }
                    }

                    layoutComponents.EndVerticalGroup();

                    if (col < totalColumns - 1)
                    {
                        layoutComponents.AddSpace((int)(DesignTokens.Spacing.XXS * guiHelper.uiScale));
                    }
                }
            }
            finally
            {
                layoutComponents.EndHorizontalGroup();

                if (config.EnableOverflowScroll)
                {
                    GUILayout.EndScrollView();
                }
            }

            return newSelectedIndex;
        }

        private int DrawSingleTab(TabsConfig config, StyleManager localStyleManager, int index, int selectedIndex, int currentNewIndex, bool isVertical = false)
        {
            var isActive = index == selectedIndex;
            var isDisabled = config.DisabledTabs != null && index < config.DisabledTabs.Length && config.DisabledTabs[index];
            var hasIcon = config.TabIcons != null && index < config.TabIcons.Length && config.TabIcons[index] != null;
            var isClosable = config.ClosableTabs != null && index < config.ClosableTabs.Length && config.ClosableTabs[index];

            var triggerStyle = localStyleManager.GetTabsTriggerStyle(isActive);
            var tabLabel = config.TabNames[index] ?? $"Tab {index + 1}";

            GUILayoutOption[] layoutOptions;
            if (isVertical)
            {
                layoutOptions = new[] { GUILayout.Width(config.TabWidth * guiHelper.uiScale), GUILayout.Height(TAB_HEIGHT * guiHelper.uiScale) };
            }
            else
            {
                layoutOptions = config.Options ?? new[] { GUILayout.Height(TAB_HEIGHT * guiHelper.uiScale) };
            }

            GUI.enabled = !isDisabled;

            var displayLabel = isClosable ? tabLabel + "  ×" : tabLabel;
            var tabRect = GUILayoutUtility.GetRect(new GUIContent(displayLabel), triggerStyle, layoutOptions);

            var closeClicked = false;
            if (isClosable)
            {
                closeClicked = HandleCloseButton(tabRect, index, config);
            }

            var clicked = !isDisabled && !closeClicked && GUI.Button(tabRect, "", triggerStyle);

            DrawTabContent(tabRect, tabLabel, triggerStyle, hasIcon ? config.TabIcons[index] : null, isClosable, isVertical);

            if (isClosable && !closeClicked)
            {
                DrawCloseButton(tabRect);
            }

            GUI.enabled = true;

            if (clicked && index != selectedIndex)
            {
                currentNewIndex = index;
                config.OnTabChange?.Invoke(index);
            }

            if (config.ShowIndicator && isActive)
            {
                DrawTabIndicator(tabRect, config.IndicatorStyle, isVertical, config.Position);
            }

            return currentNewIndex;
        }

        private void DrawTabContent(Rect tabRect, string label, GUIStyle triggerStyle, Texture2D icon, bool isClosable, bool isVertical)
        {
            var labelStyle = new UnityHelpers.GUIStyle(triggerStyle);

            if (icon != null)
            {
                var iconSize = DesignTokens.Icon.Small * guiHelper.uiScale;
                var labelContent = new GUIContent(label);
                var labelWidth = triggerStyle.CalcSize(labelContent).x;
                var totalContentWidth = iconSize + DesignTokens.Spacing.XS * guiHelper.uiScale + labelWidth;
                var contentStartX = tabRect.x + (tabRect.width - totalContentWidth) / 2;

                if (isClosable)
                    contentStartX -= DesignTokens.Spacing.MD * guiHelper.uiScale;

                var iconY = tabRect.y + (tabRect.height - iconSize) / 2;
                var iconRect = new Rect(contentStartX, iconY, iconSize, iconSize);
                GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);

                labelStyle.alignment = isVertical ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter;
                var labelRect = new Rect(contentStartX + iconSize + DesignTokens.Spacing.XS * guiHelper.uiScale, tabRect.y, labelWidth, tabRect.height);
                GUI.Label(labelRect, label, labelStyle);
            }
            else
            {
                labelStyle.alignment = isVertical ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter;
                var labelRect = isClosable ? new Rect(tabRect.x, tabRect.y, tabRect.width - CLOSE_BUTTON_HIT_AREA * guiHelper.uiScale, tabRect.height) : tabRect;
                GUI.Label(labelRect, label, labelStyle);
            }
        }

        private bool HandleCloseButton(Rect tabRect, int index, TabsConfig config)
        {
            var closeButtonSize = CLOSE_BUTTON_HIT_AREA * guiHelper.uiScale;
            var closeButtonRect = new Rect(tabRect.x + tabRect.width - closeButtonSize - DesignTokens.Spacing.XS * guiHelper.uiScale, tabRect.y + (tabRect.height - closeButtonSize) / 2, closeButtonSize, closeButtonSize);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && closeButtonRect.Contains(Event.current.mousePosition))
            {
                _pendingCloseIndex = index;
                _pendingCloseCallback = config.OnTabClose;
                Event.current.Use();
                return true;
            }

            return false;
        }

        private void DrawCloseButton(Rect tabRect)
        {
            var closeButtonSize = CLOSE_BUTTON_ICON_SIZE * guiHelper.uiScale;
            var closeX = tabRect.x + tabRect.width - closeButtonSize - DesignTokens.Spacing.SM * guiHelper.uiScale;
            var closeY = tabRect.y + (tabRect.height - closeButtonSize) / 2;
            var closeRect = new Rect(closeX, closeY, closeButtonSize, closeButtonSize);

            var closeStyle = new UnityHelpers.GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(CLOSE_BUTTON_FONT_SIZE * guiHelper.uiScale),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = closeRect.Contains(Event.current.mousePosition) ? ThemeManager.Instance.CurrentTheme.Destructive : ThemeManager.Instance.CurrentTheme.Muted },
            };

            GUI.Label(closeRect, "×", closeStyle);
        }
        #endregion

        #region Indicator Drawing
        private void DrawTabIndicator(Rect tabRect, IndicatorStyle style, bool isVertical, TabPosition position)
        {
            if (tabRect.Equals(Rect.zero))
                return;

            var indicatorColor = ThemeManager.Instance.CurrentTheme.Accent;

            switch (style)
            {
                case IndicatorStyle.Underline:
                    DrawUnderlineIndicator(tabRect, indicatorColor, isVertical, position);
                    break;
                case IndicatorStyle.Background:
                    DrawBackgroundIndicator(tabRect, indicatorColor);
                    break;
                case IndicatorStyle.Border:
                    DrawBorderIndicator(tabRect, indicatorColor, isVertical, position);
                    break;
                case IndicatorStyle.Pill:
                    DrawPillIndicator(tabRect, indicatorColor);
                    break;
            }
        }

        private void DrawUnderlineIndicator(Rect tabRect, Color color, bool isVertical, TabPosition position)
        {
            Rect indicatorRect;

            if (isVertical)
            {
                var indicatorWidth = TAB_INDICATOR_HEIGHT * guiHelper.uiScale;
                if (position == TabPosition.Left)
                {
                    indicatorRect = new Rect(tabRect.x + tabRect.width - indicatorWidth, tabRect.y, indicatorWidth, tabRect.height);
                }
                else // Right
                {
                    indicatorRect = new Rect(tabRect.x, tabRect.y, indicatorWidth, tabRect.height);
                }
            }
            else
            {
                var indicatorHeight = TAB_INDICATOR_HEIGHT * guiHelper.uiScale;
                indicatorRect = new Rect(tabRect.x, tabRect.y + tabRect.height - indicatorHeight, tabRect.width, indicatorHeight);
            }

            GUI.color = color;
            GUI.DrawTexture(indicatorRect, Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawBackgroundIndicator(Rect tabRect, Color color)
        {
            var bgColor = new Color(color.r, color.g, color.b, 0.1f);
            GUI.color = bgColor;
            GUI.DrawTexture(tabRect, Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawBorderIndicator(Rect tabRect, Color color, bool isVertical, TabPosition position)
        {
            var borderWidth = TAB_BORDER_WIDTH * guiHelper.uiScale;
            GUI.color = color;

            if (isVertical)
            {
                if (position == TabPosition.Left)
                {
                    GUI.DrawTexture(new Rect(tabRect.x + tabRect.width - borderWidth, tabRect.y, borderWidth, tabRect.height), Texture2D.whiteTexture);
                }
                else
                {
                    GUI.DrawTexture(new Rect(tabRect.x, tabRect.y, borderWidth, tabRect.height), Texture2D.whiteTexture);
                }
            }
            else
            {
                GUI.DrawTexture(new Rect(tabRect.x, tabRect.y + tabRect.height - borderWidth, tabRect.width, borderWidth), Texture2D.whiteTexture);
            }

            GUI.color = Color.white;
        }

        private void DrawPillIndicator(Rect tabRect, Color color)
        {
            var borderWidth = TAB_BORDER_WIDTH * guiHelper.uiScale;
            var pillRect = new Rect(tabRect.x + borderWidth, tabRect.y + borderWidth, tabRect.width - borderWidth * 2, tabRect.height - borderWidth * 2);

            var pillTexture = GetOrCreatePillTexture((int)pillRect.width, (int)pillRect.height, (int)(Mathf.Min(pillRect.width, pillRect.height) / 2), color);

            GUI.DrawTexture(pillRect, pillTexture);
        }

        private Texture2D GetOrCreatePillTexture(int width, int height, int radius, Color color)
        {
            var key = new PillTextureKey(width, height, radius, color);

            if (_lastPillKey.Equals(key) && _lastPillTexture != null)
            {
                return _lastPillTexture;
            }

            if (_pillTextureCache.TryGetValue(key, out var cachedTexture))
            {
                _lastPillKey = key;
                _lastPillTexture = cachedTexture;
                return cachedTexture;
            }

            var newTexture = CreatePillTexture(width, height, radius, color);
            _pillTextureCache[key] = newTexture;
            _lastPillKey = key;
            _lastPillTexture = newTexture;

            return newTexture;
        }

        private Texture2D CreatePillTexture(int width, int height, int radius, Color color)
        {
            var texture = new Texture2D(width, height);
            var centerX = width / 2f;
            var centerY = height / 2f;
            var radiusSqr = radius * radius;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var dx = x - centerX;
                    var dy = y - centerY;
                    var distanceSqr = dx * dx + dy * dy;

                    if (distanceSqr <= radiusSqr)
                    {
                        texture.SetPixel(x, y, color);
                    }
                    else
                    {
                        texture.SetPixel(x, y, Color.clear);
                    }
                }
            }

            texture.Apply();
            return texture;
        }
        #endregion

        #region Helper Methods
        private int HandleAutoClose(ref string[] tabNames, ref bool[] closableTabs, ref int selectedIndex, TabsConfig config)
        {
            if (_pendingCloseIndex >= 0 && _pendingCloseCallback == null)
            {
                var closeIndex = _pendingCloseIndex;
                _pendingCloseIndex = -1;

                if (closeIndex >= 0 && closeIndex < tabNames.Length)
                {
                    var newNames = new List<string>(tabNames);
                    var newClosable = new List<bool>(closableTabs ?? Array.Empty<bool>());

                    newNames.RemoveAt(closeIndex);
                    if (closeIndex < newClosable.Count)
                        newClosable.RemoveAt(closeIndex);

                    tabNames = newNames.ToArray();
                    closableTabs = newClosable.ToArray();

                    if (selectedIndex >= tabNames.Length)
                        selectedIndex = Math.Max(0, tabNames.Length - 1);
                    else if (selectedIndex > closeIndex)
                        selectedIndex--;

                    config.SelectedIndex = selectedIndex;
                }
            }

            return Draw(config);
        }

        private void ProcessPendingClose(TabsConfig config)
        {
            if (_pendingCloseIndex >= 0 && _pendingCloseCallback != null)
            {
                var closeIndex = _pendingCloseIndex;
                var closeCallback = _pendingCloseCallback;
                _pendingCloseIndex = -1;
                _pendingCloseCallback = null;
                closeCallback?.Invoke(closeIndex);
            }
        }

        private void RenderTabContent(TabsConfig config, int selectedIndex)
        {
            var styleManager = guiHelper.GetStyleManager();
            var contentStyle = styleManager?.GetTabsContentStyle() ?? GUIStyle.none;
            
            layoutComponents.BeginVerticalGroup(contentStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            try
            {
                config.Content?.Invoke();
            }
            finally
            {
                layoutComponents.EndVerticalGroup();
            }
        }
        #endregion

        #region Tab Content API
        public void BeginTabContent(params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            var contentStyle = styleManager.GetTabsContentStyle();
            layoutComponents.BeginVerticalGroup(contentStyle, options);
        }

        public void EndTabContent()
        {
            layoutComponents.EndVerticalGroup();
        }
        #endregion

        #region Cleanup
        protected override void OnBeforeDispose()
        {
            base.OnBeforeDispose();
            ClearPillTextureCache();
        }

        private void ClearPillTextureCache()
        {
            foreach (var texture in _pillTextureCache.Values)
            {
                if (texture != null)
                {
                    UnityEngine.Object.Destroy(texture);
                }
            }
            _pillTextureCache.Clear();
            _lastPillTexture = null;
        }
        #endregion
    }
}
