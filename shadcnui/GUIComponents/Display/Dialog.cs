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
        private static List<DialogState> _openDialogs = new List<DialogState>();

        private class DialogState
        {
            public string Id;
            public int ZIndex;
            public DialogConfig Config;
        }

        public Dialog(GUIHelper helper)
            : base(helper) { }

        public bool IsOpen => _isOpen;

        #region Config-based API
        public void DrawDialog(DialogConfig config)
        {
            if (!_isOpen || _dialogId != config.Id)
                return;

            RegisterDialog(config);
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
                _openDialogs.RemoveAll(d => d.Id == _dialogId);
                var animManager = guiHelper.GetAnimationManager();
                animManager.Remove($"dialog_alpha_{_dialogId}");
                animManager.Remove($"dialog_scale_{_dialogId}");
            }
            _isOpen = false;
            _dialogId = null;
        }

        public void DrawDialog(string dialogId, Action content, float width = 400, float height = 300)
        {
            DrawDialog(
                new DialogConfig
                {
                    Id = dialogId,
                    Content = content,
                    Width = width,
                    Height = height,
                }
            );
        }

        public void DrawDialog(string dialogId, string title, string description, Action content, Action footer = null, float width = 400, float height = 300)
        {
            DrawDialog(
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

        public bool DrawDialogTrigger(string label, ControlVariant variant = ControlVariant.Default, ControlSize size = ControlSize.Default)
        {
            return guiHelper.Button(label, variant, size, null);
        }

        public void DrawDialogHeader(string title, string description = null)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(title))
                UnityHelpers.Label(title, styleManager.GetLabelStyle(ControlVariant.Default, ControlSize.Large));
            if (!string.IsNullOrEmpty(description))
                UnityHelpers.Label(description, styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Default));
            layoutComponents.EndVerticalGroup();
        }

        public void DrawDialogContent(Action content)
        {
            layoutComponents.BeginVerticalGroup();
            content?.Invoke();
            layoutComponents.EndVerticalGroup();
        }

        public void DrawDialogFooter(Action footer)
        {
            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();
            footer?.Invoke();
            layoutComponents.EndHorizontalGroup();
        }
        #endregion

        #region Private Methods
        private void RegisterDialog(DialogConfig config)
        {
            _openDialogs.RemoveAll(d => d.Id == config.Id);
            _openDialogs.Add(
                new DialogState
                {
                    Id = config.Id,
                    ZIndex = config.ZIndex,
                    Config = config,
                }
            );
            _openDialogs.Sort((a, b) => a.ZIndex.CompareTo(b.ZIndex));
        }

        private bool DrawOverlay(DialogConfig config, float animProgress)
        {
            Color prev = GUI.color;
            Color overlayColor = ThemeManager.Instance.CurrentTheme.Overlay;
            if (animProgress < 1f)
                overlayColor.a *= animProgress;
            GUI.color = overlayColor;
            Rect overlayRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(overlayRect, Texture2D.whiteTexture);
            GUI.color = prev;

            if (config.CloseOnOverlayClick && Event.current.type == EventType.MouseDown)
            {
                Vector2 mousePos = Event.current.mousePosition;
                float dialogX = (Screen.width - config.Width) / 2f;
                float dialogY = (Screen.height - config.Height) / 2f;
                Rect dialogRect = new Rect(dialogX, dialogY, config.Width, config.Height);

                if (!dialogRect.Contains(mousePos))
                {
                    Event.current.Use();
                    return true;
                }
            }

            return false;
        }

        private void DrawDialogWindow(DialogConfig config, StyleManager styleManager, AnimationManager animManager, float animProgress)
        {
            float dialogX = (Screen.width - config.Width) / 2f;
            float dialogY = (Screen.height - config.Height) / 2f;

            Color prevColor = GUI.color;
            Matrix4x4 prevMatrix = GUI.matrix;

            ApplyDialogAnimation(animManager, config, animProgress, dialogX, dialogY, ref prevColor);

            layoutComponents.BeginVerticalGroup(styleManager.GetDialogContentStyle(), GUILayout.Width(config.Width), GUILayout.Height(config.Height));

            DrawDialogHeader(config, styleManager);
            GUILayout.Space(DesignTokens.Spacing.LG);
            config.Content?.Invoke();

            if (config.Footer != null)
            {
                GUILayout.Space(DesignTokens.Spacing.LG);
                layoutComponents.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();
                config.Footer.Invoke();
                layoutComponents.EndHorizontalGroup();
            }

            GUILayout.EndVertical();

            GUI.matrix = prevMatrix;
            GUI.color = prevColor;
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
                UnityHelpers.Label(config.Title, styleManager.GetLabelStyle(ControlVariant.Default, ControlSize.Large));

            if (!string.IsNullOrEmpty(config.Description))
                UnityHelpers.Label(config.Description, styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Default));

            layoutComponents.EndVerticalGroup();
            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button("×", styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Default), GUILayout.Width(DesignTokens.Icon.Large), GUILayout.Height(DesignTokens.Icon.Large)))
                Close();

            layoutComponents.EndHorizontalGroup();
        }
        #endregion
    }
}
