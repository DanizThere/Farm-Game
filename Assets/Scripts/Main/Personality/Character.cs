using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour, IUser
{
    public string EntityType;
    [SerializeField] private Personality _personality;
    //probaply here should be standard reaction, mood must have unique reaction;
    [SerializeField] private List<Reaction> _reaction = new();

    [SerializeField] private Mood _currentMood;

    private Action<Mood> _onChange = delegate { };

    public List<Relationship> Relationships { get; set; } = new();
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    [SerializeField] private string _id = Guid.NewGuid().ToString();

    public bool RelationshipExist(string id, out int level)
    {
        var relationShip = Relationships.FirstOrDefault(x => x.Related == id);
        if(relationShip == null)
        {
            level = 0;
            return false;
        }

        level = relationShip.Level;
        return true;
    }

    public void PutRelationship(string related, int level = 0)
    {
        var relatedExist = Relationships.FirstOrDefault(x => x.Related == related);
        if (relatedExist != null)
        {
            relatedExist.Level += level;
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

    private void Awake()
    {
        _onChange += ChangeMood;
    }


    private void ChangeMood(Mood mood)
    {
        if (mood == null) return;
        _currentMood.ResetMood();

        _currentMood = mood;
    }

    public Reaction GetReaction(ReactionTitle reactionName)
    {
        return _reaction.FirstOrDefault(x => x.ReactionName == reactionName);
    }

    public string UpdateMood(ReactionTitle reactionName, float value = 0f)
    {
        var message = string.Empty;
        var reaction = GetReaction(reactionName);
        if (reaction != null)
        {
            value = reaction.GetReactionValue(value, _personality);
            message = reaction.GetMessage();
        }
        else
            value *= _personality.Reactivity * _personality.ReactionRate;

        _currentMood.UpdateMood(value, _onChange);
        return message;
    }
}
