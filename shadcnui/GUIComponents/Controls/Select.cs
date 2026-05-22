using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class Select : BaseComponent
    {
        private readonly Dictionary<string, Vector2> _scrollPositions = new();
        private readonly Dictionary<string, Rect> _anchorRects = new();
        private readonly Dictionary<string, int> _pendingSelection = new();

        public Select(GUIHelper helper)
            : base(helper) { }

        public int Render(SelectConfig config)
        {
            if (config == null)
                return 0;

            string id = ResolveId(config.Id, config.Label, "select");
            if (_pendingSelection.TryGetValue(id, out int pending))
            {
                _pendingSelection.Remove(id);
                config.SelectedIndex = pending;
            }

            DrawLabel(config);

            GUIStyle buttonStyle = styleManager?.GetButtonStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.button;
            string label = GetSelectedLabel(config) ?? config.Placeholder ?? "Select";
            string arrow = LayerManager.Instance.IsOpen(id) ? " ▲" : " ▼";

            var options = BuildTriggerOptions(config);
            bool clicked = UnityHelpers.Button(label + arrow, buttonStyle, options.ToArray());

            if (Event.current.type == EventType.Repaint)
                _anchorRects[id] = GUILayoutUtility.GetLastRect();

            if (clicked)
            {
                if (LayerManager.Instance.IsOpen(id))
                    Close(id);
                else
                    Open(config, GetAnchorRect(id));
            }

            if (LayerManager.Instance.IsOpen(id) && Event.current.type == EventType.Repaint)
                UpdatePosition(id, config);

            return config.SelectedIndex;
        }

        internal int DrawMenu(SelectConfig config)
        {
            if (config == null)
                return 0;

            string id = ResolveId(config.Id, config.Label, "select");
            return DrawMenuInternal(id, config);
        }

        public void Open(SelectConfig config, Rect anchorRect)
        {
            if (config == null)
                return;

            string id = ResolveId(config.Id, config.Label, "select");
            _anchorRects[id] = anchorRect;

            float width = GetMenuWidth(config, anchorRect);
            float height = GetMenuHeight(config);
            Vector2 screenPos = PopupLayoutUtility.GetAnchoredScreenPosition(anchorRect, width, height, guiHelper.GetRootGuiScreenRect());
            LayerManager.Instance.Open(
                new LayerConfig
                {
                    Id = id,
                    OpenPosition = screenPos,
                    Width = width,
                    Height = height,
                    CloseOnClickOutside = true,
                    ZIndex = DesignTokens.ZIndex.Dropdown,
                    Content = () => DrawMenuInternal(id, config),
                    OnClose = () => ClearState(id),
                }
            );
        }

        public void Close(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;

            LayerManager.Instance.Close(id);
            ClearState(id);
        }

        public bool IsOpen(string id) => LayerManager.Instance.IsOpen(id);

        protected override void OnBeforeDispose()
        {
            foreach (var id in _anchorRects.Keys)
                LayerManager.Instance.Close(id);

            _scrollPositions.Clear();
            _anchorRects.Clear();
            _pendingSelection.Clear();
        }

        private int DrawMenuInternal(string id, SelectConfig config)
        {
            GUIStyle menuStyle = styleManager?.GetSelectStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.box;
            GUIStyle itemStyle = styleManager?.GetSelectItemStyle(ControlVariant.Default, config.Size, config.Appearance) ?? GUI.skin.button;

            float width = GetMenuWidth(config, GetAnchorRect(id));
            float height = GetMenuHeight(config);

            layoutComponents.BeginVerticalGroup(menuStyle, GUILayout.Width(width), GUILayout.MaxHeight(height));

            Vector2 scroll = _scrollPositions.TryGetValue(id, out var pos) ? pos : Vector2.zero;
            scroll = layoutComponents.DrawScrollView(scroll, () => DrawItems(id, config, itemStyle), GUILayout.ExpandWidth(true), GUILayout.MinHeight(0), GUILayout.MaxHeight(height));
            _scrollPositions[id] = scroll;

            layoutComponents.EndVerticalGroup();

            return config.SelectedIndex;
        }

        private void DrawItems(string id, SelectConfig config, GUIStyle itemStyle)
        {
            if (config.Options == null || config.Options.Length == 0)
            {
                GUIStyle muted = styleManager?.GetLabelStyle(ControlVariant.Muted, config.Size, config.Appearance) ?? GUI.skin.label;
                UnityHelpers.Label("No options", muted);
                return;
            }

            for (int i = 0; i < config.Options.Length; i++)
            {
                var opt = config.Options[i];
                bool prevEnabled = GUI.enabled;
                if (opt != null && opt.IsDisabled)
                    GUI.enabled = false;

                string text = opt?.Label ?? string.Empty;
                if (UnityHelpers.Button(text, itemStyle))
                {
                    _pendingSelection[id] = i;
                    config.OnSelectionChanged?.Invoke(i);
                    if (config.CloseOnSelect)
                        Close(id);
                }

                GUI.enabled = prevEnabled;
            }
        }

        private void DrawLabel(SelectConfig config)
        {
            if (string.IsNullOrEmpty(config.Label))
                return;

            GUIStyle labelStyle = styleManager?.GetLabelStyle(config.LabelVariant, config.Size, GetTextOnlyAppearance(config.Appearance)) ?? GUI.skin.label;
            UnityHelpers.Label(config.Label, labelStyle);
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
        }

        private List<GUILayoutOption> BuildTriggerOptions(SelectConfig config)
        {
            float width = config.Width > 0 ? config.Width * guiHelper.uiScale : 0f;
            return ControlLayoutUtility.BuildLayoutOptions(config.LayoutOptions, width, expandWidth: width <= 0f);
        }

        private Rect GetAnchorRect(string id)
        {
            return _anchorRects.TryGetValue(id, out var rect) ? rect : new Rect(0, 0, 240, 30);
        }

        private float GetMenuWidth(SelectConfig config, Rect anchor)
        {
            if (config.Width > 0)
                return config.Width * guiHelper.uiScale;
            return Mathf.Max(anchor.width, 200f * guiHelper.uiScale);
        }

        private float GetMenuHeight(SelectConfig config)
        {
            return Mathf.Max(120f * guiHelper.uiScale, config.MaxHeight * guiHelper.uiScale);
        }

        private void UpdatePosition(string id, SelectConfig config)
        {
            Rect anchor = GetAnchorRect(id);
            float width = GetMenuWidth(config, anchor);
            float height = GetMenuHeight(config);
            Vector2 screenPos = PopupLayoutUtility.GetAnchoredScreenPosition(anchor, width, height, guiHelper.GetRootGuiScreenRect());
            LayerManager.Instance.SetPosition(id, screenPos);
        }

        private string GetSelectedLabel(SelectConfig config)
        {
            if (config.Options == null || config.Options.Length == 0)
                return null;

            if (config.SelectedIndex < 0 || config.SelectedIndex >= config.Options.Length)
                return null;

            return config.Options[config.SelectedIndex]?.Label;
        }

        private string ResolveId(string id, string label, string fallback)
        {
            if (!string.IsNullOrEmpty(id))
                return id;
            if (!string.IsNullOrEmpty(label))
                return label;
            return fallback;
        }

        private void ClearState(string id)
        {
            _scrollPositions.Remove(id);
            _anchorRects.Remove(id);
            _pendingSelection.Remove(id);
        }

        private static ComponentAppearance GetTextOnlyAppearance(ComponentAppearance appearance)
        {
            if (appearance?.ForegroundColor == null)
                return null;

            return new ComponentAppearance { ForegroundColor = appearance.ForegroundColor };
        }
    }
}
