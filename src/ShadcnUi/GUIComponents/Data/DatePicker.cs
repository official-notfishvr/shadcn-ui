using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Data
{
    public class DatePicker : BaseComponent
    {
        private readonly Dictionary<string, Rect> _anchorRects = new();
        private readonly Dictionary<string, Rect> _screenAnchorRects = new();
        private readonly Dictionary<string, DateTime> _visibleMonths = new();
        private const float PopupWidth = 304f;
        private const float PopupHeight = 344f;

        public DatePicker(GUIHelper helper)
            : base(helper) { }

        public DateTime? Render(DatePickerConfig config)
        {
            if (config == null)
                return null;

            string id = string.IsNullOrEmpty(config.Id) ? "datepicker" : config.Id;
            if (!_visibleMonths.ContainsKey(id))
                _visibleMonths[id] = new DateTime((config.SelectedDate ?? DateTime.Today).Year, (config.SelectedDate ?? DateTime.Today).Month, 1);

            string format = string.IsNullOrWhiteSpace(config.DisplayFormat) ? "MMM d, yyyy" : config.DisplayFormat;
            string label = config.SelectedDate?.ToString(format) ?? config.Placeholder ?? "Select date";
            GUIStyle triggerStyle = styleManager.GetInputStyle(config.Variant, config.Size, false, config.IsDisabled, config.Appearance);
            Rect rect = ControlLayoutUtility.ReserveRect(new UnityHelpers.GUIContent(label), triggerStyle, config.LayoutOptions);
            bool previousEnabled = GUI.enabled;
            GUI.enabled = previousEnabled && !config.IsDisabled;
            bool clicked = GUI.Button(rect, string.Empty, triggerStyle);
            GUI.enabled = previousEnabled;
            if (clicked && !config.IsDisabled)
            {
                if (LayerManager.Instance.IsOpen(id))
                    CloseDatePicker(id);
                else
                    Open(id, rect, config);
            }

            var textStyle = ContentRenderUtility.CreateOverlayLabelStyle(triggerStyle, TextAnchor.MiddleLeft);
            textStyle.normal.textColor = config.SelectedDate.HasValue ? styleManager.GetTheme().Text : styleManager.GetTheme().Muted;
            ContentRenderUtility.DrawTextWithTrailing(
                ControlLayoutUtility.Inset(rect, triggerStyle.padding.left, triggerStyle.padding.right),
                label,
                textStyle,
                "˅",
                styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Small, config.Appearance),
                14f * guiHelper.uiScale,
                18f * guiHelper.uiScale
            );

            if (Event.current.type == EventType.Repaint)
            {
                _anchorRects[id] = rect;
                _screenAnchorRects[id] = PopupLayoutUtility.ToScreenRect(rect);
            }

            return config.SelectedDate;
        }

        internal DateTime? DrawDatePickerWithLabel(DatePickerConfig config)
        {
            if (config == null)
                return null;
            if (!string.IsNullOrEmpty(config.Label))
            {
                UnityHelpers.Label(config.Label, styleManager.GetLabelStyle(ControlVariant.Default, config.Size, GetTextOnlyAppearance(config.Appearance)));
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }
            return Render(config);
        }

        internal DateTime? DrawDateRangePicker(string placeholder, DateTime? start, DateTime? end, string id, params GUILayoutOption[] options)
        {
            return DrawDateRangePicker(
                new DatePickerConfig
                {
                    Placeholder = placeholder,
                    StartDate = start,
                    EndDate = end,
                    Id = id,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        internal DateTime? DrawDateRangePicker(DatePickerConfig config)
        {
            string text = config.StartDate.HasValue ? config.StartDate.Value.ToString("MMM d, yyyy") : (config.Placeholder ?? "Select date");
            return Render(
                new DatePickerConfig
                {
                    Id = config.Id,
                    Placeholder = text,
                    SelectedDate = config.StartDate,
                    MinDate = config.MinDate,
                    MaxDate = config.MaxDate,
                    DisplayFormat = config.DisplayFormat,
                    Variant = config.Variant,
                    Size = config.Size,
                    Appearance = config.Appearance,
                    LayoutOptions = config.LayoutOptions,
                }
            );
        }

        public void CloseDatePicker(string id) => LayerManager.Instance.Close(id);

        public bool IsDatePickerOpen(string id) => LayerManager.Instance.IsOpen(id);

        private void Open(string id, Rect anchor, DatePickerConfig config)
        {
            _anchorRects[id] = anchor;
            Rect screenAnchor = _screenAnchorRects.TryGetValue(id, out var cachedScreenAnchor) ? cachedScreenAnchor : PopupLayoutUtility.ToScreenRect(anchor);
            Vector2 pos = PopupLayoutUtility.GetAnchoredScreenPositionFromScreenRect(screenAnchor, PopupWidth * guiHelper.uiScale, PopupHeight * guiHelper.uiScale, GetScreenBounds());
            LayerManager.Instance.Open(
                new LayerConfig
                {
                    Id = id,
                    OpenPosition = pos,
                    Width = PopupWidth * guiHelper.uiScale,
                    Height = PopupHeight * guiHelper.uiScale,
                    CloseOnClickOutside = true,
                    ZIndex = DesignTokens.ZIndex.Popover,
                    Content = () => DrawCalendarPopup(id, config),
                    OnClose = () => ClearState(id),
                }
            );
        }

        protected override void OnBeforeDispose()
        {
            foreach (var id in _anchorRects.Keys)
                LayerManager.Instance.Close(id);

            _anchorRects.Clear();
            _screenAnchorRects.Clear();
            _visibleMonths.Clear();
        }

        private void DrawCalendarPopup(string id, DatePickerConfig config)
        {
            var style = styleManager.GetDatePickerStyle(config.Variant, config.Size, config.Appearance);
            layoutComponents.BeginVerticalGroup(style, GUILayout.Width(PopupWidth * guiHelper.uiScale), GUILayout.Height(PopupHeight * guiHelper.uiScale), GUILayout.ExpandHeight(false));
            CalendarRenderUtility.DrawMonthHeader(
                layoutComponents,
                styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Icon),
                styleManager.GetCardTitleStyle(ControlVariant.Default, ControlSize.Default, null),
                _visibleMonths[id],
                () => _visibleMonths[id] = _visibleMonths[id].AddMonths(-1),
                () => _visibleMonths[id] = _visibleMonths[id].AddMonths(1),
                DesignTokens.Spacing.SM
            );
            CalendarRenderUtility.DrawWeekdays(layoutComponents, styleManager.GetDatePickerWeekdayStyle(config.Appearance), 36f * guiHelper.uiScale, DesignTokens.Spacing.XS);
            CalendarRenderUtility.DrawMonthGrid(layoutComponents, _visibleMonths[id], (current, activeMonth) => DrawDateButton(id, config, current, activeMonth), DesignTokens.Spacing.XXS);
            layoutComponents.EndVerticalGroup();
        }

        private void DrawDateButton(string id, DatePickerConfig config, DateTime date, int activeMonth)
        {
            bool selected = config.SelectedDate.HasValue && config.SelectedDate.Value.Date == date.Date;
            bool today = date.Date == DateTime.Today;
            bool outside = date.Month != activeMonth;
            bool blocked = config.IsDisabled || (config.MinDate.HasValue && date.Date < config.MinDate.Value.Date) || (config.MaxDate.HasValue && date.Date > config.MaxDate.Value.Date);

            GUIStyle style =
                selected ? styleManager.GetDatePickerDaySelectedStyle(config.Appearance)
                : today ? styleManager.GetDatePickerDayTodayStyle(config.Appearance)
                : outside ? styleManager.GetDatePickerDayOutsideMonthStyle(config.Appearance)
                : styleManager.GetDatePickerDayStyle(config.Appearance);
            bool prev = GUI.enabled;
            GUI.enabled = !blocked;
            if (GUILayout.Button(date.Day.ToString(), style, GUILayout.Width(36f * guiHelper.uiScale), GUILayout.Height(36f * guiHelper.uiScale)))
            {
                config.SelectedDate = date.Date;
                config.OnDateChanged?.Invoke(config.SelectedDate);
                CloseDatePicker(id);
            }
            GUI.enabled = prev;
        }

        private void ClearState(string id)
        {
            _anchorRects.Remove(id);
            _screenAnchorRects.Remove(id);
            _visibleMonths.Remove(id);
        }

        private static Rect GetScreenBounds() => new(0f, 0f, Screen.width, Screen.height);

        private static ComponentAppearance GetTextOnlyAppearance(ComponentAppearance appearance)
        {
            if (appearance?.ForegroundColor == null)
                return null;

            return new ComponentAppearance { ForegroundColor = appearance.ForegroundColor };
        }
    }
}
