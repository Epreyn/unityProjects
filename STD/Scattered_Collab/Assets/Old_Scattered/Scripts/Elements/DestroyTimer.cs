using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour {

	public bool _destroy;
	private float _endTimeElapsed;
	public float _endDelay;

	private Animator _animator;

	void Start () {
		_animator = GetComponent<Animator>();
	}
	
	void Update () {

		if(_animator != null) _animator.SetBool("Death", _destroy);

		if (!_destroy) return;
		_endTimeElapsed += Time.deltaTime;
		if (_endTimeElapsed > _endDelay) Destroy(this.gameObject);
	}
}
