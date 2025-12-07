using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTriggerFields : MonoBehaviour {

	public float _radius;
	public bool _fight;

	public string _scriptName;
	public int _originalHp, _currentHp;
    
    public bool _dot, _hit, _freeze;
    public Color _color;

    private float _deltaTime;
    private int _roundedTime;

	void Start () {

		GetComponent<CircleCollider2D>().radius = _radius;

		if (GetComponentInParent<Verseau>()) _scriptName = GetComponentInParent<Verseau>().name;
        if (GetComponentInParent<Pleaunte>()) _scriptName = GetComponentInParent<Pleaunte>().name;
        if (GetComponentInParent<Harpie>()) _scriptName = GetComponentInParent<Harpie>().name;
        if (GetComponentInParent<Bouboule>()) _scriptName = GetComponentInParent<Bouboule>().name;

		switch(_scriptName){

                case "Verseau":
                    _originalHp = GetComponentInParent<Verseau>().Hp;
				    _currentHp = GetComponentInParent<Verseau>().Hp;
                break;

                case "Pleaunte":
                    _originalHp = GetComponentInParent<Pleaunte>().Hp;
				    _currentHp = GetComponentInParent<Pleaunte>().Hp;
                break;

                case "Harpie":
                    _originalHp = GetComponentInParent<Harpie>().Hp;
				    _currentHp = GetComponentInParent<Harpie>().Hp;
                break;

                case "Bouboule":
                    _originalHp = GetComponentInParent<Bouboule>().Hp;
				    _currentHp = GetComponentInParent<Bouboule>().Hp;
                break;
        }
	}
	
	void Update () {

        if (_currentHp <= 0) Destroy(this.gameObject);

		switch(_scriptName){

                case "Verseau":
				    _currentHp = GetComponentInParent<Verseau>().Hp;

                    // TERREUR
                    if (GetComponentInParent<Verseau>().distActivation) GetComponentInParent<Verseau>().ActivePattern = !_freeze;
                    if (_freeze) _color = Color.green;

                    // CHAGRIN
                    if (_dot && GetComponentInParent<Verseau>().Hp > 0){
                        
                        _deltaTime += Time.deltaTime;
                        _roundedTime = (int)_deltaTime;
                        if (_roundedTime % 5 == 0) GetComponentInParent<Verseau>().Hp--;
                        _color = Color.blue;
		            }

                    // RAGE
                    if (_hit && GetComponentInParent<Verseau>().Hp > 0){
                        GetComponentInParent<Verseau>().Hp--;
                        _color = Color.red;
		            }

                    if (!_hit && !_dot && !_freeze) _color = Color.white;
                break;

                case "Pleaunte":
				    _currentHp = GetComponentInParent<Pleaunte>().Hp;

                    // TERREUR
                    GetComponentInParent<Pleaunte>().ActivePattern = !_freeze;
                    if (_freeze) _color = Color.green;

                    // CHAGRIN
                    if (_dot && GetComponentInParent<Pleaunte>().Hp > 0){
                        
                        _deltaTime += Time.deltaTime;
                        _roundedTime = (int)_deltaTime;
                        if (_roundedTime % 5 == 0) GetComponentInParent<Pleaunte>().Hp--;
                        _color = Color.blue;
		            }

                    // RAGE
                    if (_hit && GetComponentInParent<Pleaunte>().Hp > 0){
                        GetComponentInParent<Pleaunte>().Hp--;
                        _color = Color.red;
		            }

                    if (!_hit && !_dot && !_freeze) _color = Color.white;
                break;

                case "Harpie":
				    _currentHp = GetComponentInParent<Harpie>().Hp;

                    // TERREUR
                    GetComponentInParent<Harpie>().ActivePattern = !_freeze;
                    if (_freeze) _color = Color.green;

                    // CHAGRIN
                    if (_dot && GetComponentInParent<Harpie>().Hp > 0){
                        
                        _deltaTime += Time.deltaTime;
                        _roundedTime = (int)_deltaTime;
                        if (_roundedTime % 5 == 0) GetComponentInParent<Harpie>().Hp--;
                        _color = Color.blue;
		            }

                    // RAGE
                    if (_hit && GetComponentInParent<Harpie>().Hp > 0){
                        GetComponentInParent<Harpie>().Hp--;
                        _color = Color.red;
		            }

                    if (!_hit && !_dot && !_freeze) _color = Color.white;
                break;

                case "Bouboule":
				    _currentHp = GetComponentInParent<Bouboule>().Hp;

                    // TERREUR
                    GetComponentInParent<Bouboule>().ActivePattern = !_freeze;
                    if (_freeze) _color = Color.green;

                    // CHAGRIN
                    if (_dot && GetComponentInParent<Bouboule>().Hp > 0){
                        
                        _deltaTime += Time.deltaTime;
                        _roundedTime = (int)_deltaTime;
                        if (_roundedTime % 5 == 0) GetComponentInParent<Bouboule>().Hp--;
                        _color = Color.blue;
		            }

                    // RAGE
                    if (_hit && GetComponentInParent<Bouboule>().Hp > 0){
                        GetComponentInParent<Bouboule>().Hp--;
                        _color = Color.red;
		            }

                    if (!_hit && !_dot && !_freeze) _color = Color.white;
                break;
        }
	}
}
