using System;
using System.Collections.Generic;
using System.Linq;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Data
{
    public class Calendar : BaseComponent
    {
        private DateTime displayedMonth;

        public DateTime? SelectedDate { get; set; }

        public List<(DateTime Start, DateTime End)> Ranges { get; set; }
        private DateTime? pendingRangeStart;

        public List<DateTime> DisabledDates { get; set; }
        public Action<DateTime> OnDateSelected { get; set; }

        private bool showMonthDropdown;
        private bool showYearDropdown;

        public Calendar(GUIHelper helper)
            : base(helper)
        {
            this.displayedMonth = DateTime.Today;

            this.Ranges = new List<(DateTime Start, DateTime End)>();
            this.DisabledDates = new List<DateTime>();
        }

        public void DrawCalendar()
        {
            var styleManager = guiHelper.GetStyleManager();

            layoutComponents.BeginVerticalGroup(styleManager.GetCalendarStyle(CalendarVariant.Default, CalendarSize.Default));

            DrawHeader(styleManager);
            DrawWeekdays(styleManager);
            DrawDays(styleManager);

            layoutComponents.EndVerticalGroup();
        }

        private void DrawHeader(StyleManager styleManager)
        {
            layoutComponents.BeginHorizontalGroup();

            GUIStyle buttonGhostStyle = styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default);

            if (UnityHelpers.Button("<", buttonGhostStyle))
            {
                displayedMonth = displayedMonth.AddMonths(-1);
            }

            if (showMonthDropdown) { }
            else
            {
                if (UnityHelpers.Button(displayedMonth.ToString("MMMM"), buttonGhostStyle))
                {
                    showMonthDropdown = true;
                }
            }

            if (showYearDropdown) { }
            else
            {
                if (UnityHelpers.Button(displayedMonth.ToString("yyyy"), buttonGhostStyle))
                {
                    showYearDropdown = true;
                }
            }

            if (UnityHelpers.Button(">", buttonGhostStyle))
            {
                displayedMonth = displayedMonth.AddMonths(1);
            }

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawWeekdays(StyleManager styleManager)
        {
            layoutComponents.BeginHorizontalGroup();
            GUIStyle weekdayStyle = styleManager.GetCalendarWeekdayStyle();
            for (int i = 0; i < 7; i++)
            {
                UnityHelpers.Label(((DayOfWeek)i).ToString().Substring(0, 2), weekdayStyle);
            }
            layoutComponents.EndHorizontalGroup();
        }

        private void DrawDays(StyleManager styleManager)
        {
            int daysInMonth = DateTime.DaysInMonth(displayedMonth.Year, displayedMonth.Month);
            int firstDayOfMonth = (int)new DateTime(displayedMonth.Year, displayedMonth.Month, 1).DayOfWeek;

            int dayCounter = 1;
            for (int i = 0; i < 6; i++)
            {
                layoutComponents.BeginHorizontalGroup();
                for (int j = 0; j < 7; j++)
                {
                    if ((i == 0 && j < firstDayOfMonth) || dayCounter > daysInMonth)
                    {
                        UnityHelpers.Label("", styleManager.GetCalendarDayOutsideMonthStyle());
                    }
                    else
                    {
                        var currentDay = new DateTime(displayedMonth.Year, displayedMonth.Month, dayCounter);
                        bool isDisabled = DisabledDates.Contains(currentDay.Date);

                        GUIStyle dayStyle = styleManager.GetCalendarDayStyle();
                        if (isDisabled)
                        {
                            dayStyle = styleManager.GetCalendarDayStyle();
                        }
                        else if (SelectedDate.HasValue && SelectedDate.Value.Date == currentDay.Date)
                        {
                            dayStyle = styleManager.GetCalendarDaySelectedStyle();
                        }
                        else if (Ranges.Any(r => currentDay.Date >= r.Start.Date && currentDay.Date <= r.End.Date))
                        {
                            dayStyle = styleManager.GetCalendarDayInRangeStyle();
                        }
                        else if (currentDay.Date == DateTime.Today)
                        {
                            dayStyle = styleManager.GetCalendarDayTodayStyle();
                        }

                        if (UnityHelpers.Button(dayCounter.ToString(), dayStyle))
                        {
                            if (!isDisabled)
                            {
                                HandleDateSelection(currentDay);
                            }
                        }
                        dayCounter++;
                    }
                }
                layoutComponents.EndHorizontalGroup();
                if (dayCounter > daysInMonth)
                    break;
            }
        }

        private void HandleDateSelection(DateTime date)
        {
            if (pendingRangeStart.HasValue)
            {
                var start = pendingRangeStart.Value;
                var end = date >= start ? date : start;

                Ranges.Add((start, end));
                pendingRangeStart = null;
            }
            else
            {
                pendingRangeStart = date;
            }

            SelectedDate = date;
            if (OnDateSelected != null)
            {
                OnDateSelected(date);
            }
        }
    }
}
