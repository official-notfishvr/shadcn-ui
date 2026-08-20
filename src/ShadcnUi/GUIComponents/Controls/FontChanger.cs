using System;
using System.Linq;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class FontChanger : BaseComponent
    {
        private Vector2 _scrollPosition;
        private Rect _triggerRect;
        private float _dropdownWidth;
        private float _dropdownHeight;

        public FontChanger(GUIHelper helper)
            : base(helper) { }

        public void Render(FontChangerConfig config)
        {
            config ??= new FontChangerConfig();
            string id = ResolveId(config.Id, "font_changer");
            _dropdownWidth = config.Width;
            _dropdownHeight = config.DropdownHeight;

            GUIStyle buttonStyle = styleManager?.GetButtonStyle(ControlVariant.Outline, ControlSize.Default, config.Appearance) ?? GUI.skin.button;
            string buttonText = guiHelper.CurrentFontName ?? GUIHelper.DefaultFontName;
            string dropdownIcon = LayerManager.Instance.IsOpen(id) ? " ^" : " v";

            var buttonOptions = config.LayoutOptions ?? new[] { GUILayout.Width(config.Width) };
            if (UnityHelpers.Button(buttonText + dropdownIcon, buttonStyle, buttonOptions))
                ToggleDropdown(config, id);

            if (Event.current.type == EventType.Repaint)
                _triggerRect = GUILayoutUtility.GetLastRect();

            UpdateDropdownPosition(id);
        }

        private void ToggleDropdown(FontChangerConfig config, string id)
        {
            if (LayerManager.Instance.IsOpen(id))
            {
                LayerManager.Instance.Close(id);
                return;
            }

            Vector2 screenPos = PopupLayoutUtility.GetAnchoredScreenPosition(_triggerRect, config.Width, config.DropdownHeight, guiHelper.GetRootGuiScreenRect());
            LayerManager.Instance.Open(
                new LayerConfig
                {
                    Id = id,
                    OpenPosition = screenPos,
                    Width = config.Width,
                    Height = config.DropdownHeight,
                    CloseOnClickOutside = true,
                    ZIndex = DesignTokens.ZIndex.Dropdown,
                    Content = () => DrawFontList(config),
                }
            );
        }

        private void UpdateDropdownPosition(string id)
        {
            if (!LayerManager.Instance.IsOpen(id))
                return;

            Vector2 screenPos = PopupLayoutUtility.GetAnchoredScreenPosition(_triggerRect, _dropdownWidth, _dropdownHeight, guiHelper.GetRootGuiScreenRect());
            LayerManager.Instance.SetPosition(id, screenPos);
        }

        private void DrawFontList(FontChangerConfig config)
        {
            var fonts = guiHelper.GetAvailableFonts().OrderBy(name => name, StringComparer.OrdinalIgnoreCase).ToArray();
            var currentFont = guiHelper.CurrentFontName;

            GUIStyle boxStyle = styleManager?.GetSelectStyle(ControlVariant.Default, ControlSize.Default, config.Appearance) ?? GUI.skin.box;
            GUIStyle itemStyle = styleManager?.GetSelectItemStyle(ControlVariant.Default, ControlSize.Default, config.Appearance) ?? GUI.skin.button;
            GUIStyle previewStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, ControlSize.Small, config.Appearance) ?? GUI.skin.label;

            GUILayout.BeginVertical(boxStyle);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(config.Width - 10), GUILayout.Height(config.DropdownHeight - 20));

            foreach (var fontName in fonts)
                DrawFontItem(fontName, currentFont, config, itemStyle, previewStyle);

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void DrawFontItem(string fontName, string currentFont, FontChangerConfig config, GUIStyle itemStyle, GUIStyle previewStyle)
        {
            bool isSelected = string.Equals(fontName, currentFont, StringComparison.OrdinalIgnoreCase);

            GUILayout.BeginVertical();

            string label = isSelected ? fontName + " *" : fontName;
            if (UnityHelpers.Button(label, itemStyle, GUILayout.ExpandWidth(true)))
            {
                guiHelper.SetFont(fontName);
                guiHelper.GetStyleManager()?.MarkStylesCorruption();
                config.OnFontChanged?.Invoke(fontName);
                LayerManager.Instance.Close(config.Id ?? "font_changer");
                GUILayout.EndVertical();
                return;
            }

            if (config.ShowPreview)
            {
                GUILayout.Label(config.PreviewText ?? "Aa", previewStyle);
            }

            GUILayout.EndVertical();
        }

        private string ResolveId(string id, string fallback)
        {
            if (!string.IsNullOrEmpty(id))
                return id;
            return fallback;
        }
    }
}
