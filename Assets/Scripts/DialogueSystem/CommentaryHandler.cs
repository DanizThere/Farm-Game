using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class CommentaryHandler : SLMonoBehaviour
{
    [SerializeField] private List<LocalizedString> _comments = new();
    private CommentaryService _commentaryService;

    protected override void SLStart()
    {
        base.SLStart();
        _commentaryService = ServiceLocator.Instance.GetService<CommentaryService>();
    }

    public void Setup(List<LocalizedString> comments)
    {
        _comments.AddRange(comments);
    }

    public void Print(Vector3 startPosition)
    {
        var comment = _comments.Random();
        if(comment != null)
        {
            if (comment.IsEmpty)
            {
                _commentaryService.Print("for what????", transform.position);

                return;
            }

            var result = comment.GetLocalizedString();
            result = string.IsNullOrEmpty(result) ? "[Not localized]" : result;

            _commentaryService.Print(result, startPosition);
            return;
        }

        _commentaryService.Print("for what????", transform.position);
    }
}
