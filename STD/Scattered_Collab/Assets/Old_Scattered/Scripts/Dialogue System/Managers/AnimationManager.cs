using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class AnimationManager : MonoBehaviour, iManager {

	public bool _dialogueEnded;

	Animator panelAnimator;
	public ManagerState currentState { get; private set; }

	public void BootSequence(int i){
		panelAnimator = GameObject.Find("Dialogues").GetComponent<Animator>();
		currentState = ManagerState.Completed;
	}

	public IEnumerator IntroAnimation(){

		_dialogueEnded = false;

		AnimationTuple introAnim = Constants.AnimationTuples.introAnimation;
		panelAnimator.SetBool(introAnim.parameter, introAnim.value);

		yield return new WaitForSeconds(1);
	}

	public IEnumerator ExitAnimation(){
		AnimationTuple exitAnim = Constants.AnimationTuples.exitAnimation;
		panelAnimator.SetBool(exitAnim.parameter, exitAnim.value);

		yield return new WaitForSeconds(1);

		_dialogueEnded = true;
	}
}
