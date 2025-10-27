using System;
using shadcnui.GUIComponents.Core;
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

        public Dialog(GUIHelper helper)
            : base(helper) { }

        public bool IsOpen => _isOpen;

        public void Open(string dialogId)
        {
            _dialogId = dialogId;
            _isOpen = true;
        }

        public void Close()
        {
            _isOpen = false;
            _dialogId = null;
        }

        public void DrawDialog(string dialogId, Action content, float width = 400, float height = 300)
        {
            if (!_isOpen || _dialogId != dialogId)
                return;

            var styleManager = guiHelper.GetStyleManager();
            DrawOverlay();

            layoutComponents.BeginVerticalGroup(styleManager.GetDialogContentStyle(), GUILayout.Width(width), GUILayout.Height(height));

            layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button("Ã—", styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default), GUILayout.Width(24), GUILayout.Height(24)))
            {
                Close();
            }
            layoutComponents.EndHorizontalGroup();

            content?.Invoke();

            GUILayout.EndVertical();
        }

        public void DrawDialog(string dialogId, string title, string description, Action content, Action footer = null, float width = 400, float height = 300)
        {
            if (!_isOpen || _dialogId != dialogId)
                return;

            var styleManager = guiHelper.GetStyleManager();
            DrawOverlay();

            layoutComponents.BeginVerticalGroup(styleManager.GetDialogContentStyle(), GUILayout.Width(width), GUILayout.Height(height));

            layoutComponents.BeginHorizontalGroup();
            layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(title))
            {
                UnityHelpers.Label(title, styleManager.GetDialogTitleStyle());
            }
            if (!string.IsNullOrEmpty(description))
            {
                UnityHelpers.Label(description, styleManager.GetDialogDescriptionStyle());
            }
            layoutComponents.EndVerticalGroup();
            GUILayout.FlexibleSpace();
            if (UnityHelpers.Button("Ã—", styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default), GUILayout.Width(24), GUILayout.Height(24)))
            {
                Close();
            }
            layoutComponents.EndHorizontalGroup();

            GUILayout.Space(16);

            content?.Invoke();

            if (footer != null)
            {
                GUILayout.Space(16);
                layoutComponents.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();
                footer?.Invoke();
                layoutComponents.EndHorizontalGroup();
            }

            GUILayout.EndVertical();
        }

        public bool DrawDialogTrigger(string label, ButtonVariant variant = ButtonVariant.Default, ButtonSize size = ButtonSize.Default)
        {
            return guiHelper.Button(label, variant, size, null);
        }

        public void DrawDialogHeader(string title, string description = null)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(title))
            {
                UnityHelpers.Label(title, styleManager.GetDialogTitleStyle());
            }
            if (!string.IsNullOrEmpty(description))
            {
                UnityHelpers.Label(description, styleManager.GetDialogDescriptionStyle());
            }
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

        private void DrawOverlay()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                Close();
            }
        }
    }
}
