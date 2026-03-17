using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Data
{
    public class TextArea : BaseComponent
    {
        public TextArea(GUIHelper helper)
            : base(helper) { }

        public string DrawTextArea(TextAreaConfig config)
        {
            if (config == null)
                return string.Empty;

            if (!string.IsNullOrEmpty(config.Label))
            {
                UnityHelpers.Label(config.Label, styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label);
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            string value = config.Rect.HasValue ? DrawRectTextArea(config) : DrawLayoutTextArea(config);
            if (config.MaxLength > 0 && value.Length > config.MaxLength)
                value = value.Substring(0, config.MaxLength);

            if (config.ShowCharCount)
                DrawCharacterCount(config, value);

            return value;
        }

        public string DrawTextArea(string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            return DrawTextArea(
                new TextAreaConfig
                {
                    Value = text,
                    Variant = variant,
                    Placeholder = placeholder,
                    IsDisabled = disabled,
                    MinHeight = minHeight,
                    MaxLength = maxLength,
                    ShowCharCount = false,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public string DrawTextArea(Rect rect, string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, int maxLength = -1)
        {
            return DrawTextArea(
                new TextAreaConfig
                {
                    Value = text,
                    Variant = variant,
                    Placeholder = placeholder,
                    IsDisabled = disabled,
                    MaxLength = maxLength,
                    ShowCharCount = false,
                    Rect = rect,
                }
            );
        }

        public string OutlineTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) => DrawTextArea(text, ControlVariant.Outline, placeholder, disabled, minHeight, maxLength, options);

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options) => DrawTextArea(text, ControlVariant.Ghost, placeholder, disabled, minHeight, maxLength, options);

        public string LabeledTextArea(string label, string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, bool showCharCount = true, params GUILayoutOption[] options)
        {
            return DrawTextArea(
                new TextAreaConfig
                {
                    Value = text,
                    Label = label,
                    Variant = variant,
                    Placeholder = placeholder,
                    IsDisabled = disabled,
                    MinHeight = minHeight,
                    MaxLength = maxLength,
                    ShowCharCount = showCharCount,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public string ResizableTextArea(string text, ref float height, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, float maxHeight = 300f, int maxLength = -1, params GUILayoutOption[] options)
        {
            height = Mathf.Clamp(height, minHeight, maxHeight);

            var layoutOptions = new List<GUILayoutOption> { GUILayout.Height(height * guiHelper.uiScale), GUILayout.ExpandWidth(true) };
            if (options != null && options.Length > 0)
                layoutOptions.AddRange(options);

            string result = DrawTextArea(
                new TextAreaConfig
                {
                    Value = text,
                    Variant = variant,
                    Placeholder = placeholder,
                    IsDisabled = disabled,
                    MinHeight = height,
                    MaxLength = maxLength,
                    ShowCharCount = false,
                    LayoutOptions = layoutOptions.ToArray(),
                }
            );

            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();
            var gripStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Default) ?? GUI.skin.label;
            if (UnityHelpers.Button("⋮⋮⋮", gripStyle, GUILayout.Width(20f * guiHelper.uiScale), GUILayout.Height(10f * guiHelper.uiScale)))
                height = height >= maxHeight ? minHeight : height + 20f;
            layoutComponents.EndHorizontalGroup();

            return result;
        }

        private string DrawLayoutTextArea(TextAreaConfig config)
        {
            string controlName = "textarea_" + config.Id;
            bool focused = GUI.GetNameOfFocusedControl() == controlName;
            var style = styleManager?.GetTextAreaStyle(config.Variant, config.Size, focused) ?? GUI.skin.textArea;

            var options = new List<GUILayoutOption> { GUILayout.MinHeight(config.MinHeight * guiHelper.uiScale), GUILayout.MaxHeight(config.MaxHeight * guiHelper.uiScale), GUILayout.ExpandWidth(true) };

            if (config.LayoutOptions != null && config.LayoutOptions.Length > 0)
                options.AddRange(config.LayoutOptions);

            bool wasEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            GUI.SetNextControlName(controlName);
            string value = UnityHelpers.TextArea(config.Value ?? string.Empty, style, options.ToArray());

            GUI.enabled = wasEnabled;

            DrawPlaceholderIfNeeded(config, style, focused);
            return value;
        }

        private string DrawRectTextArea(TextAreaConfig config)
        {
            string controlName = "textarea_rect_" + config.Id;
            bool focused = GUI.GetNameOfFocusedControl() == controlName;
            var style = styleManager?.GetTextAreaStyle(config.Variant, config.Size, focused) ?? GUI.skin.textArea;

            Rect rect = config.Rect ?? new Rect(0, 0, 200, 80);
            Rect scaled = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);

            bool wasEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            GUI.SetNextControlName(controlName);
            string value = GUI.TextArea(scaled, config.Value ?? string.Empty, style);

            GUI.enabled = wasEnabled;

            DrawPlaceholderIfNeeded(config, style, focused, scaled);
            return value;
        }

        private void DrawPlaceholderIfNeeded(TextAreaConfig config, GUIStyle style, bool focused, Rect? rectOverride = null)
        {
            if (focused || string.IsNullOrEmpty(config.Placeholder))
                return;

            string value = config.Value ?? string.Empty;
            if (!string.IsNullOrEmpty(value))
                return;

            Rect rect = rectOverride ?? GUILayoutUtility.GetLastRect();
            var placeholder = new UnityHelpers.GUIStyle(style) { normal = { textColor = styleManager?.GetTheme().Muted ?? new Color(0.64f, 0.64f, 0.71f, 1f) } };

            Rect textRect = new Rect(rect.x + style.padding.left, rect.y + style.padding.top, rect.width - style.padding.horizontal, rect.height - style.padding.vertical);

            GUI.Label(textRect, config.Placeholder, placeholder);
        }

        private void DrawCharacterCount(TextAreaConfig config, string value)
        {
            layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();

            string count = config.MaxLength > 0 ? $"{value.Length}/{config.MaxLength}" : $"{value.Length} characters";
            bool nearLimit = config.MaxLength > 0 && value.Length >= config.MaxLength * 0.9f;
            var style = new UnityHelpers.GUIStyle(styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Default) ?? GUI.skin.label);
            if (nearLimit)
                style.normal.textColor = new Color(0.9f, 0.3f, 0.3f);

            UnityHelpers.Label(count, style);
            layoutComponents.EndHorizontalGroup();
        }
    }
}
