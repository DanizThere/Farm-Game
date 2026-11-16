using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IUser
{
    public List<Relationship> Relationships { get; set; } = new();
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    [SerializeField] private string _id = Guid.NewGuid().ToString();

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
}
