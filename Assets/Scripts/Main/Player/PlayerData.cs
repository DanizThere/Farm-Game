using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public Observable<float> Money = new(0f);

    private void Start()
    {
        Money.ValueChanged += value => ServiceLocator.Instance.GetService<ViewController>().Get<PlayerInfo>().SetMoneyText(value.ToString());
    }
}
