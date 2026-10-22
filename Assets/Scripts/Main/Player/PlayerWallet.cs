using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public Observable<float> Money = new(0f);

    public void Load(PlayerData data)
    {
        Money.Value = data.Money;
    }

    public void Save(ref PlayerData data)
    {
        data.Money = Money.Value;
    }
}
