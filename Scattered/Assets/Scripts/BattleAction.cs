using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/New Battle Action", fileName = "New Battle Action.asset")]
public class BattleAction : ScriptableObject {
    #region TARGETING

    public enum TargetType {
        MonoTarget,
        MultiTarget
    }

    [Title("Targeting")] public TargetType targetType;

    [ShowIf("targetType", TargetType.MonoTarget)]
    public enum MonoTargetType {
        Self,
        FirstOpponent,
        LastOpponent,
        RandomAlly,
        RandomAllyWithoutSelf,
        RandomOpponent,
        RandomOpponentWithoutFirstOpponent
    }

    public MonoTargetType monoTargetType;

    [ShowIf("targetType", TargetType.MultiTarget)]
    public enum MultiTargetType {
        AllAllies,
        AllOpponents,
        AllAlliesWithoutSelf,
        AllOpponentsWithoutFirstOpponent,
        AllEntities,
        AllEntitiesWithoutSelf
    }

    public MultiTargetType multiTargetType;

    #endregion


    #region ACTION

    [Title("Action")]
    public enum ActionType {
        Damage,
        Heal,
        Status
    }

    public ActionType actionType;

    #region DAMAGE

    [ShowIf("actionType", ActionType.Damage)]
    [Title("Damage")]
    public enum DamageType {
        Physic,
        Magic
    }

    public DamageType damageType;

    [ShowIf("actionType", ActionType.Damage)]
    public enum DamageResistance {
        None,
        Physic,
        Magic
    }

    public DamageResistance damageResistance;

    [ShowIf("actionType", ActionType.Damage)]
    public bool isRangedDamages;

    [DisableIf("isRangedDamages")] [ShowIf("actionType", ActionType.Damage)] [Range(0, 300)]
    public int flatPercentDamages;

    [EnableIf("isRangedDamages")] [ShowIf("actionType", ActionType.Damage)] [MinMaxSlider(0, 300, ShowFields = true)]
    public Vector2 rangePercentDamages;

    #endregion

    #region HEAL

    [ShowIf("actionType", ActionType.Heal)] [Title("Heal")]
    public HealType healType;

    public enum HealType {
        Physic,
        Magic
    }

    [ShowIf("actionType", ActionType.Heal)]
    public bool isRangedHeal;

    [DisableIf("isRangedHeal")] [ShowIf("actionType", ActionType.Heal)] [Range(0, 300)]
    public int flatPercentHeal;

    [EnableIf("isRangedHeal")] [ShowIf("actionType", ActionType.Heal)] [MinMaxSlider(0, 300, ShowFields = true)]
    public Vector2 rangePercentHeal;

    #endregion

    #region STATUS

    [ShowIf("actionType", ActionType.Status)] [Title("Status")]
    public StatusType statusType;

    public enum StatusType {
        Stun,
        Burn,
        Freeze,
        Poison,
        Bleed
    }

    [ShowIf("actionType", ActionType.Status)] [Range(0, 100)]
    public int accuracyPercentage;

    [ShowIf("actionType", ActionType.Status)] [Range(0, 99)]
    public int turnDuration;

    #endregion

    #endregion
}
