# Analyse des Projets Unity - Concepts & Idees a Retenir

**Date:** Decembre 2024
**Objectif:** Identifier les concepts innovants et mecaniques interessantes a potentiellement reutiliser

---

## Vue d'Ensemble Rapide

| Projet | Genre | Maturite | Concepts Cles | Note Interet |
|--------|-------|----------|---------------|--------------|
| **AniBaston** | Combat Tour par Tour Mobile | Avance | Portails tactiques, Photon+PlayFab | ★★★★☆ |
| **Epreyn_01/RPG** | RPG Tactique | Prototype | Systeme de des pour stats, Modificateurs | ★★★★★ |
| **IsoR** | Deck-Building Roguelike 3D | Moyen | Hybride exploration/cartes, Autotiles3D | ★★★★☆ |
| **Randventure** | RPG Tactique Cartes | Avance | Actions emotionnelles, Invocation | ★★★★★ |
| **Scattered** | Combat Tactique Emotions | Prototype | 45 emotions combinables | ★★★★★ |
| **STD (Scattered)** | Platformer Narratif | Avance | Doppelganger, Sorts emotionnels | ★★★★☆ |
| **Rythm** | Jeu de Rythme | Prototype | Spawn directionnel, Sweet spot mobile | ★★★☆☆ |
| **Renewenture** | RPG Party | Prototype | Systeme de cartes UI, Stats modifiables | ★★★☆☆ |
| **MmoRogueLite** | Roguelike 2D | Embryon | Integration Aseprite | ★★☆☆☆ |
| **VinsHorsNormes** | App Metier | Production | Google Sheets backend | ★★☆☆☆ |
| **CdR_App** | Template vide | - | - | ☆☆☆☆☆ |

---

## CONCEPTS LES PLUS INTERESSANTS A RETENIR

### 1. Systeme d'Emotions Combinatoires (Scattered)
**Fichier:** `Scattered/Assets/Scripts/Emotions.cs`

**Concept:** 45 emotions basees sur la Roue de Plutchik avec un systeme de **combinaison dynamique**.

```
Joie + Confiance = Amour
Peur + Surprise = Alarme
Colere + Degout = Mepris
Tristesse + Anticipation = Pessimisme
```

**Pourquoi c'est interessant:**
- Permet de creer des **synergies emergentes** entre cartes/sorts
- Ajoute une couche strategique au-dela des simples stats numeriques
- Potentiel pour storytelling dynamique base sur les emotions
- Applicable a: combat, dialogues, relations entre personnages, crafting

**Idees d'extension:**
- Emotions qui evoluent selon les actions du joueur
- Ennemis avec des faiblesses emotionnelles
- Arbre de competences base sur les emotions maitrisees

---

### 2. Systeme de Des pour Progression (Epreyn_01)
**Fichier:** `Epreyn_01/Assets/Scripts/Character/Character.cs`

**Concept:** Au lieu d'un systeme "+X par niveau", utilisation de **lancers de des** pour la croissance des stats.

```
FastStat = 4D2   (Min 4, Max 8)
MediumStat = 2D2 (Min 2, Max 4)
SlowStat = 1D2   (Min 1, Max 2)
HP = 3D4 a 9D6 selon la classe
```

**Pourquoi c'est interessant:**
- Chaque level-up est **unique et imprevisible**
- Cree des personnages avec des "trajectoires" differentes meme a niveau egal
- Ajoute tension et excitation au moment du level-up
- Rejouabilite accrue (deux guerriers niveau 10 seront differents)

**Idees d'extension:**
- "Blessed dice" - bonus temporaires qui ameliorent les jets
- Systeme de "reroll" limite pour les stats critiques
- Visualisation des jets de des pour le feedback joueur

---

### 3. Dualite Joueur/Doppelganger (STD/Scattered)
**Fichiers:** `STD/Assets/Old_Scattered/Scripts/Neidan.cs`, `Doppel.cs`

**Concept:** Deux personnages controles simultanement avec des **mecaniques miroir**.

- **Neidan** (corps physique): Mouvement classique, projectiles d'eau
- **Doppel** (double etheree): Apparait selon les inputs, peut s'effacer, partage les sorts

**Pourquoi c'est interessant:**
- Gameplay asymetrique avec un seul joueur
- Puzzles bases sur la coordination des deux entites
- Exploration de themes philosophiques (identite, dualite)
- Le Doppel peut acceder a des zones inaccessibles au corps

**Idees d'extension:**
- Mecaniques de "transfert" entre les deux corps
- Ennemis qui ne voient qu'un des deux personnages
- Sorts qui necessitent les deux personnages alignes

