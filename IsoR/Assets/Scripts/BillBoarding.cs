using UnityEngine;

public class BillBoarding : MonoBehaviour {
    private Vector3 _cameraDirection;

    private void Update() {
        _cameraDirection = Camera.main.transform.forward;
        _cameraDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(_cameraDirection);
    }
}
