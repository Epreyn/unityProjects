using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtlasManager : MonoBehaviour, iManager {

	public static Sprite[] sprites;
	public ManagerState currentState { get; private set; }

	public void BootSequence(int i) {
		sprites = Resources.LoadAll<Sprite>("EventAtlas");
		currentState = ManagerState.Completed;
	}

	public Sprite loadSprite(string spriteName){
		foreach(Sprite s in sprites){
			if (s.name == spriteName) return s;
		}

		return null;
	}
}
