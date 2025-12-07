using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Custom/New Entity Class", fileName = "New Entity Class.asset")] 
public class EntityClass : ScriptableObject {

    [Title("Information")]
    
    public string className;

    [Title("Base Statistics")]
    
    [ProgressBar(0,10,0.373f,0.733f,0.498f, Segmented = true)] public int hp;
    [ProgressBar(0,10, 0.733f, 0.373f, 0.373f, Segmented = true)] public int physicalAttack;
    [ProgressBar(0,10,0.373f,  0.467f, 0.733f, Segmented = true)] public int magicalAttack;
    [ProgressBar(0,10,0.733f, 0.518f, 0.373f, Segmented = true)] public int physicalDefense;
    [ProgressBar(0,10,0.651f, 0.373f, 0.733f, Segmented = true)] public int magicalDefense;
}