using System.Collections;
using System.Collections.Generic;
using Scattered;
using UnityEngine;
using UnityEngine.UI;

public class FightSystem : MonoBehaviour {

	public GameObject _currentFightTrigger;
	private GameObject[] _enemies;

	public GameObject EnemyFightTriggerPrefab;

	public bool _fight;
	public float RefightDelay;
	private float _refightTimeElapsed;

	[Range(0.0f, 1.0f)]
	public float _timeLock;

	private float _xDiff, _yDiff;

	void Start () {
		_enemies = GameObject.FindGameObjectsWithTag("Enemy");

		for (int i = 0; i < _enemies.Length; i++){
			EnemyFightTriggerPrefab.GetComponent<FightTriggerFields>()._radius = 2;
			GameObject t = Instantiate(EnemyFightTriggerPrefab, _enemies[i].transform.position, Quaternion.identity) as GameObject;
			t.transform.parent = _enemies[i].transform;
		}
	}
	
	void Update () {
		if (_refightTimeElapsed != 0 && !_fight)
			GetComponentInParent<SpriteRenderer>().color = new Color(GetComponentInParent<SpriteRenderer>().color.r,
																	 GetComponentInParent<SpriteRenderer>().color.g ,
																	 GetComponentInParent<SpriteRenderer>().color.b , .2f);
		else GetComponentInParent<SpriteRenderer>().color = new Color(GetComponentInParent<SpriteRenderer>().color.r,
																	 GetComponentInParent<SpriteRenderer>().color.g ,
																	 GetComponentInParent<SpriteRenderer>().color.b , 1f);

		if (_currentFightTrigger != null
			&& _currentFightTrigger.GetComponent<FightTriggerFields>()._currentHp <= 0) _fight = false;
		
		if(_fight){
			Freeze(RigidbodyConstraints2D.FreezeAll);
			this.GetComponent<Collider2D>().enabled = false;
		}
		else{
			if (_currentFightTrigger != null) _currentFightTrigger.GetComponentInChildren<FightTriggerFields>()._fight = false;
			Freeze(RigidbodyConstraints2D.FreezeRotation);
			ReFight();
		}
		
		if (_currentFightTrigger != null){
			_xDiff = Mathf.Abs(_currentFightTrigger.gameObject.transform.position.x - gameObject.transform.position.x);
			_yDiff = Mathf.Abs(_currentFightTrigger.gameObject.transform.position.y - gameObject.transform.position.y);
			if (_xDiff > 7 || _yDiff > 7) _fight = false;
		}

		if(DoppelIsLocking()) _fight = false;
	}

	public void ReFight(){
			if (!this.GetComponent<Collider2D>().enabled
				&& !GetComponentInParent<Doppel>().Erased){
					_refightTimeElapsed += Time.deltaTime;
					if (_refightTimeElapsed > RefightDelay) this.GetComponent<Collider2D>().enabled = true;
			} else _refightTimeElapsed = 0;
	}

	void Freeze(RigidbodyConstraints2D constraint){
		this.GetComponentInParent<Rigidbody2D>().constraints = constraint;
		if(_currentFightTrigger != null) _currentFightTrigger.GetComponentInParent<Rigidbody2D>().constraints = constraint;
	}

	bool DoppelIsMoving(){
		return (Input.GetAxis("Move_Doppel") >= _timeLock / 2 || Input.GetAxis("Move_Doppel") <= -_timeLock / 2);
	}

	bool DoppelIsLocking(){
		return (Input.GetAxis("Move_Doppel") >= _timeLock || Input.GetAxis("Move_Doppel") <= -_timeLock);
	}

	// TRIGGERS
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.layer == LayerMask.NameToLayer("FightTrigger")
			&& !DoppelIsMoving()
			&& !GetComponentInParent<Doppel>().Erased) {
            _fight = true;
			_currentFightTrigger = coll.gameObject;
			_currentFightTrigger.GetComponentInChildren<FightTriggerFields>()._fight = true;
		}
    }

	void OnTriggerStay2D(Collider2D coll){ 
		if (coll.gameObject.layer == LayerMask.NameToLayer("FightTrigger")
			&& !DoppelIsMoving()
			&& !GetComponentInParent<Doppel>().Erased) {
            _fight = true;
			_currentFightTrigger = coll.gameObject;
			_currentFightTrigger.GetComponentInChildren<FightTriggerFields>()._fight = true;
		}
    }
}
