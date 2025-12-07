using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/New BA", fileName = "New BA.asset")]
public class BattleAction : ScriptableObject {

    #region ORDERING

    [HideIf("_targetType", TargetType.NextActionner)] 
    [Title("Ordering")]
    
    [HideIf("_targetType", TargetType.NextActionner)] 
    public bool _checkOrder;
    public enum AttackOrder { AttackAtFirst, AttackAtLast }
    [ShowIf("_checkOrder")]
    public AttackOrder _attackOrder;

    #endregion

    #region TARGETING

    [Title("Targeting")]
    
    public TargetType _targetType;
    public enum TargetType { Monotarget, Multitarget, NextActionner, Invoke} 
    
    [ShowIf("_targetType", TargetType.Monotarget)] 
    public MonotargetType _monotargetType;
    public enum MonotargetType 
    { Self, OpponentFocus, RandomAlly, RandomOpponent, RandomAllyWithoutFocus, RandomOpponentWithoutFocus}
    
    [ShowIf("_targetType", TargetType.Multitarget)] 
    public MultitargetType _multitargetType;
    public enum MultitargetType 
    { AllAllies, AllOpponents, AlliesWithoutFocus, OpponentsWithoutFocus, FocusAllyAndRandomAlly, FocusOpponentAndRandomOpponent}

    #endregion

    #region INVOKE

    [ShowIf("_targetType", TargetType.Invoke)] 
    [Title("Invoke")]
    
    [ShowIf("_targetType", TargetType.Invoke)] 
    public CharacterClass _invokeClass;
    
    public enum HPBase { CurrentHP, MaxHP }
    [ShowIf("_targetType", TargetType.Invoke)] 
    public HPBase _hpBase;
    
    [ShowIf("_targetType", TargetType.Invoke)] 
    [Range(0, 100)]
    public int _hpPercent;

    #endregion

    #region CARD TYPING
    
    [HideIf("_targetType", TargetType.Invoke)]
    [Title("Card Typing")]
    
    [HideIf("_targetType", TargetType.Invoke)]
    public bool _checkCardType;

    public enum CardType { Hero, Goblin, Forest, Swamp }
    [HideIf("_targetType", TargetType.Invoke)]
    [ShowIf("_checkCardType")]
    public CardType _cardType;
    
    [HideIf("_targetType", TargetType.Invoke)]
    [ShowIf("_checkCardType")]
    public enum CardSide { Allies, Opponents, All }
    public CardSide _side;
 
    [HideIf("_targetType", TargetType.Invoke)]
    [ShowIf("_checkCardType")]
    public enum State { Alive, Defeated }
    public State _state;
    
    [HideIf("_targetType", TargetType.Invoke)]
    [ShowIf("_checkCardType")]
    [Range(-1000, 1000)]
    public int _multPercent;

    #endregion

    #region ACTION

    [HideIfGroup("_targetType", TargetType.Invoke)]
    [Title("Action")]

    public ActionType _actionType;
    public enum ActionType { Damage, Heal, Shield, Buff, Debuff, DOT, HOT, 
        Stun, Execute, GoldTheft, Evasion, Intervention }

    #region DAMAGE

