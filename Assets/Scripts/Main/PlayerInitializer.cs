using Assets.Scripts.Extenisions;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private CameraHolder _cameraPrefab;
    [SerializeField] private HandController _leftHandControllerPrefab;
    [SerializeField] private HandController _rightHandControllerPrefab;
    [SerializeField] private Sprite _leftHand;
    [SerializeField] private Sprite _rightHand;

    private PlayerController _player;

    public void Initialize(PlayerData data, Vector3? startPosition = null)
    {
        _player = Instantiate(_playerPrefab);
        if(startPosition.HasValue)
            _player.SetPosition(startPosition.Value);
        var camera = Instantiate(_cameraPrefab);

        var leftController = Instantiate(_leftHandControllerPrefab);
        var rightController = Instantiate(_rightHandControllerPrefab);

        var inventory = ServiceLocator.Instance.GetService<InventoryController>();

        leftController.GetComponent<HandSlot>().Initialize(leftController, inventory);
        rightController.GetComponent<HandSlot>().Initialize(rightController, inventory);

        camera.Initialize(_player.CameraTransform, _player.BodyTransform, _player.ModelTransform);

        _player.Load(data,
        new BobbingTransforms 
        { 
            CameraHandlerTransform = camera.transform,
            CameraTransform = camera.Camera.transform
        },
        new List<HandController> { leftController, rightController});

        var handler = ServiceLocator.Instance.GetService<CameraHandler>();
        handler.SetHolder(camera);

        var viewController = ServiceLocator.Instance.GetService<ViewController>();
        var handControllerUI = viewController
            .Get<HandAccessibilityUI>();

        leftController.Initialize(camera.LeftHandTransform, handler, _player.OnGrab);
        rightController.Initialize(camera.RightHandTransform, handler, _player.OnGrab);

        var leftHand = Instantiate(handControllerUI, viewController.HandsTransform);
        leftHand.Initialize(leftController, _leftHand);

        var rightHand = Instantiate(handControllerUI, viewController.HandsTransform);
        rightHand.Initialize(rightController, _rightHand);
    }

    public void Save(ref PlayerData data, ref Vector3Serialize playerPosition)
    {
        playerPosition = _player.BodyTransform.position.ToSerializable();
        _player.Save(ref data);
    }
}
