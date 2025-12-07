using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _lerpSpeed;

    private Transform _cameraPivot;

    private CombatTrigger _combatTrigger;
    private Transform _orientation;

    private Vector3 _targetCameraPosition;
    private float _targetCameraSize;

    private void Start() {
        _cameraPivot = Camera.main.transform.parent;
        _orientation = transform.Find("Orientation");
        _combatTrigger = GetComponent<CombatTrigger>();
        DefaultCamera();
    }

    private void Update() {
        Look();
        RotateCamera();
    }

    private void FixedUpdate() {
        if (!_combatTrigger.InCombat) DefaultCamera();
        UpdatePosition();
        UpdateCameraSize();
    }

    private void UpdateCameraSize() {
        Camera.main.orthographicSize =
            Mathf.Lerp(Camera.main.orthographicSize, _targetCameraSize, Time.deltaTime * _lerpSpeed);
    }

    private void UpdatePosition() {
        _cameraPivot.position = Vector3.Lerp(_cameraPivot.position, _targetCameraPosition, Time.deltaTime * _lerpSpeed);
    }

    public void DefaultCamera() {
        _targetCameraPosition = transform.position;
        _targetCameraSize = 5;
    }

    public void CombatCamera() {
        _targetCameraPosition = _combatTrigger.CombatObject.transform.position;
        _targetCameraSize = 2;
    }

    private void RotateCamera() {
        _cameraPivot.rotation = Quaternion.Euler(30, _orientation.rotation.eulerAngles.y + 45, 0);
    }

    private void Look() {
        if (Input.GetKey(KeyCode.E))
            _orientation.transform.RotateAround(
                transform.position,
                Vector3.up,
                _rotationSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q))
            _orientation.transform.RotateAround(
                transform.position,
                Vector3.up,
                -_rotationSpeed * Time.deltaTime);
    }
}