    [HideIf("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.Damage)] [Title("Damage")]
    public DamageType _damageType;
    public enum DamageType { Physic, Magic, CurrentHP, MaxHP }

    [HideIf("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.Damage)]
    public DamageResistance _damageRes;
    public enum DamageResistance { None, Physic, Magic }
    
    [HideIf("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.Damage)]  public bool _isRangedDMG;

    [HideIf("_targetType", TargetType.Invoke)]
    [HideIf("_isRangedDMG")] [ShowIf("_actionType", ActionType.Damage)] 
    [Range(0, 1000)] public int _flatPercentDMG;

    [HideIf("_targetType", TargetType.Invoke)]
    [ShowIf("_isRangedDMG")] [ShowIf("_actionType", ActionType.Damage)]
    [MinMaxSlider(0, 1000, ShowFields = true)] public Vector2 _rangePercentDMG;

    #endregion

    #region HEAL

    [HideIfGroup("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.Heal)] [Title("Heal")]
    public HealType _healType;
    public enum HealType { Physic, Magic, CurrentHP, MaxHP }
    
    [ShowIf("_actionType", ActionType.Heal)] public bool _isRangedHEAL;

    [HideIf("_isRangedHEAL")] [ShowIf("_actionType", ActionType.Heal)] 
    [Range(0, 1000)] public int _flatPercentHEAL;

    [ShowIf("_isRangedHEAL")] [ShowIf("_actionType", ActionType.Heal)]
    [MinMaxSlider(0, 1000, ShowFields = true)] public Vector2 _rangePercentHEAL;

    #endregion
    
    #region SHIELD

    [HideIfGroup("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.Shield)] [Title("Shield")]
    public ShieldType _shieldType;
    public enum ShieldType { Physic, Magic }

    [ShowIf("_actionType", ActionType.Shield)] public bool _isRangedSHIELD;

    [HideIf("_isRangedSHIELD")] [ShowIf("_actionType", ActionType.Shield)] 
    [Range(0, 1000)] public int _flatPercentSHIELD;

    [ShowIf("_isRangedSHIELD")] [ShowIf("_actionType", ActionType.Shield)]
    [MinMaxSlider(0, 1000, ShowFields = true)] public Vector2 _rangePercentSHIELD;

    #endregion
    
    #region BUFF
    
    [HideIfGroup("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.Buff)] [Title("Buff")]

    [ShowIf("_actionType", ActionType.Buff)]
    public string _buffName;
    
    [ShowIf("_actionType", ActionType.Buff)]
    [ProgressBar(1, 99)] public int _turnNumberBUFF;

    [ShowIf("_actionType", ActionType.Buff)]
    [ProgressBar(0, 100)] public int _physicalAttPercentBUFF;

    [ShowIf("_actionType", ActionType.Buff)]
    [ProgressBar(0, 100)] public int _physicalResPercentBUFF;

    [ShowIf("_actionType", ActionType.Buff)]
    [ProgressBar(0, 100)] public int _magicalAttPercentBUFF;

    [ShowIf("_actionType", ActionType.Buff)]
    [ProgressBar(0, 100)] public int _magicalResPercentBUFF;

    #endregion
    
    #region DEBUFF
    
    [HideIfGroup("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.Debuff)] [Title("Debuff")]
    
    [ShowIf("_actionType", ActionType.Debuff)]
    public string _debuffName;

    [ShowIf("_actionType", ActionType.Debuff)]
    [ProgressBar(1, 99)] public int _turnNumberDEBUFF;

    [ShowIf("_actionType", ActionType.Debuff)]
    [ProgressBar(0, 100)] public int _physicalAttPercentDEBUFF;

    [ShowIf("_actionType", ActionType.Debuff)]
    [ProgressBar(0, 100)] public int _physicalResPercentDEBUFF;

    [ShowIf("_actionType", ActionType.Debuff)]
    [ProgressBar(0, 100)] public int _magicalAttPercentDEBUFF;

    [ShowIf("_actionType", ActionType.Debuff)]
    [ProgressBar(0, 100)] public int _magicalResPercentDEBUFF;

    #endregion

    #region DOT

    [HideIfGroup("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.DOT)] [Title("DOT")]
    
    [ShowIf("_actionType", ActionType.DOT)]
    public string _dotName;
    
    [ShowIf("_actionType", ActionType.DOT)]
    public DOTType _dotType;
    public enum DOTType { Physic, Magic }
    
    [ShowIf("_actionType", ActionType.DOT)]
    [ProgressBar(1, 99)] public int _turnNumberDOT;
    
    [ShowIf("_actionType", ActionType.DOT)] public bool _isRangedDOT;

    [HideIf("_isRangedDOT")] [ShowIf("_actionType", ActionType.DOT)] 
    [Range(0, 1000)] public int _flatPercentDOT;

    [ShowIf("_isRangedDOT")] [ShowIf("_actionType", ActionType.DOT)]
    [MinMaxSlider(0, 1000, ShowFields = true)] public Vector2 _rangePercentDOT;

    #endregion
    
    #region HOT

    [HideIfGroup("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.HOT)] [Title("HOT")]
    
    [ShowIf("_actionType", ActionType.HOT)]
    public string _hotName;
    
    [ShowIf("_actionType", ActionType.HOT)]
    public HOTType _hotType;
    public enum HOTType { Physic, Magic }
    
    [ShowIf("_actionType", ActionType.HOT)]
    [ProgressBar(1, 99)] public int _turnNumberHOT;

    [ShowIf("_actionType", ActionType.HOT)] public bool _isRangedHOT;

    [HideIf("_isRangedHOT")] [ShowIf("_actionType", ActionType.HOT)] 
    [Range(0, 1000)] public int _flatPercentHOT;

    [ShowIf("_isRangedHOT")] [ShowIf("_actionType", ActionType.HOT)] 
    [MinMaxSlider(0, 1000, ShowFields = true)] public Vector2 _rangePercentHOT;

    #endregion

    #region EXECUTE

    [HideIfGroup("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.Execute)] [Title("Execute")]
    [Range(0, 100)] public int _executePercent;

    #endregion

    #region GOLD THEFT

    [HideIfGroup("_targetType", TargetType.Invoke)]
    [ShowIf("_actionType", ActionType.GoldTheft)] [Title("Gold Theft")]
    public int _goldValue;

    #endregion

    #endregion

}