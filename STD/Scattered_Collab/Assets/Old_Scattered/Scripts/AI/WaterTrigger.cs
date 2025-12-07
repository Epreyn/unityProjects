using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public bool Hit;
    public float HitDelay, HitTimer;

    void Update(){

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Liquids")) Hit = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Liquids")) Hit = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Liquids")) Hit = false;

        /*if (collision.gameObject.layer == LayerMask.NameToLayer("Liquids")
            && HitTimer > HitDelay) {
                Hit = false;
                HitTimer = 0;
            }
        else HitTimer += Time.deltaTime;*/
    }
}