---

### 4. Sorts Bases sur les Emotions avec Effets Visuels (STD)
**Fichier:** `STD/Assets/Old_Scattered/Scripts/SpellSystem.cs`

**Concept:** Les sorts sont lies a des **etats emotionnels** avec feedback visuel par couleur.

| Emotion | Effet | Couleur |
|---------|-------|---------|
| Terreur | Freeze | Vert |
| Chagrin | DoT (Damage over Time) | Bleu |
| Rage | Degats directs | Rouge |
| Extase | (a definir) | - |
| Adoration | Attraction particules | - |

**Pourquoi c'est interessant:**
- Le joueur "ressent" l'emotion a travers le visuel
- Systeme intuitif (rouge = danger/colere)
- Permet des combos emotionnels (terreur puis rage)
- Narration emergente via les choix d'emotions

---

### 5. Systeme de Portails Tactiques (AniBaston)
**Concept identifie via tags:** `AttPortals`, `DefPortals`

**Concept:** En combat, les unites emergent de **portails positionnes strategiquement**.

- Portails d'attaque pour spawner des offensifs
- Portails de defense pour le positionnement defensif
- Implique une phase de "deployment" avant le combat

**Pourquoi c'est interessant:**
- Ajoute dimension spatiale au tour par tour
- Choix strategiques: ou placer ses portails?
- Peut etre combine avec des classes (mage derriere, guerrier devant)
- Visuellement spectaculaire (unites qui emergent)

---

### 6. Hybride Exploration 3D / Combat Cartes (IsoR)
**Architecture:** NueDeck + Exploration isometrique

**Concept:** Le joueur explore un monde 3D isometrique, et les combats se resolvent via un **systeme de deck-building**.

- Exploration: Mouvement WASD, camera rotative Q/E
- Collision avec ennemi → Transition vers mode combat cartes
- Camera zoom pour indiquer le mode combat (5x → 2x)

**Pourquoi c'est interessant:**
- Le meilleur des deux mondes: immersion 3D + profondeur tactique cartes
- Moins repetitif que du deck-building pur
- Possibilite de trouver des cartes dans l'exploration
- Le terrain d'exploration peut influencer le combat

