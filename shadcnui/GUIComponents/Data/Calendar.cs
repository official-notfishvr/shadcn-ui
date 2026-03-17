using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Data
{
    public class Calendar : BaseComponent
    {
        private static readonly string[] WeekdaysSunday = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
        private static readonly string[] WeekdaysMonday = { "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su" };

        private readonly Dictionary<string, DateTime> _displayMonths = new();
        private readonly Dictionary<string, DateTime?> _pendingRangeStart = new();

        public Calendar(GUIHelper helper)
            : base(helper) { }

        public void DrawCalendar(CalendarConfig config = null)
        {
            config ??= new CalendarConfig();
            string id = ResolveId(config.Id, "calendar");

            if (!_displayMonths.ContainsKey(id))
                _displayMonths[id] = config.SelectedDate ?? DateTime.Today;

            var style = styleManager?.GetCalendarStyle(config.Variant, config.Size) ?? GUI.skin.box;
            layoutComponents.BeginVerticalGroup(style, config.LayoutOptions ?? Array.Empty<GUILayoutOption>());

            DrawHeader(id, config);
            layoutComponents.AddSpace(DesignTokens.Spacing.SM);
            DrawWeekdays(config);
            DrawDays(id, config);

            layoutComponents.EndVerticalGroup();
        }

        private void DrawHeader(string id, CalendarConfig config)
        {
            var buttonStyle = styleManager?.GetButtonStyle(ControlVariant.Ghost, ControlSize.Default) ?? GUI.skin.button;

            layoutComponents.BeginHorizontalGroup();

            if (UnityHelpers.Button("<", buttonStyle))
                _displayMonths[id] = _displayMonths[id].AddMonths(-1);

            GUILayout.FlexibleSpace();

            UnityHelpers.Label(_displayMonths[id].ToString("MMMM yyyy"), styleManager?.GetLabelStyle(ControlVariant.Default, ControlSize.Default) ?? GUI.skin.label);

            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button(">", buttonStyle))
                _displayMonths[id] = _displayMonths[id].AddMonths(1);

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawWeekdays(CalendarConfig config)
        {
            var weekdayStyle = styleManager?.GetCalendarWeekdayStyle() ?? GUI.skin.label;
            var labels = WeekdaysSunday;

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < 7; i++)
            {
                UnityHelpers.Label(labels[i], weekdayStyle, GUILayout.Width(36f * guiHelper.uiScale));
            }
            layoutComponents.EndHorizontalGroup();
        }

        private void DrawDays(string id, CalendarConfig config)
        {
            var displayed = _displayMonths[id];
            DateTime firstOfMonth = new DateTime(displayed.Year, displayed.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(displayed.Year, displayed.Month);
            int firstDayIndex = (int)firstOfMonth.DayOfWeek;

            int dayCounter = 1;
            for (int row = 0; row < 6; row++)
            {
                layoutComponents.BeginHorizontalGroup();
                for (int col = 0; col < 7; col++)
                {
                    if ((row == 0 && col < firstDayIndex) || dayCounter > daysInMonth)
                    {
                        UnityHelpers.Label(string.Empty, styleManager?.GetCalendarDayOutsideMonthStyle() ?? GUI.skin.label, GUILayout.Width(36f * guiHelper.uiScale));
                        continue;
                    }

                    var currentDay = new DateTime(displayed.Year, displayed.Month, dayCounter);
                    bool isDisabled = config.DisabledDates?.Any(d => d.Date == currentDay.Date) == true;
                    bool isSelected = config.SelectedDate.HasValue && config.SelectedDate.Value.Date == currentDay.Date;
                    bool isToday = currentDay.Date == DateTime.Today;
                    bool inRange = config.Ranges?.Any(r => currentDay.Date >= r.Start.Date && currentDay.Date <= r.End.Date) == true;

                    GUIStyle dayStyle = styleManager?.GetCalendarDayStyle() ?? GUI.skin.button;
                    if (isDisabled)
                        dayStyle = styleManager?.GetCalendarDayStyle() ?? GUI.skin.button;
                    else if (isSelected)
                        dayStyle = styleManager?.GetCalendarDaySelectedStyle() ?? GUI.skin.button;
                    else if (inRange)
                        dayStyle = styleManager?.GetCalendarDayInRangeStyle() ?? GUI.skin.button;
                    else if (isToday)
                        dayStyle = styleManager?.GetCalendarDayTodayStyle() ?? GUI.skin.button;

                    bool wasEnabled = GUI.enabled;
                    if (isDisabled)
                        GUI.enabled = false;

                    if (UnityHelpers.Button(dayCounter.ToString(), dayStyle, GUILayout.Width(36f * guiHelper.uiScale), GUILayout.Height(28f * guiHelper.uiScale)))
                        HandleSelection(id, currentDay, config);

                    GUI.enabled = wasEnabled;
                    dayCounter++;
                }
                layoutComponents.EndHorizontalGroup();
                if (dayCounter > daysInMonth)
                    break;
            }
        }

        private void HandleSelection(string id, DateTime date, CalendarConfig config)
        {
            if (!_pendingRangeStart.TryGetValue(id, out var pending))
            {
                _pendingRangeStart[id] = date;
            }
            else
            {
                if (pending.HasValue)
                {
                    var start = pending.Value;
                    var end = date >= start ? date : start;
                    config.Ranges ??= new List<(DateTime Start, DateTime End)>();
                    config.Ranges.Add((start, end));
                    _pendingRangeStart[id] = null;
                }
            }

            config.SelectedDate = date;
            config.OnDateSelected?.Invoke(date);
        }

        private static string ResolveId(string id, string fallback)
        {
            if (!string.IsNullOrEmpty(id))
                return id;
            return fallback;
        }
    }
}
