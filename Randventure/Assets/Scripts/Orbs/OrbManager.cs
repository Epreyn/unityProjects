using UnityEngine;

public class OrbManager : MonoBehaviour {
    
    [SerializeField] private Material OrbMaterial;

    public void InitializeOrbMaterial(Transform t) {
        t.GetComponent<SpriteRenderer>().material = new Material(OrbMaterial);
    }
    
    public void DisplayXP(Transform t, Character character) {
        var cXPf = (float)character._currentXP;
        var tXPf = (float)character._targetXP;
        var percent = cXPf / tXPf;

        var targetPercent = 1 - percent * 2;
        targetPercent *= -1;
        t.GetComponent<SpriteRenderer>().material.SetFloat("_Fade", targetPercent);
    }
    
    public void DisplayHP(Transform t, Character character) {
        var cHPf = (float)character._currentHP;
        var tHPf = (float)character._HP.Value;

        var percent = cHPf / tHPf;

        var targetPercent = 1 - percent * 2;
        targetPercent *= -1;
        t.GetComponent<SpriteRenderer>().material.SetFloat("_Fade", targetPercent);
    }
}
