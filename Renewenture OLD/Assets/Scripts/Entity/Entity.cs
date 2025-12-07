using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public sealed class Entity : MonoBehaviour {
    #region INSPECTOR

    [Title("GameObject")] [ReadOnly] public GameObject entityGameObject;
    private Vector3 _originalPosition;

    [Title("Class")] public string className;

    public EntityClass entityClass;

    [Title("Spells")] public Spell[] spells;

    [Title("Health")] public EntityStat hp;
    public int currentHp;

    [Title("Status")] public List<ActiveStatus> activeStatuses;

    [Title("Statistics")] public EntityStat physicalAttack;
    public EntityStat magicalAttack;
    public EntityStat physicalDefense;
    public EntityStat magicalDefense;
    public EntityStat attackSpeed;

    #endregion

    #region INITIALIZE

    public void Initialization() {
        className = entityClass.className;
        spells = entityClass.spells;

        hp = new EntityStat(entityClass.hp);
        physicalAttack = new EntityStat(entityClass.physicalAttack);
        magicalAttack = new EntityStat(entityClass.magicalAttack);
        physicalDefense = new EntityStat(entityClass.physicalDefense);
        magicalDefense = new EntityStat(entityClass.magicalDefense);
        attackSpeed = new EntityStat(entityClass.attackSpeed);

        activeStatuses = new List<ActiveStatus>();

        DefineStats("HP", ref hp);
        currentHp = hp.Value;
        DefineStats("PA", ref physicalAttack);
        DefineStats("MA", ref magicalAttack);
        DefineStats("PD", ref physicalDefense);
        DefineStats("MD", ref magicalDefense);
        DefineStats("AS", ref attackSpeed);
    }

    private static void DefineStats(string statName, ref EntityStat stat) {
        Debug.Log(statName + " : " + stat.BaseValue);
        var percent = Mathf.RoundToInt(stat.BaseValue * 9);
        Debug.Log("FUTURE STAT" + " : " + percent);
        stat.AddModifier(new StatModifier(statName, percent, StatModType.Flat));
        // stat.AddModifier(new StatModifier(statName,
        //     stat.BaseValue * 10, StatModType.Flat));
    }

    // private void StatsGrowth() {
    //     _HP.AddModifier(new StatModifier( "HP", 
    //         Roll((int)healthDie.x, (int)healthDie.y) 
    //         + _characterClass.HP, StatModType.Flat));
    //
    //     _physicalAttack.AddModifier(new StatModifier( "PA",
    //         Roll((int)physicalAttackDie.x, (int)physicalAttackDie.y) 
    //         + _characterClass.PhysicalAttack, StatModType.Flat));
    //
    //     _magicalAttack.AddModifier(new StatModifier( "MA", 
    //         Roll((int)magicalAttackDie.x, (int)magicalAttackDie.y) 
    //         + _characterClass.MagicalAttack, StatModType.Flat));
    // }

    #endregion

    #region HEALTH METHODS

    private IEnumerator ChangeHealthOverTime(int targetValue, VisualEffectType effectType, float duration = 0.4f,
        float shakeAmplitude = 1f, float shakeFrequency = 100f, float bounceAmplitude = 2f,
        float bounceFrequency = 30f) {
        var startValue = currentHp;
        float timePassed = 0;
        _originalPosition = entityGameObject.transform.localPosition;

        while (timePassed < duration) {
            timePassed += Time.deltaTime;
            currentHp = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, timePassed / duration));

            if (effectType == VisualEffectType.Shake) {
                var shakeOffsetX = Mathf.Sin(timePassed * shakeFrequency) * shakeAmplitude;
                var shakeOffsetY = Mathf.Cos(timePassed * shakeFrequency) * shakeAmplitude;
                entityGameObject.transform.localPosition = new Vector3(_originalPosition.x + shakeOffsetX,
                    _originalPosition.y + shakeOffsetY, _originalPosition.z);
            }
            else if (effectType == VisualEffectType.Bounce) {
                var bounceOffsetY = Mathf.Sin(timePassed * bounceFrequency) * bounceAmplitude;
                entityGameObject.transform.localPosition = new Vector3(_originalPosition.x,
                    _originalPosition.y + bounceOffsetY, _originalPosition.z);
            }

            yield return null;
        }

        entityGameObject.transform.localPosition = _originalPosition;
        currentHp = targetValue;
    }


    private void ResetHealth() {
        currentHp = hp.Value;
    }

    public void ModifyHealthFlat(int value) {
        hp.AddModifier(new StatModifier("hp", value, StatModType.Flat));
    }

    public void ModifyHealthPercent(int value) {
        hp.AddModifier(new StatModifier("hp", value, StatModType.PercentAdd));
    }

    #endregion

    #region DAMAGE CALCULATION

    public IEnumerator TakePhysicalDamages(int damages) {
        var damage = Mathf.RoundToInt(damages - physicalDefense.Value);
        if (damage <= 0) damage = 1;
        yield return StartCoroutine(
            ChangeHealthOverTime(Mathf.Clamp(currentHp - damage, 0, currentHp),
                VisualEffectType.Shake));
    }

    public IEnumerator TakeMagicalDamages(int damages) {
        var damage = Mathf.RoundToInt(damages - magicalDefense.Value);
        if (damage <= 0) damage = 1;
        yield return StartCoroutine(
            ChangeHealthOverTime(Mathf.Clamp(currentHp - damage, 0, currentHp),
                VisualEffectType.Shake));
    }

    public IEnumerator TakeDamages(int damages) {
        yield return StartCoroutine(
            ChangeHealthOverTime(Mathf.Clamp(currentHp - damages, 0, currentHp),
                VisualEffectType.Shake));
    }

    public IEnumerator TakeHeal(int heal) {
        yield return StartCoroutine(
            ChangeHealthOverTime(Mathf.Clamp(currentHp + heal, 0, hp.Value),
                VisualEffectType.Bounce));
    }

    #endregion

    #region STATUS

    public bool HasStatus(BattleAction.StatusType type) {
        return activeStatuses.Any(status => status.Type == type);
    }

    public void RemoveStatus(BattleAction.StatusType type) {
        var statusToRemove = activeStatuses.FirstOrDefault(status => status.Type == type);
        if (statusToRemove != null) activeStatuses.Remove(statusToRemove);
    }

    public void ClearStatuses() {
        activeStatuses.Clear();
    }

    #endregion
}

public enum VisualEffectType {
    Shake,
    Bounce
}
