using System.Collections;
using System.Collections.Generic;
using Scattered;
using UnityEngine;

public class SpellSystem : MonoBehaviour {

	public GameObject SpellCirclePrefab;

	public bool _create;
	public bool _active;
	public float _radius;
	public string _emotion;

	public float _activeDelay;
	private float _activeTimeElpased;
	
	private float _waitingDelay = .5f, _waitingTimeElapsed;

	public Animator _animator;
	public Color _color;

	private GameObject _neidan;

	void Start () {
		_animator = GetComponent<Animator>();
		_neidan = GameObject.FindGameObjectWithTag("Neidan");
	}
	
	void Update () {

		_neidan.GetComponent<Neidan>()._freeze = _animator.GetBool("Etonnement");

		if (_create){
			GameObject t = Instantiate(SpellCirclePrefab, GetComponentInParent<Doppel>().transform.position, Quaternion.identity) as GameObject;
			t.transform.parent = transform;
			t.GetComponent<CircleCollider2D>().radius = _radius;
			t.GetComponent<CircleCollider2D>().enabled = _active;
			_create = false;
		}

		GetComponent<SpriteRenderer>().enabled = _active;

		DoppelColor();

		if (_active){
			if (_activeTimeElpased < _activeDelay) {
				_activeTimeElpased += Time.deltaTime;

				// CHAGRIN
				if (_animator.GetBool("Chagrin")){
					//t.GetComponent<CircleCollider2D>().radius += Time.deltaTime * 2;
					//transform.localScale += new Vector3(Time.deltaTime * 2, Time.deltaTime * 2, 1);
					GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
				}

				// ETONNEMENT
				if (_animator.GetBool("Etonnement")){

					float x = Input.GetAxis("Horizontal");
 					float y = Input.GetAxis("Vertical");

					int xInt = 0;
					int yInt = 0;
					
					if (x > 0) xInt = 1;
					else if (x < 0) xInt = -1;
					if (y > 0) yInt = 1;
					else if (y < 0) yInt = -1;

					var InitialVelocity = new Vector2(xInt * 50, yInt * 50);

					if (Input.GetButton("Vertical") || Input.GetButton("Horizontal")){

						if (_waitingTimeElapsed < _waitingDelay) {
							_waitingTimeElapsed += Time.deltaTime;
						}
						else {
							_waitingTimeElapsed = 0;
							
							GetComponentInParent<Doppel>().Erased = false;
							GetComponentInParent<Doppel>().EnableDoppel(true);
							GetComponentInParent<Doppel>()._rigidbody2D.velocity =  new Vector2(InitialVelocity.x, InitialVelocity.y);
							GetComponentInParent<Doppel>()._changeVelocity = true;
							
							_active = false;
							_animator.SetBool("Etonnement", false);
						}						
					}
				}
			}
			else {
				_activeTimeElpased = 0;
				_active = false;

				_animator.SetBool("Etonnement", false);
			}
		}
	}

	void DoppelColor(){
		if (_active) GetComponentInParent<Doppel>()._color = _color;
		else GetComponentInParent<Doppel>()._color = Color.white;
	}
}
