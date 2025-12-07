using System.Collections;
using UnityEngine;

public class CombatTrigger : MonoBehaviour {
    [SerializeField] private float _distance;
    [SerializeField] private float _placementDuration;

    private CameraController _cameraController;
    private Combat _combatScript;

    private GameObject _enemyTouched;

    public bool InCombat { get; private set; }
    public GameObject CombatObject { get; private set; }

    private void Start() {
        _cameraController = GetComponent<CameraController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.CompareTag("Enemy") && !InCombat) {
            InCombat = true;
            _enemyTouched = hit.gameObject;
            StartCombat();
        }
    }

    private void StartCombat() {
        SetCombatantsControllers(false);
        InstantiateCombatObject();
        StartCoroutine(PlaceCombatants());
    }

    public void EndCombat() {
        _cameraController.DefaultCamera();
        SetCombatantsControllers(true);
        Destroy(CombatObject);
        InCombat = false;
    }

    private IEnumerator PlaceCombatants() {
        _cameraController.CombatCamera();

        var playerStartPosition = transform.position;
        var enemyStartPosition = _enemyTouched.transform.position;

        var playerToEnemy = (playerStartPosition - enemyStartPosition).normalized;
        var enemyToPlayer = (enemyStartPosition - playerStartPosition).normalized;

        var playerEndPosition = playerStartPosition + playerToEnemy * _distance;
        playerEndPosition.y = playerStartPosition.y;
        var enemyEndPosition = enemyStartPosition + enemyToPlayer * _distance;
        enemyEndPosition.y = enemyStartPosition.y;

        var t = 0f;
        while (t < _placementDuration) {
            t += Time.deltaTime;
            var progress = t / _placementDuration;
            transform.position = Vector3.Lerp(playerStartPosition, playerEndPosition, progress);
            _enemyTouched.transform.position = Vector3.Lerp(enemyStartPosition, enemyEndPosition, progress);
            yield return null;
        }
    }

    private void SetCombatantsControllers(bool enabled) {
        GetComponent<PlayerController>().enabled = enabled;
        _enemyTouched.GetComponent<EnemyController>().enabled = enabled;
    }

    private void InstantiateCombatObject() {
        CombatObject = Instantiate(Resources.Load<GameObject>("Combat"));
        CombatObject.transform.position =
            transform.position + (_enemyTouched.transform.position - transform.position) / 2;
        _combatScript = CombatObject.GetComponent<Combat>();
        _combatScript.SetCombatants(gameObject, _enemyTouched);
    }
}
