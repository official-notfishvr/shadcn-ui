using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Input : BaseComponent
    {
        private readonly HashSet<string> _autoFocused = new();

        public Input(GUIHelper helper)
            : base(helper) { }

        public string Draw(InputConfig config)
        {
            if (config == null)
                return string.Empty;

            string id = ResolveId(config.Id, config.Label, config.Placeholder);
            string controlName = "input_" + id;

            if (!string.IsNullOrEmpty(config.Label))
            {
                DrawLabel(config);
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            bool focused = GUI.GetNameOfFocusedControl() == controlName;
            if (config.AutoFocus && !_autoFocused.Contains(id))
            {
                GUI.FocusControl(controlName);
                _autoFocused.Add(id);
                focused = true;
            }

            GUIStyle inputStyle = styleManager?.GetInputStyle(config.Variant, config.Size, focused, config.IsDisabled, config.Appearance) ?? GUI.skin.textField;

            bool prevEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            GUI.SetNextControlName(controlName);

            string value = DrawInputField(config, inputStyle, controlName);

            GUI.enabled = prevEnabled;

            DrawPlaceholderIfNeeded(config, inputStyle, focused, value);
            DrawHelperOrError(config);

            if (!config.IsDisabled && value != config.Value)
                config.OnValueChanged?.Invoke(value);

            return config.IsDisabled ? (config.Value ?? string.Empty) : value;
        }

        private string DrawInputField(InputConfig config, GUIStyle inputStyle, string controlName)
        {
            if (config.Icon?.Image != null && (config.Icon.Position == IconPosition.Above || config.Icon.Position == IconPosition.Below))
            {
                layoutComponents.BeginVerticalGroup();
                if (config.Icon.Position == IconPosition.Above)
                {
                    RenderIcon(config.Icon);
                    layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                }

                string val = DrawField(config, inputStyle, controlName);

                if (config.Icon.Position == IconPosition.Below)
                {
                    layoutComponents.AddSpace(config.Icon.Spacing * guiHelper.uiScale);
                    RenderIcon(config.Icon);
                }
                layoutComponents.EndVerticalGroup();
                return val;
            }

            GUIStyle styledInput = new UnityHelpers.GUIStyle(inputStyle);
            if (config.Icon?.Image != null)
            {
                float iconSize = config.Icon.Size * guiHelper.uiScale;
                float spacing = config.Icon.Spacing * guiHelper.uiScale;
                int paddingAddition = Mathf.RoundToInt(iconSize + spacing);

                if (config.Icon.Position == IconPosition.Left)
                    styledInput.padding.left += paddingAddition;
                else if (config.Icon.Position == IconPosition.Right)
                    styledInput.padding.right += paddingAddition;
            }

            string value = DrawField(config, styledInput, controlName);

            if (config.Icon?.Image != null && Event.current.type == EventType.Repaint)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                float iconSize = config.Icon.Size * guiHelper.uiScale;
                float y = rect.y + (rect.height - iconSize) / 2f;

                if (config.Icon.Position == IconPosition.Left)
                {
                    Rect iconRect = new Rect(rect.x + DesignTokens.Spacing.XS * guiHelper.uiScale, y, iconSize, iconSize);
                    RenderIcon(config.Icon, iconRect);
                }
                else if (config.Icon.Position == IconPosition.Right)
                {
                    Rect iconRect = new Rect(rect.x + rect.width - iconSize - DesignTokens.Spacing.XS * guiHelper.uiScale, y, iconSize, iconSize);
                    RenderIcon(config.Icon, iconRect);
                }
            }

            return value;
        }

        private string DrawField(InputConfig config, GUIStyle style, string controlName)
        {
            var options = BuildLayoutOptions(config);
            string current = config.Value ?? string.Empty;

            if (config.InputKind == InputKind.Password)
            {
                return UnityHelpers.PasswordField(current, config.MaskCharacter, style, options.ToArray());
            }

            if (config.MaxLength > 0)
                return UnityHelpers.TextField(current, config.MaxLength, style, options.ToArray());

            return UnityHelpers.TextField(current, style, options.ToArray());
        }

        private List<GUILayoutOption> BuildLayoutOptions(InputConfig config)
        {
            var options = new List<GUILayoutOption>(config.LayoutOptions ?? Array.Empty<GUILayoutOption>());

            float height = config.Height > 0 ? config.Height : DesignTokens.Height.Default;
            options.Add(GUILayout.Height(height * guiHelper.uiScale));

            if (config.Width > 0)
                options.Add(GUILayout.Width(config.Width * guiHelper.uiScale));
            else
                options.Add(GUILayout.ExpandWidth(true));

            return options;
        }

        private void DrawLabel(InputConfig config)
        {
            GUIStyle labelStyle = styleManager?.GetLabelStyle(config.LabelVariant, config.Size, config.Appearance) ?? GUI.skin.label;
            UnityHelpers.Label(config.Label ?? string.Empty, labelStyle);
        }

        private void DrawHelperOrError(InputConfig config)
        {
            if (!string.IsNullOrEmpty(config.ErrorText))
            {
                layoutComponents.AddSpace(DesignTokens.Spacing.XXS);
                GUIStyle errorStyle = styleManager?.GetLabelStyle(ControlVariant.Destructive, config.Size) ?? GUI.skin.label;
                UnityHelpers.Label(config.ErrorText, errorStyle);
                return;
            }

            if (!string.IsNullOrEmpty(config.HelperText))
            {
                layoutComponents.AddSpace(DesignTokens.Spacing.XXS);
                GUIStyle helperStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, config.Size) ?? GUI.skin.label;
                UnityHelpers.Label(config.HelperText, helperStyle);
            }
        }

        private void DrawPlaceholderIfNeeded(InputConfig config, GUIStyle inputStyle, bool focused, string value)
        {
            if (focused || !string.IsNullOrEmpty(value) || string.IsNullOrEmpty(config.Placeholder))
                return;

            Rect rect = GUILayoutUtility.GetLastRect();
            var placeholderStyle = new UnityHelpers.GUIStyle(inputStyle);
            placeholderStyle.normal.textColor = styleManager?.GetTheme().Muted ?? new Color(0.64f, 0.64f, 0.71f, 1f);

            Rect textRect = new Rect(rect.x + inputStyle.padding.left, rect.y + inputStyle.padding.top, rect.width - inputStyle.padding.horizontal, rect.height - inputStyle.padding.vertical);

            GUI.Label(textRect, config.Placeholder, placeholderStyle);
        }

        private string ResolveId(string id, string label, string placeholder)
        {
            if (!string.IsNullOrEmpty(id))
                return id;
            if (!string.IsNullOrEmpty(label))
                return label;
            if (!string.IsNullOrEmpty(placeholder))
                return placeholder;
            return Guid.NewGuid().ToString("N");
        }

        private void RenderIcon(IconConfig iconConfig)
        {
            if (iconConfig?.Image == null)
                return;

            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }

        private void RenderIcon(IconConfig iconConfig, Rect rect)
        {
            if (iconConfig?.Image == null)
                return;

            GUI.DrawTexture(rect, iconConfig.Image, ScaleMode.ScaleToFit);
        }
    }
}
