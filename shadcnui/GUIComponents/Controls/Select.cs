using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Controls
{
    public class Select : BaseComponent
    {
        private bool isOpen;
        private Vector2 scrollPosition;
        private string _selectId;
        private const float AnimationDuration = DesignTokens.Animation.DurationFast;
        private Rect _cachedSelectRect;

        public Select(GUIHelper helper)
            : base(helper) { }

        public bool IsOpen => isOpen;

        #region Config-based API
        public int DrawSelect(SelectConfig config)
        {
            if (!isOpen)
                return config.SelectedIndex;

            var styleManager = guiHelper.GetStyleManager();
            var animManager = guiHelper.GetAnimationManager();
            string id = _selectId ?? "select";

            int selectedIndex = DrawSelectDropdown(config, styleManager, animManager, id);
            return selectedIndex;
        }
        #endregion

        #region API
        public void Open(string id = "select")
        {
            _selectId = id;
            isOpen = true;
            var animManager = guiHelper.GetAnimationManager();
            animManager.FadeIn($"select_alpha_{id}", AnimationDuration, EasingFunctions.EaseOutCubic);
            animManager.ScaleIn($"select_scale_{id}", AnimationDuration, 0.95f, EasingFunctions.EaseOutCubic);
            animManager.SlideIn($"select_slide_{id}", Vector2.zero, new Vector2(0, -DesignTokens.Spacing.MD), AnimationDuration, EasingFunctions.EaseOutCubic);
        }

        public void Close()
        {
            if (_selectId != null)
            {
                var animManager = guiHelper.GetAnimationManager();
                animManager.FadeOut($"select_alpha_{_selectId}", AnimationDuration * 0.8f, EasingFunctions.EaseInCubic);
                animManager.ScaleOut($"select_scale_{_selectId}", AnimationDuration * 0.8f, 0.95f, EasingFunctions.EaseInCubic);
            }
            isOpen = false;
            _selectId = null;
        }

        public int DrawSelect(string[] items, int selectedIndex)
        {
            return DrawSelect(new SelectConfig { Items = items, SelectedIndex = selectedIndex });
        }
        #endregion

        #region Private Methods
        private int DrawSelectDropdown(SelectConfig config, StyleManager styleManager, AnimationManager animManager, string id)
        {
            float alpha = animManager.GetFloat($"select_alpha_{id}", 1f);
            float scale = animManager.GetFloat($"select_scale_{id}", 1f);
            Vector2 slideOffset = animManager.GetVector2($"select_slide_{id}", Vector2.zero);

            GUIStyle selectStyle = styleManager?.GetSelectStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.box;
            GUIStyle itemStyle = styleManager?.GetSelectItemStyle() ?? GUI.skin.button;
            int selectedIndex = config.SelectedIndex;

            ApplyAnimationTransform(alpha, scale, slideOffset);
            selectedIndex = DrawSelectContent(config, selectStyle, itemStyle, selectedIndex);
            RestoreGraphicsState();

            return selectedIndex;
        }

        private void ApplyAnimationTransform(float alpha, float scale, Vector2 slideOffset)
        {
            var prevColor = GUI.color;
            if (alpha < 1f)
                GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a * alpha);

            if (scale < 1f || slideOffset != Vector2.zero)
            {
                Vector2 pivot = _cachedSelectRect.center;
                var prevMatrix = GUI.matrix;
                GUI.matrix = Matrix4x4.Translate(new Vector3(pivot.x, pivot.y, 0f)) * Matrix4x4.Scale(new Vector3(scale, scale, 1f)) * Matrix4x4.Translate(new Vector3(-pivot.x + slideOffset.x, -pivot.y + slideOffset.y, 0f)) * prevMatrix;
            }
        }

        private int DrawSelectContent(SelectConfig config, GUIStyle selectStyle, GUIStyle itemStyle, int selectedIndex)
        {
            layoutComponents.BeginVerticalGroup(selectStyle, GUILayout.MaxWidth(300), GUILayout.MaxHeight(200));
            int newIndex = selectedIndex;
            scrollPosition = layoutComponents.DrawScrollView(scrollPosition, () => newIndex = DrawSelectItems(config, itemStyle, newIndex), GUILayout.ExpandWidth(true), GUILayout.MinHeight(0), GUILayout.MaxHeight(200));
            GUILayout.EndVertical();

            if (Event.current.type == EventType.Repaint)
                _cachedSelectRect = GUILayoutUtility.GetLastRect();

            return newIndex;
        }

        private int DrawSelectItems(SelectConfig config, GUIStyle itemStyle, int selectedIndex)
        {
            int newIndex = selectedIndex;
            for (int i = 0; i < config.Items.Length; i++)
            {
                if (UnityHelpers.Button(config.Items[i], itemStyle))
                {
                    newIndex = i;
                    config.OnChange?.Invoke(i);
                    Close();
                }
            }
            return newIndex;
        }

        private void RestoreGraphicsState()
        {
            GUI.matrix = Matrix4x4.identity;
            GUI.color = Color.white;
        }
        #endregion
    }
}
