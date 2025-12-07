using System.Collections;
using System.Collections.Generic;
using Scattered;
using UnityEngine;

public class Appear : MonoBehaviour {

	private float _opacityBase, _opacityShadow, _opacityLum;
	private Transform _base, _shadow, _lum;
	public int DisplayDistance;
	public GameObject _neidan;
	public bool Disappear;
	private float xDiff, yDiff;

	public bool Horizontal, Vertical;
	public float AppearRatio;


	void Start () {
		_opacityBase = 0;
		_opacityShadow = 0;
		_opacityLum = 0;
		
		_base = transform.Find("Base_Sprite");
		_shadow = transform.Find("Shadow_Sprite");
		_lum = transform.Find("Lum_Sprite");

		_neidan = GameObject.FindGameObjectWithTag("Neidan");
	}
	
	void FixedUpdate () {

		xDiff = _neidan.transform.position.x - this.transform.position.x;
		yDiff = (_neidan.transform.position.y - this.transform.position.y) / 2;

		if (App(xDiff, yDiff)) {	
			if (_opacityBase < 1) _opacityBase += Time.deltaTime * AppearRatio;
			if (_opacityBase >= 1 && _opacityShadow < 1) _opacityShadow += Time.deltaTime * AppearRatio;
			if (_opacityShadow >= 1 && _opacityLum < 1) _opacityLum += Time.deltaTime * AppearRatio;
		} else if (!App(xDiff, yDiff) && Disappear) {
			if (_opacityLum > 0) _opacityLum -= Time.deltaTime * AppearRatio;
			if (_opacityLum <= 0 && _opacityShadow > 0) _opacityShadow -= Time.deltaTime * AppearRatio;
			if (_opacityShadow <= 0 && _opacityBase > 0) _opacityBase -= Time.deltaTime * AppearRatio;
		}

		if (Horizontal) {
			_base.localScale = new Vector3(Mathf.SmoothStep(0, 1, _opacityBase), 1, 0);
			_shadow.localScale = new Vector3(Mathf.SmoothStep(0, 1, _opacityShadow), 1, 0);
			_lum.localScale = new Vector3(Mathf.SmoothStep(0, 1, _opacityLum), 1, 0);
		} else if (Vertical){
			_base.localScale = new Vector3(1, Mathf.SmoothStep(0, 1, _opacityBase), 0);
			_shadow.localScale = new Vector3(1, Mathf.SmoothStep(0, 1, _opacityShadow), 0);
			_lum.localScale = new Vector3(1, Mathf.SmoothStep(0, 1, _opacityLum), 0);
		}

		_base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, _opacityBase);
		_shadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, _opacityShadow);
		_lum.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, _opacityLum);

		_base.GetComponent<SpriteRenderer>().sortingOrder = (int)this.transform.position.z - 1;
		_shadow.GetComponent<SpriteRenderer>().sortingOrder = (int)this.transform.position.z;
		_lum.GetComponent<SpriteRenderer>().sortingOrder = (int)this.transform.position.z + 1;
	}

	private bool App (float x, float y){
		return (x < DisplayDistance && x > -DisplayDistance && y < DisplayDistance && y > -DisplayDistance);
	}

	private bool ElementVisible(bool bas, bool shadow, bool lum){
		return (bas && shadow && lum);
	}
}
