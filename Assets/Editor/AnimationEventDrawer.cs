using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimationEvent))]
public class AnimationEventDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var stateNameProperty = property.FindPropertyRelative("EventName");
        var stateEventProperty = property.FindPropertyRelative("OnAnimationEvent");

        var stateNameRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var stateEventRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUI.GetPropertyHeight(stateEventProperty));

        EditorGUI.PropertyField(stateNameRect, stateNameProperty);
        EditorGUI.PropertyField(stateEventRect, stateEventProperty);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var stateEventProperty = property.FindPropertyRelative("OnAnimationEvent");
        return EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(stateEventProperty) + 4;
    }
}
