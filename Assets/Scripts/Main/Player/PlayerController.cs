using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform CameraTransform => _cameraPosition;

    public Transform BodyTransform => _playerMovement.Body;

    public System.Action<float> OnGrab = delegate { };

    [SerializeField] private List<HandController> _hands;

    [SerializeField] private Transform _cameraPosition;

    [SerializeField] private PlayerWallet _playerWallet;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteract _playerInteract;
    [SerializeField] private Bobbing _playerBobbing;

    public HandController GetFreeHand()
    {
        var hand = _hands.FirstOrDefault(x => !x.IsBusy);

        return hand;
    }

    public void Initialize(BobbingTransforms transforms, List<HandController> hands)
    {
        _hands = hands;

        _playerBobbing.Initialize(transforms);
        _playerWallet.Initialize();
        _playerMovement.Initialize(_hands);
        _playerInteract.Initialize();

        OnGrab += _playerMovement.SetSpeedScale;
    }
}

public class BobbingTransforms
{
    public Transform CameraTransform, CameraHandlerTransform;
}
