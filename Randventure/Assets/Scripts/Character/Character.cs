using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour {

	#region INSPECTOR

	[Sirenix.OdinInspector.Title("Class")]

	public string _className;
	public CharacterClass _characterClass;
	[ReadOnly]
	public bool _isTheFocus;

	[Sirenix.OdinInspector.Title("Next Actionner")]

	public bool _characterIsHero;
	
	[ReadOnly]
	public GameObject NextActionnersGO;

	[Sirenix.OdinInspector.Title("Level")]
    
	public int _level;
	public int _targetXP;
	[ProgressBar(0, "_targetXP")] public int _currentXP;
    
	[Sirenix.OdinInspector.Title("HP")]
    
	public CharacterStat _HP;
	public int _currentHP;
	public int _currentShield;

	[Sirenix.OdinInspector.Title("Statistics")] 
    
	public CharacterStat _physicalAttack;
	public CharacterStat _magicalAttack;
	public CharacterStat _physicalDefense;
	public CharacterStat _magicalDefense;

	[Sirenix.OdinInspector.Title("States")]

	[ReadOnly]
	public bool _onStun;
	
	[ReadOnly]
	public bool _onEvasion;
	
	[ReadOnly]
	public bool _onIntervention;
	
	[ReadOnly]
	public CharacterInstance _interventionCharacter;

	public List<CharacterState> _characterStates;

	[Sirenix.OdinInspector.Title("Resources")]

	public Sprite _interventionSprite;
	public Sprite _evasionSprite;
	public Sprite _stunSprite;

	#endregion

	#region INITIALIZE

	private void Start() {
		NextActionnersGO = _characterIsHero
			? GameObject.Find("Heros_Next_Actionners")
			: GameObject.Find("Monsters_Next_Actionners");
	}

	public Character() {
		_className = "";
		_isTheFocus = false;

		_level = 0;
		_currentXP = 0;
		_targetXP  = 0;
		
		_HP              = new CharacterStat();
		_currentHP     = 0;
		_currentShield = 0;
		
		_physicalAttack  = new CharacterStat();
		_magicalAttack   = new CharacterStat();
		_physicalDefense = new CharacterStat();
		_magicalDefense  = new CharacterStat();
		
		_characterStates = new List<CharacterState>();
	}

	/*
    Fast_Stat = 4D2 --- Medium_Stat = 2D2 --- Slow_Stat = 1D2
    Fast_HP = 9D6 --- Medium_HP = 5D6 --- Slow_HP = 3D4
	
    Fast_Stat     :     Max 8 Min 4         Fast_HP     :     Max 54 Min 9
    Medium_Stat   :     Max 4 Min 2         Medium_HP   :     Max 30 Min 5
    Slow_Stat     :     Max 2 Min 1         Slow_HP     :     Max 12 Min 3
    */
	
	private Vector2 healthDie, physicalAttackDie, magicalAttackDie;

	public void Initialization() {
		_className = _characterClass._className;

		_level           = 1;
		_HP              = new CharacterStat(_characterClass.HP);
		_physicalAttack  = new CharacterStat(_characterClass.PhysicalAttack);
		_magicalAttack   = new CharacterStat(_characterClass.MagicalAttack);
		_physicalDefense = new CharacterStat(_characterClass.PhysicalDefense);
		_magicalDefense  = new CharacterStat(_characterClass.MagicalDefense);

		InitializeHPGrowth();
		
		InitializeStatGrowth(_physicalAttack.Value, ref physicalAttackDie);
		InitializeStatGrowth(_magicalAttack.Value,  ref magicalAttackDie);
		
		InitializeStatsPercent(ref _physicalDefense.BaseValue);
		InitializeStatsPercent(ref _magicalDefense.BaseValue);

		InitializeStats();
		
		ExpNextLevel();
	}

	public void AttributeCharacter(Character targetCharacter) {
		_className       = targetCharacter._className;
		_characterClass  = targetCharacter._characterClass;
		
		_level           = targetCharacter._level;
		_HP              = targetCharacter._HP;
		_physicalAttack  = targetCharacter._physicalAttack;
		_magicalAttack   = targetCharacter._magicalAttack;
		_physicalDefense = targetCharacter._physicalDefense;
		_magicalDefense  = targetCharacter._magicalDefense;
		
		_currentHP       = targetCharacter._currentHP;
		_currentShield   = targetCharacter._currentShield;
		
		_currentXP       = targetCharacter._currentXP;
		_targetXP        = targetCharacter._targetXP;

		_characterStates = targetCharacter._characterStates;
	}

	private void InitializeHPGrowth() {
		if (_characterClass.HP < 3) healthDie                                  = new Vector2(3, 4);
		else if (_characterClass.HP == 3 || _characterClass.HP == 4) healthDie = new Vector2(5, 6);
		else healthDie                                                         = new Vector2(9, 6);
	}
	
	private void InitializeStatGrowth(int stat, ref Vector2 die) {
		if (stat < 3) die                    = new Vector2(1, 2);
		else if (stat == 3 || stat == 4) die = new Vector2(2, 2);
		else die                             = new Vector2(4, 2);
	}

	private void InitializeStatsPercent(ref int stat) {
		switch (stat) {
			case 0:
				stat = 0;
				break;
			
			case 1:
				stat = 5;
				break;
			
			case 2:
				stat = 10;
				break;
			
			case 3:
				stat = 30;
				break;
			
			case 4:
				stat = 40;
				break;
			
			case 5:
				stat = 60;
				break;
			
			case 6:
				stat = 65;
				break;
			
			case 7:
				stat = 70;
				break;
			
			case 8:
				stat = 75;
				break;
			
			case 9:
				stat = 80;
				break;
			
			case 10:
				stat = 85;
				break;
		}
	}
	
	private void InitializeStats() {
		for (var i = 0; i < _level; i++) {
			StatsGrowth();
			ExpNextLevel();
		}
		SetCurrentStatsValues();
	}

	private void SetCurrentStatsValues() {
		_currentHP = _HP.Value;
		_currentShield = 0;
	}

	#endregion
    
	#region EXP CALCULATION

	public void AddEXP(int value) {
		_currentXP += value;
		CheckLevelChange();
	}

	private void CheckLevelChange() {
		while (_currentXP >= _targetXP) {
			_currentXP -= _targetXP;
			
			_level++;
			StatsGrowth();
			SetCurrentStatsValues();
			ExpNextLevel();
		}
	}

	private void ExpNextLevel() {
		_targetXP = Mathf.RoundToInt(4 * Mathf.Pow(_level, 3) / 5);
	}

	#endregion
	
	#region STATS CALCULTATION

	protected virtual int Roll(int dices, int faces) {
		var result                             = 0;
		for (var i = 0; i < dices; i++) result += Random.Range(1, faces + 1);
		return result;
	}

	private void StatsGrowth() {
		_HP.AddModifier(new StatModifier( "HP", 
			Roll((int)healthDie.x, (int)healthDie.y) 
			+ _characterClass.HP, StatModType.Flat));
		
		_physicalAttack.AddModifier(new StatModifier( "PA",
			Roll((int)physicalAttackDie.x, (int)physicalAttackDie.y) 
			+ _characterClass.PhysicalAttack, StatModType.Flat));
		
		_magicalAttack.AddModifier(new StatModifier( "MA", 
			Roll((int)magicalAttackDie.x, (int)magicalAttackDie.y) 
			+ _characterClass.MagicalAttack, StatModType.Flat));
	}

	#endregion

	#region HEALTH METHODS

	public bool FocusIsDefeated() {
		return _currentHP <= 0 && _isTheFocus;
	}

	public void IncreaseHealth(int value) {
		_currentHP = Mathf.Min(_currentHP + value, _HP.Value);
	}

	public void IncreaseShield(int value) {
		_currentShield += value;
	}
	
	public void DecreaseHealth(int value) {
		if (value >= _currentShield) {
			value          -= _currentShield;
			_currentHP =  Mathf.Max(_currentHP - value, 0);
		}
		else { _currentShield -= value; }
	}
	
	// DECREASE SHIELD

	public void ResetHealth() {
		_currentHP = _HP.Value;
	}

	public void ResetShield() {
		_currentShield = 0;
	}
	
	public void ModifyHealthFlat(int value) {
		_HP.AddModifier(new StatModifier("HP", value, StatModType.Flat));
	}
	
	public void ModifyHealthPercent(int value) {
		_HP.AddModifier(new StatModifier( "HP", value, StatModType.PercentAdd));
	}

	public int CurrentHPPercent() {
		return _currentHP * 100 / _HP.Value;
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

	#region CHARACTER STATES

	public void AddBuffState(Sprite stateIcon, string stateName, int turnNumber) {
		var state = new CharacterState {
			_stateIcon = stateIcon,
			_stateName = stateName,
			_stateTurnNumber = turnNumber,
			_stateType = CharacterState.StateType.Buff
		};
		_characterStates.Add(state);
	}
	
	public void AddDebuffState(Sprite stateIcon, string stateName, int turnNumber) {
		var state = new CharacterState {
			_stateIcon = stateIcon,
			_stateName       = stateName,
			_stateTurnNumber = turnNumber,
			_stateType       = CharacterState.StateType.Debuff
		};
		_characterStates.Add(state);
	}
	
	public void AddDOTState(Sprite stateIcon, string stateName, int turnNumber, int dotValue) {
		var state = new CharacterState {
			_stateIcon = stateIcon,
			_stateName       = stateName,
			_stateTurnNumber = turnNumber,
			_stateType       = CharacterState.StateType.DOT,
			_dotValue        = dotValue
		};
		_characterStates.Add(state);
	}
	
	public void AddHOTState(Sprite stateIcon, string stateName, int turnNumber, int hotValue) {
		var state = new CharacterState {
			_stateIcon = stateIcon,
			_stateName       = stateName,
			_stateTurnNumber = turnNumber,
			_stateType       = CharacterState.StateType.HOT,
			_hotValue        = hotValue
		};
		_characterStates.Add(state);
	}

	public void AddInterventionState(Character target) {
		var state = new CharacterState {
			_stateIcon       = _interventionSprite,
			_stateName       = "Intervention",
			_stateTurnNumber = -1,
			_stateType       = CharacterState.StateType.Intervention,
			_interventionTarget = target
		};
		_characterStates.Add(state);
	}
	
	public void AddEvasionState() {
		var state = new CharacterState {
			_stateIcon          = _evasionSprite,
			_stateName          = "Evasion",
			_stateTurnNumber    = -1,
			_stateType          = CharacterState.StateType.Evasion,
		};
		_characterStates.Add(state);
		_onEvasion = true;
	}
	
	public void AddStunState() {
		var state = new CharacterState {
			_stateIcon       = _stunSprite,
			_stateName       = "Stun",
			_stateTurnNumber = -1,
			_stateType       = CharacterState.StateType.Stun,
		};
		_characterStates.Add(state);
		_onStun = true;
	}

	public bool CheckBuffAlreadyExist(string buffName, int turnNumber) {
		var result = false;
		foreach (var t in _characterStates.
			Where(t => t._stateType == CharacterState.StateType.Buff).
			Where(t => t._stateName == buffName)) {
			t._stateTurnNumber += turnNumber;
			result             =  true;
		}
		return result;
	}
	
	public bool CheckDebuffAlreadyExist(string debuffName, int turnNumber) {
		var result = false;
		foreach (var t in _characterStates.
			Where(t => t._stateType == CharacterState.StateType.Debuff).
			Where(t => t._stateName == debuffName)) {
			t._stateTurnNumber += turnNumber;
			result             =  true;
		}
		return result;
	}
	
	public bool CheckDOTAlreadyExist(string dotName, int turnNumber) {
		var result = false;
		foreach (var t in _characterStates.
			Where(t => t._stateType == CharacterState.StateType.DOT).
			Where(t => t._stateName == dotName)) {
			t._stateTurnNumber += turnNumber;
			result             =  true;
		}
		return result;
	}
	
	public bool CheckHOTAlreadyExist(string hotName, int turnNumber) {
		var result = false;
		foreach (var t in _characterStates.
			Where(t => t._stateType == CharacterState.StateType.HOT).
			Where(t => t._stateName == hotName)) {
			t._stateTurnNumber += turnNumber;
			result             =  true;
		}
		return result;
	}

	public void ResolveStates(bool isHero) {
		for (var i = _characterStates.Count - 1; i >= 0; i--) {
			
			// APPLY
			switch (_characterStates[i]._stateType) {
				case CharacterState.StateType.DOT: {
					if (_characterStates[i]._stateTurnNumber != 0) DecreaseHealth(_characterStates[i]._dotValue);
					break;
				}
				case CharacterState.StateType.HOT: {
					if (_characterStates[i]._stateTurnNumber != 0) IncreaseHealth(_characterStates[i]._hotValue);
					break;
				}
				case CharacterState.StateType.Intervention: {
					_onIntervention = true;
					_interventionCharacter = new CharacterInstance();
					_interventionCharacter._characterInstance = _characterStates[i]._interventionTarget;
					break;
				}
			}
			
			// DECREASE
			if (_characterStates[i]._stateTurnNumber > 0) _characterStates[i]._stateTurnNumber -= 1;

			// DELETE
			switch (_characterStates[i]._stateType) {
				case CharacterState.StateType.Buff:
				case CharacterState.StateType.Debuff: {
					if (_characterStates[i]._stateTurnNumber != 0) continue;
					_physicalAttack.RemoveAllModifiersFromName(_characterStates[i]._stateName);
					_physicalDefense.RemoveAllModifiersFromName(_characterStates[i]._stateName);
					_magicalAttack.RemoveAllModifiersFromName(_characterStates[i]._stateName);
					_magicalDefense.RemoveAllModifiersFromName(_characterStates[i]._stateName);
					_characterStates.Remove(_characterStates[i]);
					break;
				}
				case CharacterState.StateType.DOT: {
					if (_characterStates[i]._stateTurnNumber == 0) _characterStates.Remove(_characterStates[i]);
					break;
				}
				case CharacterState.StateType.HOT: {
					if (_characterStates[i]._stateTurnNumber == 0) _characterStates.Remove(_characterStates[i]);
					break;
				}
			}
		}

		if (isHero) {
			var fsmBoolH = FsmVariables.GlobalVariables.FindFsmBool("Animation_Hero_States");
			fsmBoolH.Value = false;
		}
		else {
			var fsmBoolM = FsmVariables.GlobalVariables.FindFsmBool("Animation_Monster_States");
			fsmBoolM.Value = false;
		}
		
	}

	public void ResolveStatesEndTurn() {
		for (var i = _characterStates.Count - 1; i >= 0; i--) {
			// DELETE
			if (_characterStates[i]._stateType == CharacterState.StateType.Intervention ||
			    _characterStates[i]._stateType == CharacterState.StateType.Evasion      ||
			    _characterStates[i]._stateType == CharacterState.StateType.Stun) {
				_characterStates.Remove(_characterStates[i]);
			}
		}
		
		_onEvasion      = false;
		_onIntervention = false;
		_onStun         = false;
	}
	
	#endregion

	#region MODIFIERS

	public void ModifyPAFlat(string modiferName, bool buff, int value) {
		var v = buff ? value : value * -1;
		_physicalAttack.AddModifier(new StatModifier(modiferName, v, StatModType.Flat));
	}
	
	public void ModifyMAFlat(string modiferName, bool buff, int value) {
		var v = buff ? value : value * -1;
		_magicalAttack.AddModifier(new StatModifier(modiferName, v, StatModType.Flat));
	}
	
	public void ModifyPDFlat(string modiferName, bool buff, int value) {
		var v = buff ? value : value * -1;
		_physicalDefense.AddModifier(new StatModifier(modiferName, v, StatModType.Flat));
	}
	
	public void ModifyMDFlat(string modiferName, bool buff, int value) {
		var v = buff ? value : value * -1;
		_magicalDefense.AddModifier(new StatModifier(modiferName, v, StatModType.Flat));
	}

	#endregion

	#region NEXT ACTIONNERS

	public void CreateNextActionner(Sprite icon, Character launcher) {
		
		var naBuffer = new NextActionner {
			_icon = icon, 
			_launcher = launcher, 
			_battleActions = new List<BattleAction>()
		};

		NextActionnersGO.transform.GetChild(0).transform.GetComponent<NextActionnerInstance>()._naInstance = naBuffer;
		NextActionnersGO.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
	}

	public void AddBattleActionsOnNextActionner(BattleAction battleAction) {
		NextActionnersGO.transform.GetChild(0).transform.GetComponent<NextActionnerInstance>().
			_naInstance._battleActions.Add(battleAction);
	}

	#endregion
}
