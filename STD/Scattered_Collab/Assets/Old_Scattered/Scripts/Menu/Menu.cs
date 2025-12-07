using System.Collections;
using System.Collections.Generic;
using Scattered;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	private bool[] _passivesEnable;
	private GameObject[] _passives;

	private GameObject[] _bars;

	private GameObject _neidan, _doppel;
	private GameObject[] _ennemies;
	private int _lastLengthEnnemies;

	private float _angle;
	private float[] _angles;

	private Color _choice;
	private Color[] _originalColor;

	public GameObject _noName;
	private GameObject _name;
	private GameObject[] _names;

	[Range(0.0f, 1.0f)]
	private float _amountExtase;
	[Range(0.01f, 1.0f)]
	public float _smoothBulletTime;
	private float[] _originalFillAmount;
	private bool _extaseActive;



	void Start () {
		// LOADING EFFECTORS
		_neidan = GameObject.FindGameObjectWithTag("Neidan");
		_doppel = GameObject.FindGameObjectWithTag("Doppel");
		_ennemies = GameObject.FindGameObjectsWithTag("Enemy");
		_lastLengthEnnemies = _ennemies.Length;

		// LOADING PASSIVES
		_passives = GameObject.FindGameObjectsWithTag("Passive");
		_passivesEnable = new bool[_passives.Length];
		for(int i = 0; i < _passives.Length; i++) _passivesEnable[i] = false;
		
		// LOADING BARS
		_bars = GameObject.FindGameObjectsWithTag("Bar");
		foreach(GameObject b in _bars) b.GetComponent<Image>().fillAmount = 0;

		// LOADING NAMES
		_name = GameObject.FindGameObjectWithTag("MainName");
		_names = GameObject.FindGameObjectsWithTag("Name");
		foreach(GameObject n in _names) n.GetComponent<Image>().color = new Color(1,1,1,0);

		_originalFillAmount = new float[_bars.Length];
		
		// LOADING SELECTION
		_angles = new float[_bars.Length];
		_originalColor = new Color[_bars.Length];
		_choice = new Color(0, 0, 0, 1);
		for (int i = 0; i < _bars.Length; i++) _originalColor[i] = _bars[i].GetComponent<Image>().color;
	}
	
	void Update () {
		// ACTIVE PASSIVES
		for(int i = 0; i < _passives.Length; i++) _passives[i].SetActive(_passivesEnable[i]);

		// ROUND BULLET VAR
		if (_smoothBulletTime < 0.01f) _smoothBulletTime = 0.01f;
		if (_smoothBulletTime > 1f) _smoothBulletTime = 1f;

		// CONDITIONS BARS
		foreach (GameObject b in _bars){
			switch(b.name){

				case "Bar_Extase":
				b.GetComponent<Image>().fillAmount = _amountExtase;
				break;

				case "Bar_Adoration":
				if (!_doppel.GetComponent<Doppel>().Erased) b.GetComponent<Image>().fillAmount += Time.deltaTime / 30;
				break;
				
				case "Bar_Terreur":
				foreach(GameObject e in _ennemies){
					if (e != null){
						if (Mathf.Abs(_neidan.transform.position.x - e.transform.position.x) < 5
							&& Mathf.Abs(_neidan.transform.position.y - e.transform.position.y) < 10
							&& _neidan.GetComponent<Neidan>().Alive)
							b.GetComponent<Image>().fillAmount += Time.deltaTime / 10;
					}
				}
				break;
				
				case "Bar_Etonnement":
				if ((_neidan.GetComponent<UserControl>()._move != 0
					|| !_neidan.GetComponent<Neidan>()._grounded)
					&& _neidan.GetComponent<Neidan>().Alive) b.GetComponent<Image>().fillAmount += Time.deltaTime / 5;
				break;
				
				case "Bar_Chagrin":
				if (_neidan.GetComponent<Neidan>()._invincible) b.GetComponent<Image>().fillAmount += Time.deltaTime / 3;
				break;
				
				case "Bar_Aversion":
				_ennemies = GameObject.FindGameObjectsWithTag("Enemy");
				if (_lastLengthEnnemies != _ennemies.Length) b.GetComponent<Image>().fillAmount += 0.15f;
				break;
				
				case "Bar_Rage":
				if (_doppel.GetComponent<Doppel>()._animator.GetBool("Fight")) b.GetComponent<Image>().fillAmount += Time.deltaTime / 10;
				break;
				
				case "Bar_Vigilance":
				if ((_doppel.GetComponent<UserControlDoppel>()._move != 0
					|| !_doppel.GetComponent<Doppel>()._grounded)
					&& !_doppel.GetComponent<Doppel>().Erased) b.GetComponent<Image>().fillAmount += Time.deltaTime / 5;
				break;
				
				default:
				break;
			}
		}
		
		// CHOICE SPELL
		if(Input.GetButton("Throw") && UsableSpell()) {

			if (_smoothBulletTime > 0.01f) _smoothBulletTime -= Time.deltaTime * 5;
			Time.timeScale = _smoothBulletTime;

			// On récupère les valeurs du Stick 
			float x = Input.GetAxis("Horizontal"); // De -1 à Gauche à +1 à Droite (0 étant au Centre)
 			float y = Input.GetAxis("Vertical");   // De -1 à Bas    à +1 à Haut   (0 étant au Centre)
 			
			// Si le Stick est poussé dans une direction
			if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")){

				// On calcule l'angle du stick avec les valeurs récupérées plus hauts
				_angle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
				
				// Pour chaque pouvoir prêt à l'emploi
				for(int i = 0; i < _bars.Length; i++){

					// On dévit le départ du cercle de 90 degrés pour qu'il soit en haut et non à droite
					float a = _bars[i].transform.rotation.eulerAngles.z;
					a = a + 90;
 					a = (a > 180) ? a - 360 : a;
					_angles[i] = a;

					if (_angles[i] >= _angle - 20 && _angles[i] <= _angle + 20
						&& _bars[i].GetComponent<Image>().fillAmount == 1f){
							_bars[i].GetComponent<Image>().color = _choice;
							
							var s = _bars[i].name;
							s = s.Remove(0,4);
							s = s + "Name";

							foreach(GameObject n in _names){
								// Si l'émotion est séléctionnée on change la couleur de l'image en NOIR
								if (n.name == s) _name.GetComponent<Image>().sprite = n.GetComponent<Image>().sprite;
							}
						}
					else 
						// Si l'émotion est désélectionnée on change la couleur de l'image en sa couleur d'origine
						_bars[i].GetComponent<Image>().color = _originalColor[i];
				}
			}
		}
		else {
			if (_smoothBulletTime < 1f) _smoothBulletTime += Time.deltaTime * 10;
			Time.timeScale = _smoothBulletTime;

			for(int i = 0; i < _bars.Length; i++){
					if (_bars[i].GetComponent<Image>().color == _choice){
						_bars[i].GetComponent<Image>().fillAmount = 0;
						
						if (_amountExtase != 1) _amountExtase += 0.1f;
						if (_extaseActive){
							for(int j = 0; j < _bars.Length; j++) {
								_bars[j].GetComponent<Image>().fillAmount = _originalFillAmount[j];
								_extaseActive = false;
							}
						}

						Spell(_bars[i].name, _originalColor[i]);

						_bars[i].GetComponent<Image>().color = _originalColor[i];
						_name.GetComponent<Image>().sprite = _noName.GetComponent<Image>().sprite;
					}
			}
		}

		Time.fixedDeltaTime = 0.02f * Time.timeScale;

		// ANIMATION
		//UpdateAnimation();

		// RESET VARIABLES
		_lastLengthEnnemies = _ennemies.Length;
	}

	void UpdateAnimation (){

		// FULL BARS
		foreach(GameObject b in _bars){
			if (b.GetComponent<Image>().fillAmount == 1f)
				b.GetComponent<Image>().color = new Color(b.GetComponent<Image>().color.r,
														  b.GetComponent<Image>().color.g,
														  b.GetComponent<Image>().color.b,
														  Mathf.Lerp(0.0f, 1.0f, Mathf.PingPong(Time.time, 1.0f)));
		}
	}

	void Spell(string s, Color c){
		
		// COLOR
		_doppel.GetComponentInChildren<SpellSystem>()._color = c;

		switch(s){

				case "Bar_Extase":
					for(int i = 0; i < _bars.Length; i++) {
						_originalFillAmount[i] = _bars[i].GetComponent<Image>().fillAmount;
						_bars[i].GetComponent<Image>().fillAmount = 1f;
					}
					_amountExtase = 0f;
					_extaseActive = true;
				break;

				case "Bar_Adoration":
					_doppel.GetComponent<Doppel>().Adoration();
				break;
				
				case "Bar_Terreur":
					_doppel.GetComponentInChildren<SpellSystem>()._create = true;
					_doppel.GetComponentInChildren<SpellSystem>()._active = true;
					_doppel.GetComponentInChildren<SpellSystem>()._radius = 20;
					_doppel.GetComponentInChildren<SpellSystem>()._emotion = s;
					_doppel.GetComponentInChildren<SpellSystem>()._activeDelay = 3;
				break;
				
				case "Bar_Etonnement":
					_doppel.GetComponent<Doppel>().Erased = true;
					_doppel.GetComponentInChildren<SpellSystem>()._animator.SetBool("Etonnement", true);
					_doppel.GetComponentInChildren<SpellSystem>()._active = true;
					_doppel.GetComponentInChildren<SpellSystem>()._emotion = s;
					_doppel.GetComponentInChildren<SpellSystem>()._activeDelay = 1.5f;
				break;
				
				case "Bar_Chagrin":
					_doppel.GetComponentInChildren<SpellSystem>()._animator.SetBool("Chagrin", true);
					_doppel.GetComponentInChildren<SpellSystem>()._create = true;
					_doppel.GetComponentInChildren<SpellSystem>()._active = true;
					_doppel.GetComponentInChildren<SpellSystem>()._radius = 10;
					_doppel.GetComponentInChildren<SpellSystem>()._emotion = s;
					_doppel.GetComponentInChildren<SpellSystem>()._activeDelay = 2f;
				break;
				
				case "Bar_Aversion":
					_doppel.GetComponent<Doppel>().Aversion();
				break;
				
				case "Bar_Rage":
					_doppel.GetComponentInChildren<SpellSystem>()._create = true;
					_doppel.GetComponentInChildren<SpellSystem>()._active = true;
					_doppel.GetComponentInChildren<SpellSystem>()._radius = 20;
					_doppel.GetComponentInChildren<SpellSystem>()._emotion = s;
					_doppel.GetComponentInChildren<SpellSystem>()._activeDelay = 4;
				break;
				
				case "Bar_Vigilance":
					var _temp = _neidan.transform.position;
					_neidan.transform.position = _doppel.transform.position;
					_doppel.transform.position = _temp;

					_neidan.GetComponent<Neidan>()._changeVelocity = _doppel.GetComponent<Doppel>()._changeVelocity;

					var _tempVel = _neidan.GetComponent<Rigidbody2D>().velocity;
					_neidan.GetComponent<Rigidbody2D>().velocity = _doppel.GetComponent<Rigidbody2D>().velocity;
					_doppel.GetComponent<Rigidbody2D>().velocity = _tempVel;
				break;
				
				default:
				break;
		}
	}

	bool UsableSpell(){
		var t = 0;
		foreach(GameObject b in _bars) if (b.GetComponent<Image>().fillAmount == 1) t += 1;
		return (t != 0);
	}
}
