
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComponentsListSO", menuName = "Scriptable Objects/ComponentsListSO")]
public class ComponentsListSO : ScriptableObject
{
    public List<ComponentSO> componentsSOList;
}
