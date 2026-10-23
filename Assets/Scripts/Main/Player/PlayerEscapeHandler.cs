public class PlayerEscapeHandler : SLMonoBehaviour
{
    private InputHandler _inputHandler;
    private ViewController _viewController;
    //there also should be pause manager;

    protected override void SLStart()
    {
        base.SLStart();
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>();
        _viewController = ServiceLocator.Instance.GetService<ViewController>();

        _inputHandler.OnEscapeEvent += () =>
        {
            var view = _viewController.GetCurrentView();
            //if(view is null) do pause
            //if(view is MenuView) do unpause

            if (view != null) _viewController.ClearCurrentView();
        };
    }
}
