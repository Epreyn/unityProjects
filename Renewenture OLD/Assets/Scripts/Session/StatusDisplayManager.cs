using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatusDisplayManager : MonoBehaviour {
    [SerializeField] private GameObject statusPrefab;

    public void CreateStatusDisplay(ActiveStatus activeStatus, Transform statusContainer) {
        var statusObject = Instantiate(statusPrefab, statusContainer);
        var textMeshPro = statusObject.GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = activeStatus.RemainingTurns.ToString();

        var image = statusObject.GetComponent<Image>();
        image.color = activeStatus.Type switch {
            BattleAction.StatusType.Burn => Color.red,
            BattleAction.StatusType.Stun => Color.yellow,
            BattleAction.StatusType.Freeze => Color.cyan,
            BattleAction.StatusType.Poison => Color.green,
            BattleAction.StatusType.Bleed => Color.magenta,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void UpdateStatusDisplay(GameObject statusObject, ActiveStatus activeStatus) {
        var textMeshPro = statusObject.GetComponentInChildren<TextMeshProUGUI>();
        textMeshPro.text = activeStatus.RemainingTurns.ToString();
    }

    public void RemoveStatusDisplay(GameObject statusObject) {
        Destroy(statusObject);
    }
}
