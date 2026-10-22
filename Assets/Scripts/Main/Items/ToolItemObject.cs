using System.Collections.Generic;
using UnityEngine;

public abstract class ToolItemObject : ItemObject
{
    public List<ItemObjectEffortData> Efforts = new();
    public Observable<GameObject> CurrentModel { get; private set; }
    [SerializeField] private GameObject[] _itemModels;
    [SerializeField] protected Animator _animator;

    private void Awake()
    {
        CurrentModel = new(_itemModels[0]);
    }
}
