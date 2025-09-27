using System;
using System.Collections.Generic;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents
{
    public class DatePicker
    {
        private readonly GUIHelper _guiHelper;
        private readonly StyleManager _styleManager;
        private readonly Layout _layoutComponents;

        private Dictionary<string, bool> _openStates = new Dictionary<string, bool>();
        private Dictionary<string, DateTime> _displayDates = new Dictionary<string, DateTime>();

        public DatePicker(GUIHelper helper)
        {
            _guiHelper = helper;
            _styleManager = helper.GetStyleManager();
            _layoutComponents = new Layout(helper);
        }

        public DateTime? DrawDatePicker(string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            if (!_openStates.ContainsKey(id))
            {
                _openStates[id] = false;
                _displayDates[id] = selectedDate ?? DateTime.Today;
            }

            bool isOpen = _openStates[id];
            DateTime displayDate = _displayDates[id];
            string buttonText = selectedDate?.ToString("MMM dd, yyyy") ?? placeholder;

            _layoutComponents.BeginVerticalGroup();

#if IL2CPP
            if (GUILayout.Button($"📅 {buttonText}", _styleManager.selectTriggerStyle, new Il2CppReferenceArray<GUILayoutOption>(options)))
#else
            if (GUILayout.Button($"📅 {buttonText}", _styleManager.selectTriggerStyle, options))
#endif
            {
                _openStates[id] = !isOpen;
                if (selectedDate.HasValue)
                {
                    _displayDates[id] = selectedDate.Value;
                }
            }

            if (_openStates[id])
            {
                DateTime? newSelectedDate = DrawCalendarPopover(id, selectedDate, displayDate);
                if (newSelectedDate != selectedDate)
                {
                    _openStates[id] = false;
                    return newSelectedDate;
                }
            }

            _layoutComponents.EndVerticalGroup();

            return selectedDate;
        }

        public DateTime? DrawDatePickerWithLabel(string label, string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            _layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(label))
            {
#if IL2CPP
                GUILayout.Label(new GUIContent(label), _styleManager.labelDefaultStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
                GUILayout.Label(label, _styleManager.labelDefaultStyle);
#endif
                GUILayout.Space(4);
            }

            DateTime? result = DrawDatePicker(placeholder, selectedDate, id, options);

            _layoutComponents.EndVerticalGroup();
            return result;
        }

        public DateTime? DrawDateRangePicker(string placeholder, DateTime? startDate, DateTime? endDate, string id = "daterange", params GUILayoutOption[] options)
        {
            if (!_openStates.ContainsKey(id))
            {
                _openStates[id] = false;
                _displayDates[id] = startDate ?? DateTime.Today;
            }

            string buttonText = startDate.HasValue && endDate.HasValue ? $"{startDate.Value.ToString("MMM dd")} - {endDate.Value.ToString("MMM dd, yyyy")}" : placeholder;

            _layoutComponents.BeginVerticalGroup();

#if IL2CPP
            if (GUILayout.Button($"📅 {buttonText}", _styleManager.selectTriggerStyle, new Il2CppReferenceArray<GUILayoutOption>(options)))
#else
            if (GUILayout.Button($"📅 {buttonText}", _styleManager.selectTriggerStyle, options))
#endif
            {
                _openStates[id] = !_openStates[id];
            }

            if (_openStates[id])
            {
                DrawCalendarPopover(id, startDate, _displayDates[id]);
            }

            _layoutComponents.EndVerticalGroup();

            return startDate;
        }

        private DateTime? DrawCalendarPopover(string id, DateTime? selectedDate, DateTime displayDate)
        {
#if IL2CPP
            _layoutComponents.BeginVerticalGroup(_styleManager.popoverContentStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(280) }));
#else
            _layoutComponents.BeginVerticalGroup(_styleManager.popoverContentStyle, GUILayout.Width(280));
#endif

            DrawCalendarHeader(id, displayDate);
            DrawWeekdayHeaders();
            DateTime? newSelectedDate = DrawCalendarGrid(selectedDate, displayDate);

            if (newSelectedDate.HasValue)
            {
                GUILayout.Space(8);
                DrawCalendarFooter(id);
            }

            _layoutComponents.EndVerticalGroup();

            return newSelectedDate;
        }

        private void DrawCalendarHeader(string id, DateTime displayDate)
        {
            _layoutComponents.BeginHorizontalGroup();

#if IL2CPP
            if (GUILayout.Button(new GUIContent("‹"), _styleManager.buttonGhostStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(32), GUILayout.Height(32) })))
#else
            if (GUILayout.Button("‹", _styleManager.buttonGhostStyle, GUILayout.Width(32), GUILayout.Height(32)))
