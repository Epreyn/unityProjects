using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : Button {
    public bool PreventPropagationOnLongPress;

    public override void OnPointerClick(PointerEventData eventData) {
        var longPress = GetComponent<LongPressButton>();
        if (longPress == null) {
            base.OnPointerClick(eventData);
        }
        if(!longPress.WasLongPressTriggered) {
            base.OnPointerClick(eventData);
        }
    }
}