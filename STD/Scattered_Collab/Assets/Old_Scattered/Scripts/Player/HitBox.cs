using System.Collections;
using System.Collections.Generic;
using Scattered;
using UnityEngine;

public class HitBox : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll){
        if (GetComponentInParent<Neidan>()._invincible) return;
        if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy")) GetComponentInParent<Neidan>().IsAlive();
    }
}
