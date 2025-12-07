using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ManagerState{
	Offline, Initializing, Completed
}

public interface iManager {
	ManagerState currentState { get; }

	void BootSequence(int i); 
}
