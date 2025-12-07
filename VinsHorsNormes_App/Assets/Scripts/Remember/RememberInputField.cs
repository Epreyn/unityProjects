using UnityEngine;
using UnityEngine.UI;

public class RememberInputField : MonoBehaviour {

	private Toggle rememberToggle;

	private void Awake() {
		rememberToggle = GameObject.Find("RememberToggle").GetComponent<Toggle>();

		if (!rememberToggle.isOn) return;
		gameObject.GetComponent<InputField>().text = PlayerPrefs.GetString(gameObject.name);
	}

	private void LateUpdate() {
		if (!rememberToggle.isOn) return;
		PlayerPrefs.SetString(gameObject.name, gameObject.GetComponent<InputField>().text);
	}
}
