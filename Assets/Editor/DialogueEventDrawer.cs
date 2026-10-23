using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogueEvent), true)]
public class DialogueEventDrawer : PropertyDrawer
{
    private const float propertyLineHeight = 5f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = EditorGUIUtility.singleLineHeight;
        var iter = property.Copy();
        var end = property.GetEndProperty();

        if (iter.NextVisible(true))
        {
            while (!SerializedProperty.EqualContents(iter, end))
            {
                height += EditorGUI.GetPropertyHeight(iter, true) + EditorGUIUtility.standardVerticalSpacing;
                if (!iter.NextVisible(false)) break;
            }
        }
        return height + propertyLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var lineH = EditorGUIUtility.singleLineHeight;
        var pad = EditorGUIUtility.standardVerticalSpacing;

        var lineRect = new Rect(position.x, position.y, position.width, lineH);
        var btnRect = EditorGUI.PrefixLabel(lineRect, label);

        var current = GetShortTypeName(property.managedReferenceFullTypename);
        if (EditorGUI.DropdownButton(btnRect, new GUIContent(current), FocusType.Keyboard))
            ShowTypeMenu(property);

        var iter = property.Copy();
        var end = property.GetEndProperty();
        var posY = position.y + lineH + pad;

        if (iter.NextVisible(true))
        {
            EditorGUI.indentLevel++;
            while (!SerializedProperty.EqualContents(iter, end))
            {
                var childH = EditorGUI.GetPropertyHeight(iter, true);
                var rect = new Rect(position.x, posY + propertyLineHeight, position.width, childH);
                EditorGUI.PropertyField(rect, iter, true);
                posY += childH + pad;
                if (!iter.NextVisible(false)) break;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    void ShowTypeMenu(SerializedProperty property)
    {
        var baseType = GetFieldType(property);
        if (baseType == null) return;

        var types = TypeCache.GetTypesDerivedFrom(baseType)
            .Where(t => !t.IsAbstract && !t.IsInterface && t.IsSerializable)
            .OrderBy(t => t.Name)
            .ToArray();

        var current = GetShortTypeName(property.managedReferenceFullTypename);

        var menu = new GenericMenu();

        menu.AddItem(new GUIContent("(None)"), current == "(None)", () =>
        {
            property.managedReferenceValue = null;
            property.serializedObject.ApplyModifiedProperties();
        });
        menu.AddSeparator("");

        foreach (var t in types)
        {
            var type = t; // замыкание
            menu.AddItem(new GUIContent(type.Name), current == type.Name, () =>
            {
                property.managedReferenceValue = Activator.CreateInstance(type);
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        menu.ShowAsContext();
    }

    // "AssemblyName Namespace/TypeName" -> "TypeName"
    static string GetShortTypeName(string managedRefFull)
    {
        if (string.IsNullOrEmpty(managedRefFull)) return "(None)";
        var sp = managedRefFull.IndexOf(' ');
        var typePart = (sp >= 0 ? managedRefFull.Substring(sp + 1) : managedRefFull).Replace('/', '.');
        var dot = typePart.LastIndexOf('.');
        return dot >= 0 ? typePart.Substring(dot + 1) : typePart;
    }

    // Достаём Type базового класса из поля (работает и для List<T>, T[])
    static Type GetFieldType(SerializedProperty property)
    {
        var field = property.managedReferenceFieldTypename; // "Asm Namespace.Type"
        if (string.IsNullOrEmpty(field)) return null;

        var sp = field.IndexOf(' ');
        if (sp < 0) return null;

        var asmName = field.Substring(0, sp);
        var typeName = field.Substring(sp + 1).Replace('/', '.');

        var asm = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == asmName);
        return asm?.GetType(typeName);
    }
}