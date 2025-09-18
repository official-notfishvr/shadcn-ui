using System;
using shadcnui;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class GUITextAreaComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;

        public GUITextAreaComponents(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new GUILayoutComponents(helper);
        }

        public string TextArea(
            string text,
            TextAreaVariant variant = TextAreaVariant.Default,
            string placeholder = "",
            bool disabled = false,
            float minHeight = 60f,
            int maxLength = -1,
            params GUILayoutOption[] options
        )
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
#if IL2CPP
            result = GUILayout.TextArea(
                text ?? (string.IsNullOrEmpty(placeholder) ? "" : placeholder),
                textAreaStyle,
                (Il2CppReferenceArray<GUILayoutOption>)layoutOptions.ToArray()
            );
#else
            result = GUILayout.TextArea(
                text ?? (string.IsNullOrEmpty(placeholder) ? "" : placeholder),
                textAreaStyle,
                layoutOptions.ToArray()
            );
#endif

            GUI.enabled = wasEnabled;

            if (maxLength > 0 && result.Length > maxLength)
            {
                result = result.Substring(0, maxLength);
            }

            return disabled ? text : result;
        }

        public string TextArea(
            Rect rect,
            string text,
            TextAreaVariant variant = TextAreaVariant.Default,
            string placeholder = "",
            bool disabled = false,
            int maxLength = -1
        )
        {
            var styleManager = guiHelper.GetStyleManager();

            bool focused = GUI.GetNameOfFocusedControl() == "textarea_rect_" + text?.GetHashCode();

            GUIStyle textAreaStyle = styleManager.GetTextAreaStyle(variant, focused);

            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            bool wasEnabled = GUI.enabled;
            if (disabled)
                GUI.enabled = false;

            GUI.SetNextControlName("textarea_rect_" + text?.GetHashCode());

            string result = GUI.TextArea(
                scaledRect,
                text ?? (string.IsNullOrEmpty(placeholder) ? "" : placeholder),
                textAreaStyle
            );

            GUI.enabled = wasEnabled;

            if (maxLength > 0 && result.Length > maxLength)
            {
                result = result.Substring(0, maxLength);
            }

            return disabled ? text : result;
        }

        public string OutlineTextArea(
            string text,
            string placeholder = "",
            bool disabled = false,
            float minHeight = 60f,
            int maxLength = -1,
            params GUILayoutOption[] options
        )
        {
            return TextArea(
                text,
                TextAreaVariant.Outline,
                placeholder,
                disabled,
                minHeight,
                maxLength,
                options
            );
        }

        public string GhostTextArea(
            string text,
            string placeholder = "",
            bool disabled = false,
            float minHeight = 60f,
            int maxLength = -1,
            params GUILayoutOption[] options
        )
        {
            return TextArea(
                text,
                TextAreaVariant.Ghost,
                placeholder,
                disabled,
                minHeight,
                maxLength,
                options
            );
        }

        public string LabeledTextArea(
            string label,
            string text,
            TextAreaVariant variant = TextAreaVariant.Default,
            string placeholder = "",
            bool disabled = false,
            float minHeight = 60f,
            int maxLength = -1,
            bool showCharCount = true,
            params GUILayoutOption[] options
        )
        {
            var styleManager = guiHelper.GetStyleManager();

            if (!string.IsNullOrEmpty(label))
            {
#if IL2CPP
                GUILayout.Label(
                    new GUIContent(label),
                    styleManager.GetLabelStyle(LabelVariant.Default),
                    (Il2CppReferenceArray<GUILayoutOption>)null
                );
#else
                GUILayout.Label(label, styleManager.GetLabelStyle(LabelVariant.Default));
#endif
                layoutComponents.AddSpace(4);
            }

            string result = TextArea(
                text,
                variant,
                placeholder,
                disabled,
                minHeight,
                maxLength,
                options
            );

            if (showCharCount)
            {
                layoutComponents.AddSpace(4);
                layoutComponents.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();

                string countText =
                    maxLength > 0
                        ? $"{result?.Length ?? 0}/{maxLength}"
                        : $"{result?.Length ?? 0} characters";

                Color countColor =
                    (maxLength > 0 && (result?.Length ?? 0) > maxLength * 0.9f)
                        ? new Color(0.9f, 0.3f, 0.3f)
                        : new Color(0.64f, 0.64f, 0.71f);

                var countStyle = new GUIStyle(styleManager.GetLabelStyle(LabelVariant.Muted))
                {
                    normal = { textColor = countColor },
                };

#if IL2CPP
                GUILayout.Label(
                    new GUIContent(countText),
                    countStyle,
                    (Il2CppReferenceArray<GUILayoutOption>)null
                );
#else
                GUILayout.Label(countText, countStyle);
#endif

                layoutComponents.EndHorizontalGroup();
            }

            return result;
        }

        public string ResizableTextArea(
            string text,
            ref float height,
            TextAreaVariant variant = TextAreaVariant.Default,
            string placeholder = "",
            bool disabled = false,
            float minHeight = 60f,
            float maxHeight = 300f,
            int maxLength = -1,
            params GUILayoutOption[] options
        )
        {
            height = Mathf.Clamp(height, minHeight, maxHeight);

            var layoutOptions = new System.Collections.Generic.List<GUILayoutOption>();
            layoutOptions.Add(GUILayout.Height(height * guiHelper.uiScale));
            layoutOptions.Add(GUILayout.ExpandWidth(true));

            if (options != null && options.Length > 0)
                layoutOptions.AddRange(options);

            string result = TextArea(
                text,
                variant,
                placeholder,
                disabled,
                height,
                maxLength,
                layoutOptions.ToArray()
            );

            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();

            var styleManager = guiHelper.GetStyleManager();
#if IL2CPP
            if (
                GUILayout.Button(
                    "⋮⋮⋮",
                    styleManager.GetLabelStyle(LabelVariant.Muted),
                    new Il2CppReferenceArray<GUILayoutOption>(
                        new GUILayoutOption[]
                        {
                            GUILayout.Width(20 * guiHelper.uiScale),
                            GUILayout.Height(10 * guiHelper.uiScale),
                        }
                    )
                )
            )
#else
            if (
                GUILayout.Button(
                    "⋮⋮⋮",
                    styleManager.GetLabelStyle(LabelVariant.Muted),
                    new GUILayoutOption[]
                    {
                        GUILayout.Width(20 * guiHelper.uiScale),
                        GUILayout.Height(10 * guiHelper.uiScale),
                    }
                )
            )
#endif
            {
                height = height >= maxHeight ? minHeight : height + 20f;
            }

            layoutComponents.EndHorizontalGroup();

            return result;
        }
    }
}
