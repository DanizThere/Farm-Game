using TMPro;
using UnityEngine;

public class CharacteristicUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _strengthText;
    [SerializeField] private TextMeshProUGUI _hungerText;
    private Player _player;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
    }

    private void Update()
    {
        if (_player == null) return;

        _moneyText.text = $"{_player.Money}";
        _strengthText.text = $"{_player.Force:0.00}/100";
        _hungerText.text = $"{_player.Hunger:0.00}/100";
    }
}
