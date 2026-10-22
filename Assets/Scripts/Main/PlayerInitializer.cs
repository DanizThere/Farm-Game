using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private CameraHolder _cameraPrefab;
    [SerializeField] private HandController _leftHandControllerPrefab;
    [SerializeField] private HandController _rightHandControllerPrefab;

    private PlayerController _player;

    public void Initialize(PlayerData data)
    {
        _player = Instantiate(_playerPrefab);
        var camera = Instantiate(_cameraPrefab);

        var leftController = Instantiate(_leftHandControllerPrefab);
        var rightController = Instantiate(_rightHandControllerPrefab);

        var inventory = ServiceLocator.Instance.GetService<InventoryController>();

        leftController.GetComponent<HandSlot>().Initialize(leftController, inventory);
        rightController.GetComponent<HandSlot>().Initialize(rightController, inventory);

        camera.Initialize(_player.CameraTransform, _player.BodyTransform);

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
        leftHand.Initialize(leftController);

        var rightHand = Instantiate(handControllerUI, viewController.HandsTransform);
        rightHand.Initialize(rightController);
    }

    public void Save(ref PlayerData data)
    {
        _player.Save(ref data);
    }
}
