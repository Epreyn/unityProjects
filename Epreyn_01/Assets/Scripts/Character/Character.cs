using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    #region INSPECTOR
    
    [Title("Class")]
    
    [ReadOnly] public string ClassName;
    public CharacterClass characterClass;

    [Title("Level & Experience")]
    
    [ReadOnly] public int Level;
    [ReadOnly] public int TargetExperience;
    [ReadOnly] [ProgressBar(0, "TargetExperience")] public int CurrentExperience;
    
    [Title("Health")]
    
    [ReadOnly] public int CurrentHealth;

    [Title("Statistics")] 
    
    [ReadOnly] public CharacterStat Health;
    [ReadOnly] public CharacterStat Physic;
    [ReadOnly] public CharacterStat Magic;
    [ReadOnly] public CharacterStat Defense;
    [ReadOnly] public CharacterStat Resistance;

    [Title("States")] 
    
    [ReadOnly] public List<CharacterState> States;
    
    #endregion

    #region INITIALIZATION
    
    /*----------------------------------------------------------------------
     DICE SYSTEM & VALUES FOR LIFE & STATISTICS
    ------------------------------------------------------------------------
    Fast_Stat = 4D2 --- Medium_Stat = 2D2 --- Slow_Stat = 1D2
    Fast_Health = 9D6 --- Medium_Health = 5D6 --- Slow_Health = 3D4
	
    Fast_Stat     :     Max 8 Min 4         Fast_Health     :     Max 54 Min 9
    Medium_Stat   :     Max 4 Min 2         Medium_Health   :     Max 30 Min 5
    Slow_Stat     :     Max 2 Min 1         Slow_Health     :     Max 12 Min 3
    ----------------------------------------------------------------------*/
    
    private Vector2 _healthDie, _physicDie, _magicDie;

    [Title("Functions")]
    [Button] public void InitializeCharacter() {
        // TODO GET THE LEVEL TO GO
        Level = 1;
        ClassName = characterClass.Name;

        Health = new CharacterStat(characterClass.Health);
        Physic = new CharacterStat(characterClass.Physic);
        Magic = new CharacterStat(characterClass.Magic);
        Defense = new CharacterStat(characterClass.Defense);
        Resistance = new CharacterStat(characterClass.Resistance);
        
        DefineHealthDie();
        DefineStatisticDie(Physic.Value, ref _physicDie);
        DefineStatisticDie(Magic.Value, ref _magicDie);
        DefineStatisticPercent(ref Defense.BaseValue);
        DefineStatisticPercent(ref Resistance.BaseValue);
        
        GoToTheLevel(Level);
    }

    #endregion

    #region EXPERIENCE METHODS

    public void AddExperience(int value) {
        CurrentExperience += value;
        CheckLevelChange();
    }
    
    [Button] public void LevelUp() {
        CurrentExperience += TargetExperience;
        CheckLevelChange();
    }

    private void CheckLevelChange() {
        while (CurrentExperience >= TargetExperience) {
            CurrentExperience -= TargetExperience;
			
            Level++;
            StatisticsGrowth();
            CurrentHealth = Health.Value;
            SetExperienceForNextLevel();
        }
    }

    private void SetExperienceForNextLevel() {
        TargetExperience = Mathf.RoundToInt(4 * Mathf.Pow(Level, 3) / 5);
    }
    
    private void GoToTheLevel(int level) {
        for (var i = 0; i < level; i++) {
            StatisticsGrowth();
            SetExperienceForNextLevel();
        }
        CurrentHealth = Health.Value;
    }

    #endregion
    
    #region STATISTICS METHODS
    
    private void DefineHealthDie() {
        if (characterClass.Health < 3) _healthDie = new Vector2(3, 4);
        else if (characterClass.Health == 3 || characterClass.Health == 4) _healthDie = new Vector2(5, 6);
        else _healthDie  = new Vector2(9, 6);
    }
    
    private void DefineStatisticDie(int stat, ref Vector2 die) {
        if (stat < 3) die  = new Vector2(1, 2);
        else if (stat == 3 || stat == 4) die = new Vector2(2, 2);
        else die = new Vector2(4, 2);
    }
    
    private void DefineStatisticPercent(ref int stat) {
        stat = stat switch {
            0 => 0,
            1 => 10,
            2 => 20,
            3 => 30,
            4 => 40,
            5 => 50,
            6 => 60,
            7 => 70,
            8 => 80,
            9 => 90,
            10 => 100,
            _ => stat
        };
    }

    private int Roll(int dices, int faces) {
        var result = 0;
        for (var i = 0; i < dices; i++) result += Random.Range(1, faces + 1);
        return result;
    }
    
    private void StatisticsGrowth() {
        Health.AddModifier(
            new StatModifier("Health", 
            Roll(
                (int)_healthDie.x, 
                (int)_healthDie.y) 
            + characterClass.Health, StatModType.Flat));
		
        Physic.AddModifier(
            new StatModifier("Physic", 
                Roll(
                    (int)_physicDie.x, 
                    (int)_physicDie.y) 
                + characterClass.Physic, StatModType.Flat));
		
        Magic.AddModifier(
            new StatModifier("Magic", 
                Roll(
                    (int)_magicDie.x, 
                    (int)_magicDie.y) 
                + characterClass.Magic, StatModType.Flat));;
    }
    
    #endregion
    
    #region HEALTH METHODS

    public void IncreaseHealth(int value) {
        CurrentHealth = Mathf.Min(CurrentHealth + value, Health.Value);
    }

    public void DecreaseHealth(int value) {
        CurrentHealth =  Mathf.Max(CurrentHealth - value, 0);
    }

    public void ResetHealth() {
        CurrentHealth = Health.Value;
    }

    public void ModifyHealthFlat(int value) {
        Health.AddModifier(new StatModifier("Health", value, StatModType.Flat));
    }
	
    public void ModifyHealthPercent(int value) {
        Health.AddModifier(new StatModifier( "Health", value, StatModType.PercentAdd));
    }

    public int CurrentHealthPercent() {
        return CurrentHealth * 100 / Health.Value;
    }

    #endregion
    
    #region DAMAGE CALCULATION

    public int PercentDamages(int attackValue, int percent) {
        return attackValue * percent / 100;
    }

    public int AbsoluteDamages(int percentDamages) {
        return Mathf.FloorToInt(percentDamages + Random.Range(224, 256) / 256) + 1;
    }

    public int DefenseDamages(int absoluteDamages, int resistance) {
        return absoluteDamages * resistance / 100;
    }

    public int FinalDamages(int absoluteDamages, int defense) {
        return absoluteDamages - defense;
    }

    #endregion
    
    #region MODIFIERS

    public void ModifyPhysicFlat(string modiferName, bool buff, int value) {
        var v = buff ? value : value * -1;
        Physic.AddModifier(new StatModifier(modiferName, v, StatModType.Flat));
    }
	
    public void ModifyMagicFlat(string modiferName, bool buff, int value) {
        var v = buff ? value : value * -1;
        Magic.AddModifier(new StatModifier(modiferName, v, StatModType.Flat));
    }
	
    public void ModifyDefenseFlat(string modiferName, bool buff, int value) {
        var v = buff ? value : value * -1;
        Defense.AddModifier(new StatModifier(modiferName, v, StatModType.Flat));
    }
	
    public void ModifyResistanceFlat(string modiferName, bool buff, int value) {
        var v = buff ? value : value * -1;
        Resistance.AddModifier(new StatModifier(modiferName, v, StatModType.Flat));
    }

    #endregion
    
    // TODO CHARACTER STATES SECTION
}
