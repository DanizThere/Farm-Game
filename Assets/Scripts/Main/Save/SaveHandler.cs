using Assets.Scripts.Extenisions;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    private Inventory _playerInventory;
    private TimeManager _timeService;
    private GameDbContext _dbContext;
    private JsonSaveService _saveService;
    [SerializeField] private InitData _initData;
    [SerializeField] private ItemObjectSave _items;
    [SerializeField] private bool _saveOnQuit = false;

    private PlayerInitializer _player;

    private void OnApplicationQuit()
    {

        if (_saveOnQuit)
            SaveData();
    }

    public void Initialize(PlayerInitializer player)
    {
        _player = player;

        _saveService = ServiceLocator.Instance.GetService<JsonSaveService>();
        _playerInventory = ServiceLocator.Instance.GetService<InventoryController>().Inventory;
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
        _timeService.Save(ref _saveService.Data.Time);
        _dbContext.SaveNodes(ref _saveService.Data.Dialogues.Ids);
        _player.Save(ref _saveService.Data.Player, ref _saveService.Data.PlayerPosition);
        _items.Save(ref _saveService.Data.Objects);

        _saveService.Save();
    }

    private void LoadData(GameData data)
    {
        _playerInventory.Load(data.Inventory);
        _timeService.Load(data.Time);
        _player.Initialize(data.Player, data.PlayerPosition.ToVector());
        _items.Load(data.Objects);

        _dbContext.AddNodes(data.Dialogues.Ids);
    }
}
