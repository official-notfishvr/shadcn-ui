using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class ThemeChanger : BaseComponent
    {
        private const float AnimationDuration = DesignTokens.Animation.DurationFast;
        private Vector2 _scrollPosition;
        private Rect _triggerRect;

        public ThemeChanger(GUIHelper helper)
            : base(helper) { }

        #region Config-based API
        public void Draw(ThemeChangerConfig config = null)
        {
            config ??= new ThemeChangerConfig();
            var styleManager = guiHelper.GetStyleManager();
            var themeManager = ThemeManager.Instance;
            var currentTheme = themeManager.CurrentTheme;

            GUIStyle buttonStyle = styleManager?.GetButtonStyle(ControlVariant.Outline, ControlSize.Default) ?? GUI.skin.button;

            string buttonText = currentTheme?.Name ?? "Select Theme";
            string dropdownIcon = LayerManager.Instance.IsOpen(config.Id) ? " ^" : " v";

            var buttonOptions = config.Options ?? new[] { GUILayout.Width(config.Width) };
            if (UnityHelpers.Button(buttonText + dropdownIcon, buttonStyle, buttonOptions))
                HandleThemeButtonClick(config);

            if (Event.current.type == EventType.Repaint)
                _triggerRect = GUILayoutUtility.GetLastRect();

            UpdateDropdownPosition(config);
        }
        #endregion

        #region API
        public void DrawCompact(string id = "theme_compact")
        {
            Draw(
                new ThemeChangerConfig
                {
                    Id = id,
                    Width = 120f,
                    ShowPreview = false,
                }
            );
        }

        public void DrawWithPreview(string id = "theme_preview", float width = 220f)
        {
            Draw(
                new ThemeChangerConfig
                {
                    Id = id,
                    Width = width,
                    ShowPreview = true,
                }
            );
        }
        #endregion

        #region Private Methods
        private void HandleThemeButtonClick(ThemeChangerConfig config)
        {
            if (LayerManager.Instance.IsOpen(config.Id))
            {
                LayerManager.Instance.Close(config.Id);
            }
            else
            {
                Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(_triggerRect.x, _triggerRect.yMax + 4));

                LayerManager.Instance.Open(
                    new LayerConfig
                    {
                        Id = config.Id,
                        OpenPosition = screenPos,
                        Width = config.Width,
                        Height = config.DropdownHeight,
                        CloseOnClickOutside = true,
                        ZIndex = DesignTokens.ZIndex.Dropdown,
                        Content = () =>
                        {
                            var styleManager = guiHelper.GetStyleManager();
                            var themeManager = ThemeManager.Instance;
                            DrawThemeList(config, themeManager, styleManager);
                        },
                    }
                );
            }
        }

        private void UpdateDropdownPosition(ThemeChangerConfig config)
        {
            if (LayerManager.Instance.IsOpen(config.Id))
            {
                Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(_triggerRect.x, _triggerRect.yMax + 4));
                LayerManager.Instance.SetPosition(config.Id, screenPos);
            }
        }

        private void DrawThemeList(ThemeChangerConfig config, ThemeManager themeManager, StyleManager styleManager)
        {
            var themes = themeManager.Themes.Values.ToList();
            var currentTheme = themeManager.CurrentTheme;

            GUIStyle boxStyle = styleManager?.GetSelectStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.box;
            GUIStyle itemStyle = styleManager?.GetSelectItemStyle() ?? GUI.skin.button;

            GUILayout.BeginVertical(boxStyle);

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(config.Width - 10), GUILayout.Height(config.DropdownHeight - 20));

            foreach (var theme in themes)
                DrawThemeItem(theme, currentTheme, config, itemStyle);

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void DrawThemeItem(Theme theme, Theme currentTheme, ThemeChangerConfig config, GUIStyle itemStyle)
        {
            bool isSelected = theme.Name == currentTheme?.Name;

            GUILayout.BeginHorizontal();

            if (config.ShowPreview)
            {
                DrawThemePreview(theme, 24f);
                GUILayout.Space(8);
            }

            string label = isSelected ? theme.Name + " *" : theme.Name;

            if (UnityHelpers.Button(label, itemStyle, GUILayout.ExpandWidth(true)))
            {
                var themeManager = ThemeManager.Instance;
                themeManager.SetTheme(theme.Name);
                guiHelper.GetStyleManager()?.MarkStylesCorruption();
                config.OnThemeChanged?.Invoke(theme);
                LayerManager.Instance.Close(config.Id);
            }

            GUILayout.EndHorizontal();
        }

        private void DrawThemePreview(Theme theme, float size)
        {
            Rect previewRect = GUILayoutUtility.GetRect(size, size);

            float halfSize = size / 2f;
            Rect topLeft = new Rect(previewRect.x, previewRect.y, halfSize, halfSize);
            Rect topRight = new Rect(previewRect.x + halfSize, previewRect.y, halfSize, halfSize);
            Rect bottomLeft = new Rect(previewRect.x, previewRect.y + halfSize, halfSize, halfSize);
            Rect bottomRight = new Rect(previewRect.x + halfSize, previewRect.y + halfSize, halfSize, halfSize);

            Color prevColor = GUI.color;

            GUI.color = theme.Base;
            GUI.DrawTexture(topLeft, Texture2D.whiteTexture);

            GUI.color = theme.Accent;
            GUI.DrawTexture(topRight, Texture2D.whiteTexture);

            GUI.color = theme.Secondary;
            GUI.DrawTexture(bottomLeft, Texture2D.whiteTexture);

            GUI.color = theme.Text;
            GUI.DrawTexture(bottomRight, Texture2D.whiteTexture);

            GUI.color = theme.Border;
            DrawRectOutline(previewRect, 1f);

            GUI.color = prevColor;
        }

        private void DrawRectOutline(Rect rect, float thickness)
        {
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, thickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.yMax - thickness, rect.width, thickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.y, thickness, rect.height), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - thickness, rect.y, thickness, rect.height), Texture2D.whiteTexture);
        }
        #endregion
    }
}
