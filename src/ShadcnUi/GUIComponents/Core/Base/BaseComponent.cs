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
        protected readonly GUIHelper guiHelper;
        protected readonly StyleManager styleManager;
        protected readonly Layout.Layout layoutComponents;
        protected bool isDisposed;

        private bool _initialized;

        protected BaseComponent(GUIHelper helper)
        {
            guiHelper = helper ?? throw new ArgumentNullException(nameof(helper));
            styleManager = helper.GetStyleManager();
            layoutComponents = new Layout.Layout(helper);
        }

        public void EnsureInitialized()
        {
            if (_initialized)
                return;

            _initialized = true;
            Initialize();
        }

        public virtual void Initialize() { }

        protected virtual void OnBeforeDispose() { }

        public virtual void Dispose()
        {
            if (isDisposed)
                return;

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
