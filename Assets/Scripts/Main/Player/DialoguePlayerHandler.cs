public class DialoguePlayerHandler : SLMonoBehaviour
{
    private DialogueSystem _dialogueSystem;
    private InputHandler _inputHandler;

    protected override void SLStart()
    {
        base.SLStart();

        _dialogueSystem = ServiceLocator.Instance.GetService<DialogueSystem>();
        _inputHandler = ServiceLocator.Instance.GetService<InputHandler>();

        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<UIGameState>().OnStartEvent += () =>
        {
            _inputHandler.OnJumpEvent += _dialogueSystem.SkipTyping;
        };
        ServiceLocator.Instance.GetService<GameStateMachine>().GetState<UIGameState>().OnExitEvent += () =>
        {
            _inputHandler.OnJumpEvent -= _dialogueSystem.SkipTyping;
        };
    }
}
