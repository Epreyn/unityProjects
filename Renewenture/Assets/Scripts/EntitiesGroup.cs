using System.Collections.Generic;
using UnityEngine;

public class EntitiesGroup : MonoBehaviour {
    [SerializeField] private List<GameObject> cards = new ();
    
    private bool isAnimating;
    
    private GameSession _gameSession;

    private void Start() {
        _gameSession = FindObjectOfType<GameSession>();

        if (_gameSession == null) {
            Debug.LogError("GameSession object not found");
        }
        else
        {
            var startStateAnimation = 0;
            _gameSession.playerGroup.ForEach(entity => {
                var card = Instantiate(Resources.Load<GameObject>("Prefabs/Card"), transform);
                card.GetComponentInChildren<Entity>().entityClass = entity.entityClass;
                card.GetComponent<Animator>().SetInteger("StartState", startStateAnimation);
                cards.Add(card);
                startStateAnimation++;
            });
        }
    }

    public void MoveNext() {
        if(isAnimating)
            return;

        isAnimating = true;
        foreach (var card in cards)
        {
            card.GetComponent<Animator>().SetTrigger("Next");
        }
        Invoke("AnimationFinished", 1f);
    }
    
    public void MovePrevious() {
        if(isAnimating)
            return;

        isAnimating = true;
        foreach (var card in cards)
        {
            card.GetComponent<Animator>().SetTrigger("Previous");
        }
        Invoke("AnimationFinished", 1f);
    }


    private void AnimationFinished() {
        isAnimating = false;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.RightArrow))
            MoveNext();
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            MovePrevious();
    }
}