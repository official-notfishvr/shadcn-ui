using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class Button : BaseComponent
    {
        public Button(GUIHelper helper)
            : base(helper) { }

        public bool Draw(ButtonConfig config)
        {
            if (config == null)
                return false;

            GUIStyle buttonStyle = styleManager?.GetButtonStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.button;
            var options = BuildLayoutOptions(config, buttonStyle);

            bool prevEnabled = GUI.enabled;
            Color prevColor = GUI.color;

            if (config.IsDisabled)
                GUI.enabled = false;

            float opacity = Mathf.Clamp01(config.Opacity);
            if (opacity < 0.999f)
                GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a * opacity);

            bool clicked = config.Icon?.Image != null ? DrawWithIcon(config, buttonStyle, options) : DrawBasic(config, buttonStyle, options);

            GUI.enabled = prevEnabled;
            GUI.color = prevColor;

            if (clicked && !config.IsDisabled)
                config.OnClick?.Invoke();

            return clicked && !config.IsDisabled;
        }

        private List<GUILayoutOption> BuildLayoutOptions(ButtonConfig config, GUIStyle style)
        {
            var options = new List<GUILayoutOption>(config.LayoutOptions ?? Array.Empty<GUILayoutOption>());

            if (style.fixedWidth > 0)
                options.Add(GUILayout.Width(style.fixedWidth));
            else
                options.Add(GUILayout.ExpandWidth(true));

            if (style.fixedHeight > 0)
                options.Add(GUILayout.Height(style.fixedHeight));

            return options;
        }

        private bool DrawBasic(ButtonConfig config, GUIStyle style, List<GUILayoutOption> options)
        {
            var content = new UnityHelpers.GUIContent(config.Text ?? string.Empty);
            var rect = GUILayoutUtility.GetRect(content, style, options.ToArray());
            bool hovered = rect.Contains(Event.current.mousePosition);
            return DrawButtonRect(rect, content, style, hovered && !config.IsDisabled);
        }

        private bool DrawWithIcon(ButtonConfig config, GUIStyle style, List<GUILayoutOption> options)
        {
            var icon = config.Icon;
            var content = new UnityHelpers.GUIContent(config.Text ?? string.Empty);
            var rect = GUILayoutUtility.GetRect(content, style, options.ToArray());
            bool hovered = rect.Contains(Event.current.mousePosition) && !config.IsDisabled;
            bool clicked = DrawButtonRect(rect, UnityHelpers.GUIContent.none, style, hovered);
            DrawButtonIconContent(rect, style, config.Text ?? string.Empty, icon);
            return clicked;
        }

        private bool DrawButtonRect(Rect rect, UnityHelpers.GUIContent content, GUIStyle style, bool hovered)
        {
            return UnityHelpers.Button(rect, content, style);
        }

        private void DrawButtonIconContent(Rect rect, GUIStyle style, string text, IconConfig iconConfig)
        {
            if (iconConfig?.Image == null)
                return;

            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            float spacing = iconConfig.Spacing * guiHelper.uiScale;
            var labelStyle = new UnityHelpers.GUIStyle(style)
            {
                normal = { background = null },
                hover = { background = null },
                active = { background = null },
                focused = { background = null },
                onNormal = { background = null },
                onHover = { background = null },
                onActive = { background = null },
                onFocused = { background = null },
                alignment = TextAnchor.MiddleLeft,
                clipping = TextClipping.Clip,
            };

            bool hasText = !string.IsNullOrEmpty(text);
            Vector2 textSize = hasText ? labelStyle.CalcSize(new UnityHelpers.GUIContent(text)) : Vector2.zero;

            Rect iconRect;
            Rect textRect;

            if (!hasText)
            {
                iconRect = new Rect(rect.x + (rect.width - scaledSize) * 0.5f, rect.y + (rect.height - scaledSize) * 0.5f, scaledSize, scaledSize);
                GUI.DrawTexture(iconRect, iconConfig.Image, ScaleMode.ScaleToFit);
                return;
            }

            switch (iconConfig.Position)
            {
                case IconPosition.Right:
                {
                    float totalWidth = textSize.x + spacing + scaledSize;
                    float startX = rect.x + (rect.width - totalWidth) * 0.5f;
                    textRect = new Rect(startX, rect.y, textSize.x, rect.height);
                    iconRect = new Rect(startX + textSize.x + spacing, rect.y + (rect.height - scaledSize) * 0.5f, scaledSize, scaledSize);
                    break;
                }
                case IconPosition.Above:
                {
                    float totalHeight = scaledSize + spacing + textSize.y;
                    float startY = rect.y + (rect.height - totalHeight) * 0.5f;
                    iconRect = new Rect(rect.x + (rect.width - scaledSize) * 0.5f, startY, scaledSize, scaledSize);
                    textRect = new Rect(rect.x, startY + scaledSize + spacing, rect.width, textSize.y);
                    labelStyle.alignment = TextAnchor.UpperCenter;
                    break;
                }
                case IconPosition.Below:
                {
                    float totalHeight = textSize.y + spacing + scaledSize;
                    float startY = rect.y + (rect.height - totalHeight) * 0.5f;
                    textRect = new Rect(rect.x, startY, rect.width, textSize.y);
                    iconRect = new Rect(rect.x + (rect.width - scaledSize) * 0.5f, startY + textSize.y + spacing, scaledSize, scaledSize);
                    labelStyle.alignment = TextAnchor.UpperCenter;
                    break;
                }
                default:
                {
                    float totalWidth = scaledSize + spacing + textSize.x;
                    float startX = rect.x + (rect.width - totalWidth) * 0.5f;
                    iconRect = new Rect(startX, rect.y + (rect.height - scaledSize) * 0.5f, scaledSize, scaledSize);
                    textRect = new Rect(startX + scaledSize + spacing, rect.y, textSize.x, rect.height);
                    break;
                }
            }

            GUI.DrawTexture(iconRect, iconConfig.Image, ScaleMode.ScaleToFit);
            GUI.Label(textRect, text, labelStyle);
        }
    }
}
