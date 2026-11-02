using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(DialogueHandler))]
[RequireComponent(typeof(CommentaryHandler))]
[RequireComponent(typeof(Character))]
public class PlantEmotions : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private List<PlantAttributeEmotion> _attributes = new();
    [SerializeField] private DialogueNode _checkHealth;

    private List<CurrentPlantAttributeEmotion> _currentAtttributes = new();
    private Plant _plant;
    private Character _character;
    private CommentaryHandler _commentaryHandler;
    private DialogueHandler _dialogueHandler;

    private void Awake()
    {
        _plant = GetComponent<Plant>();
        _commentaryHandler = GetComponent<CommentaryHandler>();
        _character = GetComponent<Character>();
        _dialogueHandler = GetComponent<DialogueHandler>();

        _currentAtttributes.AddRange(_attributes.Select(x => new CurrentPlantAttributeEmotion
        {
            AttributeId = x.Attribute.Id,
            MinValueToEmote = x.MinValueToEmote,
            Periodicity = x.Periodicity,
            Timer = 0f
        }));
    }

    public void Tick(float deltaTime, PlantHealth health)
    {
        foreach(var attribute in _currentAtttributes)
        {
            var healthAttribute = health.PlantAttributes.FirstOrDefault(x => x.Id == attribute.AttributeId);
            if (healthAttribute == null) continue;
            if (healthAttribute.CurrentValue > attribute.MinValueToEmote) continue;

            attribute.Timer += deltaTime;
            if (attribute.Timer < attribute.Periodicity) continue;

            _commentaryHandler.Print(_startTransform.position, $"{attribute.AttributeId} сейчас: {healthAttribute.CurrentValue}");
            attribute.Timer = 0;
        }
    }

    public void GrowEmote()
    {
        _commentaryHandler.Print(_startTransform.position, "Кажется, я немного расту");
    }

    public void Emote()
    {
        var message = string.Empty;

        var node = ServiceLocator.Instance.GetService<GameDbContext>().CurrentNodes
            .Get(x => x.EntityType == _character.EntityType &&
                (x.TargetId == _plant.PlantId || x.TargetId == "all"));
        node.Add(_checkHealth);

        if(node != null)
        {
            _dialogueHandler.StartDialogue(node);
            return;
        }
        else
        {
            message = _character.UpdateMood(ReactionTitle.OnHit);
        }

        _commentaryHandler.Print(_startTransform.position, message);
    }
}
