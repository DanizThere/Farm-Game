using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private CameraHolder _cameraPrefab;
    [SerializeField] private HandController _leftHandControllerPrefab;
    [SerializeField] private HandController _rightHandControllerPrefab;

    public void Initialize()
    {
        var player = Instantiate(_playerPrefab);
        var camera = Instantiate(_cameraPrefab);

        var leftController = Instantiate(_leftHandControllerPrefab);
        var rightController = Instantiate(_rightHandControllerPrefab);

        camera.Initialize(player.CameraTransform, player.BodyTransform);

        player.Initialize(new BobbingTransforms 
        { 
            CameraHandlerTransform = camera.transform,
            CameraTransform = camera.Camera.transform
        });

        var handler = ServiceLocator.Instance.GetService<CameraHandler>();
        handler.SetHolder(camera);

        leftController.Initialize(camera.LeftHandTransform, handler, player.OnGrab);
        rightController.Initialize(camera.RightHandTransform, handler, player.OnGrab);
    }
}
