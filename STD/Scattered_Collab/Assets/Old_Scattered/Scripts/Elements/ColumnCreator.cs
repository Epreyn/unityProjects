using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scattered;
using System;

public class ColumnCreator : MonoBehaviour {

	public GameObject[] Columns;
	public Appear[] Appears;
	private GameObject _neidan;
	public int SpaceBetweenColumns, DisplayDistance, AppearRatio;
	private int Proc;
	public Transform Parent;
	public float ScaleX, ScaleY;

	void Start () {

		_neidan = GameObject.FindGameObjectWithTag("Neidan");

		foreach(Appear a in Appears){
			a._neidan = _neidan;
			a.DisplayDistance = DisplayDistance;
			a.AppearRatio = AppearRatio;
		}
	}
	
	void FixedUpdate () {

		// Si le Héros dépasse une certaine distance qui lance la conception
		if (_neidan.transform.position.x > Proc){

			// Lance la création d'une colonne en fond
			GameObject c = Instantiate(Columns[UnityEngine.Random.Range(0,5)],
						new Vector3(
							
							// Sa position en X et en Y dépend de la position du héros au moment de la création
							Proc + UnityEngine.Random.Range(DisplayDistance - 10, DisplayDistance + 10),
							UnityEngine.Random.Range(_neidan.transform.position.y - 5, _neidan.transform.position.y + 5),

							// Sa position en Z est aléatoire entre certaines limites et paraitront plus ou moins loin selon cette valeur
							RandomValueFromRanges(new Range(11,13), new Range(17,25))),
						Quaternion.identity) as GameObject;
			
			c.transform.parent = Parent;
			c.transform.localScale = new Vector3(ScaleX, ScaleY, -1);

			// On incrémente la prochaine distance où on lancera la création d'une autre colonne
			Proc = Proc + SpaceBetweenColumns;
		}

	}

	public struct Range {
     	public int min;
     	public int max;
    	public int range {get {return max-min + 1;}}
     	public Range(int aMin, int aMax) {
        	min = aMin; max = aMax;
     	}
 	}

	public static int RandomValueFromRanges(params Range[] ranges) {
    	if (ranges.Length == 0) return 0;
    	int count = 0;
    	
		foreach(Range r in ranges) count += r.range;
    
		int sel = UnityEngine.Random.Range(0,count);
     	foreach(Range r in ranges) {
        	if (sel < r.range) {
            	return r.min + sel;
        	}
        sel -= r.range;
    	}
     throw new Exception("This should never happen");
 	}
}
