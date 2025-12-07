using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class GameSession : MonoBehaviour {
    [Title("Data")] public List<EntityClass> playerClasses;
    public List<EntityClass> enemyClasses;

    [Title("Session")] public List<Entity> playerSessionGroup;


    private void Awake() {
        DontDestroyOnLoad(gameObject);

        var objs = FindObjectsOfType<GameSession>();
        if (objs.Length > 1) Destroy(gameObject);

        if (playerSessionGroup != null && playerSessionGroup.Count != 0) return;
        playerSessionGroup = new List<Entity>();
        for (var i = 0; i < 3; i++) {
            var entityParent = GameObject.Find("Player Entities");
            var newEntity = entityParent.AddComponent<Entity>();
            //var randomClass = playerClasses[i];
            var randomClass = playerClasses[Random.Range(0, playerClasses.Count)];
            newEntity.entityClass = randomClass;
            newEntity.Initialization();
            playerSessionGroup.Add(newEntity);
        }
    }
}
