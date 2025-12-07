using UnityEngine;
using UnityEngine.UI;

public class SpellButtonsCreator : MonoBehaviour {

    public GameObject SpellButtonPrefab;

    public void CreateSpellButtons(Spell[] spells) {
        var _currentCharacterClass = gameObject.GetComponentInParent<Character>().characterClass;

        for (var i = 0; i < spells.Length; i++) {
            var _prefab = Instantiate(SpellButtonPrefab, transform, false);
            _prefab.GetComponent<SpellButton>().spell = _currentCharacterClass.Spells[i];
            _prefab.GetComponent<Image>().sprite = _currentCharacterClass.Spells[i].Icon;
        }
    }

    public void CreateRandomSpellButton(Spell[] spells) {
        var _currentCharacterClass = gameObject.GetComponentInParent<Character>().characterClass;
        if (_currentCharacterClass.Spells.Length <= 0) return;
        var _randomSpellIndex = Random.Range(0, _currentCharacterClass.Spells.Length);
        var _prefab = Instantiate(SpellButtonPrefab, transform, false);
        _prefab.GetComponent<SpellButton>().spell = _currentCharacterClass.Spells[_randomSpellIndex];
        _prefab.GetComponent<Image>().sprite = _currentCharacterClass.Spells[_randomSpellIndex].Icon;
    }

    public void DeleteSpellButtons() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
}
