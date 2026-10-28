using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    private Inventory _playerInventory;
    private TimeManager _timeService;
    private ChestService _chestService;
    private GameDbContext _dbContext;
    private JsonSaveService _saveService;
    [SerializeField] private InitData _initData;
    [SerializeField] private ItemObjectSave _items;
    [SerializeField] private bool _saveOnQuit = false;

    private PlayerInitializer _player;

    private void OnApplicationQuit()
    {
        _items.Save();

        if (_saveOnQuit)
            SaveData();
    }

    public void Initialize(PlayerInitializer player)
    {
        _player = player;

        _saveService = ServiceLocator.Instance.GetService<JsonSaveService>();
        _playerInventory = ServiceLocator.Instance.GetService<InventoryController>().Inventory;
        _chestService = ServiceLocator.Instance.GetService<ChestService>();
        _timeService = ServiceLocator.Instance.GetService<TimeManager>();
        _dbContext = ServiceLocator.Instance.GetService<GameDbContext>();
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
        _dbContext.SaveNodes(ref _saveService.Data.Dialogues.Ids);
        _player.Save(ref _saveService.Data.Player);

        _saveService.Save();
    }

    private void LoadData(GameData data)
    {
        _playerInventory.Load(data.Inventory);
        _chestService.Load(data.Chests);
        _timeService.Load(data.Time);
        _player.Initialize(data.Player);

        _dbContext.AddNodes(data.Dialogues.Ids);
    }
}
