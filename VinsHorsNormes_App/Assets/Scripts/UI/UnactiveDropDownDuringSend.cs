using TMPro;
using UnityEngine;

public class UnactiveDropDownDuringSend : MonoBehaviour {

	private void Update () {
		GetComponent<TMP_Dropdown>().interactable = !Data.SendingFiles;
	}
}
