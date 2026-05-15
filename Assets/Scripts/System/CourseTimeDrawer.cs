using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CourseTime))]
public class CourseTimeDrawer : PropertyDrawer
{
    private const float Spacing = 4f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty minutesProp = property.FindPropertyRelative("minutes");
        SerializedProperty secondsProp = property.FindPropertyRelative("seconds");
        SerializedProperty millisecondsProp = property.FindPropertyRelative("milliseconds");

        position = EditorGUI.PrefixLabel(
            position,
            GUIUtility.GetControlID(FocusType.Passive),
            label
        );

        int oldIndent = EditorGUI.indentLevel;
        float oldLabelWidth = EditorGUIUtility.labelWidth;

        EditorGUI.indentLevel = 0;

        float fieldWidth = (position.width - (Spacing * 2)) / 3f;
        float minLabelWidth = 15f;

        Rect r1 = new Rect(position.x, position.y, fieldWidth, position.height);
        Rect r2 = new Rect(r1.xMax + Spacing, position.y, fieldWidth, position.height);
        Rect r3 = new Rect(r2.xMax + Spacing, position.y, fieldWidth, position.height);

        DrawClampedIntField(r1, minutesProp, "Min", minLabelWidth + 12f, 0, int.MaxValue);
        DrawClampedIntField(r2, secondsProp, "S", minLabelWidth, 0, 59);
        DrawClampedIntField(r3, millisecondsProp, "Ms", minLabelWidth + 7f, 0, 999);

        EditorGUIUtility.labelWidth = oldLabelWidth;
        EditorGUI.indentLevel = oldIndent;

        EditorGUI.EndProperty();
    }

    private void DrawClampedIntField(
        Rect rect,
        SerializedProperty property,
        string miniLabel,
        float minLabelWidth,
        int min,
        int max)
    {
        Rect labelRect = new Rect(rect.x, rect.y, minLabelWidth, rect.height);
        Rect fieldRect = new Rect(rect.x + minLabelWidth, rect.y, rect.width - minLabelWidth, rect.height);

        EditorGUI.LabelField(labelRect, miniLabel);

        int value = EditorGUI.IntField(fieldRect, property.intValue);
        property.intValue = Mathf.Clamp(value, min, max);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
