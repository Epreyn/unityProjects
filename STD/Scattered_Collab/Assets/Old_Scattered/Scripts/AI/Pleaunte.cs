using Scattered;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pleaunte : MonoBehaviour {

    #region FIELDS
    
    public bool ActivePattern;

    public int Hp;
    
    public float ProjectileDelay, TimeElapsed;
    public List<GameObject> Projectiles;

    private Animator _animator;
    private Transform _projectilePoint;
    public GameObject _doppel, _neidan;

    private float _direction;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _projectilePoint = transform.Find("ProjectileEmitter");

        _doppel = GameObject.FindGameObjectWithTag("Doppel");
        _neidan = GameObject.FindGameObjectWithTag("Neidan");
    }

    private void FixedUpdate()
    {        
        // WATER
        GetComponentInChildren<ParticleGenerator>().particleForce.x = Random.Range(-1, 1);
        GetComponentInChildren<ParticleGenerator>().particleForce.y = Random.Range(-1, 1);

        // LIFE
        if (_doppel.GetComponentInChildren<FightSystem>()._fight
            && GetComponentInChildren<FightTriggerFields>()._fight
            && Hp > 0) {
                Hp--;
                if(Time.timeScale >= 1) Hp--;

                _animator.SetBool("Fight", true);
        }
        else _animator.SetBool("Fight", false);

        if (_animator.GetBool("Fight")) MoveTo(_doppel.GetComponent<Doppel>().gameObject, true);

        // PROJECTILE
        if (!_animator.GetBool("Fight")
            && ActivePattern){
            TimeElapsed += Time.deltaTime;
            if (TimeElapsed + 0.4f > ProjectileDelay) _animator.SetBool("Projectile", true);
            if (TimeElapsed > ProjectileDelay){
                foreach (GameObject p in Projectiles){
                    CreateProjectile(p, _projectilePoint.position);
                }
                TimeElapsed = 0;
            }
        }

        // DEATH
        if (Hp == 0) {
            _animator.SetBool("Alive", false);
            GetComponentInChildren<ParticleGenerator>().particlesState = DynamicParticle.STATES.NONE;
        }
        
        UpdateAnimation();
    }

    #endregion

    #region OTHERS METHODS

    public void UpdateAnimation()
    {
        // PROJECTILE
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Projectile") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) _animator.SetBool("Projectile", false);

        // DEATH
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation_Death") &&
            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {
                Destroy(this.gameObject);
            }
    }

    public void CreateProjectile(GameObject projectile, Vector2 position){
        var clone = Instantiate(projectile, position, Quaternion.identity) as GameObject;
        clone.transform.localScale = transform.localScale;
    }

    public void MoveTo (GameObject target, bool condition){
            if (target.transform.position.x - this.transform.position.x < 0) _direction = 1;
            else _direction = -1;

            if (!condition) return;
            GetComponent<Transform>().localScale = new Vector3(_direction > 0 ? 1 : -1, 1, 1);
    }

    #endregion
}
