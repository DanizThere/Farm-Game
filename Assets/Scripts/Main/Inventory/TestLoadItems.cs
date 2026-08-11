using UnityEngine;

public class TestLoadItems : MonoBehaviour
{
    [SerializeField] private Item[] _items;

    [SerializeField] private Inventory _inv;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var item in _items)
            {
                _inv.AddItem(item, 1);
            }
        }
#endif
    }
}
