using System;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Display
{
    public class Dialog
    {
        private readonly GUIHelper _guiHelper;
        private readonly StyleManager _styleManager;
        private readonly shadcnui.GUIComponents.Layout.Layout _layoutComponents;

        private bool _isOpen = false;
        private string _dialogId;

        public Dialog(GUIHelper helper)
        {
            _guiHelper = helper;
            _styleManager = helper.GetStyleManager();
            _layoutComponents = new shadcnui.GUIComponents.Layout.Layout(helper);
        }

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

            DrawOverlay();

            _layoutComponents.BeginVerticalGroup(_styleManager.dialogContentStyle, GUILayout.Width(width), GUILayout.Height(height));

            _layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button("×", _styleManager.buttonGhostStyle, GUILayout.Width(24), GUILayout.Height(24)))
            {
                Close();
            }
            _layoutComponents.EndHorizontalGroup();

            content?.Invoke();

            GUILayout.EndVertical();
        }

        public void DrawDialog(string dialogId, string title, string description, Action content, Action footer = null, float width = 400, float height = 300)
        {
            if (!_isOpen || _dialogId != dialogId)
                return;

            DrawOverlay();

            _layoutComponents.BeginVerticalGroup(_styleManager.dialogContentStyle, GUILayout.Width(width), GUILayout.Height(height));

            _layoutComponents.BeginHorizontalGroup();
            _layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(title))
            {
                UnityHelpers.Label(title, _styleManager.dialogTitleStyle);
            }
            if (!string.IsNullOrEmpty(description))
            {
                UnityHelpers.Label(description, _styleManager.dialogDescriptionStyle);
            }
            _layoutComponents.EndVerticalGroup();
            GUILayout.FlexibleSpace();
            if (UnityHelpers.Button("×", _styleManager.buttonGhostStyle, GUILayout.Width(24), GUILayout.Height(24)))
            {
                Close();
            }
            _layoutComponents.EndHorizontalGroup();

            GUILayout.Space(16);

            content?.Invoke();

            if (footer != null)
            {
                GUILayout.Space(16);
                _layoutComponents.BeginHorizontalGroup();
                GUILayout.FlexibleSpace();
                footer?.Invoke();
                _layoutComponents.EndHorizontalGroup();
            }

            GUILayout.EndVertical();
        }

        public bool DrawDialogTrigger(string label, ButtonVariant variant = ButtonVariant.Default, ButtonSize size = ButtonSize.Default)
        {
            return _guiHelper.Button(label, variant, size, null);
        }

        public void DrawDialogHeader(string title, string description = null)
        {
            _layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(title))
            {
                UnityHelpers.Label(title, _styleManager.dialogTitleStyle);
            }
            if (!string.IsNullOrEmpty(description))
            {
                UnityHelpers.Label(description, _styleManager.dialogDescriptionStyle);
            }
            _layoutComponents.EndVerticalGroup();
        }

        public void DrawDialogContent(Action content)
        {
            _layoutComponents.BeginVerticalGroup();
            content?.Invoke();
            _layoutComponents.EndVerticalGroup();
        }

        public void DrawDialogFooter(Action footer)
        {
            _layoutComponents.BeginHorizontalGroup();
            GUILayout.FlexibleSpace();
            footer?.Invoke();
            _layoutComponents.EndHorizontalGroup();
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
