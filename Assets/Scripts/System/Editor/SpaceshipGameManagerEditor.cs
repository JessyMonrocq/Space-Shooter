using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpaceshipGameManager))]
public class SpaceshipGameManagerEditor : Editor
{
    SerializedProperty missionType;
    SerializedProperty courseManager;

    void OnEnable()
    {
        missionType = serializedObject.FindProperty("missionType");
        courseManager = serializedObject.FindProperty("courseManager");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (missionType != null )
        {
            EditorGUILayout.PropertyField(missionType);
            if (missionType.enumValueIndex == (int)SpaceshipGameManager.MissionType.Course)
            {
                EditorGUILayout.PropertyField(courseManager);
            }
        }

        DrawPropertiesExcluding(serializedObject, "missionType", "courseManager", "m_Script");

        serializedObject.ApplyModifiedProperties();
    }
}
