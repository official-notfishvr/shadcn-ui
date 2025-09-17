using shadcnui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUIButtonComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;

        public GUIButtonComponents(GUIHelper helper)
        {
            guiHelper = helper;
            layoutComponents = new GUILayoutComponents(helper);
        }


        public bool Button(string text, ButtonVariant variant = ButtonVariant.Default,
            ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false,
            params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle buttonStyle = styleManager.GetButtonStyle(variant, size);

            List<GUILayoutOption> layoutOptions = new List<GUILayoutOption>(options);

            if (buttonStyle.fixedWidth > 0)
            {
                layoutOptions.Add(GUILayout.Width(buttonStyle.fixedWidth));
            }
            if (buttonStyle.fixedHeight > 0)
            {
                layoutOptions.Add(GUILayout.Height(buttonStyle.fixedHeight));
            }

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool clicked;
#if IL2CPP
            clicked = GUILayout.Button(text ?? "Button", buttonStyle, 
                (Il2CppReferenceArray<GUILayoutOption>)layoutOptions.ToArray());
#else
            clicked = GUILayout.Button(text ?? "Button", buttonStyle, layoutOptions.ToArray());
#endif

            GUI.enabled = wasEnabled;

            if (clicked && !disabled && onClick != null)
                onClick();

            return clicked && !disabled;
        }

        public bool Button(Rect rect, string text, ButtonVariant variant = ButtonVariant.Default,
            ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle buttonStyle = styleManager.GetButtonStyle(variant, size);

            Rect scaledRect = new Rect(
                rect.x * guiHelper.uiScale,
                rect.y * guiHelper.uiScale,
                rect.width * guiHelper.uiScale,
                rect.height * guiHelper.uiScale
            );

            bool wasEnabled = GUI.enabled;
            if (disabled) GUI.enabled = false;

            bool clicked = GUI.Button(scaledRect, text ?? "Button", buttonStyle);

            GUI.enabled = wasEnabled;

            if (clicked && !disabled && onClick != null)
                onClick();

            return clicked && !disabled;
        }

        public void ButtonGroup(Action drawButtons, bool horizontal = true, float spacing = 5f)
        {
            float scaledSpacing = spacing * guiHelper.uiScale;

            if (horizontal)
            {
#if IL2CPP
                layoutComponents.BeginHorizontalGroup(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                layoutComponents.BeginHorizontalGroup();
#endif
            }
            else
            {
#if IL2CPP
                layoutComponents.BeginVerticalGroup(GUIStyle.none, (Il2CppReferenceArray<GUILayoutOption>)null);
#else
                layoutComponents.BeginVerticalGroup();
#endif
            }

            drawButtons?.Invoke();

            if (horizontal)
                layoutComponents.EndHorizontalGroup();
            else
                layoutComponents.EndVerticalGroup();

            layoutComponents.AddSpace(scaledSpacing);
        }

        public void RenderButtonSet(ButtonConfig[] buttons, bool horizontal = true, float spacing = 8f)
        {
            if (buttons == null || buttons.Length == 0) return;

            ButtonGroup(() =>
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    var config = buttons[i];
                    Button(config.Text, config.Variant, config.Size, config.OnClick, config.Disabled, config.Options);

                    if (i < buttons.Length - 1)
                    {
                        if (horizontal)
                            layoutComponents.AddSpace(spacing);
                        else
                            layoutComponents.AddSpace(spacing);
                    }
                }
            }, horizontal, 0f);
        }

        public struct ButtonConfig
        {
            public string Text;
            public ButtonVariant Variant;
            public ButtonSize Size;
            public Action OnClick;
            public bool Disabled;
            public GUILayoutOption[] Options;

            public ButtonConfig(string text, ButtonVariant variant = ButtonVariant.Default,
                ButtonSize size = ButtonSize.Default, Action onClick = null, bool disabled = false,
                params GUILayoutOption[] options)
            {
                Text = text;
                Variant = variant;
                Size = size;
                OnClick = onClick;
                Disabled = disabled;
                Options = options;
            }
        }
    }
}
