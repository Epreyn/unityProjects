using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Custom/New Character State", fileName = "New Character State.asset")]
public class CharacterState : ScriptableObject {
    [Title("State Information")]
    public string Name;
    public Sprite Icon;
    public int TurnNumber;
    
    public StateType stateType;
    public enum StateType { Buff, DeBuff, Stun, Evade, DamageOnTime, HealOnTime }
    
    [Title("Statistics For Buff and DeBuff")]
    [Range(0, 100)] public int PhysicPercent;
    [Range(0, 100)] public int MagicPercent;
    [Range(0, 100)] public int DefensePercent;
    [Range(0, 100)] public int ResistancePercent;

    [ShowIf("stateType", StateType.DamageOnTime)]
    [Title("Damage On Time")]
    [ReadOnly] public int DamageOnTimeValue;
    
    [ShowIf("stateType", StateType.HealOnTime)]
    [Title("Heal On Time")]
    [ReadOnly] public int HealOnTimeValue;
}
