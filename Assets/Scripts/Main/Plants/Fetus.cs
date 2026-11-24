using System.Collections;
using UnityEngine;

public class Fetus : ItemObject
{
    [SerializeField] private float _saturation = 10f;
    [SerializeField] private FoodType _foodType = FoodType.Vegetable;

    private bool _isTriggered;
    public override void StopUse()
    {
    }

    public override void Use()
    {
        if (_isTriggered) return;

        _isTriggered = true;
        _currentHand.Eat(_saturation, _foodType);
        StartCoroutine(EatAnimEnumerator());
    }

    private IEnumerator EatAnimEnumerator()
    {
        while((_cameraHandler.Position - transform.position).magnitude > .1f)
        {
            transform.position = Vector3.Lerp(transform.position, _cameraHandler.Position, Time.deltaTime * 5);
            yield return null;
        }

        _currentHand.ClearItem();
    }
}

public enum FoodType
{
    Vegetable,
    Fruit,
    Meal
}