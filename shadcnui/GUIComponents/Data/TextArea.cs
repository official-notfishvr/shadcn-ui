using System;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Data
{
    public class TextArea : BaseComponent
    {
        public TextArea(GUIHelper helper)
            : base(helper) { }

        public string DrawTextArea(string text, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            bool focused = GUI.GetNameOfFocusedControl() == "textarea_" + text?.GetHashCode();

            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(variant, focused);

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

            string result;

            result = UnityHelpers.TextArea(text ?? (string.IsNullOrEmpty(placeholder) ? "" : placeholder), textAreaStyle, layoutOptions.ToArray());

            GUI.enabled = wasEnabled;

            if (maxLength > 0 && result.Length > maxLength)
            {
                result = result.Substring(0, maxLength);
            }

            return disabled ? text : result;
        }

        public string DrawTextArea(Rect rect, string text, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, int maxLength = -1)
        {
            var styleManager = guiHelper.GetStyleManager();

            bool focused = GUI.GetNameOfFocusedControl() == "textarea_rect_" + text?.GetHashCode();

            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(variant, focused);

            Rect scaledRect = new Rect(rect.x * guiHelper.uiScale, rect.y * guiHelper.uiScale, rect.width * guiHelper.uiScale, rect.height * guiHelper.uiScale);

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            GUI.SetNextControlName("textarea_rect_" + text?.GetHashCode());

            string result = GUI.TextArea(scaledRect, text ?? (string.IsNullOrEmpty(placeholder) ? "" : placeholder), textAreaStyle);

            GUI.enabled = wasEnabled;

            if (maxLength > 0 && result.Length > maxLength)
            {
                result = result.Substring(0, maxLength);
            }

            return disabled ? text : result;
        }

        public string OutlineTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            return DrawTextArea(text, TextAreaVariant.Outline, placeholder, disabled, minHeight, maxLength, options);
        }

        public string GhostTextArea(string text, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, params GUILayoutOption[] options)
        {
            return DrawTextArea(text, TextAreaVariant.Ghost, placeholder, disabled, minHeight, maxLength, options);
        }

        public string LabeledTextArea(string label, string text, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, int maxLength = -1, bool showCharCount = true, params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (!string.IsNullOrEmpty(label))
            {
                UnityHelpers.Label(label, styleManager.GetLabelStyle(LabelVariant.Default));
                layoutComponents.AddSpace(4);
            }

            string result = DrawTextArea(text, variant, placeholder, disabled, minHeight, maxLength, options);

            if (showCharCount)
            {
                layoutComponents.AddSpace(4);
                layoutComponents.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();

                string countText = maxLength > 0 ? $"{result?.Length ?? 0}/{maxLength}" : $"{result?.Length ?? 0} characters";

                Color countColor = (maxLength > 0 && (result?.Length ?? 0) > maxLength * 0.9f) ? new Color(0.9f, 0.3f, 0.3f) : new Color(0.64f, 0.64f, 0.71f);

                var countStyle = new UnityHelpers.GUIStyle(styleManager.GetLabelStyle(LabelVariant.Muted)) { normal = { textColor = countColor } };

                UnityHelpers.Label(countText, countStyle);

                layoutComponents.EndHorizontalGroup();
            }

            return result;
        }

        public string ResizableTextArea(string text, ref float height, TextAreaVariant variant = TextAreaVariant.Default, string placeholder = "", bool disabled = false, float minHeight = 60f, float maxHeight = 300f, int maxLength = -1, params GUILayoutOption[] options)
        {
            height = Mathf.Clamp(height, minHeight, maxHeight);

            var layoutOptions = new System.Collections.Generic.List<GUILayoutOption>();
            layoutOptions.Add(GUILayout.Height(height * guiHelper.uiScale));
            layoutOptions.Add(GUILayout.ExpandWidth(true));

            if (options != null && options.Length > 0)
                layoutOptions.AddRange(options);

            string result = DrawTextArea(text, variant, placeholder, disabled, height, maxLength, layoutOptions.ToArray());

            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();

            var styleManager = guiHelper.GetStyleManager();

            if (UnityHelpers.Button("⋮⋮⋮", styleManager.GetLabelStyle(LabelVariant.Muted), new GUILayoutOption[] { GUILayout.Width(20 * guiHelper.uiScale), GUILayout.Height(10 * guiHelper.uiScale) }))
            {
                height = height >= maxHeight ? minHeight : height + 20f;
            }

            layoutComponents.EndHorizontalGroup();

            return result;
        }
    }
}
