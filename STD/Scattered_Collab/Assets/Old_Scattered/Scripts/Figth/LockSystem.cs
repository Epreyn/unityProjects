using System.Collections;
using System.Collections.Generic;
using Scattered;
using UnityEngine;
using UnityEngine.UI;

public class LockSystem : MonoBehaviour {

	public FightSystem _fs;

	void Start () {
		
	}
	
	void Update () {
		if (_fs._fight) Lock();
		else GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
	}

	public void Lock(){
		if (_fs._currentFightTrigger == null) return;
		GetComponent<RectTransform>().position = new Vector2((_fs.GetComponentInParent<Doppel>().transform.position.x + _fs._currentFightTrigger.transform.position.x) / 2,
										 					 (_fs.GetComponentInParent<Doppel>().transform.position.y + _fs._currentFightTrigger.transform.position.y) / 2);
		
		//GetComponent<RectTransform>().Rotate(0, 0, Time.deltaTime * 50);
		
		var t = Time.time / 5;  
    	var length = 1.2f;
    	GetComponent<RectTransform>().localScale = new Vector3(Mathf.PingPong(t, length - 1) + 1, Mathf.PingPong(t, length - 1) + 1, 0);
			
		GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f - Mathf.Abs(Input.GetAxis("Move_Doppel")));
		GetComponent<Image>().fillAmount = (float)_fs._currentFightTrigger.GetComponent<FightTriggerFields>()._currentHp / (float)_fs._currentFightTrigger.GetComponent<FightTriggerFields>()._originalHp;
	}
}
