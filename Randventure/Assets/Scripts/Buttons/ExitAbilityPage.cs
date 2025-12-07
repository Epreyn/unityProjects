using HutongGames.PlayMaker;
using UnityEngine;

public class ExitAbilityPage : MonoBehaviour {
    public void ExitAP() {
        var fsmBool = FsmVariables.GlobalVariables.FindFsmBool("Show_Ability_Page");
        fsmBool.Value = !fsmBool.Value;
    }
}
