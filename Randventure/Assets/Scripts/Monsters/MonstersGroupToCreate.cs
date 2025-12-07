using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonstersGroupToCreate : MonoBehaviour {

    #region FIELDS

    [Title("Monster Classes Type")]
    
    public CharacterClass[] simpleMonsters;
    public CharacterClass[] epicMonsters;
    public CharacterClass[] bossMonsters;

    [Title("Monster Group Generator")]
    public enum EncounterType { SimpleEncounter, EpicEncounter, BossEncounter }
    public EncounterType encounterType;
    
    [ReadOnly]
    public CharacterClass[] monstersClassesToCreate;
    
    #endregion

    #region UNITY METHODS

    private void Awake() {
        switch (encounterType) {
            case EncounterType.SimpleEncounter:
                monstersClassesToCreate = new CharacterClass[Random.Range(1, 3)];
                for (var i = 0; i < monstersClassesToCreate.Length; i++) {
                    monstersClassesToCreate[i] = simpleMonsters[Random.Range(0, simpleMonsters.Length)];
                }
                break;
            
            case EncounterType.EpicEncounter:
                monstersClassesToCreate = new CharacterClass[Random.Range(3, 3)];
                for (var i = 0; i < monstersClassesToCreate.Length; i++) {
                    monstersClassesToCreate[i] = i == 0 ? 
                        epicMonsters[Random.Range(0, epicMonsters.Length)] :
                        simpleMonsters[Random.Range(0, simpleMonsters.Length)];
                }
                break;
            
            case EncounterType.BossEncounter:
                monstersClassesToCreate = new CharacterClass[1];
                monstersClassesToCreate[0] = bossMonsters[Random.Range(0, bossMonsters.Length)];
                break;
        }
    }

    #endregion

    #region PLAYMAKER METHODS

    public void DeleteAllChild() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public void DeleteTargetChild(GameObject targetChild) {
        Destroy(targetChild);
    }

    public void GiveNameToChild(int index, GameObject targetChild) {
        targetChild.name = "Monster_" + index;
    }
    
    public int MonstersClassesToCreateLength() {
        return monstersClassesToCreate.Length;
    }
    
    public CharacterClass GetCharacterClassToCreate(int index) {
        return monstersClassesToCreate[index];
    }

    public void AttributeClassToCurrentChild(CharacterClass targetClass, GameObject currentChild) {
        currentChild.GetComponent<Character>()._characterClass = targetClass;
    }
    
    public void InitializeCharacter(GameObject currentChild) {
        currentChild.GetComponent<Character>().Initialization();
    }

    public void AttributeCharacterToCurrentChild(Character targetCharacter, GameObject currentChild) {
        currentChild.GetComponent<Character>().AttributeCharacter(targetCharacter);
    }

    public Character GetCharacter(GameObject targetGO) {
        return targetGO.GetComponent<Character>();
    }

    public int GetCurrentChildHP(GameObject targetChild) {
        return targetChild.GetComponent<Character>()._currentHP;
    }

    #endregion

    #region COOLDOWNS

    public void SetCooldown(int childCount) {
        switch (childCount) {
            case 1:
                transform.GetChild(0).transform.Find("Abilities").
                        transform.GetComponent<DisplayAbilities>().cooldown =2;
                transform.GetChild(0).transform.Find("Abilities").transform.Find("Ability_1").
                        transform.Find("Ability String").GetComponent<TextMeshPro>().text = "2";
                break;
            
            case 2:
                var rng = Random.Range(0, 1);
                for (var i = 0; i < transform.childCount; i++) {
                    transform.GetChild(i).transform.Find("Abilities").
                        transform.GetComponent<DisplayAbilities>().cooldown = rng == i ? 1 : 0;
                    transform.GetChild(i).transform.Find("Abilities").transform.Find("Ability_1").
                        transform.Find("Ability String").GetComponent<TextMeshPro>().text = rng == i ? "1" : "";
                }
                break;
            
            case 3:
                foreach (Transform child in transform) {
                    child.Find("Abilities").transform.GetComponent<DisplayAbilities>().cooldown = 0;
                    child.Find("Abilities").transform.Find("Ability_1").
                        transform.Find("Ability String").GetComponent<TextMeshPro>().text = "";
                }
                break;
        }
    }

    #endregion
}
