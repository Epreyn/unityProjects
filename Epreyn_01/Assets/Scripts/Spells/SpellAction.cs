using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Custom/New Spell Action", fileName = "New Spell Action.asset")]
public class SpellAction : ScriptableObject {
    
    [Title("Target")]
    
    public TargetType targetType;
    public enum TargetType { Self, Opponent, Allies, Enemies}

    [Range(0, 100)] public int accuracyPercent;

    [Title("Action")]

    public ActionType actionType;
    public enum ActionType { Damage, Heal, State, StateOnTime }
    
    [ShowIf("actionType", ActionType.Damage)]
    [Title("Damage")]
    
    public DamageType damageType;
    public enum DamageType { Physic, Magic, CurrentHealth, MaxHealth }

    [ShowIf("actionType", ActionType.Damage)]
    
    public DamageResistance damageResistance;
    public enum DamageResistance { None, Physic, Magic }
    
    [ShowIf("actionType", ActionType.Damage)]
    [MinMaxSlider(0, 300, ShowFields = true)] public Vector2 rangePercentDamage;

    [ShowIf("actionType", ActionType.Heal)]
    [Title("Heal")]
    
    public HealType healType;
    public enum HealType { Physic, Magic, CurrentHP, MaxHP }
    
    [ShowIf("actionType", ActionType.Heal)]
    [MinMaxSlider(0, 300, ShowFields = true)] public Vector2 rangePercentHeal;

    [ShowIf("actionType", ActionType.State)]
    [Title("State (Buff, DeBuff, Stun, etc.)")]
    
    public CharacterState classicState;

    [ShowIf("actionType", ActionType.StateOnTime)]
    [Title("State On Time (DoT, HoT, etc.)")]
    
    public OnTimeStateType onTimeStateType;
    public enum OnTimeStateType { Physic, Magic }

    [ShowIf("actionType", ActionType.StateOnTime)]
    [MinMaxSlider(0, 300, ShowFields = true)] public Vector2 rangePercentOnTimeState;
    
    [ShowIf("actionType", ActionType.StateOnTime)]
    public CharacterState onTimeState;
}