using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public Observable<float> Money = new(0f);

    public void Initialize()
    {
        Money.ValueChanged += value => ServiceLocator.Instance.GetService<ViewController>().Get<PlayerInfo>().SetMoneyText(value.ToString());
    }
}
