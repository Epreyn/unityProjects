using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Custom/New Char Class", fileName = "New Char Class.asset")]
public class CharacterClass : ScriptableObject {

    [Title("Name")] public string Name;
    
    // [Title("Sprites")]
    // public Sprite Sprite_Idle;
    // public Sprite Sprite_Attack;
    // public Sprite Sprite_Hit;
    
    [Title("Base Statistics")]
    [ProgressBar(0,10, Segmented = true)] public int Health;
    [ProgressBar(0,10,  Segmented = true)] public int Physic;
    [ProgressBar(0,10, Segmented = true)] public int Magic;
    [ProgressBar(0,10, Segmented = true)] public int Defense;
    [ProgressBar(0, 10, Segmented = true)] public int Resistance;
    [ProgressBar(0,10, Segmented = true)] public int Speed;

    [Title("Spells")] public Spell[] Spells;
}