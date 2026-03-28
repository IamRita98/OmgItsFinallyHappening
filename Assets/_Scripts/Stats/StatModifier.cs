using UnityEngine;

public class StatModifier
{
    public readonly StatModType Type;
    public readonly float Value;
    public readonly int Order;
    public readonly object Source;
    public StatModifier(float value,StatModType type,int order, object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }
    public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }//when a statMod doesnt have source and uses default order
    public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }//when a statMod has no source
    public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }//when a statMod uses default order
}
