using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Styling;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Utils
{
    internal static class ControlLayoutUtility
    {
        public static Rect ScaleRect(Rect rect, float uiScale)
        {
            return new Rect(rect.x * uiScale, rect.y * uiScale, rect.width * uiScale, rect.height * uiScale);
        }

        public static Rect Inset(Rect rect, float left = 0f, float right = 0f, float top = 0f, float bottom = 0f)
        {
            return new Rect(rect.x + left, rect.y + top, Mathf.Max(0f, rect.width - left - right), Mathf.Max(0f, rect.height - top - bottom));
        }

        public static Rect RightAligned(Rect rect, float width, float insetRight = 0f)
        {
            return new Rect(rect.xMax - insetRight - width, rect.y, width, rect.height);
        }

        public static Rect BottomAligned(Rect rect, float height)
        {
            return new Rect(rect.x, rect.yMax - height, rect.width, height);
        }

        public static Rect Centered(Rect bounds, float width, float height, float offsetX = 0f, float offsetY = 0f)
        {
            return new Rect(bounds.x + (bounds.width - width) * 0.5f + offsetX, bounds.y + (bounds.height - height) * 0.5f + offsetY, width, height);
        }

        public static List<GUILayoutOption> BuildLayoutOptions(GUILayoutOption[] source, float fixedWidth = 0f, float fixedHeight = 0f, bool expandWidth = false)
        {
            var options = new List<GUILayoutOption>(source ?? Array.Empty<GUILayoutOption>());

            if (fixedWidth > 0f)
                options.Add(GUILayout.Width(fixedWidth));
            else if (expandWidth)
                options.Add(GUILayout.ExpandWidth(true));

            if (fixedHeight > 0f)
                options.Add(GUILayout.Height(fixedHeight));

            return options;
        }

        public static Rect ReserveRect(UnityHelpers.GUIContent content, GUIStyle style, IList<GUILayoutOption> options, float minHeight = 0f)
        {
            var rect = GUILayoutUtility.GetRect(content ?? UnityHelpers.GUIContent.none, style, ToArray(options));
            if (minHeight > 0f)
                rect.height = Mathf.Max(rect.height, minHeight);
            return rect;
        }

        private static GUILayoutOption[] ToArray(IList<GUILayoutOption> options)
        {
            if (options == null || options.Count == 0)
                return Array.Empty<GUILayoutOption>();

            var array = new GUILayoutOption[options.Count];
            for (int i = 0; i < options.Count; i++)
                array[i] = options[i];
            return array;
        }
    }

    internal static class PopupLayoutUtility
    {
        public static Vector2 GetAnchoredScreenPosition(Rect anchorRect, float popupWidth, float popupHeight, Rect rootRect, float gap = 4f)
        {
            Vector2 anchorTopLeft = GUIUtility.GUIToScreenPoint(new Vector2(anchorRect.xMin, anchorRect.yMin));
            Vector2 anchorBottomLeft = GUIUtility.GUIToScreenPoint(new Vector2(anchorRect.xMin, anchorRect.yMax));

            return GetAnchoredScreenPositionFromScreenRect(new Rect(anchorTopLeft.x, anchorTopLeft.y, anchorRect.width, anchorRect.height), popupWidth, popupHeight, rootRect, gap);
        }

        public static Vector2 GetAnchoredScreenPositionFromScreenRect(Rect screenAnchorRect, float popupWidth, float popupHeight, Rect rootRect, float gap = 4f)
        {
            float x = screenAnchorRect.xMin;
            float y = screenAnchorRect.yMax + gap;

            if (y + popupHeight > rootRect.yMax)
            {
                float aboveY = screenAnchorRect.yMin - gap - popupHeight;
                if (aboveY >= rootRect.yMin)
                    y = aboveY;
                else
                    y = Mathf.Max(rootRect.yMin, rootRect.yMax - popupHeight);
            }

            if (x + popupWidth > rootRect.xMax)
                x = Mathf.Max(rootRect.xMin, rootRect.xMax - popupWidth);
            else if (x < rootRect.xMin)
                x = rootRect.xMin;

            return new Vector2(x, y);
        }

        public static Vector2 GetScreenPointBelow(Rect anchorRect, float gap = 4f)
        {
            return GUIUtility.GUIToScreenPoint(new Vector2(anchorRect.xMin, anchorRect.yMax + gap));
        }

        public static Rect ToScreenRect(Rect rect)
        {
            var topLeft = GUIUtility.GUIToScreenPoint(new Vector2(rect.xMin, rect.yMin));
            var bottomRight = GUIUtility.GUIToScreenPoint(new Vector2(rect.xMax, rect.yMax));
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }
    }

    internal static class SurfaceDrawUtility
    {
        public static void DrawSolid(Rect rect, Color color)
        {
            Color prev = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = prev;
        }

        public static void DrawRoundedFill(StyleManager styleManager, Rect rect, Color color, int radius)
        {
            GUI.DrawTexture(rect, styleManager.CreateTexture(Mathf.Max(1, Mathf.RoundToInt(rect.width)), Mathf.Max(1, Mathf.RoundToInt(rect.height)), radius, color), ScaleMode.StretchToFill);
        }

        public static void DrawRoundedBorder(StyleManager styleManager, Rect rect, int radius, Color fill, Color border, float borderThickness = 1f, float shadowAlpha = 0f, int shadowBlur = 0, Color? shadowColor = null)
        {
            GUI.DrawTexture(rect, styleManager.CreateBorderTexture(Mathf.Max(1, Mathf.RoundToInt(rect.width)), Mathf.Max(1, Mathf.RoundToInt(rect.height)), radius, fill, border, borderThickness, shadowAlpha, shadowBlur, shadowColor ?? Color.clear), ScaleMode.StretchToFill);
        }

        public static Rect ReserveSquare(float size)
        {
            return GUILayoutUtility.GetRect(size, size, GUILayout.Width(size), GUILayout.Height(size));
        }
    }

    internal static class CalendarRenderUtility
    {
        private static readonly string[] WeekdayLabels = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };

        public static void DrawMonthHeader(shadcnui.GUIComponents.Layout.Layout layout, GUIStyle buttonStyle, GUIStyle titleStyle, DateTime visibleMonth, Action previous, Action next, float spacing)
        {
            layout.BeginHorizontalGroup();
            if (GUILayout.Button("‹", buttonStyle))
                previous?.Invoke();
            GUILayout.FlexibleSpace();
            GUILayout.Label(visibleMonth.ToString("MMMM yyyy"), titleStyle);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("›", buttonStyle))
                next?.Invoke();
            layout.EndHorizontalGroup();
            layout.AddSpace(spacing);
        }

        public static void DrawWeekdays(shadcnui.GUIComponents.Layout.Layout layout, GUIStyle weekdayStyle, float cellWidth, float spacing)
        {
            layout.BeginHorizontalGroup();
            foreach (var day in WeekdayLabels)
                GUILayout.Label(day, weekdayStyle, GUILayout.Width(cellWidth));
            layout.EndHorizontalGroup();
            layout.AddSpace(spacing);
        }

        public static void DrawMonthGrid(shadcnui.GUIComponents.Layout.Layout layout, DateTime visibleMonth, Action<DateTime, int> drawDay, float rowSpacing)
        {
            DateTime first = new DateTime(visibleMonth.Year, visibleMonth.Month, 1);
            DateTime cursor = first.AddDays(-(int)first.DayOfWeek);

            for (int week = 0; week < 6; week++)
            {
                layout.BeginHorizontalGroup();
                for (int day = 0; day < 7; day++)
                {
                    DateTime current = cursor.AddDays(week * 7 + day);
                    drawDay?.Invoke(current, visibleMonth.Month);
                }
                layout.EndHorizontalGroup();

                if (week < 5)
                    layout.AddSpace(rowSpacing);
            }
        }
    }

    internal static class ContentRenderUtility
    {
        public static GUIStyle CreateOverlayLabelStyle(GUIStyle source, TextAnchor alignment)
        {
            var style = new UnityHelpers.GUIStyle(source) { alignment = alignment, clipping = TextClipping.Clip };
            style.normal.background = null;
            style.hover.background = null;
            style.active.background = null;
            style.focused.background = null;
            style.onNormal.background = null;
            style.onHover.background = null;
            style.onActive.background = null;
            style.onFocused.background = null;
            return style;
        }

        public static void DrawTextWithTrailing(Rect rect, string text, GUIStyle textStyle, string trailingText, GUIStyle trailingStyle, float trailingWidth, float trailingInsetRight, float leadingInsetLeft = 0f)
        {
            Rect textRect = ControlLayoutUtility.Inset(rect, leadingInsetLeft, trailingWidth + trailingInsetRight);
            GUI.Label(textRect, text ?? string.Empty, textStyle);

            if (!string.IsNullOrEmpty(trailingText))
                GUI.Label(ControlLayoutUtility.RightAligned(rect, trailingWidth, trailingInsetRight), trailingText, trailingStyle);
        }

        public static void DrawCenteredContent(Rect rect, GUIStyle style, string text, Texture image, IconPosition position, float iconSize, float spacing)
        {
            var labelStyle = CreateOverlayLabelStyle(style, TextAnchor.MiddleCenter);
            bool hasText = !string.IsNullOrEmpty(text);
            bool hasImage = image != null;

            if (!hasImage)
            {
                GUI.Label(rect, text ?? string.Empty, labelStyle);
                return;
            }

            if (!hasText)
            {
                Rect iconOnlyRect = ControlLayoutUtility.Centered(rect, iconSize, iconSize);
                GUI.DrawTexture(iconOnlyRect, image, ScaleMode.ScaleToFit);
                return;
            }

            Vector2 textSize = labelStyle.CalcSize(new UnityHelpers.GUIContent(text));
            Rect iconRect;
            Rect textRect;

            switch (position)
            {
                case IconPosition.Right:
                {
                    float totalWidth = textSize.x + spacing + iconSize;
                    Rect contentRect = ControlLayoutUtility.Centered(rect, totalWidth, rect.height);
                    textRect = new Rect(contentRect.x, rect.y, textSize.x, rect.height);
                    iconRect = ControlLayoutUtility.Centered(new Rect(contentRect.xMax - iconSize, rect.y, iconSize, rect.height), iconSize, iconSize);
                    labelStyle.alignment = TextAnchor.MiddleLeft;
                    break;
                }
                case IconPosition.Above:
                {
                    float totalHeight = iconSize + spacing + textSize.y;
                    Rect contentRect = ControlLayoutUtility.Centered(rect, rect.width, totalHeight);
                    iconRect = new Rect(contentRect.x + (contentRect.width - iconSize) * 0.5f, contentRect.y, iconSize, iconSize);
                    textRect = new Rect(contentRect.x, contentRect.y + iconSize + spacing, contentRect.width, textSize.y);
                    labelStyle.alignment = TextAnchor.UpperCenter;
                    break;
                }
                case IconPosition.Below:
                {
                    float totalHeight = textSize.y + spacing + iconSize;
                    Rect contentRect = ControlLayoutUtility.Centered(rect, rect.width, totalHeight);
                    textRect = new Rect(contentRect.x, contentRect.y, contentRect.width, textSize.y);
                    iconRect = new Rect(contentRect.x + (contentRect.width - iconSize) * 0.5f, contentRect.y + textSize.y + spacing, iconSize, iconSize);
                    labelStyle.alignment = TextAnchor.UpperCenter;
                    break;
                }
                default:
                {
                    float totalWidth = iconSize + spacing + textSize.x;
                    Rect contentRect = ControlLayoutUtility.Centered(rect, totalWidth, rect.height);
                    iconRect = ControlLayoutUtility.Centered(new Rect(contentRect.x, rect.y, iconSize, rect.height), iconSize, iconSize);
                    textRect = new Rect(contentRect.x + iconSize + spacing, rect.y, textSize.x, rect.height);
                    labelStyle.alignment = TextAnchor.MiddleLeft;
                    break;
                }
            }

            GUI.DrawTexture(iconRect, image, ScaleMode.ScaleToFit);
            GUI.Label(textRect, text, labelStyle);
        }

        public static void DrawLeadingIconAndText(Rect rect, GUIStyle style, string text, Texture image, float iconSize, float spacing, float reservedRight = 0f, TextAnchor alignment = TextAnchor.MiddleLeft)
        {
            var labelStyle = CreateOverlayLabelStyle(style, alignment);
            float startX = rect.x;

            if (image != null)
            {
                Rect iconRect = ControlLayoutUtility.Centered(new Rect(startX, rect.y, iconSize, rect.height), iconSize, iconSize);
                GUI.DrawTexture(iconRect, image, ScaleMode.ScaleToFit);
                startX = iconRect.xMax + spacing;
            }

            Rect textRect = new Rect(startX, rect.y, Mathf.Max(0f, rect.xMax - startX - reservedRight), rect.height);
            GUI.Label(textRect, text ?? string.Empty, labelStyle);
        }
    }
}

