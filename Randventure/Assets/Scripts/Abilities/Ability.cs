using System;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Custom/New Ability", fileName = "New Ability.asset")]
public class Ability : ScriptableObject {
    
    [Title("Ability Informations")]
    
    public Sprite _abilityIcon;
    public string _abilityName;
    [Multiline(10)]
    public string _abilityDescription;
    
    // ABILITY ANIMATION
    // ABILITY AUDIO

    [Title("Placement")] 
    
    public bool HerosOnMultitarget;
    public bool MonstersOnMultitarget;
    
    [Title("Priority Properties")]
    
    public bool _haveCritic; 
    public Priority abilityPriority;
    [ShowIf("_haveCritic")]
    public Priority abilityCriticPriority;
    public enum Priority { Low, Normal, High }
    
    [Title("Battle Actions List")]
    
    public BattleAction[] _battleActions;
    [ShowIf("_haveCritic")]
    [Space] public BattleAction[] _criticBattleActions;
}
