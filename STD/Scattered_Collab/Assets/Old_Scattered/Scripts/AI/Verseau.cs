using Scattered;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Verseau : MonoBehaviour
{
    #region FIELDS
    
    public bool distActivation;
    public bool ActivePattern;

    public int Hp;
    public float Speed;
    private float _originalSpeed;
    private float _direction;

    private Animator _animator;

    private GameObject _doppel, _neidan;

    public float IdleDelay;
    private float _idleTimeElapsed = 0;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _originalSpeed = Speed;

        _doppel = GameObject.FindGameObjectWithTag("Doppel");
        _neidan = GameObject.FindGameObjectWithTag("Neidan");
    }

    private void FixedUpdate()
    {        

        // COLOR
        if (GetComponentInChildren<FightTriggerFields>() != null)
        GetComponent<SpriteRenderer>().color = GetComponentInChildren<FightTriggerFields>()._color;

        // TIMERS
        _idleTimeElapsed += Time.deltaTime;

        // CHANGE IDLE / SPRINT
        if (_idleTimeElapsed > IdleDelay) Stop();

        // WATER
        GetComponentInChildren<ParticleGenerator>().particleForce.x = Random.Range(-1, 1);
        GetComponentInChildren<ParticleGenerator>().particleForce.y = Random.Range(-1, 1);

        if (Speed == 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Fight"))
            GetComponentInChildren<ParticleGenerator>().particlesState = DynamicParticle.STATES.WATER;
        else GetComponentInChildren<ParticleGenerator>().particlesState = DynamicParticle.STATES.NONE;

        // LIFE
        if (_doppel.GetComponentInChildren<FightSystem>()._fight
                && GetComponentInChildren<FightTriggerFields>()._fight
            && Hp > 0){              
                if(Time.timeScale >= 1) Hp--;

                Stop();
                _animator.SetBool("Fight", true);
        }
        else  _animator.SetBool("Fight", false);

        // MOVES
        if (_animator.GetBool("Fight")) MoveTo(_doppel.GetComponent<Doppel>().gameObject, true);
        else MoveTo(_neidan.GetComponent<Neidan>().gameObject, (_neidan.GetComponent<Neidan>().Alive && ActivePattern));

        // GRAPHICS UPDATE
        UpdateAnimation();
    }

    #endregion

    #region GRAPHICS METHODS

    public void UpdateAnimation()
    {
        // IDLE & SPRINT
        _animator.SetFloat("Speed", Speed);

        // IDLE
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Idle") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
            Sprint();
            _idleTimeElapsed = 0;
        }

        // DEATH
        if (Hp == 0) {
            Stop();
            _animator.SetBool("Alive", false);
            GetComponentInChildren<ParticleGenerator>().particlesState = DynamicParticle.STATES.NONE;
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Death") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                Destroy(this.gameObject);
        }
    }

    #endregion

    #region OTHERS METHODS

    public void Sprint(){
        Speed = _originalSpeed;
    }
    
    public void Stop(){
        Speed = 0;
    }

    public void MoveTo (GameObject target, bool condition){
            if (target.transform.position.x - this.transform.position.x < 0) _direction = -1;
            else _direction = 1;

            if (!condition) return;
            GetComponent<Transform>().Translate(Speed * _direction * Time.deltaTime, 0f, 0f);
            GetComponent<Transform>().localScale = new Vector3(_direction > 0 ? 1 : -1, 1, 1);
    }

    public GameObject Doppel{
        get { return _doppel; }
        set { _doppel = value; }
    }

    private void OnTriggerEnter2D(Collider2D target){
        if (target.gameObject.layer == LayerMask.NameToLayer("RayCast")
            && !distActivation) {
            ActivePattern = true;
            distActivation = true;
        }
    }

    #endregion
}
