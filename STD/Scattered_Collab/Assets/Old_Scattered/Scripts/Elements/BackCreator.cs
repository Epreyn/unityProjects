using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCreator : MonoBehaviour {

	public GameObject BackObject;
	private Vector3 _position;

	void Start () {

		_position = new Vector3(0, 30, 10);
		BackObject.transform.localScale = new Vector3(5,5,1);
		BackObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
		BackObject.GetComponent<SpriteRenderer>().flipY = true;

		for(int i = 0; i < 20; i++){
			Instantiate(BackObject, _position, Quaternion.identity);
			_position += new Vector3(0, -1.5f, 3);
			BackObject.GetComponent<SpriteRenderer>().color -= new Color32(20, 20, 20, 0);
		}
		
		BackObject.GetComponent<SpriteRenderer>().flipY = false;

		for(int i = 0; i < 20; i++){
			Instantiate(BackObject, _position, Quaternion.identity);
			_position += new Vector3(0, -1.5f, -3);
			BackObject.GetComponent<SpriteRenderer>().color += new Color32(20, 20, 20, 0);
		}
	}
	
	void FixedUpdate () {
		
	}
}
