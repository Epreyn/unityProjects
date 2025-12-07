using UnityEngine;

public class AdaptWidthToScreen : MonoBehaviour {

	[SerializeField]
	[Range(0,100)]
	private float percent;

	private GameObject canvas;

	private void Start() {
		canvas = GameObject.Find("Canvas");
	}

	private void Update () {
		GetComponent<RectTransform>().sizeDelta =
			new Vector2(canvas.GetComponent<RectTransform>().sizeDelta.x * percent / 100, 
						GetComponent<RectTransform>().sizeDelta.y);
	}
}
