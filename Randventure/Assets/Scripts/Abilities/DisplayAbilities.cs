using System;
using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DisplayAbilities : MonoBehaviour {

    public int abilityIndex;
    public int cooldown;

    public void AssignAbilities(Character character) {
        var i = 0;
        foreach (Transform child in transform.GetChild(0).transform) {
            child.GetComponent<Image>().sprite = character._characterClass._abilities[i]._abilityIcon;
            i++;
        }
    }

    public void EnableAbilities(bool enable) {
        foreach (Transform child in transform.GetChild(0).transform) {
            child.GetComponent<Button>().interactable = enable;
        }
    }

    public void AssignMonsterAbility(GameObject abilityGO, Character character) {
        abilityIndex = Random.Range(0, character._characterClass._abilities.Length);
        abilityGO.GetComponent<Image>().sprite = character._characterClass._abilities[abilityIndex]._abilityIcon;
    }

    public void GetAbilityInformations(int index) {
        var character = transform.parent.transform.GetComponent<Character>();
        
        Data.AbilityIcon        = character._characterClass._abilities[index]._abilityIcon;
        Data.AbilityName        = character._characterClass._abilities[index]._abilityName;
        Data.AbilityDescription = character._characterClass._abilities[index]._abilityDescription;

        var fsmBool = FsmVariables.GlobalVariables.FindFsmBool("Show_Ability_Page");
        fsmBool.Value = !fsmBool.Value;
    }
    
    public void GetMonsterAbilityInformations() {
        var character = transform.parent.transform.GetComponent<Character>();
        
        Data.AbilityIcon        = character._characterClass._abilities[abilityIndex]._abilityIcon;
        Data.AbilityName        = character._characterClass._abilities[abilityIndex]._abilityName;
        Data.AbilityDescription = character._characterClass._abilities[abilityIndex]._abilityDescription;

        var fsmBool = FsmVariables.GlobalVariables.FindFsmBool("Show_Ability_Page");
        fsmBool.Value = !fsmBool.Value;
    }

    public void LaunchAttack(int index) {
        abilityIndex = index;
        var fsmBool = FsmVariables.GlobalVariables.FindFsmBool("Attack_Launched");
        fsmBool.Value = true;
    }
}
