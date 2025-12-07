using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Custom/New Character Class", fileName = "New Character Class.asset")] 
public class CharacterClass : ScriptableObject {

    [Title("Informations")]
    
    public string _className;
    
    public enum CardType { None, Hero, Goblin, Forest, Swamp }
    public CardType _cardType;

    [Title("Sprites")] 
    
    public Sprite _idleSprite;
    public Sprite _attackSprite;
    public Sprite _hitSprite;

    [Title("Base Statistics")]
    
    [ProgressBar(0,10,0.373f,0.733f,0.498f, Segmented = true)] public int HP;
    [ProgressBar(0,10, 0.733f, 0.373f, 0.373f, Segmented = true)] public int PhysicalAttack;
    [ProgressBar(0,10,0.373f,  0.467f, 0.733f, Segmented = true)] public int MagicalAttack;
    [ProgressBar(0,10,0.733f, 0.518f, 0.373f, Segmented = true)] public int PhysicalDefense;
    [ProgressBar(0,10,0.651f, 0.373f, 0.733f, Segmented = true)] public int MagicalDefense;
    
    [Title("Abilities List")]
    
    public Ability[] _abilities;
}
