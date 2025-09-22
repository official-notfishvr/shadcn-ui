using System;
using UnityEngine;
#if IL2CPP
using UnhollowerBaseLib;
#endif

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
            var styleManager = guiHelper.GetStyleManager();

            layoutComponents.BeginHorizontalGroup();
#if IL2CPP
            if (GUILayout.Button("<", styleManager.buttonGhostStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0])))
#else
            if (GUILayout.Button("<", styleManager.buttonGhostStyle))
#endif
            {
                selectedDate = selectedDate.AddMonths(-1);
            }

#if IL2CPP
            GUILayout.Label(selectedDate.ToString("MMMM yyyy"), styleManager.calendarTitleStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0]));
#else
            GUILayout.Label(selectedDate.ToString("MMMM yyyy"), styleManager.calendarTitleStyle);
#endif

#if IL2CPP
            if (GUILayout.Button(">", styleManager.buttonGhostStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0])))
#else
            if (GUILayout.Button(">", styleManager.buttonGhostStyle))
#endif
            {
                selectedDate = selectedDate.AddMonths(1);
            }
            layoutComponents.EndHorizontalGroup();

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

            int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
            int firstDayOfMonth = (int)new DateTime(selectedDate.Year, selectedDate.Month, 1).DayOfWeek;

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
                        GUIStyle dayStyle = styleManager.calendarDayStyle;
                        var currentDay = new DateTime(selectedDate.Year, selectedDate.Month, dayCounter);
                        if (currentDay == DateTime.Today)
                            dayStyle = styleManager.calendarDayTodayStyle;
                        if (currentDay == selectedDate)
                            dayStyle = styleManager.calendarDaySelectedStyle;

#if IL2CPP
                        if (GUILayout.Button(dayCounter.ToString(), dayStyle, new Il2CppReferenceArray<GUILayoutOption>(new GUILayoutOption[0])))
#else
                        if (GUILayout.Button(dayCounter.ToString(), dayStyle))
#endif
                        {
                            selectedDate = currentDay;
                        }
                        dayCounter++;
                    }
                }
                layoutComponents.EndHorizontalGroup();
                if (dayCounter > daysInMonth)
                    break;
            }
        }
    }
}
