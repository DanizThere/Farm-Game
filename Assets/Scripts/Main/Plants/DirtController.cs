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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Generate();
        }
    }

    public void CreateDirt(int maxWidth, int maxLength)
    {
        foreach(var dirt in _dirt)
        {
            Destroy(dirt.gameObject);
        }
        _dirt.Clear();

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

    public Dirt GetDirtSlot(int x, int y)
    {
        var dirtSlot = _dirt.FirstOrDefault(dirt => dirt.DirtPosition.x == x && dirt.DirtPosition.y == y);
        return dirtSlot;
    }

    public void Generate()
    {
        CreateDirt(_maxWidth, _maxLength);
    }

    private Dirt GetRandomDirt()
    {
        Dirt finalDirt = _dirtVariable[0].Dirt;

        var sum = _dirtVariable.Sum(x => x.Rate);
        var value = Random.Range(0, sum);

        var rate = 0f;
        foreach(var dirt in _dirtVariable)
        {
            if(value > rate)
            {
                rate += dirt.Rate;
                finalDirt = dirt.Dirt;
            }
        }

        return finalDirt;
    }

}

[System.Serializable]
public class DirtVariable
{
    public Dirt Dirt;
    [Range(.1f, 1f)]
    public float Rate = .1f;
}
