using View;

namespace Core
{
    public interface IView
    {
        void Open();
        void Close<T>(IController<T> controller) where T : IView;
    }
}