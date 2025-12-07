using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancementJump : MonoBehaviour {

	public float _fallMultiplier = 2.5f;
	public float _lowJumpMultiplier = 2f;
	Rigidbody2D _rb;

	void Awake(){
		_rb = GetComponent<Rigidbody2D>();
	}

	void Update(){
		if (_rb.velocity.y < 0) _rb.velocity += new Vector2(0f, 9) * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
		else if (_rb.velocity.y > 0 && !Input.GetButton("Jump_Neidan")) _rb.velocity += new Vector2(0f, 9) * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
	}
}
