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
        #region State

        private DateTime displayedMonth;

        public DateTime? SelectedDate { get; set; }

        public List<(DateTime Start, DateTime End)> Ranges { get; set; }
        private DateTime? pendingRangeStart;

        public List<DateTime> DisabledDates { get; set; }
        public Action<DateTime> OnDateSelected { get; set; }

        private bool showMonthDropdown;
        private bool showYearDropdown;
        private static readonly string[] DayOfWeekShort = { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa" };
        private const float AnimationDuration = DesignTokens.Animation.DurationNormal;

        #endregion

        #region Lifecycle

        public Calendar(GUIHelper helper)
            : base(helper) { }

        public override void Initialize()
        {
            this.displayedMonth = DateTime.Today;
            this.Ranges = new List<(DateTime Start, DateTime End)>();
            this.DisabledDates = new List<DateTime>();
        }

        public override void Dispose()
        {
            Ranges?.Clear();
            DisabledDates?.Clear();
            base.Dispose();
        }

        #endregion

        #region Config-based API

        public void DrawCalendar(CalendarConfig config = null)
        {
            var styleManager = guiHelper.GetStyleManager();
            config = config ?? new CalendarConfig();

            layoutComponents.BeginVerticalGroup(styleManager.GetCalendarStyle(config.Variant, config.Size));

            DrawHeader(styleManager);
            DrawWeekdays(styleManager);
            DrawDays(styleManager);

            layoutComponents.EndVerticalGroup();
        }

        #endregion

        #region Internal Drawing

        private void DrawHeader(StyleManager styleManager)
        {
            layoutComponents.BeginHorizontalGroup();

            GUIStyle buttonGhostStyle = styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Default);

            if (UnityHelpers.Button("<", buttonGhostStyle))
            {
                displayedMonth = displayedMonth.AddMonths(-1);
                var animManager = guiHelper.GetAnimationManager();
                animManager.StartFloat($"calendar_month_shift", 0f, 1f, AnimationDuration, EasingFunctions.EaseOutCubic);
            }

            if (showMonthDropdown) { }
            else
            {
                if (UnityHelpers.Button(displayedMonth.ToString("MMMM"), buttonGhostStyle))
                {
                    showMonthDropdown = true;
                    var animManager = guiHelper.GetAnimationManager();
                    animManager.FadeIn($"calendar_month_dropdown", AnimationDuration, EasingFunctions.EaseOutCubic);
                }
            }

            if (showYearDropdown) { }
            else
            {
                if (UnityHelpers.Button(displayedMonth.ToString("yyyy"), buttonGhostStyle))
                {
                    showYearDropdown = true;
                    var animManager = guiHelper.GetAnimationManager();
                    animManager.FadeIn($"calendar_year_dropdown", AnimationDuration, EasingFunctions.EaseOutCubic);
                }
            }

            if (UnityHelpers.Button(">", buttonGhostStyle))
            {
                displayedMonth = displayedMonth.AddMonths(1);
                var animManager = guiHelper.GetAnimationManager();
                animManager.StartFloat($"calendar_month_shift", 0f, 1f, AnimationDuration, EasingFunctions.EaseOutCubic);
            }

            layoutComponents.EndHorizontalGroup();
        }

        private void DrawWeekdays(StyleManager styleManager)
        {
            layoutComponents.BeginHorizontalGroup();
            GUIStyle weekdayStyle = styleManager.GetCalendarWeekdayStyle();
            for (int i = 0; i < 7; i++)
            {
                UnityHelpers.Label(DayOfWeekShort[i], weekdayStyle);
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
                                var animManager = guiHelper.GetAnimationManager();
                                animManager.StartFloat($"calendar_day_select_{dayCounter}", 0f, 1f, AnimationDuration * 0.5f, EasingFunctions.EaseOutCubic);
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

        #endregion

        #region Selection Handling

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

        #endregion
    }
}
