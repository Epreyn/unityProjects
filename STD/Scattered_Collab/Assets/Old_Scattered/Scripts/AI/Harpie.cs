using Scattered;
using UnityEngine;

public class Harpie : MonoBehaviour {

    #region FIELDS

    public bool ActivePattern;

    public int Hp;
    public float Speed;
    private float _originalSpeed;

    private float _direction;
    private bool _facingRight, _waterTouched = false;

    private Animator _animator;

    public GameObject _doppel, _neidan;

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
        // LIFE
        if (_doppel.GetComponentInChildren<FightSystem>()._fight
            && GetComponentInChildren<FightTriggerFields>()._fight
            && Hp > 0){              
                if(Time.timeScale >= 1) Hp--;

                Stop();
                _animator.SetBool("Fight", true);
        }
        else  _animator.SetBool("Fight", false);

        // WATER
        if (GetComponentInChildren<WaterTrigger>().Hit) _waterTouched = true;

        // DIRECTION
        if (_neidan.GetComponent<Neidan>().transform.position.x - this.transform.position.x < 0){
            _direction = -1;
            _facingRight = true;
        }
        else{
            _direction = 1;
            _facingRight = false;
        }

        // MOVES
        if (_neidan.GetComponent<Neidan>().Alive
            && _neidan.GetComponent<Neidan>().FacingRight != _facingRight
            && Hp != 0
            && ActivePattern){
            MoveTo(_neidan);
        }
        else Speed = 0;

        // DEATH
        if (Hp == 0){
            _animator.SetBool("Alive", false);
            Speed = 0;
        }

        UpdateAnimation();
    }

    #endregion

    #region OTHERS METHODS
    
    public void UpdateAnimation()
    {
        // SPRINT & WALK & IDLE
        _animator.SetFloat("Speed", Speed);
        _animator.SetBool("Sprint", _waterTouched);

        // DEATH
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Death") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                Destroy(this.gameObject);
            }
    }

    public void MoveTo (GameObject target){
        if (target.transform.position.x - this.transform.position.x < 0) _direction = -1;
        else _direction = 1;

        if (_waterTouched) Speed = _originalSpeed * 2;
        else Speed = _originalSpeed;
        
        GetComponent<Transform>().Translate(Speed * _direction * Time.deltaTime, 0f, 0f);
        GetComponent<Transform>().localScale = new Vector3(_direction > 0 ? 1 : -1, 1, 1);
    }

    public void Stop(){
        Speed = 0;
    }

    #endregion
}
