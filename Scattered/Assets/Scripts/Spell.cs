using System;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Custom/New Spell", fileName = "New Spell.asset")]
public class Spell : ScriptableObject {
    [Title("Spell Information")] public string spellName;
    [Multiline(10)] public string spellDescription;

    [Title("Actions List")] public BattleAction[] battleActions;
}
