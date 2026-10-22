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

    [SerializeField] private float _xForceScale = 1f, _yForceScale = 1f;


    [SerializeField] private PlayerWallet _playerWallet;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInteract _playerInteract;
    [SerializeField] private Bobbing _playerBobbing;

    private void Update()
    {
        for(int i = 0; i < _hands.Count; i++)
        {
            if (_hands[i].IsEffortOrObjectBusy) continue;

            var order = i + 1;
            var newPosition = new Vector3();
            var handTransform = _hands[i].ModelTransform;
            var handDefaultTransform = _hands[i].DefaultModelPosition;

            newPosition.x = handDefaultTransform.x + Mathf.Sin(Time.time * order) * _xForceScale;
            newPosition.y = handDefaultTransform.y + Mathf.Cos(Time.time * order) * _yForceScale;

            handTransform.localPosition = Vector3.Lerp(handTransform.localPosition, newPosition, Time.deltaTime * 10f);
        }
    }

    public HandController GetFreeHand()
    {
        var hand = _hands.FirstOrDefault(x => !x.IsBusy);

        return hand;
    }

    public void Load(PlayerData data, BobbingTransforms transforms, List<HandController> hands)
    {
        _hands = hands;

        _playerBobbing.Initialize(transforms);
        _playerWallet.Load(data);
        _playerMovement.Initialize(_hands, data);
        _playerInteract.Initialize(data);

        OnGrab += _playerMovement.SetSpeedScale;
    }

    public void Save(ref PlayerData data)
    {
        _playerWallet.Save(ref data);
    }
}

public class BobbingTransforms
{
    public Transform CameraTransform, CameraHandlerTransform;
}
