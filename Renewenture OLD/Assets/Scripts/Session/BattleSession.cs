using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleSession : MonoBehaviour {
    public GameSession currentGameSession;

    public StatusDisplayManager statusDisplayManager;

    [Title("Animations")] public float moveCardDuration;

    [Title("Game Objects")] public GameObject playerGroupGameObject;
    public GameObject enemiesGroupGameObject;
    public GameObject cardPrefab;

    [Title("Players")] [ReadOnly] public Transform currentFirstCard;
    [ReadOnly] public int currentFirstCardCurrentHp;

    [Title("Positions")] public Vector3[] playerCardPositions;
    public Vector3 playerCardWaitingPositions;
    public Vector3[] enemiesCardPositions;
    public Vector3 enemiesCardWaitingPosition;

    [Title("Enemies")] [MinMaxSlider(1, 4, ShowFields = true)]
    public Vector2 rangeEnemiesCount;

    [ReadOnly] public bool isCurrentEnemyDead;
    public List<Entity> battleSessionEnemiesGroup;


    private void Start() {
        var gameSession = FindObjectsOfType<GameSession>();
        currentGameSession = gameSession[0];

        ClearPlayerGroupChildren();
        ClearEnemiesGroupChildren();
        CreatePlayerGroup();
        CreateEnemiesGroup();
        EnableBoxColliderOnFirstCard();
    }

    private void ClearPlayerGroupChildren() {
        foreach (Transform child in playerGroupGameObject.transform) Destroy(child.gameObject);
    }

    private void ClearEnemiesGroupChildren() {
        foreach (Transform child in enemiesGroupGameObject.transform) Destroy(child.gameObject);
    }

    private void CreatePlayerGroup() {
        var playerSessionGroup = currentGameSession.playerSessionGroup;

        for (var i = 0; i < playerSessionGroup.Count; i++) {
            var newCard = Instantiate(cardPrefab, playerGroupGameObject.transform);
            newCard.transform.Find("Entity").GetComponent<EntityReference>().entityReference = playerSessionGroup[i];
            playerSessionGroup[i].entityGameObject = newCard.transform.gameObject;
            newCard.transform.name = playerSessionGroup[i].className;

            newCard.transform.GetComponent<RectTransform>().localPosition = playerCardPositions[i];
            newCard.transform.SetAsFirstSibling();
        }
    }

    private void CreateEnemiesGroup() {
        var gameSessions = FindObjectsOfType<GameSession>();
        var randomEnemiesCount = Random.Range(rangeEnemiesCount.x, rangeEnemiesCount.y);

        battleSessionEnemiesGroup = new List<Entity>();
        for (var i = 0; i < randomEnemiesCount; i++) {
            var entityParent = GameObject.Find("Enemy Entities");
            var newEntity = entityParent.AddComponent<Entity>();
            var randomClass = gameSessions[0]
                .enemyClasses[Random.Range(0, gameSessions[0].enemyClasses.Count)];
            newEntity.entityClass = randomClass;
            newEntity.Initialization();
            battleSessionEnemiesGroup.Add(newEntity);

            var newCard = Instantiate(cardPrefab, enemiesGroupGameObject.transform);
            newCard.transform.Find("Entity").GetComponent<EntityReference>().entityReference =
                battleSessionEnemiesGroup[i];
            battleSessionEnemiesGroup[i].entityGameObject = newCard.transform.gameObject;
            newCard.transform.name = battleSessionEnemiesGroup[i].className;

            newCard.transform.GetComponent<RectTransform>().localPosition =
                enemiesCardPositions[i];
            newCard.transform.SetAsFirstSibling();

            newCard.transform.GetComponent<Draggable>().enabled = false;
            newCard.transform.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void DisableAllBoxColliders() {
        foreach (Transform child in playerGroupGameObject.transform) {
            var boxCollider2D = child.GetComponent<BoxCollider2D>();
            if (boxCollider2D == null) continue;
            boxCollider2D.enabled = false;
        }
    }

    private void EnableBoxColliderOnFirstCard() {
        foreach (Transform child in playerGroupGameObject.transform) {
            var boxCollider2D = child.GetComponent<BoxCollider2D>();
            if (boxCollider2D == null) continue;
            boxCollider2D.enabled = child == playerGroupGameObject.transform.GetChild(2);
        }
    }

    private void DetermineCurrentFirstCard() {
        var cards = new List<Transform>();
        for (var i = 2; i >= 0; i--)
            cards.Add(playerGroupGameObject.transform.GetChild(i));
        currentFirstCard = cards.First();
        var entityRef = currentFirstCard.transform.Find("Entity").GetComponent<EntityReference>().entityReference;
        currentFirstCardCurrentHp = entityRef.currentHp;
    }

    private IEnumerator ShufflePlayerCard() {
        var cards = new List<Transform>();
        for (var i = 2; i >= 0; i--)
            cards.Add(playerGroupGameObject.transform.GetChild(i));


        float elapsedTime = 0;
        var firstCard = cards.First();
        var firstCardStartPosition = firstCard.transform.localPosition;
        while (elapsedTime < moveCardDuration) {
            firstCard.transform.localPosition = Vector3.Lerp(firstCardStartPosition,
                playerCardWaitingPositions,
                elapsedTime / moveCardDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        firstCard.SetAsFirstSibling();
        cards.Remove(firstCard);
        cards.Insert(0, firstCard);

        var elapsedTimes = new float[cards.Count];
        var startPositions = new Vector3[cards.Count];
        var endPositions = new Vector3[cards.Count];
        for (var i = 0; i < cards.Count; i++) {
            var card = cards[i];
            startPositions[i] = card.transform.localPosition;
            var futureIndex = i == 0 ? 2 : i - 1;
            endPositions[i] = playerCardPositions[futureIndex];
        }

        while (elapsedTimes.Any(t => t < moveCardDuration)) {
            for (var i = 0; i < cards.Count; i++)
                if (elapsedTimes[i] < moveCardDuration) {
                    cards[i].transform.localPosition =
                        Vector3.Lerp(startPositions[i], endPositions[i], elapsedTimes[i] / moveCardDuration);
                    elapsedTimes[i] += Time.deltaTime;
                }

            yield return null;
        }
    }

    private IEnumerator ShuffleEnemiesCard() {
        if (battleSessionEnemiesGroup.Count <= 1) yield break;

        var cards = new List<Transform>();
        for (var i = battleSessionEnemiesGroup.Count - 1; i >= 0; i--)
            cards.Add(enemiesGroupGameObject.transform.GetChild(i));


        float elapsedTime = 0;
        var firstCard = cards.First();
        var firstCardStartPosition = firstCard.transform.localPosition;
        while (elapsedTime < moveCardDuration) {
            firstCard.transform.localPosition = Vector3.Lerp(firstCardStartPosition, enemiesCardWaitingPosition,
                elapsedTime / moveCardDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        firstCard.SetAsFirstSibling();
        cards.Remove(firstCard);
        cards.Insert(cards.Count, firstCard);


        var elapsedTimes = new float[cards.Count];
        var startPositions = new Vector3[cards.Count];
        var endPositions = new Vector3[cards.Count];
        for (var i = 0; i < cards.Count; i++) {
            var card = cards[i];
            startPositions[i] = card.transform.localPosition;
            var futureIndex = i - 1 < 0 ? 0 : i;
            endPositions[i] = enemiesCardPositions[futureIndex];
        }

        while (elapsedTimes.Any(t => t < moveCardDuration)) {
            for (var i = 0; i < cards.Count; i++)
                if (elapsedTimes[i] < moveCardDuration) {
                    cards[i].transform.localPosition =
                        Vector3.Lerp(startPositions[i], endPositions[i], elapsedTimes[i] / moveCardDuration);
                    elapsedTimes[i] += Time.deltaTime;
                }

            yield return null;
        }
    }

    private IEnumerator CheckEnemiesDeath() {
        var lastEnemy = enemiesGroupGameObject.transform.GetChild(enemiesGroupGameObject.transform.childCount - 1)
            .Find("Entity").GetComponent<EntityReference>().entityReference;
        if (lastEnemy.currentHp <= 0) isCurrentEnemyDead = true;

        var enemiesToRemove = battleSessionEnemiesGroup.Where(enemy => enemy.currentHp <= 0).ToList();

        foreach (var enemyToRemove in enemiesToRemove) {
            battleSessionEnemiesGroup.Remove(enemyToRemove);
            Destroy(enemyToRemove.entityGameObject);
            Destroy(enemyToRemove);
        }

        yield return null;
    }

    private IEnumerator RearrangeEnemiesCards() {
        if (battleSessionEnemiesGroup.Count == 0) yield break;

        var cards = new List<Transform>();
        for (var i = battleSessionEnemiesGroup.Count - 1; i >= 0; i--)
            cards.Add(enemiesGroupGameObject.transform.GetChild(i));

        var elapsedTimes = new float[cards.Count];
        var startPositions = new Vector3[cards.Count];
        var endPositions = new Vector3[cards.Count];

        for (var i = 0; i < cards.Count; i++) {
            startPositions[i] = cards[i].transform.localPosition;
            var futureIndex = i < enemiesCardPositions.Length ? i : enemiesCardPositions.Length - 1;
            endPositions[i] = enemiesCardPositions[futureIndex];
        }

        while (elapsedTimes.Any(t => t < moveCardDuration)) {
            for (var i = 0; i < cards.Count; i++)
                if (elapsedTimes[i] < moveCardDuration) {
                    cards[i].transform.localPosition =
                        Vector3.Lerp(startPositions[i], endPositions[i], elapsedTimes[i] / moveCardDuration);
                    elapsedTimes[i] += Time.deltaTime;
                }

            yield return null;
        }
    }

    private IEnumerator ResolveAttack(Entity currentEntity, Transform currentGroup, Transform opponentGroup,
        bool isSpellALaunched, bool isPlayer = true) {
        var spellLaunched = isPlayer
            ? currentEntity.spells[isSpellALaunched ? 0 : 1]
            : currentEntity.spells[Random.Range(0, 2)];
        var spellTargets = new List<Entity>();

        foreach (var ba in spellLaunched.battleActions) {
            spellTargets = DetermineTargets(ba, currentEntity, currentGroup, opponentGroup, spellTargets);

            foreach (var target in spellTargets) {
                var actionValue = ComputeBaseValueForAction(ba, currentEntity);
                yield return StartCoroutine(ExecuteAction(ba, target, actionValue));
            }
        }

        yield return null;
    }

    private IEnumerator ResolvePlayerAttack(bool isSpellALaunched) {
        var currentPlayerEntity = GetLastEntityFromGroup(playerGroupGameObject.transform);
        yield return ApplyStatusDamages(currentPlayerEntity);

        if (currentPlayerEntity.activeStatuses.Any(s => s.Type == BattleAction.StatusType.Stun)) yield break;

        yield return StartCoroutine(ResolveAttack(currentPlayerEntity, playerGroupGameObject.transform,
            enemiesGroupGameObject.transform, isSpellALaunched));
    }

    private IEnumerator ResolveEnemyAttack() {
        if (isCurrentEnemyDead) yield break;

        var currentEnemyEntity = GetLastEntityFromGroup(enemiesGroupGameObject.transform);
        yield return ApplyStatusDamages(currentEnemyEntity);

        if (currentEnemyEntity.activeStatuses.Any(s => s.Type == BattleAction.StatusType.Stun)) yield break;

        yield return StartCoroutine(ResolveAttack(currentEnemyEntity, enemiesGroupGameObject.transform,
            playerGroupGameObject.transform, false, false));
    }


    private List<Entity> DetermineTargets(BattleAction ba, Entity currentEntity, Transform currentGroup,
        Transform opponentGroup, List<Entity> spellTargets) {
        spellTargets.Clear();
        switch (ba.targetType) {
            case BattleAction.TargetType.MonoTarget:
                var target = DetermineMonoTarget(ba, currentEntity, currentGroup, opponentGroup);
                if (target != null) spellTargets.Add(target);
                break;
            case BattleAction.TargetType.MultiTarget:
                spellTargets.AddRange(DetermineMultiTargets(ba, currentGroup, opponentGroup));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return spellTargets;
    }

    private Entity DetermineMonoTarget(BattleAction battleAction, Entity currentEntity, Transform currentGroup,
        Transform opponentGroup) {
        return battleAction.monoTargetType switch {
            BattleAction.MonoTargetType.Self => currentEntity,
            BattleAction.MonoTargetType.FirstOpponent => GetLastEntityFromGroup(opponentGroup),
            BattleAction.MonoTargetType.LastOpponent => GetFirstEntityFromGroup(opponentGroup),
            BattleAction.MonoTargetType.RandomAlly => GetRandomEntityFromGroup(currentGroup),
            BattleAction.MonoTargetType.RandomOpponent => GetRandomEntityFromGroup(opponentGroup),
            BattleAction.MonoTargetType.RandomAllyWithoutSelf => GetRandomEntityFormGroupExceptLast(currentGroup),
            BattleAction.MonoTargetType.RandomOpponentWithoutFirstOpponent => GetRandomEntityFormGroupExceptLast(
                opponentGroup),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private List<Entity> DetermineMultiTargets(BattleAction battleAction, Transform currentGroup,
        Transform opponentGroup) {
        return battleAction.multiTargetType switch {
            BattleAction.MultiTargetType.AllAllies => GetAllEntitiesFromGroup(currentGroup),
            BattleAction.MultiTargetType.AllOpponents => GetAllEntitiesFromGroup(opponentGroup),
            BattleAction.MultiTargetType.AllAlliesWithoutSelf => GetAllEntitiesFromGroupExceptLast(currentGroup),
            BattleAction.MultiTargetType.AllOpponentsWithoutFirstOpponent => GetAllEntitiesFromGroupExceptLast(
                opponentGroup),
            BattleAction.MultiTargetType.AllEntities => GetAllEntitiesFromTwoGroups(currentGroup, opponentGroup),
            BattleAction.MultiTargetType.AllEntitiesWithoutSelf => GetAllEntitiesFromTwoGroupsExceptLast(currentGroup,
                opponentGroup),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Entity GetFirstEntityFromGroup(Transform group) {
        return group.GetChild(0).Find("Entity").GetComponent<EntityReference>().entityReference;
    }

    private Entity GetLastEntityFromGroup(Transform group) {
        return group.GetChild(group.childCount - 1).Find("Entity").GetComponent<EntityReference>().entityReference;
    }

    private Entity GetRandomEntityFromGroup(Transform group) {
        return group.GetChild(Random.Range(0, group.childCount)).Find("Entity").GetComponent<EntityReference>()
            .entityReference;
    }

    private Entity GetRandomEntityFormGroupExceptLast(Transform group) {
        return group.GetChild(Random.Range(0, group.childCount - 1)).Find("Entity").GetComponent<EntityReference>()
            .entityReference;
    }

    private List<Entity> GetAllEntitiesFromGroup(Transform group) {
        return group.GetComponentsInChildren<EntityReference>().Select(er => er.entityReference).ToList();
    }

    private List<Entity> GetAllEntitiesFromGroupExceptLast(Transform group) {
        var entities = group.GetComponentsInChildren<EntityReference>().Select(er => er.entityReference).ToList();
        entities.RemoveAt(entities.Count - 1);
        return entities;
    }

    private List<Entity> GetAllEntitiesFromTwoGroups(Transform group1, Transform group2) {
        var entities = group1.GetComponentsInChildren<EntityReference>().Select(er => er.entityReference).ToList();
        entities.AddRange(group2.GetComponentsInChildren<EntityReference>().Select(er => er.entityReference).ToList());
        return entities;
    }

    private List<Entity> GetAllEntitiesFromTwoGroupsExceptLast(Transform group1, Transform group2) {
        var entities = group1.GetComponentsInChildren<EntityReference>().Select(er => er.entityReference).ToList();
        entities.AddRange(group2.GetComponentsInChildren<EntityReference>().Select(er => er.entityReference).ToList());
        entities.RemoveAt(entities.Count - 1);
        return entities;
    }

    private int ComputeBaseValueForAction(BattleAction battleAction, Entity currentPlayerEntity) {
        return battleAction.actionType switch {
            BattleAction.ActionType.Damage => ComputeBaseDamageValue(battleAction, currentPlayerEntity),
            BattleAction.ActionType.Heal => ComputeBaseHealValue(battleAction, currentPlayerEntity),
            BattleAction.ActionType.Status => 0,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private int ComputeBaseDamageValue(BattleAction battleAction, Entity currentPlayerEntity) {
        return battleAction.damageType switch {
            BattleAction.DamageType.Physic => currentPlayerEntity.physicalAttack.Value,
            BattleAction.DamageType.Magic => currentPlayerEntity.magicalAttack.Value,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private int ComputeBaseHealValue(BattleAction battleAction, Entity currentPlayerEntity) {
        return battleAction.healType switch {
            BattleAction.HealType.Physic => currentPlayerEntity.physicalAttack.Value,
            BattleAction.HealType.Magic => currentPlayerEntity.magicalAttack.Value,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void TryApplyStatus(BattleAction battleAction, Entity targetEntity) {
        var roll = Random.Range(0, 101);
        if (roll <= battleAction.accuracyPercentage) ApplyStatus(battleAction, targetEntity);
    }

    private void ApplyStatus(BattleAction battleAction, Entity targetEntity) {
        var newStatus = ScriptableObject.CreateInstance<ActiveStatus>();
        newStatus.Type = battleAction.statusType;
        newStatus.RemainingTurns = battleAction.turnDuration;
        newStatus.name = battleAction.statusType.ToString();

        if (targetEntity.activeStatuses.Any(s => s.Type == battleAction.statusType)) {
            var existingStatus = targetEntity.activeStatuses.First(s => s.Type == battleAction.statusType);

            existingStatus.RemainingTurns += battleAction.turnDuration;
            statusDisplayManager.UpdateStatusDisplay(
                targetEntity.entityGameObject.transform.Find("Statuses").gameObject, existingStatus);
        }
        else {
            targetEntity.activeStatuses.Add(newStatus);
            ApplyStatusStatisticsModifiers(targetEntity);

            statusDisplayManager.CreateStatusDisplay(newStatus,
                targetEntity.entityGameObject.transform.Find("Statuses"));
        }
    }

    private void ApplyStatusStatisticsModifiers(Entity entity) {
        foreach (var status in entity.activeStatuses.ToList())
            switch (status.Type) {
                case BattleAction.StatusType.Burn:
                    var burnModifier =
                        new StatModifier(BattleAction.StatusType.Burn.ToString(),
                            -entity.physicalAttack.Value / 4,
                            StatModType.Flat);
                    entity.physicalAttack.AddModifierIfDoesntExist(burnModifier);
                    break;
                case BattleAction.StatusType.Stun:
                    // NONE
                    break;
                case BattleAction.StatusType.Freeze:
                    var freezeModifier =
                        new StatModifier(BattleAction.StatusType.Freeze.ToString(),
                            -entity.magicalAttack.Value / 2,
                            StatModType.Flat);
                    entity.magicalAttack.AddModifierIfDoesntExist(freezeModifier);
                    break;
                case BattleAction.StatusType.Poison:
                    // NONE
                    break;
                case BattleAction.StatusType.Bleed:
                    // NONE
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }

    private void TryToRemoveStatusStatisticsModifiers(Entity entity) {
        foreach (var status in entity.activeStatuses.ToList()
                     .Where(status => entity.activeStatuses.All(s => s.Type != status.Type)))
            switch (status.Type) {
                case BattleAction.StatusType.Burn:
                    entity.physicalAttack.RemoveAllModifiersFromSource(BattleAction.StatusType.Burn.ToString());
                    break;
                case BattleAction.StatusType.Stun:
                    // NONE
                    break;
                case BattleAction.StatusType.Freeze:
                    entity.magicalAttack.RemoveAllModifiersFromSource(BattleAction.StatusType.Freeze.ToString());
                    break;
                case BattleAction.StatusType.Poison:
                    // NONE
                    break;
                case BattleAction.StatusType.Bleed:
                    // NONE
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }

    private IEnumerator ApplyStatusDamages(Entity entity) {
        foreach (var status in entity.activeStatuses.ToList()) {
            switch (status.Type) {
                case BattleAction.StatusType.Burn:
                    var burnDamages = entity.hp.Value / 16;
                    yield return entity.TakeDamages(burnDamages);
                    break;
                case BattleAction.StatusType.Stun:
                    break;
                case BattleAction.StatusType.Freeze:
                    break;
                case BattleAction.StatusType.Poison:
                    var poisonDamages = status.RemainingTurns;
                    yield return entity.TakeDamages(poisonDamages);
                    break;
                case BattleAction.StatusType.Bleed:
                    var bleedDamages = entity.hp.Value / 8;
                    yield return entity.TakeDamages(bleedDamages);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            status.RemainingTurns--;

            statusDisplayManager.UpdateStatusDisplay(entity.entityGameObject.transform.Find("Statuses").gameObject,
                status);

            if (status.RemainingTurns > 0) continue;
            entity.activeStatuses.Remove(status);
            var statusGameObjectToRemove = entity.entityGameObject.transform.Find("Statuses")
                .Find(status.Type.ToString()).gameObject;
            TryToRemoveStatusStatisticsModifiers(entity);
            statusDisplayManager.RemoveStatusDisplay(statusGameObjectToRemove);
        }

        yield return null;
    }

    private IEnumerator ExecuteAction(BattleAction ba, Entity target, int actionValue) {
        switch (ba.actionType) {
            case BattleAction.ActionType.Damage:
                yield return ba.damageResistance switch {
                    BattleAction.DamageResistance.Physic => StartCoroutine(target.TakePhysicalDamages(actionValue)),
                    BattleAction.DamageResistance.Magic => StartCoroutine(target.TakeMagicalDamages(actionValue)),
                    BattleAction.DamageResistance.None => null,
                    _ => throw new ArgumentOutOfRangeException()
                };

                break;
            case BattleAction.ActionType.Heal:
                if (target.currentHp > 0) yield return StartCoroutine(target.TakeHeal(actionValue));
                break;
            case BattleAction.ActionType.Status:
                TryApplyStatus(ba, target);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public IEnumerator ResolveTurn(bool isSpellALaunched) {
        DisableAllBoxColliders();

        // TODO :  Vérifier la vitesse d'attaque de chaque entité et definir l'ordre d'attaque

        yield return StartCoroutine(ResolvePlayerAttack(isSpellALaunched));
        yield return StartCoroutine(CheckEnemiesDeath());
        yield return StartCoroutine(RearrangeEnemiesCards());
        yield return StartCoroutine(ResolveEnemyAttack());
        yield return StartCoroutine(ShufflePlayerCard());

        DetermineCurrentFirstCard();
        while (currentFirstCardCurrentHp <= 0) {
            yield return StartCoroutine(ShufflePlayerCard());
            DetermineCurrentFirstCard();
        }

        yield return StartCoroutine(ShuffleEnemiesCard());

        EnableBoxColliderOnFirstCard();

        isCurrentEnemyDead = false;

        yield return null;
    }
}
