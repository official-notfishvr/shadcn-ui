using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Data
{
    public class Calendar : BaseComponent
    {
        private readonly Dictionary<string, DateTime> _visibleMonths = new();

        public Calendar(GUIHelper helper)
            : base(helper) { }

        public void DrawCalendar(CalendarConfig config = null)
        {
            config ??= new CalendarConfig();
            string id = string.IsNullOrEmpty(config.Id) ? "calendar" : config.Id;
            DateTime selected = config.SelectedDate ?? DateTime.Today;
            if (!_visibleMonths.ContainsKey(id))
                _visibleMonths[id] = new DateTime(selected.Year, selected.Month, 1);

            var style = styleManager.GetCalendarStyle(config.Variant, config.Size, config.Appearance);
            layoutComponents.BeginVerticalGroup(style);
            DrawHeader(id);
            DrawWeekdays(styleManager.GetCalendarWeekdayStyle(config.Appearance));
            DrawMonthGrid(config, id);
            layoutComponents.EndVerticalGroup();
        }

        private void DrawHeader(string id)
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

        private void DrawWeekdays(GUIStyle weekdayStyle)
        {
            layoutComponents.BeginHorizontalGroup();
            foreach (var day in new[] { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" })
                GUILayout.Label(day, weekdayStyle, GUILayout.Width(36f * guiHelper.uiScale));
            layoutComponents.EndHorizontalGroup();
            layoutComponents.AddSpace(DesignTokens.Spacing.XS);
        }

        private void DrawMonthGrid(CalendarConfig config, string id)
        {
            DateTime month = _visibleMonths[id];
            DateTime first = new DateTime(month.Year, month.Month, 1);
            int startOffset = (int)first.DayOfWeek;
            DateTime gridStart = first.AddDays(-startOffset);

            for (int week = 0; week < 6; week++)
            {
                layoutComponents.BeginHorizontalGroup();
                for (int day = 0; day < 7; day++)
                {
                    DateTime current = gridStart.AddDays(week * 7 + day);
                    DrawDay(config, current, month.Month);
                }
                layoutComponents.EndHorizontalGroup();
                if (week < 5)
                    layoutComponents.AddSpace(DesignTokens.Spacing.XXS);
            }
        }

        private void DrawDay(CalendarConfig config, DateTime date, int activeMonth)
        {
            bool selected = config.SelectedDate.HasValue && config.SelectedDate.Value.Date == date.Date;
            bool today = date.Date == DateTime.Today;
            bool outside = date.Month != activeMonth;
            bool disabled = config.DisabledDates != null && config.DisabledDates.Contains(date.Date);

            GUIStyle style =
                selected ? styleManager.GetCalendarDaySelectedStyle(config.Appearance)
                : today ? styleManager.GetCalendarDayTodayStyle(config.Appearance)
                : outside ? styleManager.GetCalendarDayOutsideMonthStyle(config.Appearance)
                : styleManager.GetCalendarDayStyle(config.Appearance);

            bool prev = GUI.enabled;
            GUI.enabled = !disabled;
            if (GUILayout.Button(date.Day.ToString(), style, GUILayout.Width(36f * guiHelper.uiScale), GUILayout.Height(36f * guiHelper.uiScale)))
                config.OnDateSelected?.Invoke(date.Date);
            GUI.enabled = prev;
        }
    }
}
