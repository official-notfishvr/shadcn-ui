using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Layout
{
    public class Tabs : BaseComponent
    {
        #region Enums
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
        }
        #endregion

        #region Data Types
        public struct TabConfig
        {
            public string Name;
            public Action Content;
            public bool Disabled;

            public TabConfig(string name, Action content, bool disabled = false)
            {
                Name = name;
                Content = content;
                Disabled = disabled;
            }
        }
        #endregion

        public Tabs(GUIHelper helper)
            : base(helper) { }

        #region Config-based API
        public int Draw(TabsConfig config)
        {
            return DrawTabsInternal(config);
        }

        public int DrawWithAutoClose(ref string[] tabNames, ref bool[] closableTabs, int selectedIndex, Action content = null, Action<int> onTabChange = null)
        {
            var config = new TabsConfig(tabNames, selectedIndex)
            {
                ClosableTabs = closableTabs,
                Content = content,
                OnTabChange = onTabChange,
            };

            var result = DrawTabsInternalWithAutoClose(ref tabNames, ref closableTabs, ref selectedIndex, config);
            return result;
        }
        #endregion

        private int _pendingCloseIndex = -1;
        private Action<int> _pendingCloseCallback = null;

        #region Internal Drawing

        private int DrawTabsInternalWithAutoClose(ref string[] tabNames, ref bool[] closableTabs, ref int selectedIndex, TabsConfig config)
        {
            if (tabNames == null || tabNames.Length == 0)
            {
                config.Content?.Invoke();
                return selectedIndex;
            }

            if (_pendingCloseIndex >= 0 && _pendingCloseCallback == null)
            {
                var closeIndex = _pendingCloseIndex;
                _pendingCloseIndex = -1;

                var newNames = new List<string>(tabNames);
                var newClosable = new List<bool>(closableTabs);

                if (closeIndex >= 0 && closeIndex < newNames.Count)
                {
                    newNames.RemoveAt(closeIndex);
                    if (closeIndex < newClosable.Count)
                        newClosable.RemoveAt(closeIndex);

                    tabNames = newNames.ToArray();
                    closableTabs = newClosable.ToArray();

                    config.TabNames = tabNames;
                    config.ClosableTabs = closableTabs;

                    if (selectedIndex >= tabNames.Length)
                        selectedIndex = Math.Max(0, tabNames.Length - 1);
                    else if (selectedIndex > closeIndex)
                        selectedIndex--;

                    config.SelectedIndex = selectedIndex;
                }

                if (tabNames.Length == 0)
                {
                    config.Content?.Invoke();
                    return selectedIndex;
                }
            }

            return DrawTabsInternal(config);
        }

        private int DrawTabsInternal(TabsConfig config)
        {
            if (config.TabNames == null || config.TabNames.Length == 0)
            {
                config.Content?.Invoke();
                return config.SelectedIndex;
            }

            if (_pendingCloseIndex >= 0 && _pendingCloseCallback != null)
            {
                var closeIndex = _pendingCloseIndex;
                var closeCallback = _pendingCloseCallback;
                _pendingCloseIndex = -1;
                _pendingCloseCallback = null;
                closeCallback(closeIndex);
            }

            var styleManager = guiHelper.GetStyleManager();
            var selectedIndex = Mathf.Clamp(config.SelectedIndex, 0, config.TabNames.Length - 1);
            var newSelectedIndex = selectedIndex;

            Action drawTabButtons = () =>
            {
                var tabsPerLine = (int)Mathf.Ceil((float)config.TabNames.Length / config.MaxLines);

                for (var line = 0; line < config.MaxLines; line++)
                {
                    var horizontalStarted = false;

                    try
                    {
                        layoutComponents.BeginHorizontalGroup(styleManager.GetTabsListStyle());
                        horizontalStarted = true;

                        for (var i = line * tabsPerLine; i < (line + 1) * tabsPerLine && i < config.TabNames.Length; i++)
                        {
                            var isActive = i == selectedIndex;
                            var isDisabled = config.DisabledTabs != null && i < config.DisabledTabs.Length && config.DisabledTabs[i];
                            var triggerStyle = styleManager.GetTabsTriggerStyle(isActive);

                            var hasIcon = config.TabIcons != null && i < config.TabIcons.Length && config.TabIcons[i] != null;
                            var isClosable = config.ClosableTabs != null && i < config.ClosableTabs.Length && config.ClosableTabs[i];

                            var tabLabel = config.TabNames[i] ?? $"Tab {i + 1}";
                            var displayLabel = isClosable ? tabLabel + "  ×" : tabLabel;

                            GUILayoutOption[] finalOptions;
                            if (config.Options == null || config.Options.Length == 0)
                            {
                                finalOptions = new GUILayoutOption[] { GUILayout.Height(Mathf.RoundToInt(DesignTokens.Tab.Height * guiHelper.uiScale)) };
                            }
                            else
                            {
                                var tabOptions = new List<GUILayoutOption>(config.Options.Length + 1);
                                tabOptions.Add(GUILayout.Height(Mathf.RoundToInt(DesignTokens.Tab.Height * guiHelper.uiScale)));
                                tabOptions.AddRange(config.Options);
                                finalOptions = tabOptions.ToArray();
                            }

                            GUI.enabled = !isDisabled;
                            var tabRect = GUILayoutUtility.GetRect(new GUIContent(displayLabel), triggerStyle, finalOptions);

                            var closeClicked = false;
                            if (isClosable)
                            {
                                var closeButtonSize = DesignTokens.CloseButton.HitArea * guiHelper.uiScale;
                                var closeButtonRect = new Rect(tabRect.x + tabRect.width - closeButtonSize - DesignTokens.Spacing.XS * guiHelper.uiScale, tabRect.y + (tabRect.height - closeButtonSize) / 2, closeButtonSize, closeButtonSize);

                                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && closeButtonRect.Contains(Event.current.mousePosition))
                                {
                                    closeClicked = true;
                                    _pendingCloseIndex = i;
                                    _pendingCloseCallback = config.OnTabClose;
                                    Event.current.Use();
                                }
                            }

                            var clicked = !isDisabled && !closeClicked && GUI.Button(tabRect, "", triggerStyle);

                            if (hasIcon)
                            {
                                var iconSize = DesignTokens.Icon.Small * guiHelper.uiScale;
                                var labelContent = new GUIContent(tabLabel);
                                var labelWidth = triggerStyle.CalcSize(labelContent).x;
                                var totalContentWidth = iconSize + DesignTokens.Spacing.XS * guiHelper.uiScale + labelWidth;
                                var contentStartX = tabRect.x + (tabRect.width - totalContentWidth) / 2;
                                if (isClosable)
                                    contentStartX -= DesignTokens.Spacing.MD * guiHelper.uiScale;

                                var iconY = tabRect.y + (tabRect.height - iconSize) / 2;
                                var iconRect = new Rect(contentStartX, iconY, iconSize, iconSize);
                                GUI.DrawTexture(iconRect, config.TabIcons[i], ScaleMode.ScaleToFit);

                                var labelStyle = new UnityHelpers.GUIStyle(triggerStyle);
                                labelStyle.alignment = TextAnchor.MiddleLeft;
                                var labelRect = new Rect(contentStartX + iconSize + DesignTokens.Spacing.XS * guiHelper.uiScale, tabRect.y, labelWidth, tabRect.height);
                                GUI.Label(labelRect, tabLabel, labelStyle);
                            }
                            else
                            {
                                var labelStyle = new UnityHelpers.GUIStyle(triggerStyle);
                                labelStyle.alignment = TextAnchor.MiddleCenter;
                                var labelRect = isClosable ? new Rect(tabRect.x, tabRect.y, tabRect.width - DesignTokens.CloseButton.HitArea * guiHelper.uiScale, tabRect.height) : tabRect;
                                GUI.Label(labelRect, tabLabel, labelStyle);
                            }

                            if (isClosable)
                            {
                                var closeButtonSize = DesignTokens.CloseButton.IconSize * guiHelper.uiScale;
                                var closeX = tabRect.x + tabRect.width - closeButtonSize - DesignTokens.Spacing.SM * guiHelper.uiScale;
                                var closeY = tabRect.y + (tabRect.height - closeButtonSize) / 2;
                                var closeRect = new Rect(closeX, closeY, closeButtonSize, closeButtonSize);

                                var closeStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                                closeStyle.fontSize = Mathf.RoundToInt(DesignTokens.CloseButton.FontSize * guiHelper.uiScale);
                                closeStyle.fontStyle = FontStyle.Bold;
                                closeStyle.alignment = TextAnchor.MiddleCenter;
                                closeStyle.normal.textColor = closeRect.Contains(Event.current.mousePosition) ? ThemeManager.Instance.CurrentTheme.Destructive : ThemeManager.Instance.CurrentTheme.Muted;

                                GUI.Label(closeRect, "×", closeStyle);
                            }

                            GUI.enabled = true;

                            if (clicked && i != selectedIndex)
                            {
                                newSelectedIndex = i;
                                config.OnTabChange?.Invoke(i);
                            }

                            if (config.ShowIndicator && isActive)
                            {
                                switch (config.IndicatorStyle)
                                {
                                    case IndicatorStyle.Underline:
                                        DrawUnderlineIndicator(tabRect, config.IndicatorStyle);
                                        break;
                                    case IndicatorStyle.Background:
                                        DrawBackgroundIndicator(tabRect, config.IndicatorStyle);
                                        break;
                                    case IndicatorStyle.Border:
                                        DrawBorderIndicator(tabRect, config.IndicatorStyle);
                                        break;
                                }
                            }

                            if (i < (line + 1) * tabsPerLine - 1 && i < config.TabNames.Length - 1)
                            {
                                layoutComponents.AddSpace((int)DesignTokens.Spacing.XXS);
                            }
                        }
                    }
                    catch (Exception) { }
                    finally
                    {
                        if (horizontalStarted)
                        {
                            try
                            {
                                layoutComponents.EndHorizontalGroup();
                            }
                            catch { }
                        }
                    }
                }
            };

            Action drawVerticalTabButtons = () =>
            {
                var tabsPerColumn = (int)Mathf.Ceil((float)config.TabNames.Length / config.MaxLines);

                for (var col = 0; col < config.MaxLines; col++)
                {
                    var verticalStarted = false;
                    try
                    {
                        layoutComponents.BeginVerticalGroup(GUILayout.Width(config.TabWidth * guiHelper.uiScale));
                        verticalStarted = true;

                        for (var i = col * tabsPerColumn; i < (col + 1) * tabsPerColumn && i < config.TabNames.Length; i++)
                        {
                            var isActive = i == selectedIndex;
                            var isDisabled = config.DisabledTabs != null && i < config.DisabledTabs.Length && config.DisabledTabs[i];
                            var triggerStyle = styleManager.GetTabsTriggerStyle(isActive);

                            var hasIcon = config.TabIcons != null && i < config.TabIcons.Length && config.TabIcons[i] != null;
                            var isClosable = config.ClosableTabs != null && i < config.ClosableTabs.Length && config.ClosableTabs[i];

                            var tabLabel = config.TabNames[i] ?? $"Tab {i + 1}";
                            var displayLabel = isClosable ? tabLabel + "  ×" : tabLabel;

                            GUILayoutOption[] finalOptions;
                            if (config.Options == null || config.Options.Length == 0)
                            {
                                finalOptions = new GUILayoutOption[] { GUILayout.Width(config.TabWidth * guiHelper.uiScale), GUILayout.Height(Mathf.RoundToInt(DesignTokens.Tab.Height * guiHelper.uiScale)), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false) };
                            }
                            else
                            {
                                var tabOptions = new List<GUILayoutOption>(config.Options.Length + 4);
                                tabOptions.Add(GUILayout.Width(config.TabWidth * guiHelper.uiScale));
                                tabOptions.Add(GUILayout.Height(Mathf.RoundToInt(DesignTokens.Tab.Height * guiHelper.uiScale)));
                                tabOptions.Add(GUILayout.ExpandWidth(false));
                                tabOptions.Add(GUILayout.ExpandHeight(false));
                                tabOptions.AddRange(config.Options);
                                finalOptions = tabOptions.ToArray();
                            }

                            GUI.enabled = !isDisabled;

                            var tabRect = GUILayoutUtility.GetRect(new GUIContent(displayLabel), triggerStyle, finalOptions);

                            var closeClicked = false;
                            if (isClosable)
                            {
                                var closeButtonSize = DesignTokens.CloseButton.HitArea * guiHelper.uiScale;
                                var closeButtonRect = new Rect(tabRect.x + tabRect.width - closeButtonSize - DesignTokens.Spacing.XS * guiHelper.uiScale, tabRect.y + (tabRect.height - closeButtonSize) / 2, closeButtonSize, closeButtonSize);

                                if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && closeButtonRect.Contains(Event.current.mousePosition))
                                {
                                    closeClicked = true;
                                    _pendingCloseIndex = i;
                                    _pendingCloseCallback = config.OnTabClose;
                                    Event.current.Use();
                                }
                            }

                            var clicked = !isDisabled && !closeClicked && GUI.Button(tabRect, "", triggerStyle);

                            if (hasIcon)
                            {
                                var iconSize = DesignTokens.Icon.Small * guiHelper.uiScale;
                                var labelContent = new GUIContent(tabLabel);
                                var labelWidth = triggerStyle.CalcSize(labelContent).x;
                                var totalContentWidth = iconSize + DesignTokens.Spacing.XS * guiHelper.uiScale + labelWidth;
                                var contentStartX = tabRect.x + (tabRect.width - totalContentWidth) / 2;
                                if (isClosable)
                                    contentStartX -= DesignTokens.Spacing.MD * guiHelper.uiScale;

                                var iconY = tabRect.y + (tabRect.height - iconSize) / 2;
                                var iconRect = new Rect(contentStartX, iconY, iconSize, iconSize);
                                GUI.DrawTexture(iconRect, config.TabIcons[i], ScaleMode.ScaleToFit);

                                var labelStyle = new UnityHelpers.GUIStyle(triggerStyle);
                                labelStyle.alignment = TextAnchor.MiddleLeft;
                                var labelRect = new Rect(contentStartX + iconSize + DesignTokens.Spacing.XS * guiHelper.uiScale, tabRect.y, labelWidth, tabRect.height);
                                GUI.Label(labelRect, tabLabel, labelStyle);
                            }
                            else
                            {
                                var labelStyle = new UnityHelpers.GUIStyle(triggerStyle);
                                labelStyle.alignment = TextAnchor.MiddleCenter;
                                var labelRect = isClosable ? new Rect(tabRect.x, tabRect.y, tabRect.width - DesignTokens.CloseButton.HitArea * guiHelper.uiScale, tabRect.height) : tabRect;
                                GUI.Label(labelRect, tabLabel, labelStyle);
                            }

                            if (isClosable)
                            {
                                var closeButtonSize = DesignTokens.CloseButton.IconSize * guiHelper.uiScale;
                                var closeX = tabRect.x + tabRect.width - closeButtonSize - DesignTokens.Spacing.SM * guiHelper.uiScale;
                                var closeY = tabRect.y + (tabRect.height - closeButtonSize) / 2;
                                var closeRect = new Rect(closeX, closeY, closeButtonSize, closeButtonSize);

                                var closeStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                                closeStyle.fontSize = Mathf.RoundToInt(DesignTokens.CloseButton.FontSize * guiHelper.uiScale);
                                closeStyle.fontStyle = FontStyle.Bold;
                                closeStyle.alignment = TextAnchor.MiddleCenter;
                                closeStyle.normal.textColor = closeRect.Contains(Event.current.mousePosition) ? ThemeManager.Instance.CurrentTheme.Destructive : ThemeManager.Instance.CurrentTheme.Muted;

                                GUI.Label(closeRect, "×", closeStyle);
                            }

                            GUI.enabled = true;

                            if (clicked && i != selectedIndex)
                            {
                                newSelectedIndex = i;
                                config.OnTabChange?.Invoke(i);
                            }

                            if (config.ShowIndicator && isActive)
                            {
                                switch (config.IndicatorStyle)
                                {
                                    case IndicatorStyle.Underline:
                                        DrawUnderlineIndicator(tabRect, config.IndicatorStyle);
                                        break;
                                    case IndicatorStyle.Background:
                                        DrawBackgroundIndicator(tabRect, config.IndicatorStyle);
                                        break;
                                    case IndicatorStyle.Border:
                                        DrawBorderIndicator(tabRect, config.IndicatorStyle);
                                        break;
                                }
                            }

                            if (i < (col + 1) * tabsPerColumn - 1 && i < config.TabNames.Length - 1)
                            {
                                layoutComponents.AddSpace((int)DesignTokens.Spacing.XXS);
                            }
                        }
                    }
                    catch (Exception) { }
                    finally
                    {
                        if (verticalStarted)
                        {
                            try
                            {
                                layoutComponents.EndVerticalGroup();
                            }
                            catch { }
                        }
                    }
                }
            };

            if (config.Position == TabPosition.Top)
            {
                drawTabButtons();
                RenderTabContent(config, newSelectedIndex);
            }
            else if (config.Position == TabPosition.Bottom)
            {
                RenderTabContent(config, newSelectedIndex);
                drawTabButtons();
            }
            else if (config.Position == TabPosition.Left)
            {
                var mainHorizontalStarted = false;
                try
                {
                    layoutComponents.BeginHorizontalGroup();
                    mainHorizontalStarted = true;
                    drawVerticalTabButtons();
                    RenderTabContent(config, newSelectedIndex);
                }
                catch (Exception) { }
                finally
                {
                    if (mainHorizontalStarted)
                    {
                        try
                        {
                            layoutComponents.EndHorizontalGroup();
                        }
                        catch { }
                    }
                }
            }
            else if (config.Position == TabPosition.Right)
            {
                var mainHorizontalStarted = false;
                try
                {
                    layoutComponents.BeginHorizontalGroup();
                    mainHorizontalStarted = true;
                    RenderTabContent(config, newSelectedIndex);
                    drawVerticalTabButtons();
                }
                catch (Exception) { }
                finally
                {
                    if (mainHorizontalStarted)
                    {
                        try
                        {
                            layoutComponents.EndHorizontalGroup();
                        }
                        catch { }
                    }
                }
            }

            return newSelectedIndex;
        }

        private void RenderTabContent(TabsConfig config, int selectedIndex)
        {
            config.Content?.Invoke();
        }

        private void DrawUnderlineIndicator(Rect tabRect, IndicatorStyle style)
        {
            if (!tabRect.Equals(Rect.zero))
            {
                var indicatorColor = ThemeManager.Instance.CurrentTheme.Accent;
                var indicatorHeight = DesignTokens.Tab.IndicatorHeight;
                var indicatorRect = new Rect(tabRect.x, tabRect.y + tabRect.height - indicatorHeight, tabRect.width, indicatorHeight);

                GUI.color = indicatorColor;
                GUI.DrawTexture(indicatorRect, Texture2D.whiteTexture);
                GUI.color = Color.white;
            }
        }

        private void DrawBackgroundIndicator(Rect tabRect, IndicatorStyle style)
        {
            if (!tabRect.Equals(Rect.zero))
            {
                var indicatorColor = ThemeManager.Instance.CurrentTheme.Accent;
                indicatorColor.a = 0.1f;

                GUI.color = indicatorColor;
                GUI.DrawTexture(tabRect, Texture2D.whiteTexture);
                GUI.color = Color.white;
            }
        }

        private void DrawBorderIndicator(Rect tabRect, IndicatorStyle style)
        {
            if (!tabRect.Equals(Rect.zero))
            {
                var indicatorColor = ThemeManager.Instance.CurrentTheme.Accent;
                var borderWidth = DesignTokens.Tab.BorderWidth;

                GUI.color = indicatorColor;
                GUI.DrawTexture(new Rect(tabRect.x, tabRect.y, tabRect.width, borderWidth), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(tabRect.x, tabRect.y + tabRect.height - borderWidth, tabRect.width, borderWidth), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(tabRect.x, tabRect.y, borderWidth, tabRect.height), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(tabRect.x + tabRect.width - borderWidth, tabRect.y, borderWidth, tabRect.height), Texture2D.whiteTexture);
                GUI.color = Color.white;
            }
        }
        #endregion

        #region Tab Content
        public void BeginTabContent(params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup(styleManager.GetTabsContentStyle(), options);
        }

        public void EndTabContent()
        {
            layoutComponents.EndVerticalGroup();
        }

        #endregion
    }
}
