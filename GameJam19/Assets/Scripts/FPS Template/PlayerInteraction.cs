using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : Photon.MonoBehaviour {

    [SerializeField] Transform mainPlayerCam;
    ConvictionController cc;

    private void Start() {
        if (!photonView.isMine) {
            enabled = false;
            return;
        }
        cc = GetComponent<ConvictionController>();
    }

    void Update () {
        if (Input.GetMouseButtonDown(1)) {
            RaycastHit[] hits = Physics.RaycastAll(mainPlayerCam.position, mainPlayerCam.forward, cc.level.Value * 5f);
            foreach (RaycastHit hit in hits) {
                if (hit.transform.GetComponent(typeof(Interactable))) {
                    hit.transform.GetComponent<Interactable>().Interact(gameObject);
                    return;
                }
            }
        }
	}
}
