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

            if (IsOpen(id))
                UpdatePosition(id);
        }

        public void Open(DropdownMenuConfig config, Rect anchorRect)
        {
            if (config == null)
                return;

            string id = ResolveId(config.Id, "dropdown");
            _anchorRects[id] = anchorRect;
            _menuStacks[id] = BuildStack(config.Items);

            Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(anchorRect.x, anchorRect.yMax + 4));
            LayerManager.Instance.Open(
                new LayerConfig
                {
                    Id = id,
                    OpenPosition = screenPos,
                    Width = GetMenuWidth(config, anchorRect),
                    Height = GetMenuHeight(config),
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
                if (UnityHelpers.Button("<- Back", itemStyle))
                {
                    stack.Pop();
                    return;
                }
                UnityHelpers.Box(string.Empty, separatorStyle);
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
                        UnityHelpers.Box(string.Empty, separatorStyle);
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
            string label = item.Text ?? string.Empty;
            if (hasChildren)
                label += " >";

            if (UnityHelpers.Button(label, itemStyle))
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

        private void UpdatePosition(string id)
        {
            Rect anchor = GetAnchorRect(id);
            Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(anchor.x, anchor.yMax + 4));
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
