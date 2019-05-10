using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteractable : Interactable {

    float timer = 0f;
    [SerializeField] float coolDown;
    int growth = 1;

    public override void Interact(GameObject actor) {
        if (Time.time < timer + coolDown) {
            return;
        }
        actor.GetComponent<ConvictionController>().GenerateConviction(true, 15 + 3 * growth);
        transform.localScale *= 1.2f;
        timer = Time.time;
        growth++;
    }
}
