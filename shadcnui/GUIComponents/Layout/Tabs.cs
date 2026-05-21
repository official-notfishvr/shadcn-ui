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

        private int _pendingCloseIndex = -1;
        private Action<int> _pendingCloseCallback;
        private Vector2 _tabScrollPosition = Vector2.zero;

        public Tabs(GUIHelper helper)
            : base(helper) { }

        #region Public Drawing API
        public int Render(TabsConfig config)
        {
            if (config.TabLabels == null || config.TabLabels.Length == 0)
            {
                config.Content?.Invoke();
                guiHelper.FlushAutoRenderBuilder();
                return config.SelectedIndex;
            }

            ProcessPendingClose(config);
            return DrawTabs(config);
        }

        internal int DrawWithAutoClose(ref string[] tabNames, ref bool[] closableTabs, int selectedIndex, Action content = null, Action<int> onTabChange = null)
        {
            if (tabNames == null || tabNames.Length == 0)
            {
                content?.Invoke();
                guiHelper.FlushAutoRenderBuilder();
                return selectedIndex;
            }

            var config = new TabsConfig(tabNames, selectedIndex)
            {
                ClosableTabs = closableTabs,
                Content = content,
                OnSelectionChanged = onTabChange,
            };

            return HandleAutoClose(ref tabNames, ref closableTabs, ref selectedIndex, config);
        }

        #endregion

        #region Core Drawing Logic
        private int DrawTabs(TabsConfig config)
        {
            var selectedIndex = Mathf.Clamp(config.SelectedIndex, 0, config.TabLabels.Length - 1);
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
            var tabCount = config.TabLabels.Length;
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
                    layoutComponents.BeginVerticalGroup(localStyleManager.GetTabsListStyle(config.Variant, config.Size, config.Appearance), GUILayout.Width(config.TabWidth * guiHelper.uiScale));

                    for (var i = 0; i < config.TabLabels.Length; i++)
                    {
                        newSelectedIndex = DrawSingleTab(config, localStyleManager, i, selectedIndex, newSelectedIndex, true);

                        if (i < config.TabLabels.Length - 1)
                        {
                            layoutComponents.AddSpace((int)(DesignTokens.Spacing.XXS * guiHelper.uiScale));
                        }
                    }
                }
                else
                {
                    layoutComponents.BeginHorizontalGroup(localStyleManager.GetTabsListStyle(config.Variant, config.Size, config.Appearance));

                    for (var i = 0; i < config.TabLabels.Length; i++)
                    {
                        newSelectedIndex = DrawSingleTab(config, localStyleManager, i, selectedIndex, newSelectedIndex);

                        if (i < config.TabLabels.Length - 1)
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
            var totalLines = Mathf.Min(config.MaxLines, (int)Mathf.Ceil((float)config.TabLabels.Length / tabsPerLine));

            if (config.EnableOverflowScroll)
            {
                var totalHeight = TAB_HEIGHT * guiHelper.uiScale * totalLines + DesignTokens.Spacing.XXS * guiHelper.uiScale * (totalLines - 1) + 4;
                _tabScrollPosition = GUILayout.BeginScrollView(_tabScrollPosition, GUIStyle.none, GUIStyle.none, GUILayout.Height(totalHeight));
            }

            try
            {
                layoutComponents.BeginVerticalGroup(localStyleManager.GetTabsListStyle(config.Variant, config.Size, config.Appearance));

                for (var line = 0; line < totalLines; line++)
                {
                    layoutComponents.BeginHorizontalGroup();

                    for (var i = line * tabsPerLine; i < (line + 1) * tabsPerLine && i < config.TabLabels.Length; i++)
                    {
                        newSelectedIndex = DrawSingleTab(config, localStyleManager, i, selectedIndex, newSelectedIndex);

                        if (i < (line + 1) * tabsPerLine - 1 && i < config.TabLabels.Length - 1)
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
            var totalColumns = Mathf.Min(config.MaxLines, (int)Mathf.Ceil((float)config.TabLabels.Length / tabsPerColumn));

            if (config.EnableOverflowScroll)
            {
                var totalWidth = config.TabWidth * guiHelper.uiScale * totalColumns + DesignTokens.Spacing.XXS * guiHelper.uiScale * (totalColumns - 1) + 4;
                _tabScrollPosition = GUILayout.BeginScrollView(_tabScrollPosition, GUILayout.Width(totalWidth), GUILayout.ExpandHeight(true));
            }

            try
            {
                layoutComponents.BeginHorizontalGroup(localStyleManager.GetTabsListStyle(config.Variant, config.Size, config.Appearance));

                for (var col = 0; col < totalColumns; col++)
                {
                    layoutComponents.BeginVerticalGroup(GUILayout.Width(config.TabWidth * guiHelper.uiScale));

                    for (var i = col * tabsPerColumn; i < (col + 1) * tabsPerColumn && i < config.TabLabels.Length; i++)
                    {
                        newSelectedIndex = DrawSingleTab(config, localStyleManager, i, selectedIndex, newSelectedIndex, true);

                        if (i < (col + 1) * tabsPerColumn - 1 && i < config.TabLabels.Length - 1)
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

            var triggerStyle = localStyleManager.GetTabsTriggerStyle(isActive, config.Variant, config.Size, config.Appearance);
            var tabLabel = config.TabLabels[index] ?? $"Tab {index + 1}";

            GUILayoutOption[] layoutOptions;
            if (isVertical)
            {
                layoutOptions = new[] { GUILayout.Width(config.TabWidth * guiHelper.uiScale), GUILayout.Height(TAB_HEIGHT * guiHelper.uiScale) };
            }
            else
            {
                layoutOptions = config.LayoutOptions ?? new[] { GUILayout.Height(TAB_HEIGHT * guiHelper.uiScale) };
            }

            GUI.enabled = !isDisabled;

            var tabRect = ControlLayoutUtility.ReserveRect(new UnityHelpers.GUIContent(tabLabel), triggerStyle, layoutOptions);

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
                config.OnSelectionChanged?.Invoke(index);
            }

            if (config.ShowIndicator && isActive)
            {
                DrawTabIndicator(tabRect, config.IndicatorStyle, isVertical, config.Position);
            }

            return currentNewIndex;
        }

        private void DrawTabContent(Rect tabRect, string label, GUIStyle triggerStyle, Texture2D icon, bool isClosable, bool isVertical)
        {
            if (icon != null)
            {
                ContentRenderUtility.DrawLeadingIconAndText(
                    isClosable ? ControlLayoutUtility.Inset(tabRect, right: CLOSE_BUTTON_HIT_AREA * guiHelper.uiScale) : tabRect,
                    triggerStyle,
                    label,
                    icon,
                    DesignTokens.Icon.Small * guiHelper.uiScale,
                    DesignTokens.Spacing.XS * guiHelper.uiScale,
                    alignment: isVertical ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter
                );
            }
            else
            {
                var labelStyle = ContentRenderUtility.CreateOverlayLabelStyle(triggerStyle, isVertical ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter);
                var labelRect = isClosable ? ControlLayoutUtility.Inset(tabRect, right: CLOSE_BUTTON_HIT_AREA * guiHelper.uiScale) : tabRect;
                GUI.Label(labelRect, label, labelStyle);
            }
        }

        private bool HandleCloseButton(Rect tabRect, int index, TabsConfig config)
        {
            var closeButtonSize = CLOSE_BUTTON_HIT_AREA * guiHelper.uiScale;
            var closeButtonRect = ControlLayoutUtility.Centered(ControlLayoutUtility.RightAligned(tabRect, closeButtonSize, DesignTokens.Spacing.XS * guiHelper.uiScale), closeButtonSize, closeButtonSize);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && closeButtonRect.Contains(Event.current.mousePosition))
            {
                _pendingCloseIndex = index;
                _pendingCloseCallback = config.OnTabClosed;
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
            var textureManager = guiHelper.GetStyleManager().Textures;

            switch (style)
            {
                case IndicatorStyle.Underline:
                    textureManager.DrawTabUnderlineIndicator(tabRect, indicatorColor, isVertical, position == TabPosition.Left, TAB_INDICATOR_HEIGHT, guiHelper.uiScale);
                    break;
                case IndicatorStyle.Background:
                    textureManager.DrawTabBackgroundIndicator(tabRect, indicatorColor);
                    break;
                case IndicatorStyle.Border:
                    textureManager.DrawTabBorderIndicator(tabRect, indicatorColor, isVertical, position == TabPosition.Left, TAB_BORDER_WIDTH, guiHelper.uiScale);
                    break;
            }
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

            return Render(config);
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
            var contentStyle = styleManager?.GetTabsContentStyle(config.Variant, config.Size, config.Appearance) ?? GUIStyle.none;

            layoutComponents.BeginVerticalGroup(contentStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            try
            {
                config.Content?.Invoke();
                guiHelper.FlushAutoRenderBuilder();
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
            guiHelper.FlushAutoRenderBuilder();
            layoutComponents.EndVerticalGroup();
        }
        #endregion
    }
}
