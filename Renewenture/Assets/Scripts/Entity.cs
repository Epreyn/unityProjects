using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour {

	#region INSPECTOR

	[Title("Class")]

	public string className;

	public EntityClass entityClass;

	[Title("Level")]
    
	public int level;
	public int targetExp;
	[ProgressBar(0, "targetExp")] public int currentExp;
    
	[Title("hp")]
    
	public EntityStat hp;

	public int currentHp;

	[Title("Statistics")] 
    
	public EntityStat physicalAttack;
	public EntityStat magicalAttack;
	public EntityStat physicalDefense;
	public EntityStat magicalDefense;
	
	[Title("Inspector Actions")]
	
	[Button("Level Up")]
	private void LevelUpButton()
	{
		AddExp(targetExp - currentExp);
	}

	#endregion

	#region INITIALIZE

	private void Start() {
		Initialization();
	}

	public Entity() {
	
	}

	/*
    FastStat = 4D2 --- MediumStat = 2D2 --- SlowStat = 1D2
    Fasthp = 9D6 --- Mediumhp = 5D6 --- Slowhp = 3D4
	
    FastStat     :     Max 8 Min 4         Fasthp     :     Max 54 Min 9
    MediumStat   :     Max 4 Min 2         Mediumhp   :     Max 30 Min 5
    SlowStat     :     Max 2 Min 1         Slowhp     :     Max 12 Min 3
    */
	
	private Vector2 _healthDie, _physicalAttackDie, _magicalAttackDie;

	public void Initialization() {
		className = entityClass.className;

		level           = 1;
		hp              = new EntityStat(entityClass.hp);
		physicalAttack  = new EntityStat(entityClass.physicalAttack);
		magicalAttack   = new EntityStat(entityClass.magicalAttack);
		physicalDefense = new EntityStat(entityClass.physicalDefense);
		magicalDefense  = new EntityStat(entityClass.magicalDefense);

		InitializeHpGrowth();
		
		InitializeStatGrowth(physicalAttack.Value, ref _physicalAttackDie);
		InitializeStatGrowth(magicalAttack.Value,  ref _magicalAttackDie);
		
		InitializeStatsPercent(ref physicalDefense.BaseValue);
		InitializeStatsPercent(ref magicalDefense.BaseValue);

		InitializeStats();
		
		ExpNextLevel();
	}

	public void AttributeEntity(Entity targetEntity) {
		className       = targetEntity.className;
		entityClass  = targetEntity.entityClass;
		
		level           = targetEntity.level;
		hp              = targetEntity.hp;
		physicalAttack  = targetEntity.physicalAttack;
		magicalAttack   = targetEntity.magicalAttack;
		physicalDefense = targetEntity.physicalDefense;
		magicalDefense  = targetEntity.magicalDefense;
		
		currentHp       = targetEntity.currentHp;
		
		currentExp       = targetEntity.currentExp;
		targetExp        = targetEntity.targetExp;
	}

	private void InitializeHpGrowth()
	{
		_healthDie = entityClass.hp switch
		{
			< 3 => new Vector2(3, 4),
			3 or 4 => new Vector2(5, 6),
			_ => new Vector2(9, 6)
		};
	}
	
	private static void InitializeStatGrowth(int stat, ref Vector2 die)
	{
		die = stat switch
		{ 
			< 3 => new Vector2(1, 2),
			3 or 4 => new Vector2(2, 2),
			_ => new Vector2(4, 2)
		};
	}

	private static void InitializeStatsPercent(ref int stat) {
		int[] statsPercentValues = {0, 5, 10, 30, 40, 60, 65, 70, 75, 80, 85};
		if (stat >= 0 && stat < statsPercentValues.Length) {
			stat = statsPercentValues[stat];
		}
	}
	
	private void InitializeStats() {
		for (var i = 0; i < level; i++) {
			StatsGrowth();
			ExpNextLevel();
		}
		SetCurrentStatsValues();
	}

	private void SetCurrentStatsValues() {
		currentHp = hp.Value;
	}

	#endregion
    
	#region EXP CALCULATION

	public void AddExp(int value) {
		currentExp += value;
		CheckLevelChange();
	}

	private void CheckLevelChange() {
		while (currentExp >= targetExp) {
			currentExp -= targetExp;
			
			level++;
			StatsGrowth();
			SetCurrentStatsValues();
			ExpNextLevel();
		}
	}

	private void ExpNextLevel() {
		targetExp = Mathf.RoundToInt(4 * Mathf.Pow(level, 3) / 5);
	}

	#endregion
	
	#region STATS CALCULTATION

	protected virtual int Roll(int dices, int faces) {
		var result = 0;
		for (var i = 0; i < dices; i++) result += Random.Range(1, faces + 1);
		return result;
	}

	private void StatsGrowth() {
		hp.AddModifier(new StatModifier( "hp", 
			Roll((int)_healthDie.x, (int)_healthDie.y) 
			+ entityClass.hp, StatModType.Flat));
		
		physicalAttack.AddModifier(new StatModifier( "PA",
			Roll((int)_physicalAttackDie.x, (int)_physicalAttackDie.y) 
			+ entityClass.physicalAttack, StatModType.Flat));
		
		magicalAttack.AddModifier(new StatModifier( "MA", 
			Roll((int)_magicalAttackDie.x, (int)_magicalAttackDie.y) 
			+ entityClass.magicalAttack, StatModType.Flat));
	}

	#endregion

	#region HEALTH METHODS

	public void IncreaseHealth(int value) {
		currentHp = Mathf.Clamp(currentHp + value, 0, hp.Value);
	}

	public void DecreaseHealth(int value) {
		currentHp = Mathf.Clamp(currentHp - value, 0, currentHp);
	}


	public void ResetHealth() {
		currentHp = hp.Value;
	}
	
	public void ModifyHealthFlat(int value) {
		hp.AddModifier(new StatModifier("hp", value, StatModType.Flat));
	}
	
	public void ModifyHealthPercent(int value) {
		hp.AddModifier(new StatModifier( "hp", value, StatModType.PercentAdd));
	}

	#endregion

	#region DAMAGE CALCULATION

	public int PercentDamages(int attackValue, int percent) {
		return attackValue * percent / 100;
	}

	public int AbsoluteDamages(int percentDamages) {
		return Mathf.FloorToInt(percentDamages + Random.Range(224, 256) / 256) + 1;
	}

	public int Defense(int absoluteDamages, int resistance) {
		return absoluteDamages * resistance / 100;
	}

	public int Damages(int absoluteDamages, int defense) {
		return absoluteDamages - defense;
	}

	#endregion
	
}
