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

        public DatePicker(GUIHelper helper)
            : base(helper) { }

        public DateTime? DrawDatePicker(string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            if (!_openStates.ContainsKey(id))
            {
                _openStates[id] = false;
                _displayDates[id] = selectedDate ?? DateTime.Today;
            }

            var styleManager = guiHelper.GetStyleManager();
            bool isOpen = _openStates[id];
            DateTime displayDate = _displayDates[id];
            string buttonText = selectedDate?.ToString("MMM dd, yyyy") ?? placeholder;

            layoutComponents.BeginVerticalGroup();

            if (UnityHelpers.Button($"{buttonText}", styleManager.GetSelectTriggerStyle(), options))
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

            layoutComponents.EndVerticalGroup();

            return selectedDate;
        }

        public DateTime? DrawDatePickerWithLabel(string label, string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(label))
            {
                UnityHelpers.Label(label, styleManager.GetLabelStyle(LabelVariant.Default));
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

            if (UnityHelpers.Button($"{buttonText}", styleManager.GetSelectTriggerStyle(), options))
            {
                _openStates[id] = !_openStates[id];
            }

            if (_openStates[id])
            {
                DrawCalendarPopover(id, startDate, _displayDates[id]);
            }

            layoutComponents.EndVerticalGroup();

            return startDate;
        }

        private DateTime? DrawCalendarPopover(string id, DateTime? selectedDate, DateTime displayDate)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup(styleManager.GetPopoverContentStyle(), GUILayout.Width(280));

            DrawCalendarHeader(id, displayDate);
            DrawWeekdayHeaders();
            DateTime? newSelectedDate = DrawCalendarGrid(selectedDate, displayDate);

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

            if (UnityHelpers.Button("â€¹", styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default), GUILayout.Width(32), GUILayout.Height(32)))
            {
                _displayDates[id] = displayDate.AddMonths(-1);
            }

            GUILayout.FlexibleSpace();
            string currentMonthYear = displayDate.ToString("MMMM yyyy");

            UnityHelpers.Label(currentMonthYear, styleManager.GetDatePickerTitleStyle());
            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button("â€º", styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default), GUILayout.Width(32), GUILayout.Height(32)))
            {
                _displayDates[id] = displayDate.AddMonths(1);
            }

            layoutComponents.EndHorizontalGroup();
            GUILayout.Space(8);
        }

        private void DrawWeekdayHeaders()
        {
            var styleManager = guiHelper.GetStyleManager();
            string[] weekdays = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };

            layoutComponents.BeginHorizontalGroup();
            foreach (string day in weekdays)
            {
                UnityHelpers.Label(day, styleManager.GetDatePickerWeekdayStyle(), GUILayout.Width(36), GUILayout.Height(24));
            }
            layoutComponents.EndHorizontalGroup();
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
                layoutComponents.BeginHorizontalGroup();

                for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
                {
                    DateTime currentDate = firstDisplayDate.AddDays(week * 7 + dayOfWeek);
                    bool isCurrentMonth = currentDate.Month == displayDate.Month;
                    bool isSelected = selectedDate.HasValue && currentDate.Date == selectedDate.Value.Date;
                    bool isToday = currentDate.Date == DateTime.Today;

                    GUIStyle dayStyle = GetDayStyle(isCurrentMonth, isSelected, isToday);

                    if (UnityHelpers.Button(currentDate.Day.ToString(), dayStyle, GUILayout.Width(36), GUILayout.Height(32)))
                    {
                        newSelectedDate = currentDate;
                    }
                }

                layoutComponents.EndHorizontalGroup();

                if (week < 5)
                    GUILayout.Space(2);
            }

            return newSelectedDate;
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

            if (UnityHelpers.Button("Today", styleManager.GetButtonStyle(ButtonVariant.Outline, ButtonSize.Default), GUILayout.Height(32)))
            {
                _displayDates[id] = DateTime.Today;
            }

            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button("Clear", styleManager.GetButtonStyle(ButtonVariant.Ghost, ButtonSize.Default), GUILayout.Height(32)))
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
