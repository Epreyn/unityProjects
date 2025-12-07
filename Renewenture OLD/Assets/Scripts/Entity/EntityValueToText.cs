using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EntityValueType {
    Name,
    SpellA,
    SpellADescription,
    SpellB,
    SpellBDescription,
    CurrentHealth,
    PhysicalAttack,
    MagicalAttack,
    PhysicalDefense,
    MagicalDefense,
    AttackSpeed
}

public class EntityValueToText : MonoBehaviour {
    private Entity _entityReference;
    public EntityValueType valueType;
    private TextMeshProUGUI _textMeshPro;

    private string _valueToDisplay;

    private void Start() {
        var currentParent = transform.parent;
        Transform entityTransform = null;

        while (currentParent != null) {
            entityTransform = currentParent.Find("Entity");
            if (entityTransform != null) break;
            currentParent = currentParent.parent;
        }

        if (entityTransform == null) {
            Debug.LogError("Entity GameObject not found");
            return;
        }

        _entityReference = entityTransform.gameObject.GetComponent<EntityReference>().entityReference;
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        DefineValueToDisplay();
    }


    private void DefineValueToDisplay() {
        _valueToDisplay = valueType switch {
            EntityValueType.Name => _entityReference.className,
            EntityValueType.SpellA => _entityReference.spells[0].spellName,
            EntityValueType.SpellADescription => _entityReference.spells[0].spellDescription,
            EntityValueType.SpellB => _entityReference.spells[1].spellName,
            EntityValueType.SpellBDescription => _entityReference.spells[1].spellDescription,
            EntityValueType.CurrentHealth => _entityReference.currentHp.ToString(),
            EntityValueType.PhysicalAttack => _entityReference.physicalAttack.Value.ToString(),
            EntityValueType.MagicalAttack => _entityReference.magicalAttack.Value.ToString(),
            EntityValueType.PhysicalDefense => _entityReference.physicalDefense.Value.ToString(),
            EntityValueType.MagicalDefense => _entityReference.magicalDefense.Value.ToString(),
            EntityValueType.AttackSpeed => _entityReference.attackSpeed.Value.ToString(),
            _ => _valueToDisplay
        };
    }

    private void FixedUpdate() {
        DefineValueToDisplay();
        _textMeshPro.text = _valueToDisplay;
    }
}
