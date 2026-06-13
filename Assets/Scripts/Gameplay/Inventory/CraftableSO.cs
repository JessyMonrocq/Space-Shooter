using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftableSO", menuName = "Scriptable Objects/CraftableSO")]
public class CraftableSO : ScriptableObject
{
    public string CraftableName;
    [TextArea] public string CraftableDescription;
    public Sprite CraftableIcon;
    public Type CraftableType;
    public ComponentSO[] ComponentRequirements;

    public enum Type
    {
        ShipPart,
        Electronic,
        Fuel
    }
}
