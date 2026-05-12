using System;
using UnityEngine;


[Serializable]
public class SpaceshipRef
{
    [SerializeField] private SpaceshipReference SpaceshipReference;
    [SerializeField] private bool IsUnlocked;
}

[Serializable]
public class SpaceshipCategory
{
    [SerializeField] private string CategoryName;
    [SerializeField] private SpaceshipRef[] CategoryList;
}

[CreateAssetMenu(fileName = "SpaceshipListSO", menuName = "Scriptable Objects/SpaceshipListSO")]
public class SpaceshipListSO : ScriptableObject
{
    [SerializeField] private SpaceshipCategory[] SpaceshipList;
}
