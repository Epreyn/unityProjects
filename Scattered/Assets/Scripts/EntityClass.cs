using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/New Entity Class", fileName = "New Entity Class.asset")]
public class EntityClass : ScriptableObject {
    [Title("Information")] public string className;

    [Title("Base Statistics")] [ProgressBar(0, 10, 0.373f, 0.733f, 0.498f, Segmented = true)]
    public int health;

    [ProgressBar(0, 10, 0.733f, 0.373f, 0.373f, Segmented = true)]
    public int attack;

    [ProgressBar(0, 10, 0.733f, 0.518f, 0.373f, Segmented = true)]
    public int defense;

    [ProgressBar(0, 10, 0.518f, 0.733f, 0.373f, Segmented = true)]
    public int speed;

    [Title("Deck List")] public List<Card> deck;
}