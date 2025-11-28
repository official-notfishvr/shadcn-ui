using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Data
{
    public class DatePicker : BaseComponent
    {
        private Dictionary<string, bool> _openStates = new Dictionary<string, bool>();
        private Dictionary<string, DateTime> _displayDates = new Dictionary<string, DateTime>();
        private Dictionary<string, DateTime> _focusedDates = new Dictionary<string, DateTime>();
        private bool _weekStartsMonday = true;
        private static readonly string[] WeekdaysMonday = { "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su" };
        private static readonly string[] WeekdaysSunday = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };

        public void SetWeekStartsMonday(bool value)
        {
            _weekStartsMonday = value;
        }

        public DatePicker(GUIHelper helper)
            : base(helper) { }

        public DateTime? DrawDatePicker(string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            if (!_openStates.ContainsKey(id))
            {
                _openStates[id] = false;
                _displayDates[id] = selectedDate ?? DateTime.Today;
                _focusedDates[id] = selectedDate ?? DateTime.Today;
            }

            var styleManager = guiHelper.GetStyleManager();
            bool isOpen = _openStates[id];
            DateTime displayDate = _displayDates[id];
            string buttonText = selectedDate?.ToString("MMM dd, yyyy") ?? placeholder;

            layoutComponents.BeginVerticalGroup();

            if (UnityHelpers.Button($"{buttonText}", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Default), options))
            {
                _openStates[id] = !isOpen;
                if (selectedDate.HasValue)
                {
                    _displayDates[id] = selectedDate.Value;
                }
            }

            if (_openStates[id])
            {
                DateTime? newSelectedDate = DrawCalendarPopover(id, selectedDate, displayDate, null, null);
                if (newSelectedDate != selectedDate)
                {
                    _openStates[id] = false;
                    return newSelectedDate;
                }
            }

            layoutComponents.EndVerticalGroup();

            return selectedDate;
        }

        public DateTime? DrawDatePickerWithLabel(string label, string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(label))
            {
                UnityHelpers.Label(label, styleManager.GetLabelStyle(ControlVariant.Default));
                GUILayout.Space(4);
            }

            DateTime? result = DrawDatePicker(placeholder, selectedDate, id, options);

            layoutComponents.EndVerticalGroup();
            return result;
        }

        public DateTime? DrawDateRangePicker(string placeholder, DateTime? startDate, DateTime? endDate, string id = "daterange", params GUILayoutOption[] options)
        {
            if (!_openStates.ContainsKey(id))
            {
                _openStates[id] = false;
                _displayDates[id] = startDate ?? DateTime.Today;
            }

            var styleManager = guiHelper.GetStyleManager();
            string buttonText = startDate.HasValue && endDate.HasValue ? $"{startDate.Value.ToString("MMM dd")} - {endDate.Value.ToString("MMM dd, yyyy")}" : placeholder;

            layoutComponents.BeginVerticalGroup();

            if (UnityHelpers.Button($"{buttonText}", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Default), options))
            {
                _openStates[id] = !_openStates[id];
            }

            if (_openStates[id])
            {
                DrawCalendarPopover(id, startDate, _displayDates[id], null, null);
            }

            layoutComponents.EndVerticalGroup();

            return startDate;
        }

        public DateTime? DrawDatePicker(string placeholder, DateTime? selectedDate, DateTime? minDate, DateTime? maxDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            if (!_openStates.ContainsKey(id))
            {
                _openStates[id] = false;
                _displayDates[id] = selectedDate ?? DateTime.Today;
                _focusedDates[id] = selectedDate ?? DateTime.Today;
            }

            var styleManager = guiHelper.GetStyleManager();
            bool isOpen = _openStates[id];
            DateTime displayDate = _displayDates[id];
            string buttonText = selectedDate?.ToString("MMM dd, yyyy") ?? placeholder;

            layoutComponents.BeginVerticalGroup();

            if (UnityHelpers.Button($"{buttonText}", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Default), options))
            {
                _openStates[id] = !isOpen;
                if (selectedDate.HasValue)
                {
                    _displayDates[id] = selectedDate.Value;
                }
            }

            if (_openStates[id])
            {
                DateTime? newSelectedDate = DrawCalendarPopover(id, selectedDate, displayDate, minDate, maxDate);
                if (newSelectedDate != selectedDate)
                {
                    _openStates[id] = false;
                    return newSelectedDate;
                }
            }

            layoutComponents.EndVerticalGroup();

            return selectedDate;
        }

        public DateTime? DrawDateRangePicker(string placeholder, DateTime? startDate, DateTime? endDate, DateTime? minDate, DateTime? maxDate, string id = "daterange", params GUILayoutOption[] options)
        {
            if (!_openStates.ContainsKey(id))
            {
                _openStates[id] = false;
                _displayDates[id] = startDate ?? DateTime.Today;
            }

            var styleManager = guiHelper.GetStyleManager();
            string buttonText = startDate.HasValue && endDate.HasValue ? $"{startDate.Value.ToString("MMM dd")} - {endDate.Value.ToString("MMM dd, yyyy")}" : placeholder;

            layoutComponents.BeginVerticalGroup();

            if (UnityHelpers.Button($"{buttonText}", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Default), options))
            {
                _openStates[id] = !_openStates[id];
            }

            if (_openStates[id])
            {
                DrawCalendarPopover(id, startDate, _displayDates[id], minDate, maxDate);
            }

            layoutComponents.EndVerticalGroup();

            return startDate;
        }

        private DateTime? DrawCalendarPopover(string id, DateTime? selectedDate, DateTime displayDate, DateTime? minDate, DateTime? maxDate)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup(styleManager.GetDatePickerStyle(ControlVariant.Default, ControlSize.Default), GUILayout.Width(280));

            DrawCalendarHeader(id, displayDate);
            DrawWeekdayHeaders();
            DateTime? newSelectedDate = DrawCalendarGrid(id, selectedDate, displayDate, minDate, maxDate);
            HandleCalendarKeyboard(id, ref newSelectedDate);

            if (newSelectedDate.HasValue)
            {
                GUILayout.Space(8);
                DrawCalendarFooter(id);
            }

            layoutComponents.EndVerticalGroup();

            return newSelectedDate;
        }

        private void DrawCalendarHeader(string id, DateTime displayDate)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginHorizontalGroup();

            if (UnityHelpers.Button("<<", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Icon)))
            {
                _displayDates[id] = displayDate.AddYears(-1);
            }

            if (UnityHelpers.Button("<", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Icon)))
            {
                _displayDates[id] = displayDate.AddMonths(-1);
            }

            GUILayout.FlexibleSpace();
            string currentMonthYear = displayDate.ToString("MMMM yyyy");

            UnityHelpers.Label(currentMonthYear, styleManager.GetLabelStyle(ControlVariant.Default, ControlSize.Large));
            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button(">", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Icon)))
            {
                _displayDates[id] = displayDate.AddMonths(1);
            }

            if (UnityHelpers.Button(">>", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Icon)))
            {
                _displayDates[id] = displayDate.AddYears(1);
            }

            layoutComponents.EndHorizontalGroup();
            GUILayout.Space(8);
        }

        private void DrawWeekdayHeaders()
        {
            var styleManager = guiHelper.GetStyleManager();
            string[] weekdays = _weekStartsMonday ? WeekdaysMonday : WeekdaysSunday;

            layoutComponents.BeginHorizontalGroup();
            foreach (string day in weekdays)
            {
                UnityHelpers.Label(day, styleManager.GetDatePickerWeekdayStyle(), GUILayout.Width(36), GUILayout.Height(24));
            }
            layoutComponents.EndHorizontalGroup();
            GUILayout.Space(4);
        }

        private DateTime? DrawCalendarGrid(string id, DateTime? selectedDate, DateTime displayDate, DateTime? minDate, DateTime? maxDate)
        {
            DateTime firstDayOfMonth = new DateTime(displayDate.Year, displayDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(displayDate.Year, displayDate.Month);
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            if (_weekStartsMonday)
            {
                firstDayOfWeek = (firstDayOfWeek == 0) ? 6 : firstDayOfWeek - 1;
            }

            DateTime firstDisplayDate = firstDayOfMonth.AddDays(-firstDayOfWeek);
            DateTime? newSelectedDate = selectedDate;

            for (int week = 0; week < 6; week++)
            {
                layoutComponents.BeginHorizontalGroup();

                for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
                {
                    DateTime currentDate = firstDisplayDate.AddDays(week * 7 + dayOfWeek);
                    bool isCurrentMonth = currentDate.Month == displayDate.Month;
                    bool isSelected = selectedDate.HasValue && currentDate.Date == selectedDate.Value.Date;
                    bool isToday = currentDate.Date == DateTime.Today;

                    GUIStyle dayStyle = GetDayStyle(isCurrentMonth, isSelected, isToday);

                    bool withinRange = (!minDate.HasValue || currentDate.Date >= minDate.Value.Date) && (!maxDate.HasValue || currentDate.Date <= maxDate.Value.Date);
                    bool wasEnabled = GUI.enabled;
                    if (!withinRange)
                        GUI.enabled = false;

                    if (UnityHelpers.Button(currentDate.Day.ToString(), dayStyle, GUILayout.Width(36), GUILayout.Height(32)))
                    {
                        newSelectedDate = currentDate;
                        _focusedDates[id] = currentDate;
                    }

                    GUI.enabled = wasEnabled;
                }

                layoutComponents.EndHorizontalGroup();

                if (week < 5)
                    GUILayout.Space(2);
            }

            return newSelectedDate;
        }

        private void HandleCalendarKeyboard(string id, ref DateTime? selected)
        {
            if (Event.current.type != EventType.KeyDown)
                return;

            if (!_focusedDates.ContainsKey(id))
                _focusedDates[id] = _displayDates.ContainsKey(id) ? _displayDates[id] : DateTime.Today;

            DateTime focus = _focusedDates[id];
            bool used = false;
            switch (Event.current.keyCode)
            {
                case KeyCode.LeftArrow:
                    focus = focus.AddDays(-1);
                    used = true;
                    break;
                case KeyCode.RightArrow:
                    focus = focus.AddDays(1);
                    used = true;
                    break;
                case KeyCode.UpArrow:
                    focus = focus.AddDays(-7);
                    used = true;
                    break;
                case KeyCode.DownArrow:
                    focus = focus.AddDays(7);
                    used = true;
                    break;
                case KeyCode.Return:
                    selected = focus;
                    used = true;
                    break;
                case KeyCode.Escape:
                    _openStates[id] = false;
                    used = true;
                    break;
            }

            if (used)
            {
                _focusedDates[id] = focus;
                _displayDates[id] = new DateTime(focus.Year, focus.Month, 1);
                Event.current.Use();
            }
        }

        private GUIStyle GetDayStyle(bool isCurrentMonth, bool isSelected, bool isToday)
        {
            var styleManager = guiHelper.GetStyleManager();

            if (isSelected)
                return styleManager.GetDatePickerDaySelectedStyle();

            if (isToday)
                return styleManager.GetDatePickerDayTodayStyle();

            if (!isCurrentMonth)
                return styleManager.GetDatePickerDayOutsideMonthStyle();

            return styleManager.GetDatePickerDayStyle();
        }

        private void DrawCalendarFooter(string id)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginHorizontalGroup();

            if (UnityHelpers.Button("Today", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Default), GUILayout.Height(32)))
            {
                _displayDates[id] = DateTime.Today;
            }

            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button("Clear", styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Default), GUILayout.Height(32)))
            {
                _openStates[id] = false;
            }

            layoutComponents.EndHorizontalGroup();
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
