using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
public enum StatModType
{
    Flat=100,
    Percent=200
}
[Serializable]
public class CharacterStat
{

    public float BaseValue;
    private float lastBaseValue = float.MinValue;
    private List<StatModifier> statModifiers;
    private bool hasChanged = true;
    private float value;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;
    public CharacterStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }
    public CharacterStat(float baseValue):this()
    {
        BaseValue = baseValue;
    }
    public float Value {
        get { 
        if (hasChanged||lastBaseValue!=BaseValue)
        {
            lastBaseValue = BaseValue;
            value = CalculateFinalValue();
            hasChanged = false;
        }  
            return value;
        } 
    }
    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        for(int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
            if (mod.Type == StatModType.Flat) finalValue += mod.Value;
            else if (mod.Type == StatModType.Percent) finalValue *= 1 + mod.Value;
        }
        return (float)Mathf.Round(finalValue);
    }
    public void AddModifier(StatModifier mod)
    {
        hasChanged = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
    }
    private int CompareModifierOrder(StatModifier a,StatModifier b)
    {
        if (a.Order < b.Order) return -1;
        else if (a.Order > b.Order) return 1;
        return 0;
    }
    public bool RemoveModifier(StatModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
            hasChanged = true;
            return true;
        }
        return false;
    }
    public bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;
        for(int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                hasChanged = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }

        return didRemove;
    }
    
    
}
