using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Checkbox : BaseComponent
    {
        public Checkbox(GUIHelper helper)
            : base(helper) { }

        #region Config-based API
        public bool DrawCheckbox(CheckboxConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle checkboxStyle = styleManager?.GetCheckboxStyle(config.Variant, config.Size) ?? GUI.skin.toggle;

            bool wasEnabled = GUI.enabled;
            if (config.Disabled)
                GUI.enabled = false;

            bool newValue = GetCheckboxValue(config, checkboxStyle);

            GUI.enabled = wasEnabled;

            if (newValue != config.Value && !config.Disabled)
                config.OnToggle?.Invoke(newValue);

            return config.Disabled ? config.Value : newValue;
        }
        #endregion

        #region API
        public bool DrawCheckbox(string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false, params GUILayoutOption[] options)
        {
            return DrawCheckbox(
                new CheckboxConfig
                {
                    Text = text,
                    Value = value,
                    Variant = variant,
                    Size = size,
                    OnToggle = onToggle,
                    Disabled = disabled,
                    Options = options,
                }
            );
        }

        public bool DrawCheckbox(Rect rect, string text, bool value, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default, Action<bool> onToggle = null, bool disabled = false)
        {
            return DrawCheckbox(
                new CheckboxConfig
                {
                    Rect = rect,
                    Text = text,
                    Value = value,
                    Variant = variant,
                    Size = size,
                    OnToggle = onToggle,
                    Disabled = disabled,
                }
            );
        }
        #endregion

        #region Private Methods
        private bool GetCheckboxValue(CheckboxConfig config, GUIStyle checkboxStyle)
        {
            bool useExpandWidth = config.Size != ControlSize.Icon;

            if (config.Icon?.Image != null)
                return DrawCheckboxWithIcon(config, checkboxStyle);

            if (config.Rect.HasValue)
                return DrawCheckboxAtRect(config, checkboxStyle);

            return DrawCheckboxLayout(config, checkboxStyle, useExpandWidth);
        }

        private bool DrawCheckboxWithIcon(CheckboxConfig config, GUIStyle checkboxStyle)
        {
            GUILayout.BeginHorizontal();
            RenderIcon(config.Icon);
            layoutComponents.AddSpace(config.Icon.Spacing);
            bool newValue = UnityHelpers.Toggle(config.Value, config.Text ?? "Checkbox", checkboxStyle);
            GUILayout.EndHorizontal();
            return newValue;
        }

        private bool DrawCheckboxAtRect(CheckboxConfig config, GUIStyle checkboxStyle)
        {
            Rect r = config.Rect.Value;
            Rect scaledRect = new Rect(r.x * guiHelper.uiScale, r.y * guiHelper.uiScale, r.width * guiHelper.uiScale, r.height * guiHelper.uiScale);
            return UnityHelpers.Toggle(scaledRect, config.Value, config.Text ?? "Checkbox", checkboxStyle);
        }

        private bool DrawCheckboxLayout(CheckboxConfig config, GUIStyle checkboxStyle, bool useExpandWidth)
        {
            GUILayoutOption[] options = BuildToggleLayoutOptions(config.Options, useExpandWidth);

            if (!config.ShowCheckmark)
            {
                return UnityHelpers.Toggle(config.Value, config.Text ?? "Checkbox", checkboxStyle, options);
            }

            GUILayout.BeginHorizontal(options);

            GUIStyle labelStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
            labelStyle.alignment = TextAnchor.MiddleLeft;
            labelStyle.fontSize = Mathf.RoundToInt(14 * guiHelper.uiScale);
            labelStyle.normal.textColor = checkboxStyle.normal.textColor;
            GUILayout.Label(config.Text ?? "Checkbox", labelStyle, GUILayout.ExpandWidth(true));

            GUILayout.FlexibleSpace();

            float checkboxSize = 20 * guiHelper.uiScale;
            Rect checkboxRect = GUILayoutUtility.GetRect(checkboxSize, checkboxSize, GUILayout.Width(checkboxSize), GUILayout.Height(checkboxSize));

            GUIStyle boxStyle = new UnityHelpers.GUIStyle(GUI.skin.box);

            if (config.ShowCheckmark)
            {
                if (config.Value)
                    boxStyle.normal.background = checkboxStyle.onNormal.background ?? checkboxStyle.normal.background;
                else
                    boxStyle.normal.background = checkboxStyle.normal.background;
            }
            else
            {
                var styleManager = guiHelper.GetStyleManager();
                GUIStyle solidStyle = styleManager?.GetCheckboxSolidStyle(config.Variant, config.Size) ?? checkboxStyle;

                if (config.Value)
                    boxStyle.normal.background = solidStyle.onNormal.background ?? solidStyle.normal.background;
                else
                    boxStyle.normal.background = solidStyle.normal.background;
            }

            GUI.Box(checkboxRect, GUIContent.none, boxStyle);

            if (config.Value && config.ShowCheckmark)
            {
                GUIStyle checkStyle = new UnityHelpers.GUIStyle(GUI.skin.label);
                checkStyle.alignment = TextAnchor.MiddleCenter;
                checkStyle.fontSize = Mathf.RoundToInt(12 * guiHelper.uiScale);
                checkStyle.normal.textColor = checkboxStyle.onNormal.textColor;
                GUI.Label(checkboxRect, "✓", checkStyle);
            }

            if (Event.current.type == EventType.MouseDown && checkboxRect.Contains(Event.current.mousePosition))
            {
                Event.current.Use();
                GUILayout.EndHorizontal();
                return !config.Value;
            }

            GUILayout.EndHorizontal();

            Rect rowRect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.MouseDown && rowRect.Contains(Event.current.mousePosition) && !checkboxRect.Contains(Event.current.mousePosition))
            {
                Event.current.Use();
                return !config.Value;
            }

            return config.Value;
        }

        private static GUILayoutOption[] BuildToggleLayoutOptions(GUILayoutOption[] configOptions, bool expandWidth)
        {
            int extra = expandWidth ? 1 : 0;
            if (configOptions == null || configOptions.Length == 0)
                return expandWidth ? new[] { GUILayout.ExpandWidth(true) } : Array.Empty<GUILayoutOption>();
            var options = new GUILayoutOption[configOptions.Length + extra];
            configOptions.CopyTo(options, 0);
            if (expandWidth)
                options[configOptions.Length] = GUILayout.ExpandWidth(true);
            return options;
        }

        private void RenderIcon(IconConfig iconConfig)
        {
            float scaledSize = iconConfig.Size * guiHelper.uiScale;
            UnityHelpers.Label(iconConfig.Image, GUILayout.Width(scaledSize), GUILayout.Height(scaledSize));
        }
        #endregion
    }
}
