using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class GameSession : MonoBehaviour
{
    [Title("Data")]
    
    public List<EntityClass> playerClasses;
    public List<Entity> playerGroup;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        var objs = FindObjectsOfType<GameSession>();
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        if (playerGroup == null || playerGroup.Count == 0)
        {
            playerGroup = new List<Entity>();
            for (var i = 0; i < 3; i++)
            {
                var newEntity = new Entity();
                var randomClass = playerClasses[Random.Range(0, playerClasses.Count)];
                newEntity.entityClass = randomClass;
                newEntity.Initialization();
                playerGroup.Add(newEntity);
            }
        }
    }
}