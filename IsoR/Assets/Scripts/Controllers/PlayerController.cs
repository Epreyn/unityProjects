using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float _speed;

    private Transform _cameraPivot;
    private CharacterController _characterController;

    private Vector3 _input;

    private void Start() {
        _cameraPivot = Camera.main.transform.parent;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        GatherInput();
    }

    private void FixedUpdate() {
        Move();
    }

    private void GatherInput() {
        _input.x = Input.GetAxis("Horizontal");
        _input.z = Input.GetAxis("Vertical");
    }

    private void Move() {
        var movementDirection = new Vector3(_input.x, 0, _input.z);
        var magnitude = Mathf.Clamp01(movementDirection.magnitude) * _speed;
        movementDirection.Normalize();

        movementDirection = Quaternion.Euler(0, _cameraPivot.rotation.eulerAngles.y, 0) * movementDirection;

        var velocity = movementDirection * magnitude;

        _characterController.SimpleMove(velocity);
    }
}
