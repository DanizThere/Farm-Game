using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    private Inventory _playerInventory;
    private TimeManager _timeService;
    private ChestService _chestService;

    private JsonSaveService _saveService;
    [SerializeField] private InitData _initData;

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void Initialize()
    {
        _saveService = ServiceLocator.Instance.GetService<JsonSaveService>();
        _playerInventory = ServiceLocator.Instance.GetService<InventoryController>().Inventory;
        _chestService = ServiceLocator.Instance.GetService<ChestService>();
        _timeService = ServiceLocator.Instance.GetService<TimeManager>();

        _saveService.OnLoaded += LoadData;

        if (_saveService.HasSave) _saveService.Load();
        else
        {
            _saveService.NewGame(_initData);
        }
    }

    public void SaveData()
    {
        _playerInventory.Save(_saveService.Data.Inventory);
        _chestService.Save(ref _saveService.Data.Chests);
        _timeService.Save(ref _saveService.Data.Time);

        _saveService.Save();
    }

    private void LoadData(GameData data)
    {
        _playerInventory.Load(data.Inventory);
        _chestService.Load(data.Chests);
        _timeService.Load(data.Time);
    }
}
