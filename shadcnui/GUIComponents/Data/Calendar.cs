using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class Calendar
    {
        private GUIHelper guiHelper;
        private Layout layoutComponents;
        private DateTime displayedMonth;

        public DateTime? SelectedDate { get; set; }

        public List<(DateTime Start, DateTime End)> Ranges { get; set; }
        private DateTime? pendingRangeStart;

        public List<DateTime> DisabledDates { get; set; }
        public Action<DateTime> OnDateSelected { get; set; }

        private bool showMonthDropdown;
        private bool showYearDropdown;

        public Calendar(GUIHelper helper)
        {
            this.guiHelper = helper;
            this.layoutComponents = new Layout(helper);
            this.displayedMonth = DateTime.Today;

            this.Ranges = new List<(DateTime Start, DateTime End)>();
            this.DisabledDates = new List<DateTime>();
        }

        public void DrawCalendar()
        {
            var styleManager = guiHelper.GetStyleManager();

            DrawHeader(styleManager);
            DrawWeekdays(styleManager);
            DrawDays(styleManager);
        }

        private void DrawHeader(StyleManager styleManager)
        {
            layoutComponents.BeginHorizontalGroup();

#if IL2CPP
            if (GUILayout.Button("<", styleManager.buttonGhostStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0])))
#else
            if (GUILayout.Button("<", styleManager.buttonGhostStyle))
#endif
            {
                displayedMonth = displayedMonth.AddMonths(-1);
            }

            if (showMonthDropdown) { }
            else
            {
#if IL2CPP
                if (GUILayout.Button(displayedMonth.ToString("MMMM"), styleManager.buttonGhostStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0])))
#else
                if (GUILayout.Button(displayedMonth.ToString("MMMM"), styleManager.buttonGhostStyle))
#endif
                {
                    showMonthDropdown = true;
                }
            }

            if (showYearDropdown) { }
            else
            {
#if IL2CPP
                if (GUILayout.Button(displayedMonth.ToString("yyyy"), styleManager.buttonGhostStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0])))
#else
                if (GUILayout.Button(displayedMonth.ToString("yyyy"), styleManager.buttonGhostStyle))
#endif
                {
                    showYearDropdown = true;
                }
            }

#if IL2CPP
            if (GUILayout.Button(">", styleManager.buttonGhostStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0])))
#else
            if (GUILayout.Button(">", styleManager.buttonGhostStyle))
#endif
            {
                displayedMonth = displayedMonth.AddMonths(1);
            }

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawWeekdays(StyleManager styleManager)
        {
            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < 7; i++)
            {
#if IL2CPP
                GUILayout.Label(((DayOfWeek)i).ToString().Substring(0, 2), styleManager.calendarWeekdayStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
                GUILayout.Label(((DayOfWeek)i).ToString().Substring(0, 2), styleManager.calendarWeekdayStyle);
#endif
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
#if IL2CPP
                        GUILayout.Label("", styleManager.calendarDayOutsideMonthStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
                        GUILayout.Label("", styleManager.calendarDayOutsideMonthStyle);
#endif
                    }
                    else
                    {
                        var currentDay = new DateTime(displayedMonth.Year, displayedMonth.Month, dayCounter);
                        bool isDisabled = DisabledDates.Contains(currentDay.Date);

                        GUIStyle dayStyle = styleManager.calendarDayStyle;
                        if (isDisabled)
                        {
                            dayStyle = styleManager.calendarDayDisabledStyle;
                        }
                        else if (SelectedDate.HasValue && SelectedDate.Value.Date == currentDay.Date)
                        {
                            dayStyle = styleManager.calendarDaySelectedStyle;
                        }
                        else if (Ranges.Any(r => currentDay.Date >= r.Start.Date && currentDay.Date <= r.End.Date))
                        {
                            dayStyle = styleManager.calendarDayInRangeStyle;
                        }
                        else if (currentDay.Date == DateTime.Today)
                        {
                            dayStyle = styleManager.calendarDayTodayStyle;
                        }

#if IL2CPP
                        if (GUILayout.Button(dayCounter.ToString(), dayStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0])))
#else
                        if (GUILayout.Button(dayCounter.ToString(), dayStyle))
#endif
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
