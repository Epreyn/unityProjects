using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesManager : MonoBehaviour {
    
    public List<CharacterState> _characterStates;
    
    public void DeleteChilds() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public void DefineStatesList() {
        _characterStates =  transform.parent.transform.GetComponent<Character>()._characterStates;
    }

    public void AttributeCurrentState(int index) {
        transform.GetChild(index).transform.GetComponent<StateInstance>()._stateInstance = _characterStates[index];
    }

    public void AttributeXPosToCurrentState(bool isHero, int index) {
        transform.GetChild(index).transform.localPosition = isHero ? 
            new Vector3(index * 1.35f, 0, 0) : 
            new Vector3(0, index * -1.35f, 0);
    }
}
