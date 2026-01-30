using System;
using shadcnui.GUIComponents.Core.Styling;

namespace shadcnui.GUIComponents.Core.Base
{
    public interface IComponent : IDisposable
    {
        void Initialize();
        void EnsureInitialized();
    }

    public abstract class BaseComponent : IComponent
    {
        protected GUIHelper guiHelper;
        protected StyleManager styleManager;
        protected Layout.Layout layoutComponents;
        protected bool isDisposed = false;
        private bool _initialized = false;

        public BaseComponent(GUIHelper helper)
        {
            guiHelper = helper;
            styleManager = helper.GetStyleManager();
            layoutComponents = new Layout.Layout(helper);
        }

        public void EnsureInitialized()
        {
            if (!_initialized)
            {
                _initialized = true;
                Initialize();
            }
        }

        public virtual void Initialize() { }

        protected virtual void OnBeforeDispose() { }

        public virtual void Dispose()
        {
            if (!isDisposed)
            {
                try
                {
                    OnBeforeDispose();
                }
                finally
                {
                    isDisposed = true;
                }
            }
        }
    }
}
