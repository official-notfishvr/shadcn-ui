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

        #region Config-based API
        public string DrawTextArea(TextAreaConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (!string.IsNullOrEmpty(config.Label))
            {
                UnityHelpers.Label(config.Label, styleManager.GetLabelStyle(ControlVariant.Default));
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            string result = config.Rect.HasValue ? DrawTextAreaRect(config) : DrawTextAreaLayout(config);

            DrawCharacterCount(config, styleManager, result);

            return result;
        }
        #endregion

        #region API
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
                    LayoutOptions = options,
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

        public string OutlineTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            return DrawTextArea(
                new TextAreaConfig
                {
                    Value = text,
                    Variant = ControlVariant.Outline,
                    Placeholder = placeholder,
                    IsDisabled = disabled,
                    MinHeight = minHeight,
                    MaxLength = maxLength,
                    ShowCharCount = false,
                    LayoutOptions = options,
                }
            );
        }

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            return DrawTextArea(
                new TextAreaConfig
                {
                    Value = text,
                    Variant = ControlVariant.Ghost,
                    Placeholder = placeholder,
                    IsDisabled = disabled,
                    MinHeight = minHeight,
                    MaxLength = maxLength,
                    ShowCharCount = false,
                    LayoutOptions = options,
                }
            );
        }

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
                    LayoutOptions = options,
                }
            );
        }

        public string ResizableTextArea(string text, ref float height, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, float maxHeight = 300f, int maxLength = -1, params GUILayoutOption[] options)
        {
            height = Mathf.Clamp(height, minHeight, maxHeight);

            var layoutOptions = new List<GUILayoutOption>();
            layoutOptions.Add(GUILayout.Height(height * guiHelper.uiScale));
            layoutOptions.Add(GUILayout.ExpandWidth(true));
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
            var styleManager = guiHelper.GetStyleManager();
            if (UnityHelpers.Button("⋮⋮⋮", styleManager.GetLabelStyle(ControlVariant.Muted), new GUILayoutOption[] { GUILayout.Width(20 * guiHelper.uiScale), GUILayout.Height(10 * guiHelper.uiScale) }))
                height = height >= maxHeight ? minHeight : height + 20f;
            layoutComponents.EndHorizontalGroup();

            return result;
        }
        #endregion

        #region Private Methods
        private string DrawTextAreaLayout(TextAreaConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            string controlName = "textarea_" + config.Id;
            bool focused = GUI.GetNameOfFocusedControl() == controlName;
            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(config.Variant, ControlSize.Default, focused);
            float scaledMinHeight = config.MinHeight * guiHelper.uiScale;

            var layoutOptions = new List<GUILayoutOption>();
            layoutOptions.Add(GUILayout.MinHeight(scaledMinHeight));
            layoutOptions.Add(GUILayout.ExpandWidth(true));

            if (config.LayoutOptions != null && config.LayoutOptions.Length > 0)
                layoutOptions.AddRange(config.LayoutOptions);

            bool wasEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            GUI.SetNextControlName(controlName);
            string result = UnityHelpers.TextArea(config.Value ?? GetPlaceholderText(config), textAreaStyle, layoutOptions.ToArray());

            GUI.enabled = wasEnabled;

            return ClampLength(result, config.MaxLength);
        }

        private string DrawTextAreaRect(TextAreaConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            string controlName = "textarea_rect_" + config.Id;
            bool focused = GUI.GetNameOfFocusedControl() == controlName;
            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(config.Variant, ControlSize.Default, focused);

            Rect scaledRect = new Rect(config.Rect.Value.x * guiHelper.uiScale, config.Rect.Value.y * guiHelper.uiScale, config.Rect.Value.width * guiHelper.uiScale, config.Rect.Value.height * guiHelper.uiScale);

            bool wasEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            GUI.SetNextControlName(controlName);
            string result = GUI.TextArea(scaledRect, config.Value ?? GetPlaceholderText(config), textAreaStyle);

            GUI.enabled = wasEnabled;

            return ClampLength(result, config.MaxLength);
        }

        private void DrawCharacterCount(TextAreaConfig config, StyleManager styleManager, string result)
        {
            if (!config.ShowCharCount)
                return;

            layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();

            string countValue = config.MaxLength > 0 ? $"{result?.Length ?? 0}/{config.MaxLength}" : $"{result?.Length ?? 0} characters";

            bool isNearLimit = config.MaxLength > 0 && (result?.Length ?? 0) > config.MaxLength * 0.9f;
            Color countColor = isNearLimit ? new Color(0.9f, 0.3f, 0.3f) : new Color(0.64f, 0.64f, 0.71f);

            var countStyle = new UnityHelpers.GUIStyle(styleManager.GetLabelStyle(ControlVariant.Muted)) { normal = { textColor = countColor } };
            UnityHelpers.Label(countValue, countStyle);
            layoutComponents.EndHorizontalGroup();
        }

        private string GetPlaceholderText(TextAreaConfig config)
        {
            return string.IsNullOrEmpty(config.Placeholder) ? "" : config.Placeholder;
        }

        private static string ClampLength(string text, int maxLength)
        {
            if (maxLength <= 0 || text == null)
                return text ?? "";
            return text.Length > maxLength ? text.Substring(0, maxLength) : text;
        }
        #endregion
    }
}
