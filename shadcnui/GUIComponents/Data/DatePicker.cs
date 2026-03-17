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
        private static readonly string[] Weekdays = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };

        private readonly Dictionary<string, DateTime> _displayMonths = new();
        private readonly Dictionary<string, DateTime?> _rangeAnchor = new();
        private readonly Dictionary<string, Rect> _anchorRects = new();

        public DatePicker(GUIHelper helper)
            : base(helper) { }

        public DateTime? DrawDatePicker(DatePickerConfig config)
        {
            if (config == null)
                return null;

            string id = ResolveId(config.Id, "datepicker");
            EnsureDisplayMonth(id, config.SelectedDate ?? DateTime.Today);

            if (!string.IsNullOrEmpty(config.Label))
            {
                UnityHelpers.Label(config.Label, styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label);
                layoutComponents.AddSpace(DesignTokens.Spacing.XS);
            }

            string buttonText = config.SelectedDate?.ToString("MMM dd, yyyy") ?? config.Placeholder ?? "Select date";
            bool clicked = UnityHelpers.Button(buttonText, styleManager?.GetButtonStyle(config.Variant, config.Size) ?? GUI.skin.button, config.LayoutOptions);

            if (Event.current.type == EventType.Repaint)
                _anchorRects[id] = GUILayoutUtility.GetLastRect();

            if (clicked)
            {
                if (IsDatePickerOpen(id))
                    CloseDatePicker(id);
                else
                    OpenDatePicker(id, config);
            }

            if (IsDatePickerOpen(id))
                UpdatePosition(id);

            return config.SelectedDate;
        }

        public DateTime? DrawDatePickerWithLabel(DatePickerConfig config) => DrawDatePicker(config);

        public DateTime? DrawDateRangePicker(DatePickerConfig config)
        {
            if (config == null)
                return null;

            string id = ResolveId(config.Id, "daterange");
            EnsureDisplayMonth(id, config.StartDate ?? DateTime.Today);

            string buttonText = config.StartDate.HasValue && config.EndDate.HasValue ? $"{config.StartDate.Value:MMM dd} - {config.EndDate.Value:MMM dd, yyyy}" : config.Placeholder ?? "Select range";

            bool clicked = UnityHelpers.Button(buttonText, styleManager?.GetButtonStyle(config.Variant, config.Size) ?? GUI.skin.button, config.LayoutOptions);

            if (Event.current.type == EventType.Repaint)
                _anchorRects[id] = GUILayoutUtility.GetLastRect();

            if (clicked)
            {
                if (IsDatePickerOpen(id))
                    CloseDatePicker(id);
                else
                    OpenRangePicker(id, config);
            }

            if (IsDatePickerOpen(id))
                UpdatePosition(id);

            return config.StartDate;
        }

        public DateTime? DrawDatePicker(string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            return DrawDatePicker(
                new DatePickerConfig
                {
                    Id = id,
                    Placeholder = placeholder,
                    SelectedDate = selectedDate,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public DateTime? DrawDateRangePicker(string placeholder, DateTime? startDate, DateTime? endDate, string id = "daterange", params GUILayoutOption[] options)
        {
            return DrawDateRangePicker(
                new DatePickerConfig
                {
                    Id = id,
                    Placeholder = placeholder,
                    StartDate = startDate,
                    EndDate = endDate,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public DateTime? DrawDateRangePicker(string placeholder, DateTime? startDate, DateTime? endDate, DateTime? minDate, DateTime? maxDate, string id = "daterange", params GUILayoutOption[] options)
        {
            return DrawDateRangePicker(
                new DatePickerConfig
                {
                    Id = id,
                    Placeholder = placeholder,
                    StartDate = startDate,
                    EndDate = endDate,
                    MinDate = minDate,
                    MaxDate = maxDate,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public DateTime? DrawDatePickerWithLabel(string label, string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            return DrawDatePicker(
                new DatePickerConfig
                {
                    Id = id,
                    Label = label,
                    Placeholder = placeholder,
                    SelectedDate = selectedDate,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public DateTime? DrawDatePickerWithLabel(string label, string placeholder, DateTime? selectedDate, DateTime? minDate, DateTime? maxDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            return DrawDatePicker(
                new DatePickerConfig
                {
                    Id = id,
                    Label = label,
                    Placeholder = placeholder,
                    SelectedDate = selectedDate,
                    MinDate = minDate,
                    MaxDate = maxDate,
                    LayoutOptions = options ?? Array.Empty<GUILayoutOption>(),
                }
            );
        }

        public void CloseDatePicker(string id) => LayerManager.Instance.Close(id);

        public bool IsDatePickerOpen(string id) => LayerManager.Instance.IsOpen(id);

        private void OpenDatePicker(string id, DatePickerConfig config)
        {
            Rect anchor = GetAnchorRect(id);
            Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(anchor.x, anchor.yMax + 4));

            LayerManager.Instance.Open(
                new LayerConfig
                {
                    Id = id,
                    OpenPosition = screenPos,
                    Width = GetPopupWidth(anchor),
                    Height = GetPopupHeight(),
                    CloseOnClickOutside = true,
                    ZIndex = DesignTokens.ZIndex.Popover,
                    Content = () => DrawCalendarPopup(id, config, isRange: false),
                }
            );
        }

        private void OpenRangePicker(string id, DatePickerConfig config)
        {
            Rect anchor = GetAnchorRect(id);
            Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(anchor.x, anchor.yMax + 4));

            LayerManager.Instance.Open(
                new LayerConfig
                {
                    Id = id,
                    OpenPosition = screenPos,
                    Width = GetPopupWidth(anchor),
                    Height = GetPopupHeight(),
                    CloseOnClickOutside = true,
                    ZIndex = DesignTokens.ZIndex.Popover,
                    Content = () => DrawCalendarPopup(id, config, isRange: true),
                }
            );
        }

        private void DrawCalendarPopup(string id, DatePickerConfig config, bool isRange)
        {
            var popupStyle = styleManager?.GetDatePickerStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.box;
            layoutComponents.BeginVerticalGroup(popupStyle, GUILayout.Width(GetPopupWidth(GetAnchorRect(id))));

            DrawHeader(id);
            DrawWeekdays();

            if (!isRange)
            {
                DateTime? picked = DrawGrid(id, config.SelectedDate, config.MinDate, config.MaxDate, null);
                if (picked.HasValue)
                {
                    config.SelectedDate = picked.Value;
                    CloseDatePicker(id);
                }
            }
            else
            {
                var range = GetRange(config);
                DateTime? highlight = range.Start.HasValue && !range.End.HasValue ? range.Start : null;
                DateTime? picked = DrawGrid(id, highlight, config.MinDate, config.MaxDate, range);
                if (picked.HasValue)
                {
                    HandleRangePick(id, picked.Value, config);
                }
            }

            layoutComponents.EndVerticalGroup();
        }

        private void DrawHeader(string id)
        {
            var ghost = styleManager?.GetButtonStyle(ControlVariant.Ghost, ControlSize.Default) ?? GUI.skin.button;
            layoutComponents.BeginHorizontalGroup();

            if (UnityHelpers.Button("<", ghost))
                _displayMonths[id] = _displayMonths[id].AddMonths(-1);

            GUILayout.FlexibleSpace();
            UnityHelpers.Label(_displayMonths[id].ToString("MMMM yyyy"), styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label);
            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button(">", ghost))
                _displayMonths[id] = _displayMonths[id].AddMonths(1);

            layoutComponents.EndHorizontalGroup();
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
        }

        private void DrawWeekdays()
        {
            var weekdayStyle = styleManager?.GetDatePickerWeekdayStyle() ?? GUI.skin.label;
            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < 7; i++)
                UnityHelpers.Label(Weekdays[i], weekdayStyle, GUILayout.Width(36f * guiHelper.uiScale));
            layoutComponents.EndHorizontalGroup();
            layoutComponents.AddSpace(DesignTokens.Spacing.XS);
        }

        private DateTime? DrawGrid(string id, DateTime? selectedDate, DateTime? min, DateTime? max, (DateTime? Start, DateTime? End)? range)
        {
            DateTime display = _displayMonths[id];
            DateTime first = new DateTime(display.Year, display.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(display.Year, display.Month);
            int firstIndex = (int)first.DayOfWeek;

            int day = 1;
            for (int row = 0; row < 6; row++)
            {
                layoutComponents.BeginHorizontalGroup();
                for (int col = 0; col < 7; col++)
                {
                    if ((row == 0 && col < firstIndex) || day > daysInMonth)
                    {
                        UnityHelpers.Label(string.Empty, styleManager?.GetDatePickerDayOutsideMonthStyle() ?? GUI.skin.label, GUILayout.Width(36f * guiHelper.uiScale));
                        continue;
                    }

                    DateTime current = new DateTime(display.Year, display.Month, day);
                    bool isToday = current.Date == DateTime.Today;
                    bool isSelected = selectedDate.HasValue && current.Date == selectedDate.Value.Date;
                    bool inRange = range.HasValue && range.Value.Start.HasValue && range.Value.End.HasValue && current.Date >= range.Value.Start.Value.Date && current.Date <= range.Value.End.Value.Date;

                    bool isDisabled = (min.HasValue && current.Date < min.Value.Date) || (max.HasValue && current.Date > max.Value.Date);

                    GUIStyle dayStyle = styleManager?.GetDatePickerDayStyle() ?? GUI.skin.button;
                    if (isSelected || inRange)
                        dayStyle = styleManager?.GetDatePickerDaySelectedStyle() ?? GUI.skin.button;
                    else if (isToday)
                        dayStyle = styleManager?.GetDatePickerDayTodayStyle() ?? GUI.skin.button;

                    bool wasEnabled = GUI.enabled;
                    if (isDisabled)
                        GUI.enabled = false;

                    bool clicked = UnityHelpers.Button(day.ToString(), dayStyle, GUILayout.Width(36f * guiHelper.uiScale), GUILayout.Height(28f * guiHelper.uiScale));

                    GUI.enabled = wasEnabled;

                    if (clicked && !isDisabled)
                    {
                        layoutComponents.EndHorizontalGroup();
                        return current;
                    }

                    day++;
                }
                layoutComponents.EndHorizontalGroup();
                if (day > daysInMonth)
                    break;
            }

            return null;
        }

        private void HandleRangePick(string id, DateTime picked, DatePickerConfig config)
        {
            if (!_rangeAnchor.TryGetValue(id, out var anchor) || !anchor.HasValue)
            {
                _rangeAnchor[id] = picked;
                config.StartDate = picked;
                config.EndDate = null;
                return;
            }

            DateTime start = anchor.Value;
            DateTime end = picked >= start ? picked : start;

            config.StartDate = start <= end ? start : end;
            config.EndDate = start <= end ? end : start;
            _rangeAnchor[id] = null;
            CloseDatePicker(id);
        }

        private (DateTime? Start, DateTime? End) GetRange(DatePickerConfig config)
        {
            if (config.StartDate.HasValue && config.EndDate.HasValue)
                return (config.StartDate, config.EndDate);

            string id = ResolveId(config.Id, "daterange");
            if (_rangeAnchor.TryGetValue(id, out var anchor) && anchor.HasValue)
                return (anchor.Value, null);

            return (null, null);
        }

        private void EnsureDisplayMonth(string id, DateTime date)
        {
            if (!_displayMonths.ContainsKey(id))
                _displayMonths[id] = new DateTime(date.Year, date.Month, 1);
        }

        private Rect GetAnchorRect(string id)
        {
            return _anchorRects.TryGetValue(id, out var rect) ? rect : new Rect(0f, 0f, 240f, 30f);
        }

        private float GetPopupWidth(Rect anchor) => Mathf.Max(anchor.width, 260f * guiHelper.uiScale);

        private float GetPopupHeight() => 320f * guiHelper.uiScale;

        private void UpdatePosition(string id)
        {
            Rect anchor = GetAnchorRect(id);
            Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(anchor.x, anchor.yMax + 4));
            LayerManager.Instance.SetPosition(id, screenPos);
        }

        private static string ResolveId(string id, string fallback)
        {
            if (!string.IsNullOrEmpty(id))
                return id;
            return fallback;
        }
    }
}
