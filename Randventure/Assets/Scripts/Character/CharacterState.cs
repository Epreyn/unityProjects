using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class CharacterState {

    public Sprite _stateIcon;
    public string _stateName;
    public int _stateTurnNumber;

    public StateType _stateType;
    public enum StateType { Buff, Debuff, DOT, HOT, Stun, Evasion, Intervention }

    #region DOT
    
    [ShowIf("_stateType", StateType.DOT)] [Title("DOT")]

    [ShowIf("_stateType", StateType.DOT)]
    public int _dotValue;
    
    #endregion
    
    #region HOT
    
    [ShowIf("_stateType", StateType.HOT)] [Title("HOT")]
    
    [ShowIf("_stateType", StateType.HOT)]
    public int _hotValue;
    
    #endregion
    
    #region INTERVENTION
    
    [ShowIf("_stateType", StateType.Intervention)] [Title("Intervention")]
    
    [ShowIf("_stateType", StateType.Intervention)]
    public Character _interventionTarget;
    
    #endregion
    
}
