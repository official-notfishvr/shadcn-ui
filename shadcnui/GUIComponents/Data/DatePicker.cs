using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Styling;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Data
{
    public class DatePicker : BaseComponent
    {
        #region State

        private Dictionary<string, bool> _openStates = new Dictionary<string, bool>();
        private Dictionary<string, DateTime> _displayDates = new Dictionary<string, DateTime>();
        private Dictionary<string, DateTime> _focusedDates = new Dictionary<string, DateTime>();
        private bool _weekStartsMonday = true;
        private static readonly string[] WeekdaysMonday = { "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su" };
        private static readonly string[] WeekdaysSunday = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
        private const float AnimationDuration = DesignTokens.Animation.DurationNormal;

        #endregion

        #region Lifecycle

        public DatePicker(GUIHelper helper)
            : base(helper) { }

        public override void Initialize() { }

        public override void Dispose()
        {
            _openStates?.Clear();
            _displayDates?.Clear();
            _focusedDates?.Clear();
            base.Dispose();
        }

        #endregion

        #region Config-based API

        public DateTime? DrawDatePicker(DatePickerConfig config)
        {
            if (!_openStates.ContainsKey(config.Id))
            {
                _openStates[config.Id] = false;
                _displayDates[config.Id] = config.SelectedDate ?? DateTime.Today;
                _focusedDates[config.Id] = config.SelectedDate ?? DateTime.Today;
            }

            var styleManager = guiHelper.GetStyleManager();
            bool isOpen = _openStates[config.Id];
            DateTime displayDate = _displayDates[config.Id];
            string buttonText = config.SelectedDate?.ToString("MMM dd, yyyy") ?? config.Placeholder;

            layoutComponents.BeginVerticalGroup();

            if (guiHelper.Button($"{buttonText}", ControlVariant.Default, ControlSize.Default, null, false, 1f, config.Options))
            {
                _openStates[config.Id] = !isOpen;
                if (config.SelectedDate.HasValue)
                {
                    _displayDates[config.Id] = config.SelectedDate.Value;
                }

                var animManager = guiHelper.GetAnimationManager();
                if (!isOpen)
                {
                    animManager.FadeIn($"datepicker_popover_{config.Id}", AnimationDuration, EasingFunctions.EaseOutCubic);
                    animManager.ScaleIn($"datepicker_scale_{config.Id}", AnimationDuration, 0.92f, EasingFunctions.EaseOutCubic);
                    animManager.SlideIn($"datepicker_slide_{config.Id}", Vector2.zero, new Vector2(0, -DesignTokens.Spacing.LG), AnimationDuration, EasingFunctions.EaseOutCubic);
                }
                else
                {
                    animManager.FadeOut($"datepicker_popover_{config.Id}", AnimationDuration * 0.7f, EasingFunctions.EaseInCubic);
                    animManager.ScaleOut($"datepicker_scale_{config.Id}", AnimationDuration * 0.7f, 0.92f, EasingFunctions.EaseInCubic);
                }
            }

            if (_openStates[config.Id])
            {
                DateTime? newSelectedDate = DrawCalendarPopover(config.Id, config.SelectedDate, displayDate, config.MinDate, config.MaxDate);
                if (newSelectedDate != config.SelectedDate)
                {
                    _openStates[config.Id] = false;
                    var animManager = guiHelper.GetAnimationManager();
                    animManager.FadeOut($"datepicker_popover_{config.Id}", AnimationDuration * 0.7f, EasingFunctions.EaseInCubic);
                    return newSelectedDate;
                }
            }

            layoutComponents.EndVerticalGroup();

            return config.SelectedDate;
        }

        public DateTime? DrawDatePickerWithLabel(DatePickerConfig config)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginVerticalGroup();
            if (!string.IsNullOrEmpty(config.Label))
            {
                UnityHelpers.Label(config.Label, styleManager.GetLabelStyle(ControlVariant.Default));
                GUILayout.Space(DesignTokens.Spacing.XS);
            }

            DateTime? result = DrawDatePicker(config);

            layoutComponents.EndVerticalGroup();
            return result;
        }

        public DateTime? DrawDateRangePicker(DatePickerConfig config)
        {
            if (!_openStates.ContainsKey(config.Id))
            {
                _openStates[config.Id] = false;
                _displayDates[config.Id] = config.StartDate ?? DateTime.Today;
            }

            var styleManager = guiHelper.GetStyleManager();
            string buttonText = config.StartDate.HasValue && config.EndDate.HasValue ? $"{config.StartDate.Value.ToString("MMM dd")} - {config.EndDate.Value.ToString("MMM dd, yyyy")}" : config.Placeholder;

            layoutComponents.BeginVerticalGroup();

            if (guiHelper.Button($"{buttonText}", ControlVariant.Default, ControlSize.Default, null, false, 1f, config.Options))
            {
                _openStates[config.Id] = !_openStates[config.Id];

                var animManager = guiHelper.GetAnimationManager();
                if (_openStates[config.Id])
                {
                    animManager.FadeIn($"daterange_popover_{config.Id}", AnimationDuration, EasingFunctions.EaseOutCubic);
                    animManager.ScaleIn($"daterange_scale_{config.Id}", AnimationDuration, 0.92f, EasingFunctions.EaseOutCubic);
                    animManager.SlideIn($"daterange_slide_{config.Id}", Vector2.zero, new Vector2(0, -DesignTokens.Spacing.LG), AnimationDuration, EasingFunctions.EaseOutCubic);
                }
                else
                {
                    animManager.FadeOut($"daterange_popover_{config.Id}", AnimationDuration * 0.7f, EasingFunctions.EaseInCubic);
                    animManager.ScaleOut($"daterange_scale_{config.Id}", AnimationDuration * 0.7f, 0.92f, EasingFunctions.EaseInCubic);
                }
            }

            if (_openStates[config.Id])
            {
                DrawCalendarPopover(config.Id, config.StartDate, _displayDates[config.Id], config.MinDate, config.MaxDate);
            }

            layoutComponents.EndVerticalGroup();

            return config.StartDate;
        }

        #endregion

        #region API

        public DateTime? DrawDatePicker(string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            return DrawDatePicker(
                new DatePickerConfig
                {
                    Placeholder = placeholder,
                    SelectedDate = selectedDate,
                    Id = id,
                    Options = options,
                }
            );
        }

        public DateTime? DrawDatePickerWithLabel(string label, string placeholder, DateTime? selectedDate, string id = "datepicker", params GUILayoutOption[] options)
        {
            return DrawDatePickerWithLabel(
                new DatePickerConfig
                {
                    Label = label,
                    Placeholder = placeholder,
                    SelectedDate = selectedDate,
                    Id = id,
                    Options = options,
                }
            );
        }

        public DateTime? DrawDateRangePicker(string placeholder, DateTime? startDate, DateTime? endDate, string id = "daterange", params GUILayoutOption[] options)
        {
            return DrawDateRangePicker(
                new DatePickerConfig
                {
                    Placeholder = placeholder,
                    StartDate = startDate,
                    EndDate = endDate,
                    Id = id,
                    Options = options,
                }
            );
        }

        #endregion

        #region Internal Drawing

        private DateTime? DrawCalendarPopover(string id, DateTime? selectedDate, DateTime displayDate, DateTime? minDate, DateTime? maxDate)
        {
            var styleManager = guiHelper.GetStyleManager();
            var animManager = guiHelper.GetAnimationManager();

            float alpha = animManager.GetFloat($"datepicker_popover_{id}", 1f);
            if (alpha == 0f)
                alpha = animManager.GetFloat($"daterange_popover_{id}", 1f);

            float scale = animManager.GetFloat($"datepicker_scale_{id}", 1f);
            if (scale == 1f)
                scale = animManager.GetFloat($"daterange_scale_{id}", 1f);

            Vector2 slideOffset = animManager.GetVector2($"datepicker_slide_{id}", Vector2.zero);
            if (slideOffset == Vector2.zero)
                slideOffset = animManager.GetVector2($"daterange_slide_{id}", Vector2.zero);

            Color prevColor = GUI.color;
            Matrix4x4 prevMatrix = GUI.matrix;

            if (alpha < 1f)
                GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, prevColor.a * alpha);

            if (scale < 1f || slideOffset != Vector2.zero)
            {
                GUIUtility.ScaleAroundPivot(new Vector3(scale, scale, 1f), Vector2.zero);
                GUI.matrix = Matrix4x4.Translate(new Vector3(slideOffset.x, slideOffset.y, 0f)) * GUI.matrix;
            }

            layoutComponents.BeginVerticalGroup(styleManager.GetDatePickerStyle(ControlVariant.Default, ControlSize.Default), GUILayout.Width(280));

            DrawCalendarHeader(id, displayDate);
            DrawWeekdayHeaders();
            DateTime? newSelectedDate = DrawCalendarGrid(id, selectedDate, displayDate, minDate, maxDate);

            if (newSelectedDate.HasValue)
            {
                GUILayout.Space(DesignTokens.Spacing.SM);
                DrawCalendarFooter(id);
            }

            layoutComponents.EndVerticalGroup();

            GUI.matrix = prevMatrix;
            GUI.color = prevColor;

            return newSelectedDate;
        }

        private void DrawCalendarHeader(string id, DateTime displayDate)
        {
            var styleManager = guiHelper.GetStyleManager();
            layoutComponents.BeginHorizontalGroup();

            GUIStyle buttonGhostStyle = styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Default);

            if (UnityHelpers.Button("<", buttonGhostStyle))
            {
                _displayDates[id] = displayDate.AddMonths(-1);
                var animManager = guiHelper.GetAnimationManager();
                animManager.StartFloat($"datepicker_month_shift_{id}", 0f, 1f, AnimationDuration * 0.8f, EasingFunctions.EaseOutCubic);
            }

            if (UnityHelpers.Button(displayDate.ToString("MMMM"), buttonGhostStyle)) { }

            if (UnityHelpers.Button(displayDate.ToString("yyyy"), buttonGhostStyle)) { }

            if (UnityHelpers.Button(">", buttonGhostStyle))
            {
                _displayDates[id] = displayDate.AddMonths(1);
                var animManager = guiHelper.GetAnimationManager();
                animManager.StartFloat($"datepicker_month_shift_{id}", 0f, 1f, AnimationDuration * 0.8f, EasingFunctions.EaseOutCubic);
            }

            layoutComponents.EndHorizontalGroup();
            GUILayout.Space(DesignTokens.Spacing.SM);
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
            GUILayout.Space(DesignTokens.Spacing.XS);
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

                    if (UnityHelpers.Button(currentDate.Day.ToString(), dayStyle))
                    {
                        newSelectedDate = currentDate;
                        _focusedDates[id] = currentDate;
                        var animManager = guiHelper.GetAnimationManager();
                        animManager.StartFloat($"datepicker_day_select_{id}_{currentDate.Day}", 0f, 1f, AnimationDuration * 0.6f, EasingFunctions.EaseOutCubic);
                    }

                    GUI.enabled = wasEnabled;
                }

                layoutComponents.EndHorizontalGroup();

                if (week < 5)
                    GUILayout.Space(DesignTokens.Spacing.XXS);
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

            if (UnityHelpers.Button("Today", styleManager.GetButtonStyle(ControlVariant.Outline, ControlSize.Default), GUILayout.Height(32)))
            {
                _displayDates[id] = DateTime.Today;
                var animManager = guiHelper.GetAnimationManager();
                animManager.StartFloat($"datepicker_today_btn_{id}", 0f, 1f, AnimationDuration * 0.5f, EasingFunctions.EaseOutCubic);
            }

            GUILayout.FlexibleSpace();

            if (UnityHelpers.Button("Clear", styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Default), GUILayout.Height(32)))
            {
                _openStates[id] = false;
                var animManager = guiHelper.GetAnimationManager();
                animManager.FadeOut($"datepicker_popover_{id}", AnimationDuration * 0.7f, EasingFunctions.EaseInCubic);
                animManager.FadeOut($"daterange_popover_{id}", AnimationDuration * 0.7f, EasingFunctions.EaseInCubic);
            }

            layoutComponents.EndHorizontalGroup();
        }

        #endregion

        #region Public Helpers

        public void CloseDatePicker(string id)
        {
            if (_openStates.ContainsKey(id))
            {
                _openStates[id] = false;
                var animManager = guiHelper.GetAnimationManager();
                animManager.FadeOut($"datepicker_popover_{id}", AnimationDuration * 0.7f, EasingFunctions.EaseInCubic);
                animManager.FadeOut($"daterange_popover_{id}", AnimationDuration * 0.7f, EasingFunctions.EaseInCubic);
            }
        }

        public bool IsDatePickerOpen(string id)
        {
            return _openStates.ContainsKey(id) && _openStates[id];
        }

        #endregion
    }
}
