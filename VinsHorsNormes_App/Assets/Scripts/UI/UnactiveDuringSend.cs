using UnityEngine;
using UnityEngine.UI;

public class UnactiveDuringSend : MonoBehaviour {

	private void Update () {
		GetComponent<Button>().interactable = !Data.SendingFiles;
	}
}
