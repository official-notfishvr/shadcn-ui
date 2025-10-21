using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class Button : BaseComponent
    {
        public Button(GUIHelper helper)
            : base(helper) { }

        public bool DrawButton(ButtonConfig config)
        {
            GUIStyle buttonStyle = styleManager?.GetButtonStyle(config.Variant, config.Size) ?? GUI.skin.button;

            var layoutOptions = new List<GUILayoutOption>(config.Options ?? Array.Empty<GUILayoutOption>());

            if (buttonStyle.fixedWidth > 0)
                layoutOptions.Add(GUILayout.Width(buttonStyle.fixedWidth));
            if (buttonStyle.fixedHeight > 0)
                layoutOptions.Add(GUILayout.Height(buttonStyle.fixedHeight));

            var wasEnabled = GUI.enabled;
            if (config.Disabled)
                GUI.enabled = false;

            var originalColor = GUI.color;
            GUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a * config.Opacity);

            var clicked = UnityHelpers.Button(config.Text ?? "Button", buttonStyle, layoutOptions.ToArray());

            GUI.color = originalColor;
            GUI.enabled = wasEnabled;

            if (clicked && !config.Disabled)
                config.OnClick?.Invoke();

            return clicked && !config.Disabled;
        }

        public bool DrawButton(string text, ButtonVariant variant = ButtonVariant.Default, ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options)
        {
            var config = new ButtonConfig(text)
            {
                Variant = variant,
                Size = size,
                OnClick = onClick,
                Disabled = disabled,
                Opacity = opacity,
                Options = options,
            };
            return DrawButton(config);
        }

        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f)
        {
            var scaledSpacing = spacing * guiHelper.uiScale;

            if (horizontal)
                layoutComponents.BeginHorizontalGroup();
            else
                layoutComponents.BeginVerticalGroup();

            drawButtons?.Invoke();

            if (horizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();

            layoutComponents.AddSpace(scaledSpacing);
        }
    }

    public class ButtonConfig
    {
        public string Text { get; set; }
        public ButtonVariant Variant { get; set; }
        public ButtonSize Size { get; set; }
        public Action OnClick { get; set; }
        public bool Disabled { get; set; }
        public float Opacity { get; set; }
        public GUILayoutOption[] Options { get; set; }

        public ButtonConfig(string text)
        {
            Text = text;
            Variant = ButtonVariant.Default;
            Size = ButtonSize.Default;
            OnClick = null;
            Disabled = false;
            Opacity = 1f;
            Options = Array.Empty<GUILayoutOption>();
        }
    }
}
