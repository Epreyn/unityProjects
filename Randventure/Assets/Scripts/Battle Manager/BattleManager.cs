using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    public GameObject GetMOnstersGroupGO(GameObject monstersGroupGO) {
        return monstersGroupGO;
    }

    public bool GetCurrentChildIsTheFocus(GameObject currentChild) {
        return currentChild.GetComponent<Character>()._isTheFocus;
    }
    
    public int GetCurrentChildCooldown(GameObject currentChild) {
        return currentChild.transform.Find("Abilities").transform.GetComponent<DisplayAbilities>().cooldown;
    }

    public void DecreaseCooldown(GameObject currentChild) {
        currentChild.transform.Find("Abilities").transform.GetComponent<DisplayAbilities>().cooldown--;
        currentChild.transform.Find("Abilities").transform.Find("Ability_1").
            transform.Find("Ability String").GetComponent<TextMeshPro>().text = 
            currentChild.transform.Find("Abilities").transform.GetComponent<DisplayAbilities>().cooldown == 0 ? "" :
            currentChild.transform.Find("Abilities").transform.GetComponent<DisplayAbilities>().cooldown.ToString();
    }

    public int GetAbilityIndex(Character character) {
        return character.transform.Find("Abilities").transform.GetComponent<DisplayAbilities>().abilityIndex;
    }

    public bool DefineAttackFirst(string heroPriority, string monsterPriority) {
        var result  = false;

        switch (heroPriority) {
            case "Low":
                result = false;
                break;
            case "High":
                result = true;
                break;
            case "Normal":
                switch (monsterPriority) {
                    case "Low":
                        result = true;
                        break;
                    case "High":
                        result = false;
                        break;
                }

                break;
        }

        return result;
    }

    public bool HerosOnMultitarget(bool HA_HOM, bool MA_HOM) {
        return HA_HOM || MA_HOM;
    }
    
    public bool MonstersOnMultitarget(bool HA_MOM, bool MA_MOM) {
        return HA_MOM || MA_MOM;
    }

    public bool CheckAllAttackResolved(bool heroAttackResolved, bool monsterAttackResolve) {
        return heroAttackResolved && monsterAttackResolve;
    }
}
