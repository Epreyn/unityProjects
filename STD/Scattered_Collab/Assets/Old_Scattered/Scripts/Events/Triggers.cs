using System.Collections;
using System.Collections.Generic;
using Scattered;
using UnityEngine;

public class Triggers : MonoBehaviour {

	public GameObject[] EventTriggers;
	public int _lastLengthT, _indexT;

	private bool[] _triggerOnce;

	// PRINCIPALS GAMEOBJECTS
	private GameObject Doppel, Menu, Camera, SceneController;
	
	// PLATFORMS
	private GameObject P2;

	// WALLS
	private GameObject W1, W2;

	void Start () {

		// DIALOGUE SYSTEM
		SceneController = GameObject.Find("SceneController");

		// TRIGGER SYSTEM
		EventTriggers = GameObject.FindGameObjectsWithTag("T");
		_lastLengthT = EventTriggers.Length;
		_indexT = 0;
		_triggerOnce = new bool[EventTriggers.Length];

		// DOPPEL
		Doppel = GameObject.FindGameObjectWithTag("Doppel");
		Doppel.SetActive(false);

		// MENU
		Menu = GameObject.FindGameObjectWithTag("Menu");
		Menu.SetActive(false);

		// CAMERA
		Camera = GameObject.FindGameObjectWithTag("MainCamera");

		// W1
		W1 = GameObject.Find("W1");
		W2 = GameObject.Find("W2");

		// P2
		P2 = GameObject.Find("P2");
		P2.SetActive(false);
	}
	
	void Update () {

		switch(_indexT){

			case 0:
			Event0();
			break;

			case 1:
			Event1();
			break;

			case 2:
			Event2();
			break;

			default:
			break;
		}

		// CHANGE EVENT
		EventTriggers = GameObject.FindGameObjectsWithTag("T");
		if (_lastLengthT != EventTriggers.Length) _indexT++;
		_lastLengthT = EventTriggers.Length;
	}

	private void Event0(){

		if (SceneController.GetComponent<MasterManager>()._dialogueEnded){
			if(Camera.GetComponent<Camera2DFollow>().Zoom < 27.5f) Camera.GetComponent<Camera2DFollow>().Zoom += Time.deltaTime * 15;
		} else Camera.GetComponent<Camera2DFollow>().Zoom = 6;
	}

	private void Event1(){
		P2.SetActive(true);
		Doppel.SetActive(true);
			
		if (W1 != null){
			W1.GetComponent<DestroyTimer>()._destroy = true;
			Camera.GetComponent<Camera2DFollow>().target = W1.transform;
			Camera.GetComponent<Camera2DFollow>().damping = .2f;
		}

		if (!_triggerOnce[_indexT]
			&& Camera.GetComponent<Camera2DFollow>().target.name == "Neidan") {
			_triggerOnce[_indexT] = true;
			SceneController.GetComponent<MasterManager>()._reload = true;
		}
	}

	private void Event2(){

		if (W2 != null){
			W2.GetComponent<DestroyTimer>()._destroy = true;
			Camera.GetComponent<Camera2DFollow>().target = W2.transform;
			Camera.GetComponent<Camera2DFollow>().damping = .2f;
		}
	}
}
