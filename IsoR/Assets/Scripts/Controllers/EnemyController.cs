using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _moveRange;
    [SerializeField] private Vector2 _pauseRange;

    private CharacterController _characterController;

    private bool _isPaused = true;
    private float _moveTimer;
    private float _pauseTimer;
    private Vector3 _randomDirection;

    private void Start() {
        _characterController = GetComponent<CharacterController>();
        SetRandomValues();
    }

    private void Update() {
        TimerCheck();
    }

    private void FixedUpdate() {
        Move();
    }

    private void TimerCheck() {
        if (_isPaused) {
            _pauseTimer -= Time.deltaTime;

            if (!(_pauseTimer <= 0f)) return;
            _isPaused = false;
            SetRandomValues();
        }
        else {
            _moveTimer -= Time.deltaTime;

            if (!(_moveTimer <= 0f)) return;
            _isPaused = true;
            SetRandomValues();
        }
    }

    private void Move() {
        if (_isPaused) return;
        var magnitude = Mathf.Clamp01(_randomDirection.magnitude) * _speed;
        _randomDirection.Normalize();
        var velocity = _randomDirection * magnitude;
        _characterController.SimpleMove(velocity);
    }

    private void SetRandomValues() {
        _moveTimer = Random.Range(_moveRange.x, _moveRange.y);
        _pauseTimer = Random.Range(_pauseRange.x, _pauseRange.y);
        _randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }
}
