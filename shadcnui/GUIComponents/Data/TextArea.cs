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

            string result;
            if (config.Rect.HasValue)
            {
                result = DrawTextAreaRect(config.Rect.Value, config.Text, config.Variant, config.Placeholder, config.Disabled, config.MaxLength);
            }
            else
            {
                result = DrawTextAreaLayout(config.Text, config.Variant, config.Placeholder, config.Disabled, config.MinHeight, config.MaxLength, config.Options);
            }

            if (config.ShowCharCount)
            {
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
                layoutComponents.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();
                string countText = config.MaxLength > 0 ? $"{result?.Length ?? 0}/{config.MaxLength}" : $"{result?.Length ?? 0} characters";
                Color countColor = (config.MaxLength > 0 && (result?.Length ?? 0) > config.MaxLength * 0.9f) ? new Color(0.9f, 0.3f, 0.3f) : new Color(0.64f, 0.64f, 0.71f);
                var countStyle = new UnityHelpers.GUIStyle(styleManager.GetLabelStyle(ControlVariant.Muted)) { normal = { textColor = countColor } };
                UnityHelpers.Label(countText, countStyle);
                layoutComponents.EndHorizontalGroup();
            }

            return result;
        }
        #endregion

        #region Internal Drawing
        private string DrawTextAreaLayout(string text, ControlVariant variant, string placeholder, bool disabled, float minHeight, int maxLength, GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            bool focused = GUI.GetNameOfFocusedControl() == "textarea_" + text?.GetHashCode();
            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(variant, ControlSize.Default, focused);
            float scaledMinHeight = minHeight * guiHelper.uiScale;

            var layoutOptions = new System.Collections.Generic.List<GUILayoutOption>();
            layoutOptions.Add(GUILayout.MinHeight(scaledMinHeight));
            layoutOptions.Add(GUILayout.ExpandWidth(true));

            if (options != null && options.Length > 0)
                layoutOptions.AddRange(options);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            GUI.SetNextControlName("textarea_" + text?.GetHashCode());
            string result = UnityHelpers.TextArea(text ?? (string.IsNullOrEmpty(placeholder) ? "" : placeholder), textAreaStyle, layoutOptions.ToArray());

            GUI.enabled = wasEnabled;

            if (maxLength > 0 && result.Length > maxLength)
                result = result.Substring(0, maxLength);

            return disabled ? text : result;
        }

        private string DrawTextAreaRect(Rect rect, string text, ControlVariant variant, string placeholder, bool disabled, int maxLength)
        {
            var styleManager = guiHelper.GetStyleManager();
            bool focused = GUI.GetNameOfFocusedControl() == "textarea_rect_" + text?.GetHashCode();
            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(variant, ControlSize.Default, focused);
            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            GUI.SetNextControlName("textarea_rect_" + text?.GetHashCode());
            string result = GUI.TextArea(scaledRect, text ?? (string.IsNullOrEmpty(placeholder) ? "" : placeholder), textAreaStyle);

            GUI.enabled = wasEnabled;

            if (maxLength > 0 && result.Length > maxLength)
                result = result.Substring(0, maxLength);

            return disabled ? text : result;
        }
        #endregion

        #region API
        public string DrawTextArea(string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            return DrawTextArea(
                new TextAreaConfig
                {
                    Text = text,
                    Variant = variant,
                    Placeholder = placeholder,
                    Disabled = disabled,
                    MinHeight = minHeight,
                    MaxLength = maxLength,
                    ShowCharCount = false,
                    Options = options,
                }
            );
        }

        public string DrawTextArea(Rect rect, string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, int maxLength = -1)
        {
            return DrawTextArea(
                new TextAreaConfig
                {
                    Text = text,
                    Variant = variant,
                    Placeholder = placeholder,
                    Disabled = disabled,
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
                    Text = text,
                    Variant = ControlVariant.Outline,
                    Placeholder = placeholder,
                    Disabled = disabled,
                    MinHeight = minHeight,
                    MaxLength = maxLength,
                    ShowCharCount = false,
                    Options = options,
                }
            );
        }

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            return DrawTextArea(
                new TextAreaConfig
                {
                    Text = text,
                    Variant = ControlVariant.Ghost,
                    Placeholder = placeholder,
                    Disabled = disabled,
                    MinHeight = minHeight,
                    MaxLength = maxLength,
                    ShowCharCount = false,
                    Options = options,
                }
            );
        }

        public string LabeledTextArea(string label, string text, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, bool showCharCount = true, params GUILayoutOption[] options)
        {
            return DrawTextArea(
                new TextAreaConfig
                {
                    Text = text,
                    Label = label,
                    Variant = variant,
                    Placeholder = placeholder,
                    Disabled = disabled,
                    MinHeight = minHeight,
                    MaxLength = maxLength,
                    ShowCharCount = showCharCount,
                    Options = options,
                }
            );
        }

        public string ResizableTextArea(string text, ref float height, ControlVariant variant = ControlVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, float maxHeight = 300f, int maxLength = -1, params GUILayoutOption[] options)
        {
            height = Mathf.Clamp(height, minHeight, maxHeight);

            var layoutOptions = new System.Collections.Generic.List<GUILayoutOption>();
            layoutOptions.Add(GUILayout.Height(height * guiHelper.uiScale));
            layoutOptions.Add(GUILayout.ExpandWidth(true));
            if (options != null && options.Length > 0)
                layoutOptions.AddRange(options);

            string result = DrawTextArea(
                new TextAreaConfig
                {
                    Text = text,
                    Variant = variant,
                    Placeholder = placeholder,
                    Disabled = disabled,
                    MinHeight = height,
                    MaxLength = maxLength,
                    ShowCharCount = false,
                    Options = layoutOptions.ToArray(),
                }
            );

            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();
            var styleManager = guiHelper.GetStyleManager();
            if (UnityHelpers.Button("⋮⋮⋮", styleManager.GetLabelStyle(ControlVariant.Muted), new GUILayoutOption[] { GUILayout.Width(20 * guiHelper.uiScale), GUILayout.Height(10 * guiHelper.uiScale) }))
            {
                height = height >= maxHeight ? minHeight : height + 20f;
            }
            layoutComponents.EndHorizontalGroup();

            return result;
        }
        #endregion
    }
}
