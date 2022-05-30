namespace Core
{
    public interface IController<in TView>
        where TView : IView
    {
        void OnOpen(TView view);
        void OnClose(TView view);
    }
}