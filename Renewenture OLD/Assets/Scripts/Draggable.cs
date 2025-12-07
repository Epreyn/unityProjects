using System.Collections;
using UnityEngine;

public class Draggable : MonoBehaviour {
    private Vector3 _mOffset;
    private float _mZCoord;
    private Vector3 _startPosition;
    private Quaternion _startRotationQuaternion;

    private bool _isMovingBack;
    public float moveBackTime;

    private GameObject _spellA, _spellB;
    private bool _isSpellChosen;
    private bool _isSpellALaunched;


    private void Start() {
        _spellA = transform.Find("Spell A").gameObject;
        _spellB = transform.Find("Spell B").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        switch (collision.gameObject.name) {
            case "Left":
                _spellA.SetActive(true);
                _isSpellChosen = true;
                _isSpellALaunched = true;
                break;
            case "Right":
                _spellB.SetActive(true);
                _isSpellChosen = true;
                _isSpellALaunched = false;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        switch (collision.gameObject.name) {
            case "Left" or "Right":
                _spellA.SetActive(false);
                _spellB.SetActive(false);
                _isSpellChosen = false;
                break;
        }
    }


    private Vector3 GetMouseAsWorldPoint() {
        var mousePoint = Input.mousePosition;
        mousePoint.z = _mZCoord;
        return Camera.main!.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDown() {
        if (_isMovingBack) return;

        var position = gameObject.transform.position;
        _mZCoord = Camera.main!.WorldToScreenPoint(position).z;
        _mOffset = position - GetMouseAsWorldPoint();
        _startPosition = position;
        _startRotationQuaternion = transform.rotation;
    }

    private void OnMouseDrag() {
        if (_isMovingBack) return;

        var newPosition = GetMouseAsWorldPoint() + _mOffset;
        newPosition.y = _startPosition.y;

        transform.position = newPosition;

        var positionDifference = newPosition - _startPosition;
        var rotation = new Vector3(0, 0, -positionDifference.x * 10f);
        transform.eulerAngles = rotation;
    }

    private void OnMouseUp() {
        if (_isMovingBack) return;
        var isSpellChosen = _isSpellChosen;
        StartCoroutine(MoveBackToStartPosition(isSpellChosen));
    }

    private IEnumerator MoveBackToStartPosition(bool isSpellChosen) {
        _isMovingBack = true;
        float elapsedTime = 0;
        var startPosition = transform.position;
        var startRotation = transform.rotation;

        while (elapsedTime < moveBackTime) {
            var t = elapsedTime / moveBackTime;
            transform.position = Vector3.Lerp(startPosition, _startPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, _startRotationQuaternion, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = _startPosition;
        transform.rotation = _startRotationQuaternion;
        _isMovingBack = false;

        if (isSpellChosen) {
            var battleSession = FindObjectsOfType<BattleSession>();
            battleSession[0].StartCoroutine(battleSession[0].ResolveTurn(_isSpellALaunched));
        }
    }
}
