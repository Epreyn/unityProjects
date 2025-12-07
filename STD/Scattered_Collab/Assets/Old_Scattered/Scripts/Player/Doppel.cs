using System;
using System.Collections;
using UnityEngine;

namespace Scattered
{
    public class Doppel : MonoBehaviour
    {
        #region FIELDS

        // CONTROL
        public float MaxSpeed;
        public bool AirControl;

        // GROUND
        public LayerMask _whatIsGround;
        private Transform _groundCheck;
        const float GroundedRadius = .2f;
        public bool _grounded;

        // COMPONENTS
        public Animator _animator;
        public Rigidbody2D _rigidbody2D;
        public bool _changeVelocity;
        private float _velDelay = 0.75f, _velTimeElapsed;

        // STATES
        public bool _facingRight = true;
        public bool Erased = true, Usable = true;

        private bool _onRayCast;
        public float OutRayCastDelay;
        private float _outRayCastTimer = 0;

        // FLAME
        private GameObject _neidan;
        private Transform _flame;
        public Color _color;

        public GameObject Attract, Repulse;

        #endregion

        #region UNITY METHODS

        private void Awake() {
            _groundCheck = transform.Find("GroundCheck");
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _flame = transform.Find("Doppel_Flame");
            _neidan = GameObject.FindGameObjectWithTag("Neidan");

            _color = Color.white;
        }

        private void Update() {
            GetComponent<SpriteRenderer>().color = _color;

            _grounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, GroundedRadius, _whatIsGround);
            for (int i = 0; i < colliders.Length; i++){
                if (colliders[i].gameObject != gameObject) _grounded = true;
            }

            _animator.SetBool("Ground", _grounded);
            _animator.SetFloat("vSpeed", _rigidbody2D.velocity.y);
            _animator.SetBool("Fight", GetComponentInChildren<FightSystem>()._fight);
            
            if (!_onRayCast) {
                _outRayCastTimer += Time.deltaTime;
                if (_outRayCastTimer > OutRayCastDelay) Erased = true;
            }
            else _outRayCastTimer = 0;

            if (Erased){
                GetComponentInChildren<FightSystem>().GetComponent<CircleCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;
            }
            EnableFlame(Erased);
        }

        public void Move(float move) {

            if (!Usable) return;
            if (GetComponentInChildren<SpellSystem>()._animator.GetBool("Etonnement")) return;
            
            EnableDoppel(move != 0);

            if (_grounded || AirControl) {
                _animator.SetFloat("Speed", Mathf.Abs(move));

                if (_changeVelocity){
                    if (_velTimeElapsed < _velDelay) _velTimeElapsed += Time.deltaTime;
                    else if (move != 0){
                            _velTimeElapsed = 0;
                            _changeVelocity = false;
                    }
                }
                else _rigidbody2D.velocity = new Vector2(move * MaxSpeed, _rigidbody2D.velocity.y);

                if (((move > 0 && !_facingRight) || (move < 0 && _facingRight))
                    && !GetComponentInChildren<FightSystem>()._fight) Flip();
            }

            if (_grounded && _animator.GetBool("Ground")) {
                _grounded = false;
                _animator.SetBool("Ground", false);
            }
        }

        #endregion

        #region OTHERS MEHODS

        private void Flip(){
            _facingRight = !_facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        void OnTriggerEnter2D(Collider2D coll){
            if (coll.gameObject.layer == LayerMask.NameToLayer("RayCast")) {
                _onRayCast = true;
            }
        }

        void OnTriggerStay2D(Collider2D coll){
            if (coll.gameObject.layer == LayerMask.NameToLayer("RayCast")) _onRayCast = true;
        }

        void OnTriggerExit2D(Collider2D coll){
            if (coll.gameObject.layer == LayerMask.NameToLayer("RayCast")) _onRayCast = false;
        }

        public void EnableDoppel(bool isMoving){
            if (!isMoving) return;
            Erased = false;
            GetComponentInChildren<FightSystem>().ReFight();
            GetComponent<SpriteRenderer>().enabled = true;
        }

        public void EnableFlame(bool enabled){
            if (enabled) {
                _flame.GetComponent<SpriteRenderer>().enabled = true;
                this.transform.position = _neidan.transform.position;
            }
            else _flame.GetComponent<SpriteRenderer>().enabled = false;
        }

        public void Adoration(){
            Instantiate(Attract, transform.position, Quaternion.identity);
        }

        public void Aversion(){
            Instantiate(Repulse, transform.position, Quaternion.identity);
        }

        #endregion
    }
}
