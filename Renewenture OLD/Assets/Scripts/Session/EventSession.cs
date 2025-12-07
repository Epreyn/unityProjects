using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Events {
    BattleLow,
    BattleMedium,
    BattleHigh
}

public class EventSession : MonoBehaviour {
    public GameSession currentGameSession;

    [Title("Game Objects")] public GameObject playerGroupGameObject;
    public GameObject cardPrefab;

    [Title("Positions")] public Vector3[] playerCardPositions;

    [Title("Events")] public List<Events> events;

    private void Start() {
        var gameSession = FindObjectsOfType<GameSession>();
        currentGameSession = gameSession[0];

        CreatePlayerGroup();
        CreateEvents();
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

    private void CreateEvents() {
        events.Add((Events)Random.Range(0, Enum.GetValues(typeof(Events)).Length));
        events.Add((Events)Random.Range(0, Enum.GetValues(typeof(Events)).Length));
    }
}
