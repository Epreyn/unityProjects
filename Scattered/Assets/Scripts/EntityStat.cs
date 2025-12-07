using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class EntityStat {
    [ReadOnly] public int BaseValue;

    [ShowInInspector]
    public virtual int Value {
        get {
            if (_isDirty || BaseValue != _lastBasedValue) {
                _lastBasedValue = BaseValue;
                _value = CalculateFinalValue();
                _isDirty = false;
            }

            return _value;
        }
    }

    protected bool _isDirty = true;
    protected int _value;
    protected int _lastBasedValue = int.MinValue;

    protected readonly List<StatModifier> _statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public EntityStat() {
        _statModifiers = new List<StatModifier>();
        StatModifiers = _statModifiers.AsReadOnly();
    }

    public EntityStat(int baseValue) : this() {
        BaseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier modifier) {
        _isDirty = true;
        _statModifiers.Add(modifier);
        _statModifiers.Sort(CompareModifierOrder);
    }

    public virtual void AddModifierIfDoesntExist(StatModifier modifier) {
        if (_statModifiers.All(existingMod => existingMod.Name != modifier.Name))
            AddModifier(modifier);
    }

    public virtual bool RemoveModifier(StatModifier modifier) {
        if (_statModifiers.Remove(modifier)) {
            _isDirty = true;
            return true;
        }

        return false;
    }

    public virtual bool RemoveAllModifiersFromSource(object source) {
        var didRemove = false;

        for (var i = _statModifiers.Count - 1; i >= 0; i--) {
            if (_statModifiers[i].Source != source) continue;
            _isDirty = true;
            didRemove = true;
            _statModifiers.RemoveAt(i);
        }

        return didRemove;
    }

    public virtual bool RemoveAllModifiersFromName(string modifierName) {
        var didRemove = false;

        for (var i = _statModifiers.Count - 1; i >= 0; i--)
            if (string.Equals(_statModifiers[i].Name, modifierName, StringComparison.OrdinalIgnoreCase)) {
                _isDirty = true;
                didRemove = true;
                _statModifiers.RemoveAt(i);
            }

        return didRemove;
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b) {
        return a.Order.CompareTo(b.Order);
    }

    protected virtual int CalculateFinalValue() {
        var finalValue = BaseValue;
        var sumPercentAdd = 0;

        for (var i = 0; i < _statModifiers.Count; i++) {
            var modifier = _statModifiers[i];

            switch (modifier.Type) {
                case StatModType.Flat:
                    finalValue += modifier.Value;
                    break;
                case StatModType.PercentAdd:
                    sumPercentAdd += modifier.Value;
                    if (i + 1 >= _statModifiers.Count || _statModifiers[i].Type != StatModType.PercentAdd) {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }

                    break;
                case StatModType.PercentMult:
                    finalValue *= 1 + modifier.Value;
                    break;
                default:
                    Debug.Log("Unknown StatModType encountered in CalculateFinalValue().");
                    break;
            }
        }

        return finalValue;
    }
}
