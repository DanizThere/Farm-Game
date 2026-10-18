using System;
using UnityEngine;

[Serializable]
public class CommentaryBootstraper : Bootstraper
{
    private CommentaryService _service = new();
    public override void Inject(ServiceLocator serviceLocator)
    {
        serviceLocator.Add(_service);
    }
}
