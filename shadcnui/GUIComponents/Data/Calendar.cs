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

        public void Render(CalendarConfig config = null)
        {
            config ??= new CalendarConfig();
            string id = string.IsNullOrEmpty(config.Id) ? "calendar" : config.Id;
            DateTime selected = config.SelectedDate ?? DateTime.Today;
            if (!_visibleMonths.ContainsKey(id))
                _visibleMonths[id] = new DateTime(selected.Year, selected.Month, 1);

            var style = styleManager.GetCalendarStyle(config.Variant, config.Size, config.Appearance);
            layoutComponents.BeginVerticalGroup(style);
            CalendarRenderUtility.DrawMonthHeader(
                layoutComponents,
                styleManager.GetButtonStyle(ControlVariant.Ghost, ControlSize.Icon),
                styleManager.GetCardTitleStyle(ControlVariant.Default, ControlSize.Default, null),
                _visibleMonths[id],
                () => _visibleMonths[id] = _visibleMonths[id].AddMonths(-1),
                () => _visibleMonths[id] = _visibleMonths[id].AddMonths(1),
                DesignTokens.Spacing.SM
            );
            CalendarRenderUtility.DrawWeekdays(layoutComponents, styleManager.GetCalendarWeekdayStyle(config.Appearance), 36f * guiHelper.uiScale, DesignTokens.Spacing.XS);
            CalendarRenderUtility.DrawMonthGrid(layoutComponents, _visibleMonths[id], (current, activeMonth) => DrawDay(config, current, activeMonth), DesignTokens.Spacing.XXS);
            layoutComponents.EndVerticalGroup();
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
