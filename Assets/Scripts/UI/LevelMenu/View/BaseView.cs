using Core;
using UnityEngine;
using UnityEngine.UI;

namespace View
{

    public abstract class BaseView<TView> : MonoBehaviour, IView
        where TView : IView
    {
        public Button firstSelectedButton;
        protected abstract TView View { get; }

        protected abstract IController<T> CreateController<T>()
            where T : IView;

        public void Open()
        {
            firstSelectedButton?.Select();
            var controller = CreateController<TView>();
            if (View is TView view)
                controller?.OnOpen(view);
            gameObject.SetActive(true);
        }

        public void Close<T>(IController<T> controller)
            where T : IView
        {
            gameObject.SetActive(false);
            if (View is T view)
                controller?.OnClose(view);
        }
    }
}
