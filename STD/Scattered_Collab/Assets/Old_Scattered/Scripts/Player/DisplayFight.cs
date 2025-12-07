using System.Collections;
using System.Collections.Generic;
using Scattered;
using UnityEngine;

public class DisplayFight : MonoBehaviour {

	public bool Active;
	public float DisplayDelay;
	private float _displayTimeElapsed;

	private GameObject _doppel;
	private Transform _hit, _average, _critical;
	public int Power;


	void Start () {
		_doppel = GameObject.FindGameObjectWithTag("Doppel");

		_hit = transform.Find("Hit");
		_average = transform.Find("Average");
		_critical = transform.Find("Critical");
	}
	
	void Update () {

		// POWER
		switch(Power){

			case 2 :
			GetComponent<SpriteRenderer>().sprite = _average.GetComponent<SpriteRenderer>().sprite;
			break;

			case 3 :
			GetComponent<SpriteRenderer>().sprite = _critical.GetComponent<SpriteRenderer>().sprite;
			break;

			default :
			GetComponent<SpriteRenderer>().sprite = _hit.GetComponent<SpriteRenderer>().sprite;
			break;
		}

		// FLIP X
		if (!_doppel.GetComponent<Doppel>()._facingRight) GetComponent<SpriteRenderer>().flipX = true;
		else GetComponent<SpriteRenderer>().flipX = false;

		// DEFINE ACTIVE
		if (Active) {
			_displayTimeElapsed += Time.deltaTime;
			transform.position = new Vector2 (_doppel.transform.position.x,
											  _doppel.transform.position.y + 5) + Random.insideUnitCircle * 0.25F;

		} else transform.position = new Vector2 (_doppel.transform.position.x, _doppel.transform.position.y + 5);

		if (_displayTimeElapsed >= DisplayDelay){
			Active = false;
			_displayTimeElapsed = 0;
		}
		
		// DISPLAY
		if (_displayTimeElapsed != 0) GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		else GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
	}
}