namespace shadcnui.GUIComponents.Core.Base
{
    using shadcnui.GUIComponents.Core.Utils;

    public abstract class BooleanControlBase : BaseComponent
    {
        protected BooleanControlBase(GUIHelper helper)
            : base(helper) { }

        protected bool RenderBoolControl(BoolControlConfigBase config, GUIStyle probeStyle, float minHeight, Func<Rect, BoolControlConfigBase, bool> drawControl)
        {
            bool prevEnabled = GUI.enabled;
            if (config.IsDisabled)
                GUI.enabled = false;

            Rect rect = ResolveControlRect(config, probeStyle, minHeight);
            bool newValue = drawControl(rect, config);

            GUI.enabled = prevEnabled;

            if (newValue != config.Value && !config.IsDisabled)
                config.OnValueChanged?.Invoke(newValue);

            return config.IsDisabled ? config.Value : newValue;
        }

        protected Rect ResolveControlRect(BoolControlConfigBase config, GUIStyle probeStyle, float minHeight)
        {
            if (config.Rect.HasValue)
                return ControlLayoutUtility.ScaleRect(config.Rect.Value, guiHelper.uiScale);

            var options = BuildRowLayoutOptions(config);
            var content = new UnityHelpers.GUIContent(config.Label ?? string.Empty);
            return ControlLayoutUtility.ReserveRect(content, probeStyle ?? GUIStyle.none, options, minHeight);
        }

