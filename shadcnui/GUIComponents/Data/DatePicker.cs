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
        private readonly Dictionary<string, DateTime> _visibleMonths = new();

        public DatePicker(GUIHelper helper)
            : base(helper) { }

        public DateTime? DrawDatePicker(DatePickerConfig config)
        {
            if (config == null)
                return null;

            string id = string.IsNullOrEmpty(config.Id) ? "datepicker" : config.Id;
            if (!_visibleMonths.ContainsKey(id))
                _visibleMonths[id] = new DateTime((config.SelectedDate ?? DateTime.Today).Year, (config.SelectedDate ?? DateTime.Today).Month, 1);

            string label = config.SelectedDate?.ToString("MMM d, yyyy") ?? config.Placeholder ?? "Select date";
            GUIStyle triggerStyle = styleManager.GetInputStyle(ControlVariant.Outline, config.Size, false, config.IsDisabled, config.Appearance);
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(label), triggerStyle, config.LayoutOptions ?? Array.Empty<GUILayoutOption>());
            if (GUI.Button(rect, string.Empty, triggerStyle))
            {
                if (LayerManager.Instance.IsOpen(id))
                    CloseDatePicker(id);
                else
                    Open(id, rect, config);
            }

            var textStyle = new UnityHelpers.GUIStyle(triggerStyle) { alignment = TextAnchor.MiddleLeft };
            textStyle.normal.background = null;
            textStyle.normal.textColor = config.SelectedDate.HasValue ? styleManager.GetTheme().Text : styleManager.GetTheme().Muted;
            GUI.Label(new Rect(rect.x + triggerStyle.padding.left, rect.y, rect.width - triggerStyle.padding.horizontal - 18f * guiHelper.uiScale, rect.height), label, textStyle);
            GUI.Label(new Rect(rect.xMax - 18f * guiHelper.uiScale, rect.y, 14f * guiHelper.uiScale, rect.height), "˅", styleManager.GetLabelStyle(ControlVariant.Muted, ControlSize.Small, config.Appearance));

            if (Event.current.type == EventType.Repaint)
                _anchorRects[id] = rect;

            return config.SelectedDate;
        }

        public DateTime? DrawDatePickerWithLabel(DatePickerConfig config)
        {
            if (config == null)
                return null;
            if (!string.IsNullOrEmpty(config.Label))
            {
                UnityHelpers.Label(config.Label, styleManager.GetLabelStyle(ControlVariant.Default, config.Size, config.Appearance));
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }
            return DrawDatePicker(config);
        }

        public DateTime? DrawDateRangePicker(string placeholder, DateTime? start, DateTime? end, string id, params GUILayoutOption[] options)
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

        public DateTime? DrawDateRangePicker(DatePickerConfig config)
        {
            string text = config.StartDate.HasValue ? config.StartDate.Value.ToString("MMM d, yyyy") : (config.Placeholder ?? "Select date");
            return DrawDatePicker(
                new DatePickerConfig
                {
                    Id = config.Id,
                    Placeholder = text,
                    SelectedDate = config.StartDate,
                    MinDate = config.MinDate,
                    MaxDate = config.MaxDate,
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
            Vector2 pos = GUIUtility.GUIToScreenPoint(new Vector2(anchor.xMin, anchor.yMax + 4f));
            LayerManager.Instance.Open(
                new LayerConfig
                {
                    Id = id,
                    OpenPosition = pos,
                    Width = 280f * guiHelper.uiScale,
                    Height = 320f * guiHelper.uiScale,
                    CloseOnClickOutside = true,
                    ZIndex = DesignTokens.ZIndex.Popover,
                    Content = () => DrawCalendarPopup(id, config),
                }
            );
        }

        private void DrawCalendarPopup(string id, DatePickerConfig config)
        {
            var style = styleManager.GetDatePickerStyle(config.Variant, config.Size, config.Appearance);
            layoutComponents.BeginVerticalGroup(style, GUILayout.Width(280f * guiHelper.uiScale));
            DrawCalendarHeader(id);
            DrawWeekdays(config);
            DrawGrid(id, config);
            layoutComponents.EndVerticalGroup();
        }

        private void DrawCalendarHeader(string id)
        {
            layoutComponents.BeginHorizontalGroup();
            if (GUILayout.Button("‹", styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Icon)))
                _visibleMonths[id] = _visibleMonths[id].AddMonths(-1);
            GUILayout.FlexibleSpace();
            GUILayout.Label(_visibleMonths[id].ToString("MMMM yyyy"), styleManager.GetCardTitleStyle(ControlVariant.Default, ControlSize.Default, null));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("›", styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Icon)))
                _visibleMonths[id] = _visibleMonths[id].AddMonths(1);
            layoutComponents.EndHorizontalGroup();
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
        }

        private void DrawWeekdays(DatePickerConfig config)
        {
            layoutComponents.BeginHorizontalGroup();
            foreach (var day in new[] { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" })
                GUILayout.Label(day, styleManager.GetDatePickerWeekdayStyle(config.Appearance), GUILayout.Width(36f * guiHelper.uiScale));
            layoutComponents.EndHorizontalGroup();
            layoutComponents.AddSpace(DesignTokens.Spacing.XS);
        }

        private void DrawGrid(string id, DatePickerConfig config)
        {
            DateTime month = _visibleMonths[id];
            DateTime first = new DateTime(month.Year, month.Month, 1);
            DateTime cursor = first.AddDays(-(int)first.DayOfWeek);

            for (int week = 0; week < 6; week++)
            {
                layoutComponents.BeginHorizontalGroup();
                for (int day = 0; day < 7; day++)
                {
                    DateTime current = cursor.AddDays(week * 7 + day);
                    DrawDateButton(id, config, current, month.Month);
                }
                layoutComponents.EndHorizontalGroup();
                if (week < 5)
                    layoutComponents.AddSpace(DesignTokens.Spacing.XXS);
            }
        }

        private void DrawDateButton(string id, DatePickerConfig config, DateTime date, int activeMonth)
        {
            bool selected = config.SelectedDate.HasValue && config.SelectedDate.Value.Date == date.Date;
            bool today = date.Date == DateTime.Today;
            bool outside = date.Month != activeMonth;
            bool blocked = (config.MinDate.HasValue && date.Date < config.MinDate.Value.Date) || (config.MaxDate.HasValue && date.Date > config.MaxDate.Value.Date);

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
                CloseDatePicker(id);
            }
            GUI.enabled = prev;
        }
    }
}
