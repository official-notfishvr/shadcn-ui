using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Display
{
    public class Dialog : BaseComponent
    {
        private bool _isOpen = false;
        private string _dialogId;
        private const float AnimationDuration = DesignTokens.Animation.DurationNormal;

        public Dialog(GUIHelper helper)
            : base(helper) { }

        public bool IsOpen => _isOpen;

        #region Config-based API
        public void Render(DialogConfig config)
        {
            if (!_isOpen || _dialogId != config.Id)
                return;

            var styleManager = guiHelper.GetStyleManager();
            var animManager = guiHelper.GetAnimationManager();

            float animProgress = animManager.GetFloat($"dialog_alpha_{config.Id}", 1f);

            bool overlayClicked = DrawOverlay(config, animProgress);
            if (overlayClicked && config.CloseOnOverlayClick)
            {
                Close();
                return;
            }

            DrawDialogWindow(config, styleManager, animManager, animProgress);
        }
        #endregion

        #region API
        public void Open(string dialogId)
        {
            _dialogId = dialogId;
            _isOpen = true;
            var animManager = guiHelper.GetAnimationManager();
            animManager.FadeIn($"dialog_alpha_{dialogId}", AnimationDuration, EasingFunctions.EaseOutCubic);
            animManager.ScaleIn($"dialog_scale_{dialogId}", AnimationDuration, 0.95f, EasingFunctions.EaseOutCubic);
        }

        public void Close()
        {
            if (_dialogId != null)
            {
                var animManager = guiHelper.GetAnimationManager();
                animManager.Remove($"dialog_alpha_{_dialogId}");
                animManager.Remove($"dialog_scale_{_dialogId}");
            }
            _isOpen = false;
            _dialogId = null;
        }

        internal void DrawDialog(string dialogId, Action content, float width = 400, float height = 300)
        {
            Render(
                new DialogConfig
                {
                    Id = dialogId,
                    Content = content,
                    Width = width,
                    Height = height,
                }
            );
        }

        internal void DrawDialog(string dialogId, string title, string description, Action content, Action footer = null, float width = 400, float height = 300)
        {
            Render(
                new DialogConfig
                {
                    Id = dialogId,
                    Title = title,
                    Description = description,
                    Content = content,
                    Footer = footer,
                    Width = width,
                    Height = height,
                }
            );
        }

        internal bool DrawDialogTrigger(string label, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            return guiHelper.Button(label, variant, size, null);
        }

        internal void DrawDialogHeader(string title, string description = null)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(title))
                UnityHelpers.Label(title, styleManager.GetCardTitleStyle(ControlVariant.Default, ControlSize.Default, null));
            if (!string.IsNullOrEmpty(description))
                UnityHelpers.Label(description, styleManager.GetCardDescriptionStyle(ControlVariant.Default, ControlSize.Default, null));
            layoutComponents.EndVerticalGroup();
        }

        internal void DrawDialogContent(Action content)
        {
            layoutComponents.BeginVerticalGroup();
            try
            {
                content?.Invoke();
                guiHelper.FlushAutoRenderBuilder();
            }
            finally
            {
                layoutComponents.EndVerticalGroup();
            }
        }

        internal void DrawDialogFooter(Action footer)
        {
            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();
            try
            {
                footer?.Invoke();
                guiHelper.FlushAutoRenderBuilder();
            }
            finally
            {
                layoutComponents.EndHorizontalGroup();
            }
        }
        #endregion

        #region Private Methods
        private bool DrawOverlay(DialogConfig config, float animProgress)
        {
            Color prev = GUI.color;
            Color overlayColor = ThemeManager.Instance.CurrentTheme.Overlay;
            if (animProgress < 1f)
                overlayColor.a *= animProgress;
            GUI.color = overlayColor;

            Rect overlayRect = config.ParentWindowRect.HasValue ? new Rect(0, 0, config.ParentWindowRect.Value.width, config.ParentWindowRect.Value.height) : new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(overlayRect, Texture2D.whiteTexture);
            GUI.color = prev;

            if (config.CloseOnOverlayClick && Event.current.type == EventType.MouseDown)
            {
                Vector2 mousePos = Event.current.mousePosition;
                float dialogX,
                    dialogY;
                GetDialogPosition(config, out dialogX, out dialogY);
                Rect dialogRect = new Rect(dialogX, dialogY, config.Width, config.Height);

                if (!dialogRect.Contains(mousePos))
                {
                    Event.current.Use();
                    return true;
                }
            }

            return false;
        }

        private void GetDialogPosition(DialogConfig config, out float x, out float y)
        {
            if (config.ParentWindowRect.HasValue)
            {
                var parent = config.ParentWindowRect.Value;
                x = (parent.width - config.Width) / 2f;
                y = (parent.height - config.Height) / 2f;
            }
            else
            {
                x = (Screen.width - config.Width) / 2f;
                y = (Screen.height - config.Height) / 2f;
            }
        }

        private void DrawDialogWindow(DialogConfig config, StyleManager styleManager, AnimationManager animManager, float animProgress)
        {
            float dialogX,
                dialogY;
            GetDialogPosition(config, out dialogX, out dialogY);

            Color prevColor = GUI.color;
            Matrix4x4 prevMatrix = GUI.matrix;

            ApplyDialogAnimation(animManager, config, animProgress, dialogX, dialogY, ref prevColor);

            layoutComponents.BeginVerticalGroup(styleManager.GetDialogContentStyle(config.Variant, config.Size, config.Appearance), GUILayout.Width(config.Width), GUILayout.Height(config.Height));
            try
            {
                DrawDialogHeader(config, styleManager);
                GUILayout.Space(DesignTokens.Spacing.LG);
                config.Content?.Invoke();
                guiHelper.FlushAutoRenderBuilder();

                if (config.Footer != null)
                {
                    GUILayout.Space(DesignTokens.Spacing.LG);
                    layoutComponents.BeginHorizontalGroup();
                    GUILayout.FlexibleSpace();
                    try
                    {
                        config.Footer.Invoke();
                        guiHelper.FlushAutoRenderBuilder();
                    }
                    finally
                    {
                        layoutComponents.EndHorizontalGroup();
                    }
                }
            }
            finally
            {
                GUILayout.EndVertical();
                GUI.matrix = prevMatrix;
                GUI.color = prevColor;
            }
        }

        private void ApplyDialogAnimation(AnimationManager animManager, DialogConfig config, float animProgress, float dialogX, float dialogY, ref Color prevColor)
        {
            if (animProgress >= 1f)
                return;

            float scale = animManager.GetFloat($"dialog_scale_{config.Id}", 1f);
            Vector2 dialogCenter = new Vector2(dialogX + config.Width / 2f, dialogY + config.Height / 2f);
            GUI.matrix = Matrix4x4.TRS(new Vector3(dialogCenter.x * (1 - scale), dialogCenter.y * (1 - scale), 0), Quaternion.identity, new Vector3(scale, scale, 1f));
            GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a * animProgress);
        }

        private void DrawDialogHeader(DialogConfig config, StyleManager styleManager)
        {
            layoutComponents.BeginHorizontalGroup();
            layoutComponents.BeginVerticalGroup();

            if (!string.IsNullOrEmpty(config.Title))
                UnityHelpers.Label(config.Title, styleManager.GetCardTitleStyle(ControlVariant.Default, ControlSize.Default, config.Appearance));

            if (!string.IsNullOrEmpty(config.Description))
                UnityHelpers.Label(config.Description, styleManager.GetCardDescriptionStyle(ControlVariant.Default, ControlSize.Default, config.Appearance));

            layoutComponents.EndVerticalGroup();
            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button("×", styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Icon, config.Appearance), GUILayout.Width(DesignTokens.Height.Default * guiHelper.uiScale), GUILayout.Height(DesignTokens.Height.Default * guiHelper.uiScale)))
                Close();

            layoutComponents.EndHorizontalGroup();
        }
        #endregion
    }
}
