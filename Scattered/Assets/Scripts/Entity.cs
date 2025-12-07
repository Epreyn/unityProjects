using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public sealed class Entity : MonoBehaviour {
    #region INSPECTOR

    [Title("GameObject")] [ReadOnly] public GameObject entityGameObject;

    [Title("Class")] public string className;
    public EntityClass entityClass;

    [Title("Health")] public EntityStat health;
    public int currentHealth;

    [Title("Statistics")] public EntityStat attack;

    public EntityStat defense;
    public EntityStat speed;

    [Title("Deck")] public List<Card> deck;

    #endregion

    #region INITIALIZE

    public void Initialization() {
        className = entityClass.className;
        deck = entityClass.deck;

        health = new EntityStat(entityClass.health);
        attack = new EntityStat(entityClass.attack);
        defense = new EntityStat(entityClass.defense);
        speed = new EntityStat(entityClass.speed);

        DefineStats("health", ref health);
        currentHealth = health.Value;
        DefineStats("attack", ref attack);
        DefineStats("defense", ref defense);
        DefineStats("speed", ref speed);
    }

    private static void DefineStats(string statName, ref EntityStat stat) {
        stat.AddModifier(new StatModifier(statName, stat.BaseValue * 10, StatModType.Flat));
    }

    #endregion

    #region HEALTH METHODS

    private void ResetHealth() {
        currentHealth = health.Value;
    }

    public void ModifyHealthFlat(int value) {
        health.AddModifier(new StatModifier("health", value, StatModType.Flat));
    }

    public void ModifyHealthPercent(int value) {
        health.AddModifier(new StatModifier("health", value, StatModType.PercentAdd));
    }

    #endregion

    #region DAMAGE CALCULATION

    public void TakeDamages(int damages) {
        var damage = Mathf.RoundToInt(damages - defense.Value);
        if (damage <= 0) damage = 1;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, health.Value);
    }

    public void TakeHeal(int heal) {
        currentHealth = Mathf.Clamp(currentHealth + heal, 0, health.Value);
    }

    #endregion
}