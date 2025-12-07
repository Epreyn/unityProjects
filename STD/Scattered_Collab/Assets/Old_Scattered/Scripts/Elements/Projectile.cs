using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public Vector2 InitialVelocity;
	private Rigidbody2D _rigidbody2D;
	private Animator _animator;

	void Awake(){
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}

	void Start () {
		var startVelX = InitialVelocity.x * transform.localScale.x;
		_rigidbody2D.velocity = new Vector2(startVelX, InitialVelocity.y);
	}
	
	void Update () {
		if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Water_Explosion") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D target){
		if (target.gameObject.transform.position.y < transform.position.y) {
			GetComponentInParent<Collider2D>().enabled = false;
			_animator.SetBool("Alive", false);
		}
	}
}
