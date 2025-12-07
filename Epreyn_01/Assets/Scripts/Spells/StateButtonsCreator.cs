using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateButtonsCreator : MonoBehaviour {

    public GameObject StatePrefab;

    public void CreateOrUpdateState() {
        DeleteStates();
        var _currentCharacterStates = gameObject.GetComponentInParent<Character>().States;

        foreach (var t in _currentCharacterStates) {
            var _prefab = Instantiate(StatePrefab, transform, false);
            _prefab.GetComponent<Image>().sprite = t.Icon;
            _prefab.GetComponent<StateButton>().characterState = t;
            _prefab.GetComponentInChildren<TextMeshPro>().text = t.TurnNumber.ToString();
        }
    }
    
    public void DeleteStates() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
    
    public void AddState(CharacterState characterState) {
        var _currentCharacterStates = gameObject.GetComponentInParent<Character>().States;
        _currentCharacterStates.Add(characterState);
        CreateOrUpdateState();
    }

    
}