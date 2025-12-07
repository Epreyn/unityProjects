using UnityEngine;
using UnityEngine.UI;

public class RememberToggle : MonoBehaviour {

	private void Awake() {
		GetComponent<Toggle>().isOn = PlayerPrefs.GetInt("Toggle") > 0;
	}

	public void SaveChanges() {
		PlayerPrefs.SetInt("Toggle", GetComponent<Toggle>().isOn ? 1 : 0);
	}
}