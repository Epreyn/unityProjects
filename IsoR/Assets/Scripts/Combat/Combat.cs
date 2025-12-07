using UnityEngine;

public class Combat : MonoBehaviour {
    private GameObject _enemy;
    private GameObject _player;

    public void SetCombatants(GameObject player, GameObject enemy) {
        _player = player;
        _enemy = enemy;
    }

    public void EndCombat() {
        _player.GetComponent<CombatTrigger>().EndCombat();

        Destroy(gameObject);
    }
}
