using System;
using UnityEngine;

public interface ISaveService : IService
{
    public GameData Data { get; }
    public bool HasSave { get; }
    public void NewGame(InitData data);
    public void Save();
    public bool Load();
    public void Delete();
    public event Action<GameData> OnLoaded;
}