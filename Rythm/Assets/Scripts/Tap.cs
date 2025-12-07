using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tap : MonoBehaviour {
    private Track _track;
    private float _multiplier = 1;
    
    private Transform _parent;
    private int _xDirection;
    private int _yDirection;

    private RectTransform _rectTransform;
    private Image _image;
    private TextMeshProUGUI _text;
    private bool _isDestroyed = false;
    
    public void SetTrack(Track track) {
        _track = track;
    }
    
    public void SetMultiplier(float multiplier) {
        _multiplier = multiplier;
    }
    
    private void Awake() {
        _parent = transform.parent;
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _rectTransform = GetComponent<RectTransform>(); 
    }

    public void Start() {
        switch (_parent.name) {
            case "Point_A":
                _xDirection = 1;
                _yDirection = -1;
                break;
            case "Point_A_V":
                _xDirection = 0;
                _yDirection = -1;
                break;
            case "Point_A_H":
                _xDirection = 1;
                _yDirection = 0;
                break;
            case "Point_B":
                _xDirection = -1;
                _yDirection = -1;
                break;
            case "Point_B_V":
                _xDirection = 0;
                _yDirection = -1;
                break;
            case "Point_B_H":
                _xDirection = -1;
                _yDirection = 0;
                break;
            case "Point_C":
                _xDirection = -1;
                _yDirection = 1;
                break;
            case "Point_C_V":
                _xDirection = 0;
                _yDirection = 1;
                break;
            case "Point_C_H":
                _xDirection = -1;
                _yDirection = 0;
                break;
            case "Point_D":
                _xDirection = 1;
                _yDirection = 1;
                break;
            case "Point_D_V":
                _xDirection = 0;
                _yDirection = 1;
                break;
            case "Point_D_H":
                _xDirection = 1;
                _yDirection = 0;
                break;
        }
    }

    private void Move() {
        if (_isDestroyed) {
            return;
        }
        transform.position += new Vector3(_xDirection, _yDirection) * Time.deltaTime * 60f / _track.bpm * 4 * _multiplier;
    }
    
    private void CheckDestroy() {
        if (transform.position.x < -10 || transform.position.x > 10 || transform.position.y < -10 || transform.position.y > 10) {
            Destroy(gameObject);
        }
    }
    
    public IEnumerator DestroyTap() {
        _isDestroyed = true;
        var imageColor = _image.color;
        var textColor = _text.color;
        while (imageColor.a > 0 && textColor.a > 0) {
            imageColor.a -= 0.1f;
            textColor.a -= 0.1f;
            
            _image.color = imageColor;
            _text.color = textColor;
            
            _rectTransform.localScale *= 1.1f;
            
            yield return new WaitForSeconds(0.015f);
        }
        Destroy(gameObject);
    }
    
    public void Update() {
        Move();
        CheckDestroy();
    }
}
