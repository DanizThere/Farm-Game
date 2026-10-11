using UnityEngine;

public class HandAccessibilityUI : View
{
    [SerializeField] private GameObject _blob;
    private HandController _hand;

    private void Update()
    {
        _blob.SetActive(_hand.CanInteract);
    }

    public void Initialize(HandController hand)
    {
        _hand = hand;
    }

    public override void Show()
    {
    }

    public override void Hide()
    {
    }
}
