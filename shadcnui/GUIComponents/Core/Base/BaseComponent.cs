using System;
using shadcnui.GUIComponents.Core.Styling;

namespace shadcnui.GUIComponents.Core.Base
{
    public interface IComponent : IDisposable
    {
        void Initialize();
    }

    public abstract class BaseComponent : IComponent
    {
        protected GUIHelper guiHelper;
        protected StyleManager styleManager;
        protected Layout.Layout layoutComponents;
        protected bool isDisposed = false;

        public BaseComponent(GUIHelper helper)
        {
            guiHelper = helper;
            styleManager = helper.GetStyleManager();
            layoutComponents = new Layout.Layout(helper);
            Initialize();
        }

        // not fully used yet
        public virtual void Initialize() { }

        // not fully used yet
        public virtual void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
            }
        }
    }
}
