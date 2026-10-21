using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class CommentaryHandler : SLMonoBehaviour
{
    private CommentaryService _commentaryService;

    protected override void SLStart()
    {
        base.SLStart();
        _commentaryService = ServiceLocator.Instance.GetService<CommentaryService>();
    }

    public void Print(Vector3 startPosition, string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            _commentaryService.Print(message, startPosition);
            return;
        }

        _commentaryService.Print("for what????", transform.position);
    }
}
