using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ComponentItem))]
public class ComponentItemEditor : Editor
{
    SerializedProperty componentsListProperty;
    SerializedProperty assignedComponentProperty;
    SerializedProperty interactionIconProperty;
    SerializedProperty screenEdgePaddingProperty;
    SerializedProperty waypointScaleMultProperty;
    SerializedProperty waypointScaleDurationProperty;

    void OnEnable()
    {
        componentsListProperty = serializedObject.FindProperty("componentsListSO");
        assignedComponentProperty = serializedObject.FindProperty("assignedComponent");
        interactionIconProperty = serializedObject.FindProperty("interactionIcon");
        screenEdgePaddingProperty = serializedObject.FindProperty("screenEdgePadding");
        waypointScaleMultProperty = serializedObject.FindProperty("waypointScaleMult");
        waypointScaleDurationProperty = serializedObject.FindProperty("waypointScaleDuration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(componentsListProperty);

        ComponentsListSO listSO = componentsListProperty.objectReferenceValue as ComponentsListSO;
        ComponentSO current = assignedComponentProperty.objectReferenceValue as ComponentSO;

        if (listSO != null && listSO.componentsSOList != null && listSO.componentsSOList.Count > 0)
        {
            string[] options = new string[listSO.componentsSOList.Count];
            for (int i = 0; i < options.Length; i++)
            {
                var so = listSO.componentsSOList[i];
                options[i] = (so != null && !string.IsNullOrEmpty(so.ComponentName)) ? so.ComponentName : (so != null ? so.name : "<null>");
            }

            int currentIndex = -1;
            for (int i = 0; i < listSO.componentsSOList.Count; i++)
            {
                if (listSO.componentsSOList[i] == current)
                {
                    currentIndex = i;
                    break;
                }
            }

            int selected = EditorGUILayout.Popup("Assigned Component", currentIndex, options);
            if (selected != currentIndex && selected >= 0 && selected < listSO.componentsSOList.Count)
            {
                assignedComponentProperty.objectReferenceValue = listSO.componentsSOList[selected];
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Requires a ComponentsListSO with at least one ComponentSO item", MessageType.Info);
            EditorGUILayout.PropertyField(assignedComponentProperty);
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(interactionIconProperty);
        EditorGUILayout.PropertyField(screenEdgePaddingProperty);
        EditorGUILayout.PropertyField(waypointScaleMultProperty);
        EditorGUILayout.PropertyField(waypointScaleDurationProperty);

        serializedObject.ApplyModifiedProperties();
    }
}
