using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCollision : MonoBehaviour {

	private float _endTimeElapsed;
	private float _endDelay;
	private string _emotion;

	void Start () {
		//if (GetComponentInParent<SpellSystem>()._activeDelay != null)
		_endDelay = GetComponentInParent<SpellSystem>()._activeDelay;
		_emotion = GetComponentInParent<SpellSystem>()._emotion;
	}
	
	void Update () {
		_endTimeElapsed += Time.deltaTime;
		if (_endTimeElapsed > _endDelay) {
			if (_emotion == "Bar_Chagrin")
				GetComponentInParent<SpellSystem>()._animator.SetBool("Chagrin", false);
			Destroy(this.gameObject);
		}
	}

		void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.layer == LayerMask.NameToLayer("FightTrigger")) {
            switch(GetComponentInParent<SpellSystem>()._emotion){

				case "Bar_Extase":
				break;

				case "Bar_Adoration":
				break;
				
				case "Bar_Terreur":
				coll.gameObject.GetComponent<FightTriggerFields>()._freeze = true;
				break;
				
				case "Bar_Etonnement":
				break;
				
				case "Bar_Chagrin":
				coll.gameObject.GetComponent<FightTriggerFields>()._dot = true;
				break;
				
				case "Bar_Aversion":
				break;
				
				case "Bar_Rage":
				coll.gameObject.GetComponent<FightTriggerFields>()._hit = true;
				break;
				
				case "Bar_Vigilance":
				break;
				
				default:
				break;
			}
        }
    }

    void OnTriggerStay2D(Collider2D coll){
        if (coll.gameObject.layer == LayerMask.NameToLayer("FightTrigger")) {
            switch(GetComponentInParent<SpellSystem>()._emotion){

				case "Bar_Extase":
				break;

				case "Bar_Adoration":
				break;
				
				case "Bar_Terreur":
				coll.gameObject.GetComponent<FightTriggerFields>()._freeze = true;
				break;
				
				case "Bar_Etonnement":
				break;
				
				case "Bar_Chagrin":
				coll.gameObject.GetComponent<FightTriggerFields>()._dot = true;
				break;
				
				case "Bar_Aversion":
				break;
				
				case "Bar_Rage":
				coll.gameObject.GetComponent<FightTriggerFields>()._hit = true;
				break;
				
				case "Bar_Vigilance":
				break;
				
				default:
				break;
			}
        }
    }

	void OnTriggerExit2D(Collider2D coll){
        if (coll.gameObject.layer == LayerMask.NameToLayer("FightTrigger")) {
            switch(GetComponentInParent<SpellSystem>()._emotion){

				case "Bar_Extase":
				break;

				case "Bar_Adoration":
				break;
				
				case "Bar_Terreur":
				coll.gameObject.GetComponent<FightTriggerFields>()._freeze = false;
				break;
				
				case "Bar_Etonnement":
				break;
				
				case "Bar_Chagrin":
				break;
				
				case "Bar_Aversion":
				break;
				
				case "Bar_Rage":
				coll.gameObject.GetComponent<FightTriggerFields>()._hit = false;
				break;
				
				case "Bar_Vigilance":
				break;
				
				default:
				break;
			}
        }
    }
}
