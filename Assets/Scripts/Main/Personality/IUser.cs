using System.Collections.Generic;
using UnityEngine;

public interface IUser
{
    public string Id { get; set; }
    public List<Relationship> Relationships { get; set; }
    public void PutRelationship(string related, int level = 0);
    public void RemoveRelationship(string related);
}
