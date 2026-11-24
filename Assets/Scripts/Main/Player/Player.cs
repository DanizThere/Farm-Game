using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour, IUser
{
    public List<Relationship> Relationships { get; set; } = new();
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    [SerializeField] private string _id = Guid.NewGuid().ToString();

    [SerializeField] private bool _isTicking = true;
    [SerializeField] private float _defaultHunger;
    [SerializeField] private float _defaultForce;
    [SerializeField] private float _defaultMoney;

    private FixedSizeQueue<FoodType> _foodQuery = new(10);

    private float _hunger;
    private float _force;
    private float _money;

    private void Awake()
    {
        _hunger = _defaultHunger;
        _force = _defaultForce;
        _money = _defaultMoney;
    }

    private void Update()
    {
        if (!_isTicking) return;

        var val = Time.deltaTime;

        _hunger = Mathf.Clamp(_hunger - val, 0f, 100f);
        _force = Mathf.Clamp(_force - val * (100 - _hunger / 100), 0f, 100f);
    }

    public void AppendMoney(float value, bool force = false)
    {
        var newValue = _money + value;
        if (newValue < 0f && !force) return;

        _money = newValue;
    }


    public void PutRelationship(string related, int level = 0)
    {
        var relatedExist = Relationships.FirstOrDefault(x => x.Related == related);
        if (relatedExist != null)
        {
            relatedExist.Level = level;
            return;
        }

        var relationship = new Relationship
        {
            Level = level,
            Main = Id,
            Related = related
        };
        Relationships.Add(relationship);
    }

    public void RemoveRelationship(string related)
    {
        var relatedExist = Relationships.FirstOrDefault(x => x.Related == related);
        if (relatedExist != null)
        {
            Relationships.Remove(relatedExist);
            return;
        }
    }

    public void Eat(float saturation, FoodType foodType)
    {
        var similars = _foodQuery.GetSimilarCount(foodType);
        _foodQuery.Enqueue(foodType);
        _hunger = Mathf.Clamp(_hunger + saturation / similars, 0, 100f);
    }
}
