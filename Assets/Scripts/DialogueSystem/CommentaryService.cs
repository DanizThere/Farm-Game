using UnityEngine;

public class CommentaryService : IService
{
    public void Print(string message, Vector3 startPosition, float duration = 5f)
    {
        var viewController = ServiceLocator.Instance.GetService<ViewController>();
        var ui = GameObject.Instantiate(viewController.Get<CommentaryUI>(), viewController.Canvas3D.transform);

        ui.Setup(message, startPosition, duration);
    }
}
