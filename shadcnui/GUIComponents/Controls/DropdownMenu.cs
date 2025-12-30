using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Controls
{
    public class DropdownMenu : BaseComponent
    {
        private bool _isOpen;
        private Vector2 _scrollPosition;
        private readonly Stack<List<DropdownMenuItem>> _menuStack = new Stack<List<DropdownMenuItem>>();
        private string _menuId;
        private const float AnimationDuration = DesignTokens.Animation.DurationFast;
        private Rect _cachedDropdownRect;

        public DropdownMenu(GUIHelper helper)
            : base(helper) { }

        public bool IsOpen => _isOpen;

        public void Open(List<DropdownMenuItem> rootItems, string id = "dropdown")
        {
            _menuStack.Clear();
            _menuStack.Push(rootItems);
            _isOpen = true;
            _menuId = id;
            var animManager = guiHelper.GetAnimationManager();
            animManager.FadeIn($"dropdown_alpha_{id}", AnimationDuration, EasingFunctions.EaseOutCubic);
            animManager.ScaleIn($"dropdown_scale_{id}", AnimationDuration, 0.95f, EasingFunctions.EaseOutCubic);
            animManager.SlideIn($"dropdown_slide_{id}", Vector2.zero, new Vector2(0, -DesignTokens.Spacing.LG), AnimationDuration, EasingFunctions.EaseOutCubic);
        }

        public void Close()
        {
            if (_menuId != null)
            {
                var animManager = guiHelper.GetAnimationManager();
                animManager.FadeOut($"dropdown_alpha_{_menuId}", AnimationDuration * 0.8f, EasingFunctions.EaseInCubic);
                animManager.ScaleOut($"dropdown_scale_{_menuId}", AnimationDuration * 0.8f, 0.95f, EasingFunctions.EaseInCubic);
            }
            _menuStack.Clear();
            _isOpen = false;
            _menuId = null;
        }

        public void Draw(DropdownMenuConfig config)
        {
            if (config?.Items == null || config.Items.Count == 0)
            {
                Close();
                return;
            }
            if (!_isOpen)
                Open(config.Items);
            DrawDropdownMenu(config.Options);
        }

        private void DrawDropdownMenu(GUILayoutOption[] options)
        {
            if (!_isOpen || _menuStack.Count == 0)
                return;
            var styleManager = guiHelper.GetStyleManager();
            var animManager = guiHelper.GetAnimationManager();
            string id = _menuId ?? "dropdown";
            float alpha = animManager.GetFloat($"dropdown_alpha_{id}", 1f);
            float scale = animManager.GetFloat($"dropdown_scale_{id}", 1f);
            Vector2 slideOffset = animManager.GetVector2($"dropdown_slide_{id}", Vector2.zero);

            var layoutOptions = new List<GUILayoutOption> { GUILayout.ExpandWidth(true), GUILayout.MinHeight(0), GUILayout.MaxHeight(200) };
            if (options != null)
                layoutOptions.AddRange(options);
            GUIStyle contentStyle = styleManager.GetDropdownMenuStyle(ControlVariant.Default, ControlSize.Default);
            GUIStyle itemStyle = styleManager.GetDropdownMenuItemStyle();
            GUIStyle separatorStyle = styleManager.GetSeparatorStyle(SeparatorOrientation.Horizontal, ControlVariant.Default, ControlSize.Default);
            Color prevColor = GUI.color;
            Matrix4x4 prevMatrix = GUI.matrix;

            if (alpha < 1f)
                GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a * alpha);

            bool needsTransform = scale < 1f || slideOffset != Vector2.zero;
            if (needsTransform)
            {
                Vector2 pivot = _cachedDropdownRect.center;
                GUI.matrix = Matrix4x4.Translate(new Vector3(pivot.x, pivot.y, 0f)) * Matrix4x4.Scale(new Vector3(scale, scale, 1f)) * Matrix4x4.Translate(new Vector3(-pivot.x + slideOffset.x, -pivot.y + slideOffset.y, 0f)) * prevMatrix;
            }

            layoutComponents.BeginVerticalGroup(contentStyle, layoutOptions.ToArray());
            _scrollPosition = layoutComponents.DrawScrollView(
                _scrollPosition,
                () =>
                {
                    if (_menuStack.Count > 1)
                    {
                        if (UnityHelpers.Button("<- Back", itemStyle))
                        {
                            _menuStack.Pop();
                            return;
                        }
                        UnityHelpers.Box("", separatorStyle);
                    }
                    var currentItems = _menuStack.Peek();
                    foreach (var item in currentItems)
                        DrawMenuItem(item);
                },
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
            );
            layoutComponents.EndVerticalGroup();

            if (Event.current.type == EventType.Repaint)
                _cachedDropdownRect = GUILayoutUtility.GetLastRect();

            GUI.matrix = prevMatrix;
            GUI.color = prevColor;
        }

        private void DrawMenuItem(DropdownMenuItem item)
        {
            var styleManager = guiHelper.GetStyleManager();
            GUIStyle headerStyle = styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Small);
            GUIStyle separatorStyle = styleManager.GetSeparatorStyle(SeparatorOrientation.Horizontal, ControlVariant.Default, ControlSize.Default);
            GUIStyle itemStyle = styleManager.GetDropdownMenuItemStyle();
            switch (item.Type)
            {
                case DropdownMenuItemType.Header:
#if IL2CPP_MELONLOADER_PRE57
                    GUILayout.Label(item.Content, headerStyle, shadcnui.GUIComponents.Layout.Layout.EmptyOptions);
#else
                    GUILayout.Label(item.Content, headerStyle);
#endif
                    break;
                case DropdownMenuItemType.Separator:
                    UnityHelpers.Box("", separatorStyle);
                    break;
                case DropdownMenuItemType.Item:
                    if (item.SubItems != null && item.SubItems.Count > 0)
                    {
#if IL2CPP_MELONLOADER_PRE57
                        if (GUILayout.Button(item.Content, itemStyle, shadcnui.GUIComponents.Layout.Layout.EmptyOptions))
                            _menuStack.Push(item.SubItems);
#else
                        if (GUILayout.Button(item.Content, itemStyle))
                            _menuStack.Push(item.SubItems);
#endif
                    }
                    else
                    {
#if IL2CPP_MELONLOADER_PRE57
                        if (GUILayout.Button(item.Content, itemStyle, shadcnui.GUIComponents.Layout.Layout.EmptyOptions))
                        {
                            item.OnClick?.Invoke();
                            Close();
                        }
#else
                        if (GUILayout.Button(item.Content, itemStyle))
                        {
                            item.OnClick?.Invoke();
                            Close();
                        }
#endif
                    }
                    break;
            }
        }
    }
}