**Idees d'extension:**
- Cartes specifiques a certaines zones
- Elements de terrain qui s'ajoutent au deck temporairement
- Fuite possible en combat (retour a l'exploration)

---

### 7. Systeme de "Next Actionner" (Randventure)
**Fichier:** `Randventure/Assets/Scripts/Next Actionners/`

**Concept:** Visualisation en temps reel de **qui va agir et avec quelle action**.

- File d'attente visible des prochaines actions
- Le joueur peut cibler le "prochain acteur" specifiquement
- Certains sorts peuvent "Skip" ou reordonner la file

**Pourquoi c'est interessant:**
- Transparence complete sur le deroulement du combat
- Decisions strategiques basees sur l'ordre
- Permet des sorts de "manipulation de timeline"
- Reduit la frustration du RNG

---

### 8. Actions Composables Multi-Effets (Randventure/Epreyn)
**Fichier:** `Randventure/Assets/Scripts/Abilities/BattleAction.cs`

**Concept:** Une action peut avoir **plusieurs effets enchaines**, chacun configurable independamment.

```
Holy Nova = [
  Action 1: Heal allies (50-100% Magic),
  Action 2: Damage enemies (30-60% Magic)
]

Shield Bash = [
  Action 1: Damage (Physical),
  Action 2: Apply Stun (2 turns, 60% accuracy)
]
```

**Pourquoi c'est interessant:**
- Permet des sorts complexes sans code supplementaire
- Balancing via ScriptableObjects (pas de recompilation)
- Combinaisons infinies via l'editeur
- Designers peuvent creer des sorts sans programmer

---

### 9. Systeme d'Invocation Dynamique (Randventure)
**Concept identifie:** `ActionType.Invoke`

**Concept:** Certains sorts **invoquent des allies temporaires** bases sur des CharacterClass.

- L'invocation herite des stats de la classe
- Dure un certain nombre de tours
- Peut avoir ses propres actions

**Pourquoi c'est interessant:**
- Ajoute de la variete tactique
- Synergie avec les classes (Necromancien invoque squelettes)
- Gestion de ressources (combien d'invocations actives?)
- Possibilite d'invoquer des ennemis vaincus

---

### 10. Sweet Spot Mobile (Rythm)
**Fichier:** `Rythm/Assets/Scripts/GameplayController.cs`

**Concept:** Au lieu d'une ligne fixe, le "sweet spot" du jeu de rythme est **mobile et controlable**.

- Fleches directionnelles pour deplacer le sweet spot
- Les notes arrivent de 12 directions differentes
- Le joueur doit positionner ET timer son input

**Pourquoi c'est interessant:**
- Double challenge: position + timing
- Plus engage que les jeux de rythme classiques
- Peut etre adapte a d'autres genres (tower defense rythmique?)
- Multijoueur possible: chacun controle son sweet spot

---

## PATTERNS DE CODE REUTILISABLES

### Pattern: Modificateurs de Stats Empilables
**Present dans:** Epreyn_01, Randventure, Renewenture, Scattered

```csharp
public enum StatModType {
    Flat = 100,      // +10 Defense
    PercentAdd = 200, // +15% (additionne avec autres %)
    PercentMult = 300 // x1.2 (multiplie)
}
```

**Ordre d'application:** Flat → PercentAdd → PercentMult

**Avantages:**
- Buffs/Debuffs proprement geres
- Facile a ajouter/retirer des effets
- Calcul deterministe et predictible
- Cache avec dirty flag pour performance

---

### Pattern: ScriptableObjects pour Game Data
**Present dans:** Tous les projets RPG

**Utilisation:**
- `CharacterClass.asset` - Definition des classes
- `Spell.asset` - Sorts et leurs effets
- `BattleAction.asset` - Actions atomiques
- `CharacterState.asset` - Buffs/Debuffs

**Avantages:**
- Separation code/data complete
- Designers peuvent modifier sans recompiler
- Versionnable, mergeable
- Previsualisation dans l'Inspector

---

### Pattern: Manager Singleton avec Persistence
**Present dans:** AniBaston, IsoR, STD

```csharp
public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    void Awake() {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
```

---

## PROJETS PAR NIVEAU DE MATURITE

### Production-Ready
- **AniBaston** - Multiplayer complet avec backend PlayFab
- **VinsHorsNormes** - Application metier fonctionnelle

### Prototype Avance
- **Randventure** - Systeme de combat complet, balancing en cours
- **STD/Scattered** - Gameplay dual-character jouable
- **IsoR** - Loop exploration/combat fonctionnel

### Prototype Initial
- **Epreyn_01/RPG** - Fondations solides, combat non implemente
- **Rythm** - Mecanique de base fonctionnelle
- **Renewenture** - UI cartes, pas de combat
- **Scattered (emotions)** - Framework emotions, pas de resolution

### Embryonnaire
- **MmoRogueLite** - Juste la structure
- **CdR_App** - Projet vide

---

## RECOMMANDATIONS FINALES

### Top 3 des Concepts a Explorer
1. **Emotions Combinatoires** (Scattered) - Originalite maximale, peu explore dans les jeux
2. **Dice-Based Progression** (Epreyn) - Rejouabilite et unpredictabilite
3. **Dualite Player/Doppel** (STD) - Gameplay asymetrique innovant

### Meilleure Base Code a Reutiliser
- **Systeme de Stats Modifiables** - Robuste, teste dans 4 projets
- **Framework ScriptableObject** - Architecture propre et scalable
- **Systeme d'Actions Composables** - Flexibilite maximale pour le game design

### Combinaisons Interessantes
- Emotions + Deck-Building = Cartes avec affinites emotionnelles qui se combinent
- Dice Progression + Roguelike = Chaque run avec des builds uniques
- Doppelganger + Combat Tactique = Controler 2 unites avec des roles differents

---

## ANNEXE: Chemins des Fichiers Cles

```
# Emotions System
/Scattered/Assets/Scripts/Emotions.cs
/Scattered/Assets/Scripts/BattleAction.cs

# Dice & Stats
/Epreyn_01/Assets/Scripts/Character/Character.cs
/Epreyn_01/Assets/Scripts/Character/CharacterStat.cs
/Epreyn_01/Assets/Scripts/Character/StatModifier.cs

# Doppelganger
/STD/Assets/Old_Scattered/Scripts/Neidan.cs
/STD/Assets/Old_Scattered/Scripts/Doppel.cs

# Combat Actions
/Randventure/Assets/Scripts/Abilities/BattleAction.cs
/Randventure/Assets/Scripts/Character/Character.cs

# Rhythm Game
/Rythm/Assets/Scripts/Tap.cs
/Rythm/Assets/Scripts/GameplayController.cs

# Card/Deck System
/IsoR/Assets/NueGames/NueDeck/Scripts/Card/
```

---

*Document genere automatiquement par Claude Code - Decembre 2024*
