using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JsonSaveService : ISaveService
{
    private const string FileName = "GameData.json";
    private const string BackupExt = ".bak";

    private readonly string _savePath;
    private readonly string _backupPath;

    public GameData Data { get; private set; } = new();

    public event Action<GameData> OnLoaded;

    public JsonSaveService()
    {
        _savePath = Path.Combine(Application.persistentDataPath, "saves", FileName);
        _backupPath = _savePath + BackupExt;
    }

    public bool HasSave => File.Exists(_savePath);

    public void NewGame(InitData data)
    {
        Data = new GameData
        {
            Chests = data.Chests,
            Inventory = data.Inventory,
            Time = new TimeSerialize
            {
                CurrentDate = data.Time.StartDate,
                StartHour = data.Time.StartHour,
                SunriseHour = data.Time.SunriseHour,
                SunsetHour = data.Time.SunsetHour,
                TimeMultiplier = data.Time.TimeMultiplier,
            }
        };

        OnLoaded?.Invoke(Data);
    }

    public void Save()
    {
        try
        {
            string tmpPath = _savePath + ".tmp";
            string json = JsonConvert.SerializeObject(Data);
            File.WriteAllText(tmpPath, json);

            if (File.Exists(_savePath))
            {
                if (File.Exists(_backupPath)) File.Delete(_backupPath);
                File.Move(_savePath, _backupPath);
            }

            File.Move(tmpPath, _savePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e}");
        }
    }

    public bool Load()
    {
        if (!TryLoadFrom(_savePath, out var data))
        {
            Debug.LogWarning("[SaveService] Main save corrupted, trying backup...");
            if (!TryLoadFrom(_backupPath, out data))
            {
                Debug.LogError("[SaveService] No valid save found.");
                return false;
            }
        }

        Data = data;
        OnLoaded?.Invoke(Data);
        Debug.Log("[SaveService] Loaded.");
        return true;
    }

    public void Delete()
    {
        try
        {
            if (File.Exists(_savePath)) File.Delete(_savePath);
            if (File.Exists(_backupPath)) File.Delete(_backupPath);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveService] Delete failed: {e}");
        }
    }

    private bool TryLoadFrom(string path, out GameData data)
    {
        data = null;
        if (!File.Exists(path)) return false;

        try
        {
            string json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return false;

            data = JsonConvert.DeserializeObject<GameData>(json);
            if (data == null) return false;

            data.Inventory ??= new InventorySerializable();
            data.Time ??= new TimeSerialize();
            data.Chests ??= new ChestsSerialize();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Load from {path} failed: {e}");
            return false;
        }
    }
}
