using TMPro;
using UnityEngine;

public enum EntityValueType {
    Name,
    CurrentHealth,
    Attack,
    Defense,
    Speed
}

public class EntityValueToText : MonoBehaviour {
    public EntityValueType valueType;
    private Entity _entityReference;
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

    private void FixedUpdate() {
        DefineValueToDisplay();
        _textMeshPro.text = _valueToDisplay;
    }


    private void DefineValueToDisplay() {
        _valueToDisplay = valueType switch {
            EntityValueType.Name => _entityReference.className,
            EntityValueType.CurrentHealth => _entityReference.currentHealth.ToString(),
            EntityValueType.Attack => _entityReference.attack.Value.ToString(),
            EntityValueType.Defense => _entityReference.defense.Value.ToString(),
            EntityValueType.Speed => _entityReference.speed.Value.ToString(),
            _ => _valueToDisplay
        };
    }
}