        protected List<GUILayoutOption> BuildRowLayoutOptions(BoolControlConfigBase config)
        {
            return ControlLayoutUtility.BuildLayoutOptions(config.LayoutOptions, expandWidth: config.FullRowClick);
        }

        protected GUIStyle GetBooleanLabelStyle(BoolControlConfigBase config)
        {
            return styleManager?.GetLabelStyle(config.IsDisabled ? ControlVariant.Muted : config.LabelVariant, config.Size, config.Appearance) ?? GUI.skin.label;
        }

        protected float DrawLeadingIcon(Rect rowRect, IconConfig icon)
        {
            if (icon?.Image == null)
                return rowRect.x;

            float iconSize = icon.Size * guiHelper.uiScale;
            var iconRect = new Rect(rowRect.x, rowRect.y + (rowRect.height - iconSize) * 0.5f, iconSize, iconSize);
            GUI.DrawTexture(iconRect, icon.Image, ScaleMode.ScaleToFit);
            return iconRect.xMax + icon.Spacing * guiHelper.uiScale;
        }

        protected bool HandleToggleInput(Rect rect, bool currentValue, bool disabled)
        {
            if (disabled)
                return currentValue;

            var evt = Event.current;
            if (evt.type == EventType.MouseDown && evt.button == 0 && rect.Contains(evt.mousePosition))
            {
                evt.Use();
                return !currentValue;
            }

            return currentValue;
        }
    }
}
