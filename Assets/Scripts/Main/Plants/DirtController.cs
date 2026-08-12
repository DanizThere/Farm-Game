using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirtController : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] private List<DirtVariable> _dirtVariable = new();

    [SerializeField] private int _maxWidth;
    [SerializeField] private int _maxLength;

    private List<Dirt> _dirt = new();

    public void CreateDirt(int maxWidth, int maxLength)
    {
        for(int i = 0; i < maxWidth; i++)
        {
            for(int j = 0; j < maxLength; j++)
            {
                var dirt = GetRandomDirt();

                var dirtGO = Instantiate(dirt, _parent);
                dirtGO.InitializePosition(i, j);

                _dirt.Add(dirtGO);
            }
        }
    }

    public void DestroyDirt(int x, int y)
    {
        var dirt = GetDirtSlot(x, y);
        if (dirt == null)
        {
            Debug.Log("there is no dirt");
            return;
        }

        Destroy(dirt.gameObject);
    }

    private Dirt GetRandomDirt()
    {
        var value = Random.value;
        Dirt finalDirt = _dirtVariable[0].Dirt;

        var sum = _dirtVariable.Sum(x => x.Rate);
        var rate = 0f;
        foreach(var dirt in _dirtVariable)
        {
            if(dirt.Rate > rate)
            {
                rate += dirt.Rate;
                finalDirt = dirt.Dirt;
            }
        }

        return finalDirt;
    }

    public Dirt GetDirtSlot(int x, int y)
    {
        var dirtSlot = _dirt.FirstOrDefault(dirt => dirt.DirtPosition.x == x && dirt.DirtPosition.y == y);
        return dirtSlot;
    }
}

[System.Serializable]
public class DirtVariable
{
    public Dirt Dirt;
    [Range(.1f, 1f)]
    public float Rate;
}
