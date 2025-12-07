using UnityEngine;

public class NextActionnerInstance : MonoBehaviour {
    public NextActionner _naInstance;

    public BattleAction GetCurrentBAofNA(int index) {
        return _naInstance._battleActions[index];
    }
}
