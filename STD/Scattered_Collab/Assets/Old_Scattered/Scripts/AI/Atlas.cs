using System.Collections;
using System.Collections.Generic;
using Scattered;
using UnityEngine;

public class Atlas : MonoBehaviour {
	
	#region FIELDS
    
    public int Hp;
    public float Speed;
    private float _originalSpeed;
	public bool Hit;
	private int _lastHp;

    private Animator _animator;

    private GameObject _neidan;
	private GameObject[] _enemies;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
		_enemies = GameObject.FindGameObjectsWithTag("Enemy");
		Hp = _enemies.Length;
		_lastHp = Hp;

        _animator = GetComponent<Animator>();
        _originalSpeed = Speed;

        _neidan = GameObject.FindGameObjectWithTag("Neidan");
    }

    private void FixedUpdate()
    {
		// LIFE
		_enemies = GameObject.FindGameObjectsWithTag("Enemy");
		Hp = _enemies.Length;
		if (_lastHp != Hp) Hit = true;

        // MOVES
		if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Hit")
			&& !_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Death"))
        MoveTo(_neidan.GetComponent<Neidan>().gameObject, _neidan.GetComponent<Neidan>().Alive);

        // GRAPHICS UPDATE
        UpdateAnimation();

		_lastHp = Hp;
    }

    #endregion

    #region GRAPHICS METHODS

	private void UpdateAnimation()
    {
        // IDLE & SPRINT
        _animator.SetFloat("Speed", Speed);

        // IDLE
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Idle") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            Sprint();
        }

		// HIT
		if (Hit) {
			Stop();
            _animator.SetBool("Hit", true);
		}

		if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Hit") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f){
            Hit = false;
			_animator.SetBool("Hit", false);
        }

        // DEATH
        if (Hp == 0) {
            Stop();
            _animator.SetBool("Alive", false);
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Death") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                Destroy(this.gameObject);
        }

        // EFFECT
        /*if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Idle")){
            Mat.SetFloat("_Magnitude", 0.03f);
        } else Mat.SetFloat("_Magnitude", 0f);*/
    }

    #endregion

    #region OTHERS METHODS

	private void Sprint(){
        Speed = _originalSpeed;
    }

	private void Stop(){
        Speed = 0;
    }

	private void MoveTo (GameObject target, bool condition){
            Speed = target.transform.position.x - this.transform.position.x < 0 ? 0 : _originalSpeed;

            if (!condition) return;
            GetComponent<Transform>().Translate(Speed * Time.deltaTime, 0f, 0f);
    }

    #endregion
}
