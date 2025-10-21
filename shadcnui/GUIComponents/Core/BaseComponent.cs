using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shadcnui.GUIComponents.Core
{
    public abstract class BaseComponent
    {
        protected GUIHelper guiHelper;
        protected StyleManager styleManager;
        protected Layout.Layout layoutComponents;

        public BaseComponent(GUIHelper helper)
        {
            guiHelper = helper;
            styleManager = helper.GetStyleManager();
            layoutComponents = new Layout.Layout(helper);
        }

        protected void LogException(Exception ex, string methodName)
        {
            GUILogger.LogException(ex, methodName, GetType().Name);
        }
    }
}
