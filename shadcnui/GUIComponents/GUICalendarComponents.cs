
using System;
using UnityEngine;

namespace shadcnui.GUIComponents
{
    public class GUICalendarComponents
    {
        private GUIHelper guiHelper;
        private GUILayoutComponents layoutComponents;
        private DateTime selectedDate;

        public GUICalendarComponents(GUIHelper helper)
        {
            this.guiHelper = helper;
            layoutComponents = new GUILayoutComponents(helper);
            selectedDate = DateTime.Today;
        }

        public void Calendar()
        {
            layoutComponents.BeginVerticalGroup(guiHelper.GetStyleManager().GetCalendarStyle(CalendarVariant.Default, CalendarSize.Default));

            layoutComponents.BeginHorizontalGroup(guiHelper.GetStyleManager().calendarHeaderStyle);
            if (GUILayout.Button("<", guiHelper.GetStyleManager().buttonGhostStyle))
            {
                selectedDate = selectedDate.AddMonths(-1);
            }
            GUILayout.Label(selectedDate.ToString("MMMM yyyy"), guiHelper.GetStyleManager().calendarTitleStyle);
            if (GUILayout.Button(">", guiHelper.GetStyleManager().buttonGhostStyle))
            {
                selectedDate = selectedDate.AddMonths(1);
            }
            layoutComponents.EndHorizontalGroup();

            layoutComponents.BeginHorizontalGroup();
            for (int i = 0; i < 7; i++)
            {
                GUILayout.Label(((DayOfWeek)i).ToString().Substring(0, 2), guiHelper.GetStyleManager().calendarWeekdayStyle);
            }
            layoutComponents.EndHorizontalGroup();

            int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
            int firstDayOfMonth = (int)new DateTime(selectedDate.Year, selectedDate.Month, 1).DayOfWeek;

            int dayCounter = 1;
            for (int i = 0; i < 6; i++)
            {
                layoutComponents.BeginHorizontalGroup();
                for (int j = 0; j < 7; j++)
                {
                    if (i == 0 && j < firstDayOfMonth)
                    {
                        GUILayout.Label("", guiHelper.GetStyleManager().calendarDayOutsideMonthStyle);
                    }
                    else if (dayCounter > daysInMonth)
                    {
                        GUILayout.Label("", guiHelper.GetStyleManager().calendarDayOutsideMonthStyle);
                    }
                    else
                    {
                        GUIStyle dayStyle = guiHelper.GetStyleManager().calendarDayStyle;
                        if (new DateTime(selectedDate.Year, selectedDate.Month, dayCounter) == DateTime.Today)
                        {
                            dayStyle = guiHelper.GetStyleManager().calendarDayTodayStyle;
                        }
                        if (new DateTime(selectedDate.Year, selectedDate.Month, dayCounter) == selectedDate)
                        {
                            dayStyle = guiHelper.GetStyleManager().calendarDaySelectedStyle;
                        }

                        if (GUILayout.Button(dayCounter.ToString(), dayStyle))
                        {
                            selectedDate = new DateTime(selectedDate.Year, selectedDate.Month, dayCounter);
                        }
                        dayCounter++;
                    }
                }
                layoutComponents.EndHorizontalGroup();
                if (dayCounter > daysInMonth)
                {
                    break;
                }
            }

            layoutComponents.EndVerticalGroup();
        }
    }
}
