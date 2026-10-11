using UnityEngine;

public class InventoryController : SLMonoBehaviour, IService
{
    [SerializeField] private Inventory _inventory;
    private InventoryUI _inventoryUI;

    private GameStateMachine _gameStateMachine;
    private ViewController _viewController;

    private Observable<ISelectable> _selectableObservable;

    protected override void SLStart()
    {
        LoadInventory();

        _gameStateMachine = ServiceLocator.Instance.GetService<GameStateMachine>() ??
            throw new System.NullReferenceException($"{nameof(GameStateMachine)} doens't exist");
        _viewController = ServiceLocator.Instance.GetService<ViewController>() ??
            throw new System.NullReferenceException($"{nameof(ViewController)} doens't exist");

        _gameStateMachine.GetState<OuterWorldGameState>().OnStartEvent += HideUI;
        _gameStateMachine.GetState<UIGameState>().OnStartEvent += ShowUI;


        _selectableObservable = new(null);
        _selectableObservable.ValueChanged += value => value.Show();
    }

    private void OnDestroy()
    {
        SaveInventory();
    }

    private void SaveInventory()
    {
        var data = _inventory.SaveData();

        ServiceLocator.Instance.GetService<JsonSaveService>().Save("InventoryData", data);
    }

    private void LoadInventory()
    {
        ServiceLocator.Instance.GetService<JsonSaveService>().Load<InventorySerializable>("InventoryData", data =>
        {
            if(data == null)
            {
                print("InventoryData not found");
                return;
            }
            _inventory.Load(data);
        });
    }

    public void Show()
    {
        _gameStateMachine.SetState<UIGameState>();
    }

    public void Hide()
    {
        _gameStateMachine.SetState<OuterWorldGameState>();
    }

    private void ShowUI()
    {
        _inventoryUI = Instantiate(_viewController.Get<InventoryUI>(), _viewController.Canvas2D.transform);
        _inventoryUI.Setup(_inventory);
    }

    private void HideUI()
    {
        Destroy(_inventoryUI.gameObject);
        _inventoryUI = null;
    }
}
