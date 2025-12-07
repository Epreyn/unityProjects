using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AtlasManager))]
[RequireComponent(typeof(AnimationManager))]
[RequireComponent(typeof(PanelManager))]

public class MasterManager : MonoBehaviour {

	public bool _dialogueEnded;
	public bool _reload;
	public int _index = 1;

	private List<iManager> _managerList = new List<iManager>();

	public static AtlasManager atlasManager { get; private set; }
	public static AnimationManager animationManager { get; private set; }
	public static PanelManager panelManager { get; private set; }

	void Awake(){
		atlasManager = GetComponent<AtlasManager>();
		animationManager = GetComponent<AnimationManager>();
		panelManager = GetComponent<PanelManager>();
		
		_managerList.Add(atlasManager);
		_managerList.Add(animationManager);
		_managerList.Add(panelManager);

		StartCoroutine(BootAllManagers(_index));
	}

	private IEnumerator BootAllManagers(int i) {
		foreach(iManager manager in _managerList){
			manager.BootSequence(i);
		}

		yield return null;
	}

	void Update(){

		_dialogueEnded = animationManager._dialogueEnded;

		if (_reload){
			StartCoroutine(BootAllManagers(++_index));
			_reload = false;
		}
	}
}
