using System;
using UnityEngine;

[Serializable]
public class ComponentStack
{
    public ComponentSO component;
    public int amount;

    public ComponentStack(ComponentSO component, int amount)
    {
        this.component = component;
        this.amount = amount;
    }
}
