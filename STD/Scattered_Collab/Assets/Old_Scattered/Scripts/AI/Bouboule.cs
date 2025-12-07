using Scattered;
using UnityEngine;

public class Bouboule : MonoBehaviour{

		#region FIELDS

	    public bool ActivePattern;

    	public int Hp;
		public float Speed, MaxSpeed;
     	private bool _transformed = false, _isTransforming = false, _isBreathing = false, _isPassed = false, _triggerOnce = false;

		private float _direction = 1, _lastDirection, _dir = -1;
		private float DirectionTimeElapsed;
		public float DirectionDelay;

	    private Animator _animator;
		private CapsuleCollider2D _capsuleCollider2D;
		private BoxCollider2D _boxCollider2D;

	    public GameObject _doppel, _neidan;
		public GameObject Attract;

		#endregion

		#region UNITY METHODS

		private void Awake()
	    {
	        _animator = GetComponent<Animator>();
			_capsuleCollider2D = GetComponent<CapsuleCollider2D>();
			_boxCollider2D = GetComponentInChildren<BoxCollider2D>();

			DirectionDelay = Random.Range(2, 5);

			_doppel = GameObject.FindGameObjectWithTag("Doppel");
			_neidan = GameObject.FindGameObjectWithTag("Neidan");
	    }

	    private void FixedUpdate()
	    {
			// TIMERS
			if (!_transformed) DirectionTimeElapsed += Time.deltaTime;

	        // LIFE
	        if (!_transformed
	            && Hp > 0
	            && _doppel.GetComponentInChildren<FightSystem>()._fight
            	&& GetComponentInChildren<FightTriggerFields>()._fight
				&& Time.timeScale >= 1) Hp--;

			if (_transformed
	            && Hp > 0
	            && _doppel.GetComponentInChildren<FightSystem>()._fight
            	&& GetComponentInChildren<FightTriggerFields>()._fight
				&& Time.timeScale >= 1) Hp--;

			// CHANGE HEIGHT
			if (_transformed
				&& _capsuleCollider2D.size != new Vector2(5, 4)){
				
				Hp = 5;
				
				// DEF DIR
				if (_neidan.GetComponent<Neidan>().transform.position.x - this.transform.position.x < 0) _direction = -1;
				else _direction = 1;

				_capsuleCollider2D.size = new Vector2(5, 4);
				_capsuleCollider2D.offset = new Vector2 (1.5f, -3.5f);
				_boxCollider2D.size = new Vector2(10, 4);
				_boxCollider2D.offset = new Vector2 (0, -3.5f);
			}

			// DEATH
	        if (Hp <= 0)
	        {
	            _animator.SetBool("Alive", false);
	            Speed = 0;

				_isBreathing = false;

	        }

	        UpdateAnimation();
	    }

		#endregion

		#region OTHERS METHODS

	    public void UpdateAnimation()
	    {
	        // SPRINT & IDLE
	        _animator.SetFloat("Speed", Speed);
			_animator.SetBool("Transformed", _transformed);
			_animator.SetBool("isTransforming", _isTransforming);
			_animator.SetBool("isBreathing", _isBreathing);

			// MOVES
			if (!_transformed){

				if (DirectionTimeElapsed > DirectionDelay){ 

					_direction = Random.Range(-1, 2);
					DirectionTimeElapsed = 0;
					DirectionDelay = Random.Range(2, 5);
					if (_direction != 0) _lastDirection = _direction;
				}

				if (_direction != 0 && Hp != 0) Speed = 1;
				else Speed = 0;

				if (!_animator.GetCurrentAnimatorStateInfo (0).IsName ("Animation_Transform"))
				GetComponent<Transform>().Translate(Speed * _direction * Time.deltaTime, 0f, 0f);
				
				if (_direction != 0) GetComponent<Transform>().localScale = new Vector3(_direction, 1, 1);
				else GetComponent<Transform>().localScale = new Vector3(_lastDirection, 1, 1);
			}
			else {
				if (!_isPassed) {
					if (_neidan.GetComponent<Neidan>().transform.position.x - this.transform.position.x < 0) _dir = -1;
					else _dir = 1;
				}

				if (_dir != _lastDirection) _isPassed = true;

				if (_isPassed){
					if (Speed > 0) Speed -= 0.2f;
					else _isBreathing = true;
				}
				else if (Speed < MaxSpeed) Speed += 0.1f;

				_lastDirection = _dir;

				if (_neidan.GetComponent<Neidan>().Alive
					&& ActivePattern){
					GetComponent<Transform>().Translate(Speed * _direction * Time.deltaTime, 0f, 0f);
					GetComponent<Transform>().localScale = new Vector3(_direction, 1, 1);
				}
				else Speed = 0;
			}

			// TRANSFORMING
			if (GetComponentInChildren<WaterTrigger>().Hit && _transformed == false) _isTransforming = true;
			if (_animator.GetCurrentAnimatorStateInfo (0).IsName ("Animation_Transform") &&
			    _animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
					_isTransforming = false;
					_transformed = true;
			}

			// BREATHING
			if (_isBreathing && !_triggerOnce
				&& ActivePattern){
				Instantiate(Attract, transform.position, Quaternion.identity);
				_triggerOnce = true;

				if (_neidan.GetComponent<Neidan>().transform.position.x - this.transform.position.x < 0) _direction = -1;
					else _direction = 1;
			}

			if (_animator.GetCurrentAnimatorStateInfo (0).IsName ("Animation_Breath") &&
			    _animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
					_isBreathing = false;
					_isPassed = false;
					_triggerOnce = false;
				}
			
			// DEATH LITTLE
			if (_animator.GetCurrentAnimatorStateInfo (0).IsName ("Animation_Death_Little") &&
			    _animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f) {
					Destroy (this.gameObject);
				}

			// DEATH
			if (_animator.GetCurrentAnimatorStateInfo (0).IsName ("Animation_Death") &&
			    _animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f){
					Destroy (this.gameObject);
				}
	    }

		#endregion
}