using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class DropdownMenu : BaseComponent
    {
        private readonly Dictionary<string, Vector2> _scrollPositions = new();
        private readonly Dictionary<string, Rect> _anchorRects = new();
        private readonly Dictionary<string, Stack<List<DropdownMenuItem>>> _menuStacks = new();
        private readonly Dictionary<string, bool> _inlineOpen = new();

        public DropdownMenu(GUIHelper helper)
            : base(helper) { }

        public void Draw(DropdownMenuConfig config)
        {
            if (config == null)
                return;

            string id = ResolveId(config.Id, "dropdown");

            if (config.Trigger == null && !config.AnchorRect.HasValue)
            {
                if (config.Items == null || config.Items.Count == 0)
                {
                    CloseInline(id);
                    return;
                }

                if (!IsInlineOpen(id))
                    OpenInline(id, config);

                DrawInline(id, config);
                return;
            }

            if (config.Trigger != null)
            {
                bool clicked = config.Trigger();
                if (Event.current.type == EventType.Repaint)
                    _anchorRects[id] = GUILayoutUtility.GetLastRect();

                if (clicked)
                {
                    if (IsOpen(id))
                        Close(id);
                    else
                        Open(config, GetAnchorRect(id));
                }
            }

            if (config.AnchorRect.HasValue)
                _anchorRects[id] = config.AnchorRect.Value;

            if (IsOpen(id) && Event.current.type == EventType.Repaint)
                UpdatePosition(id, config);
        }

        public void Open(DropdownMenuConfig config, Rect anchorRect)
        {
            if (config == null)
                return;

            string id = ResolveId(config.Id, "dropdown");
            _anchorRects[id] = anchorRect;
            _menuStacks[id] = BuildStack(config.Items);

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
                    CloseOnClickOutside = config.CloseOnClickOutside,
                    ZIndex = config.ZIndex,
                    Content = () => DrawMenuInternal(id, config),
                }
            );
        }

        public void Close(string id)
        {
            LayerManager.Instance.Close(id);
            CloseInline(id);
            _menuStacks.Remove(id);
        }

        public bool IsOpen(string id) => LayerManager.Instance.IsOpen(id) || IsInlineOpen(id);

        private void DrawMenuInternal(string id, DropdownMenuConfig config)
        {
            var menuStyle = styleManager?.GetDropdownMenuStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.box;
            var itemStyle = styleManager?.GetDropdownMenuItemStyle(ControlVariant.Default, config.Size, config.Appearance) ?? GUI.skin.button;
            var separatorStyle = styleManager?.GetSeparatorStyle(SeparatorOrientation.Horizontal, ControlVariant.Default, config.Size, config.Appearance) ?? GUI.skin.box;
            var headerStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, config.Size, config.Appearance) ?? GUI.skin.label;

            float width = GetMenuWidth(config, GetAnchorRect(id));
            float height = GetMenuHeight(config);

            layoutComponents.BeginVerticalGroup(menuStyle, GUILayout.Width(width), GUILayout.MaxHeight(height));

            Vector2 scroll = _scrollPositions.TryGetValue(id, out var pos) ? pos : Vector2.zero;
            scroll = layoutComponents.DrawScrollView(scroll, () => DrawItems(id, config, itemStyle, separatorStyle, headerStyle), GUILayout.ExpandWidth(true), GUILayout.MinHeight(0), GUILayout.MaxHeight(height));
            _scrollPositions[id] = scroll;

            layoutComponents.EndVerticalGroup();
        }

        private void DrawInline(string id, DropdownMenuConfig config)
        {
            var menuStyle = styleManager?.GetDropdownMenuStyle(config.Variant, config.Size, config.Appearance) ?? GUI.skin.box;
            var itemStyle = styleManager?.GetDropdownMenuItemStyle(ControlVariant.Default, config.Size, config.Appearance) ?? GUI.skin.button;
            var separatorStyle = styleManager?.GetSeparatorStyle(SeparatorOrientation.Horizontal, ControlVariant.Default, config.Size, config.Appearance) ?? GUI.skin.box;
            var headerStyle = styleManager?.GetLabelStyle(ControlVariant.Muted, config.Size, config.Appearance) ?? GUI.skin.label;

            float width = GetMenuWidth(config, GetAnchorRect(id));
            float height = GetMenuHeight(config);

            layoutComponents.BeginVerticalGroup(menuStyle, GUILayout.Width(width), GUILayout.MaxHeight(height));

            Vector2 scroll = _scrollPositions.TryGetValue(id, out var pos) ? pos : Vector2.zero;
            scroll = layoutComponents.DrawScrollView(scroll, () => DrawItems(id, config, itemStyle, separatorStyle, headerStyle), GUILayout.ExpandWidth(true), GUILayout.MinHeight(0), GUILayout.MaxHeight(height));
            _scrollPositions[id] = scroll;

            layoutComponents.EndVerticalGroup();
        }

        private void DrawItems(string id, DropdownMenuConfig config, GUIStyle itemStyle, GUIStyle separatorStyle, GUIStyle headerStyle)
        {
            if (!_menuStacks.TryGetValue(id, out var stack) || stack.Count == 0)
                return;

            if (stack.Count > 1)
            {
                if (DrawMenuRow(new DropdownMenuItem(DropdownMenuItemType.Item, "Back"), itemStyle, true))
                {
                    stack.Pop();
                    return;
                }
                UnityHelpers.Box(string.Empty, separatorStyle, GUILayout.Height(1f * guiHelper.uiScale), GUILayout.ExpandWidth(true));
            }

            var items = stack.Peek();
            foreach (var item in items)
            {
                switch (item.Type)
                {
                    case DropdownMenuItemType.Header:
                        UnityHelpers.Label(item.Text ?? string.Empty, headerStyle);
                        break;
                    case DropdownMenuItemType.Separator:
                        UnityHelpers.Box(string.Empty, separatorStyle, GUILayout.Height(1f * guiHelper.uiScale), GUILayout.ExpandWidth(true));
                        break;
                    case DropdownMenuItemType.Item:
                        DrawMenuItem(id, config, item, itemStyle, stack);
                        break;
                }
            }
        }

        private void DrawMenuItem(string id, DropdownMenuConfig config, DropdownMenuItem item, GUIStyle itemStyle, Stack<List<DropdownMenuItem>> stack)
        {
            bool prevEnabled = GUI.enabled;
            if (item.IsDisabled)
                GUI.enabled = false;

            bool hasChildren = item.SubItems != null && item.SubItems.Count > 0;
            if (DrawMenuRow(item, itemStyle, false, hasChildren))
            {
                if (hasChildren)
                {
                    stack.Push(item.SubItems);
                }
                else
                {
                    item.OnClick?.Invoke();
                    if (config.CloseOnSelect)
                        Close(id);
                }
            }

            GUI.enabled = prevEnabled;
        }

        private bool DrawMenuRow(DropdownMenuItem item, GUIStyle itemStyle, bool isBack = false, bool hasChildren = false)
        {
            string text = item?.Text ?? string.Empty;
            Rect rect = ControlLayoutUtility.ReserveRect(new UnityHelpers.GUIContent(text), itemStyle, ControlLayoutUtility.BuildLayoutOptions(null, fixedHeight: DesignTokens.Height.Default * guiHelper.uiScale, expandWidth: true));
            bool clicked = GUI.Button(rect, string.Empty, itemStyle);

            float contentX = rect.x + itemStyle.padding.left;
            if (item?.Icon != null)
            {
                float iconSize = 14f * guiHelper.uiScale;
                var iconRect = new Rect(contentX, rect.y + (rect.height - iconSize) * 0.5f, iconSize, iconSize);
                GUI.DrawTexture(iconRect, item.Icon, ScaleMode.ScaleToFit);
                contentX += iconSize + DesignTokens.Spacing.SM * guiHelper.uiScale;
            }

            string indicator =
                isBack ? "‹"
                : hasChildren ? "›"
                : string.Empty;
            var textStyle = ContentRenderUtility.CreateOverlayLabelStyle(itemStyle, TextAnchor.MiddleLeft);
            textStyle.normal.textColor = styleManager.GetTheme().Text;
            ContentRenderUtility.DrawTextWithTrailing(new Rect(contentX, rect.y, Mathf.Max(0f, rect.width - (contentX - rect.x)), rect.height), text, textStyle, indicator, styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Small), 12f * guiHelper.uiScale, 16f * guiHelper.uiScale);

            return clicked;
        }

        private Stack<List<DropdownMenuItem>> BuildStack(List<DropdownMenuItem> root)
        {
            var stack = new Stack<List<DropdownMenuItem>>();
            stack.Push(root ?? new List<DropdownMenuItem>());
            return stack;
        }

        private bool IsInlineOpen(string id) => _inlineOpen.TryGetValue(id, out var open) && open;

        private void OpenInline(string id, DropdownMenuConfig config)
        {
            _menuStacks[id] = BuildStack(config.Items);
            _inlineOpen[id] = true;
        }

        private void CloseInline(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;
            _inlineOpen[id] = false;
        }

        private Rect GetAnchorRect(string id)
        {
            return _anchorRects.TryGetValue(id, out var rect) ? rect : new Rect(0, 0, 240, 30);
        }

        private float GetMenuWidth(DropdownMenuConfig config, Rect anchor)
        {
            if (config.Width > 0)
                return config.Width * guiHelper.uiScale;
            return Mathf.Max(anchor.width, 200f * guiHelper.uiScale);
        }

        private float GetMenuHeight(DropdownMenuConfig config)
        {
            return Mathf.Max(120f * guiHelper.uiScale, config.MaxHeight * guiHelper.uiScale);
        }

        private void UpdatePosition(string id, DropdownMenuConfig config)
        {
            Rect anchor = GetAnchorRect(id);
            float width = GetMenuWidth(config, anchor);
            float height = GetMenuHeight(config);
            Vector2 screenPos = PopupLayoutUtility.GetAnchoredScreenPosition(anchor, width, height, guiHelper.GetRootGuiScreenRect());
            LayerManager.Instance.SetPosition(id, screenPos);
        }

        private string ResolveId(string id, string fallback)
        {
            if (!string.IsNullOrEmpty(id))
                return id;
            return fallback;
        }
    }
}
