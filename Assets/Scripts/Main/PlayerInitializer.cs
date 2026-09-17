using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private PlayerController _playerPrefab;
    [SerializeField] private CameraHolder _cameraPrefab;
    [SerializeField] private HandController _leftHandControllerPrefab;
    [SerializeField] private HandController _rightHandControllerPrefab;

    //test
    private void Start()
    {
        Initialize();
    }

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
        leftController.Initialize(camera.LeftHandTransform);
        rightController.Initialize(camera.RightHandTransform);

        ServiceLocator.Instance
            .GetService<CameraHandler>()
            .SetHolder(camera);
    }
}
