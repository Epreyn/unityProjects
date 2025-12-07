using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;	

public class PanelConfig : MonoBehaviour {

	public bool characterIsTalking;

	public Image avatarImage;
	public Image textImage;
	public Text characterName;
	public Text dialogue;

	private Color maskActivateColor = new Color(103f/255f, 101f/255f, 101f/255f);

	public void ToggleCharacterMask(){
		if (characterIsTalking) {
			avatarImage.color = Color.white;
			textImage.color = Color.white;
		}
		else {
			avatarImage.color = maskActivateColor;
			textImage.color = maskActivateColor;
		}
	}

	public void Configure(Dialogue currentDialogue){
		ToggleCharacterMask();

		avatarImage.sprite = MasterManager.atlasManager.loadSprite(currentDialogue.atlasImageName);
		characterName.text = currentDialogue.name;

		if (characterIsTalking) {
			StartCoroutine(AnimateText(currentDialogue.dialogueText));
		}
		else {
			dialogue.text = "";
		}
	}

	IEnumerator AnimateText(string dialogueText){
		dialogue.text = "";

		foreach(char letter in dialogueText){
			dialogue.text += letter;
			//FindObjectOfType<AudioManager>().Play("Clic");
			yield return new WaitForSeconds(0.05f);
		}
	}
}
