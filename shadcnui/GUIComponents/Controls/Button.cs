using System;
using System.Collections.Generic;
using shadcnui;
using shadcnui.GUIComponents.Core;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class Button
    {
        private readonly GUIHelper _guiHelper;
        private readonly shadcnui.GUIComponents.Layout.Layout _layoutComponents;

        public Button(GUIHelper helper)
        {
            _guiHelper = helper;
            _layoutComponents = new shadcnui.GUIComponents.Layout.Layout(helper);
        }

        public bool Draw(ButtonConfig config)
        {
            var styleManager = _guiHelper.GetStyleManager();
            var buttonStyle = styleManager.GetButtonStyle(config.Variant, config.Size);

            var layoutOptions = new List<GUILayoutOption>(config.Options ?? Array.Empty<GUILayoutOption>());

            if (buttonStyle.fixedWidth > 0)
            {
                layoutOptions.Add(GUILayout.Width(buttonStyle.fixedWidth));
            }
            if (buttonStyle.fixedHeight > 0)
            {
                layoutOptions.Add(GUILayout.Height(buttonStyle.fixedHeight));
            }

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

        public bool Draw(string text, ButtonVariant variant = ButtonVariant.Default, ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false, float opacity = 1f, params GUILayoutOption[] options)
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
            return Draw(config);
        }

        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f)
        {
            var scaledSpacing = spacing * _guiHelper.uiScale;

            if (horizontal)
            {
                _layoutComponents.BeginHorizontalGroup();
            }
            else
            {
                _layoutComponents.BeginVerticalGroup();
            }

            drawButtons?.Invoke();

            if (horizontal)
                _layoutComponents.EndHorizontalGroup();
            else
                _layoutComponents.EndVerticalGroup();

            _layoutComponents.AddSpace(scaledSpacing);
        }

        public void DrawButtonSet(ButtonConfig[] buttons, bool horizontal = true, float spacing = 8f)
        {
            if (buttons == null || buttons.Length == 0)
                return;

            ButtonGroup(
                () =>
                {
                    for (var i = 0; i < buttons.Length; i++)
                    {
                        var config = buttons[i];
                        Draw(config);

                        if (i < buttons.Length - 1)
                        {
                            if (horizontal)
                                _layoutComponents.AddSpace(spacing);
                            else
                                _layoutComponents.AddSpace(spacing);
                        }
                    }
                },
                horizontal,
                0f
            );
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
