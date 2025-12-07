using UnityEngine;

public class Loader : MonoBehaviour {

	private void Update () {
		foreach (Transform t in transform) {
			t.gameObject.SetActive(Data.SendingFiles);
		}
	}
}
