using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TutorialBoxMessage : SLMonoBehaviour
{
    private TutorialBoxUI _tutorialBoxUI;
    [SerializeField] private string _message;
    private ViewController _viewController;

    protected override void SLStart()
    {
        base.SLStart();

        _viewController = ServiceLocator.Instance.GetService<ViewController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out _))
        {
            var go = Instantiate(_viewController.Get<TutorialBoxUI>(), _viewController.Canvas2D.transform);
            _tutorialBoxUI = go;

            _tutorialBoxUI.Setup(_message);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out _))
        {
            Destroy(_tutorialBoxUI.gameObject);
            _tutorialBoxUI = null;
        }
    }
}
