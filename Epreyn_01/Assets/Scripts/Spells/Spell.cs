using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Custom/New Spell", fileName = "New Spell.asset")]
public class Spell : ScriptableObject {

    [Title("Spell Information")]
    public string Name;
    public Sprite Icon;
    [Multiline(10)] public string Description;
    
    // TODO SPELL AUDIO
    // TODO SPELL ANIMATION

    [Title("Spell Actions")] public SpellAction[] SpellActions;
}