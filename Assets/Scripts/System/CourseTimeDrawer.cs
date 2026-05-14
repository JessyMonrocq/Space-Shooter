using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CourseTime))]
public class CourseTimeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty minutes = property.FindPropertyRelative("minutes");
        SerializedProperty seconds = property.FindPropertyRelative("seconds");
        SerializedProperty milliseconds = property.FindPropertyRelative("milliseconds");

        position = EditorGUI.PrefixLabel(position, label);

        float width = position.width / 3f;

        Rect r1 = new Rect(position.x, position.y, width - 4, position.height);
        Rect r2 = new Rect(position.x + width, position.y, width - 4, position.height);
        Rect r3 = new Rect(position.x + width * 2, position.y, width, position.height);

        minutes.intValue = Mathf.Max(0, EditorGUI.DelayedIntField(r1, "m", minutes.intValue));
        seconds.intValue = Mathf.Clamp(EditorGUI.DelayedIntField(r2, "s", seconds.intValue), 0, 59);
        milliseconds.intValue = Mathf.Clamp(EditorGUI.DelayedIntField(r3, "ms", milliseconds.intValue), 0, 999);

        EditorGUI.EndProperty();
    }
}
