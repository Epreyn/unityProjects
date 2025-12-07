using UnityEngine;
using UnityEngine.UI;

public class UnactiveOutBound : MonoBehaviour {

	private int threshold = 50;
	private float distanceTop, distanceBottom, anchTop, anchBottom;
	
	private void Update () {
		distanceTop = transform.parent.GetComponent<RectTransform>().anchoredPosition.y
			+ GetComponent<RectTransform>().anchoredPosition.y;
		
		distanceBottom = transform.parent.GetComponent<RectTransform>().anchoredPosition.y + 650
			+ GetComponent<RectTransform>().anchoredPosition.y;

		anchTop = distanceBottom;
		anchBottom = distanceTop - GetComponent<RectTransform>().sizeDelta.y;

		if (anchBottom > threshold || anchTop < -threshold) {
			GetComponent<Image>().enabled = false;
			foreach (Transform child in transform) {
				child.gameObject.SetActive(false);
			}
		}
		else {
			GetComponent<Image>().enabled = true;
			foreach (Transform child in transform) {
				child.gameObject.SetActive(true);
			}
		}
	}
}
