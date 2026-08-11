using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private ISelectable _selectable;
    private CameraHandler _cameraHandler;

    private void Start()
    {
        _cameraHandler = ServiceLocator.Instance.GetService<CameraHandler>();
    }

    private void Update()
    {
        if(Physics.Raycast(_cameraHandler.Position, _cameraHandler.GetDirection(), out var hit))
        {
            if (hit.transform.TryGetComponent<ISelectable>(out _selectable))
            {
                print(_selectable);
                _selectable.Show();
                return;
            }
            else
            {
                _selectable?.Hide();
                _selectable = null;
                return;
            }
        }
        else
        {
            _selectable?.Hide();
            _selectable = null;
            return;
        }
    }
}
