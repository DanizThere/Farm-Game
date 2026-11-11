using UnityEngine;
using UnityEngine.UI;

public class HandAccessibilityUI : View
{
    [SerializeField] private GameObject _blob;
    [SerializeField] private Image _image;
    private HandController _hand;

    private void Update()
    {
        _blob.SetActive(_hand.CanInteract);
    }

    public void Initialize(HandController hand, Sprite sprite)
    {
        _hand = hand;
        _image.sprite = sprite;
    }

    public override void Show()
    {
    }

    public override void Hide()
    {
    }
}