#endif
            {
                _displayDates[id] = displayDate.AddMonths(-1);
            }

            GUILayout.FlexibleSpace();
            string currentMonthYear = displayDate.ToString("MMMM yyyy");
#if IL2CPP
            GUILayout.Label(new GUIContent(currentMonthYear), _styleManager.datePickerTitleStyle, Layout.EmptyOptions);
#else
            GUILayout.Label(currentMonthYear, _styleManager.datePickerTitleStyle);
#endif
            GUILayout.FlexibleSpace();

#if IL2CPP
            if (GUILayout.Button(new GUIContent("›"), _styleManager.buttonGhostStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(32), GUILayout.Height(32) })))
#else
            if (GUILayout.Button("›", _styleManager.buttonGhostStyle, GUILayout.Width(32), GUILayout.Height(32)))
#endif
            {
                _displayDates[id] = displayDate.AddMonths(1);
            }

            _layoutComponents.EndHorizontalGroup();
            GUILayout.Space(8);
        }

        private void DrawWeekdayHeaders()
        {
            string[] weekdays = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };

            _layoutComponents.BeginHorizontalGroup();
            foreach (string day in weekdays)
            {
#if IL2CPP
                GUILayout.Label(new GUIContent(day), _styleManager.datePickerWeekdayStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(36), GUILayout.Height(24) }));
#else
                GUILayout.Label(day, _styleManager.datePickerWeekdayStyle, GUILayout.Width(36), GUILayout.Height(24));
#endif
            }
            _layoutComponents.EndHorizontalGroup();
            GUILayout.Space(4);
        }

        private DateTime? DrawCalendarGrid(DateTime? selectedDate, DateTime displayDate)
        {
            DateTime firstDayOfMonth = new DateTime(displayDate.Year, displayDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(displayDate.Year, displayDate.Month);
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            DateTime firstDisplayDate = firstDayOfMonth.AddDays(-firstDayOfWeek);
            DateTime? newSelectedDate = selectedDate;

            for (int week = 0; week < 6; week++)
            {
                _layoutComponents.BeginHorizontalGroup();

                for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
                {
                    DateTime currentDate = firstDisplayDate.AddDays(week * 7 + dayOfWeek);
                    bool isCurrentMonth = currentDate.Month == displayDate.Month;
                    bool isSelected = selectedDate.HasValue && currentDate.Date == selectedDate.Value.Date;
                    bool isToday = currentDate.Date == DateTime.Today;

                    GUIStyle dayStyle = GetDayStyle(isCurrentMonth, isSelected, isToday);

#if IL2CPP
                    if (GUILayout.Button(new GUIContent(currentDate.Day.ToString()), dayStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Width(36), GUILayout.Height(32) })))
#else
                    if (GUILayout.Button(currentDate.Day.ToString(), dayStyle, GUILayout.Width(36), GUILayout.Height(32)))
#endif
                    {
                        newSelectedDate = currentDate;
                    }
                }

                _layoutComponents.EndHorizontalGroup();

                if (week < 5)
                    GUILayout.Space(2);
            }

            return newSelectedDate;
        }

        private GUIStyle GetDayStyle(bool isCurrentMonth, bool isSelected, bool isToday)
        {
            if (isSelected)
                return _styleManager.datePickerDaySelectedStyle;

            if (isToday)
                return _styleManager.datePickerDayTodayStyle;

            if (!isCurrentMonth)
                return _styleManager.datePickerDayOutsideMonthStyle;

            return _styleManager.datePickerDayStyle;
        }

        private void DrawCalendarFooter(string id)
        {
            _layoutComponents.BeginHorizontalGroup();

#if IL2CPP
            if (GUILayout.Button(new GUIContent("Today"), _styleManager.buttonOutlineStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Height(32) })))
#else
            if (GUILayout.Button("Today", _styleManager.buttonOutlineStyle, GUILayout.Height(32)))
#endif
            {
                _displayDates[id] = DateTime.Today;
            }

            GUILayout.FlexibleSpace();

#if IL2CPP
            if (GUILayout.Button(new GUIContent("Clear"), _styleManager.buttonGhostStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[] { GUILayout.Height(32) })))
#else
            if (GUILayout.Button("Clear", _styleManager.buttonGhostStyle, GUILayout.Height(32)))
#endif
            {
                _openStates[id] = false;
            }

            _layoutComponents.EndHorizontalGroup();
        }

        public void CloseDatePicker(string id)
        {
            if (_openStates.ContainsKey(id))
            {
                _openStates[id] = false;
            }
        }

        public bool IsDatePickerOpen(string id)
        {
            return _openStates.ContainsKey(id) && _openStates[id];
        }
    }
}
