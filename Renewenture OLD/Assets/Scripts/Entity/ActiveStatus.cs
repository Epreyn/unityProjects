using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStatus : ScriptableObject {
    public BattleAction.StatusType Type;
    public int RemainingTurns;
}
