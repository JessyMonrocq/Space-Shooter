using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "ComponentSO", menuName = "Scriptable Objects/ComponentSO")]
public class ComponentSO : ScriptableObject
{
    public string ComponentName;
    [TextArea] public string ComponentDescription;
    public Sprite ComponentIcon;
    public Type ComponentType;
    [Min(0)] public int ComponentSize = 1;

    public enum Type
    {
        Scrap,
        Metal,
        Gas,
        Liquid
    }
}
