using System;
using System.Collections;
using UnityEngine;

namespace Scattered
{
    public class Neidan : MonoBehaviour
    {
        public float _velY;

        public float _maxSpeed;
        public float _jumpForce;
        public bool _airControl;
        public LayerMask _whatIsGround;

        private Transform _groundCheck;
        const float GroundedRadius = .2f;
        public bool _grounded;
        private bool _lastGrounded;

        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        public bool _changeVelocity;
        private float _velDelay = 0.75f, _velTimeElapsed;
        public bool FacingRight = true;

        public bool Alive = true;
        private bool _throwing = false;
        private bool _reviving = false;
        private bool _endJump = false;

        public float InvincibleDelay;
        private float InvincibleTimer;
        public bool _invincible = false;

        public bool _freeze;

        public bool _audioFSPlayOnce;

        private void Awake() {
            _groundCheck = transform.Find("GroundCheck");
            _animator = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        #region Update Methods

        private void FixedUpdate()
        {
            if (_freeze) GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            else GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

            _grounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, GroundedRadius, _whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    _grounded = true;
            }

            _animator.SetBool("Ground", _grounded);
            _velY = Mathf.Clamp(_rigidbody2D.velocity.y, -100f, 50f);
            _animator.SetFloat("vSpeed", _velY);

            // INVINCIBLE
            if (_invincible){
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.4f);

                if (InvincibleTimer > InvincibleDelay) {
                    InvincibleTimer = 0;
                    _invincible = false;
                }
                else InvincibleTimer += Time.deltaTime;
            }
            else GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

            if (!_lastGrounded && _grounded) {
                _endJump = true;
                _animator.SetBool("EndJump", _endJump);
            }

            _lastGrounded = _grounded;
        }

        public void Move(float move, bool jump)
        {
            if (!Alive) return;
            if (_throwing) return;
            if (_reviving) return;

            if (_grounded || _airControl)
            {
                _animator.SetFloat("Speed", Mathf.Abs(move));

                if (_changeVelocity){
                    if (_velTimeElapsed < _velDelay) _velTimeElapsed += Time.deltaTime;
                    else if (move != 0){
                            _velTimeElapsed = 0;
                            _changeVelocity = false;
                    }
                }
                else _rigidbody2D.velocity = new Vector2(move * _maxSpeed, _rigidbody2D.velocity.y);

                if (move > 0 && !FacingRight)
                    Flip();
                else if (move < 0 && FacingRight)
                    Flip();
            }

            if (_grounded && jump && _animator.GetBool("Ground"))
            {
                _grounded = false;
                _animator.SetBool("Ground", false);
                _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
            }
        }

        public void UpdateAnimation()
        {
            PlayingOnceOnSpecificSprite(_audioFSPlayOnce, "Clic", "Neidan_8", "Neidan_13");

            // END JUMP
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_EndJumpToIdle") &&
                _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                _endJump = false;
                _animator.SetBool("EndJump", _endJump);
            }
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_EndJumpToSprint") &&
                _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                _endJump = false;
                _animator.SetBool("EndJump", _endJump);
            }

            // DEATH
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Death") &&
                _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                ////////////////////////
                // LIMBES ACTION HERE //
                ////////////////////////

                _reviving = true;
                _animator.SetBool("Revive", true);
            }

            // REVIVING
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Revive") &&
                _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Alive = true;
                _animator.SetBool("Alive", true);

                _reviving = false;
                _animator.SetBool("Revive", false);

                _invincible = true;
            }

            // THROW
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Throw") &&
                _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                _throwing = false;
                _animator.SetBool("Throw", false);
            }
        }

        #endregion

        #region OTHERS METHODS

        public void IsAlive(){
            _throwing = false;
            _animator.SetBool("Throw", false);

            Alive = false;
            _animator.SetBool("Alive", false);

            _rigidbody2D.velocity = Vector2.zero;
        }

        public void Throw(){
            if (!_grounded) return;

            _rigidbody2D.velocity = Vector2.zero;
            //_doppel.GetComponent<Doppel>().PowerUp = true;

            _throwing = true;
            _animator.SetBool("Throw", true);
        }

        private void Flip(){
            FacingRight = !FacingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        void OnTriggerEnter2D(Collider2D coll){
            if (_invincible) return;

            if (coll.gameObject.tag == "Projectile") IsAlive();
        }

        void PlayingOnceOnSpecificSprite(bool playedOnce, string soundName, params string[] spriteNames){

            foreach(string s in spriteNames){
                if (GetComponent<SpriteRenderer>().sprite.name == s
                    && !playedOnce) {
                    FindObjectOfType<AudioManager>().Play(soundName);
                    playedOnce = true;
                    return;
                }
                else if (GetComponent<SpriteRenderer>().sprite.name != s
                    && playedOnce) {
                    playedOnce = false;
                    return;
                }
            }

            if (!_audioFSPlayOnce
                && (GetComponent<SpriteRenderer>().sprite.name == "Neidan_8"
                || GetComponent<SpriteRenderer>().sprite.name == "Neidan_13")){
                    FindObjectOfType<AudioManager>().Play("Clic");
                    _audioFSPlayOnce = true;
                }

            if (!_audioFSPlayOnce
                && (GetComponent<SpriteRenderer>().sprite.name == "Neidan_8"
                || GetComponent<SpriteRenderer>().sprite.name == "Neidan_13")){
                    FindObjectOfType<AudioManager>().Play("Clic");
                    _audioFSPlayOnce = true;
                }
            else if (_audioFSPlayOnce
                && GetComponent<SpriteRenderer>().sprite.name != "Neidan_8"
                && GetComponent<SpriteRenderer>().sprite.name != "Neidan_13") _audioFSPlayOnce = false;
        }

        #endregion
    }
}
