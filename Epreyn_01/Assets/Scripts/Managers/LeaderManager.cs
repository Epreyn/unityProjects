using UnityEngine;
using Sirenix.OdinInspector;

public class LeaderManager : MonoBehaviour {

    [Title("Leader Character")]
    public Character LeaderCharacter;

    [Title("Hero Classes")]
    [ReadOnly] public int CurrentHeroClassIndex;
    public CharacterClass[] HeroClasses;
    
    private void Awake() {
        CurrentHeroClassIndex = 0;
        LeaderCharacter.characterClass = HeroClasses[CurrentHeroClassIndex];
    }

    public void PreviousHeroClass() {
        if (CurrentHeroClassIndex <= 0) CurrentHeroClassIndex = HeroClasses.Length - 1;
        else CurrentHeroClassIndex--;
        LeaderCharacter.characterClass = HeroClasses[CurrentHeroClassIndex];
    }
    public void NextHeroClass() {
        if (CurrentHeroClassIndex >= HeroClasses.Length - 1) CurrentHeroClassIndex = 0;
        else CurrentHeroClassIndex++;
        LeaderCharacter.characterClass = HeroClasses[CurrentHeroClassIndex];
    }
}
