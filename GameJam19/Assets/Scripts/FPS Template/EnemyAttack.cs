using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    [SerializeField] int damage = 1;
    [SerializeField] bool suicidal = true;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.All, damage, gameObject.name);
            if (suicidal) {
                GetComponent<EnemyHealth>().TakeDamage(50000, "");
            }
        }
    }
}
