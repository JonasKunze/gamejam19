using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] bool canFly;

	Transform enemyTf;
	Rigidbody enemyRb;

	float dist = 0;
	int nearestPlayer;
	Transform target;

	// Use this for initialization
	void Start () {
		enemyTf = GetComponent<Transform>(); 
		enemyRb = GetComponent<Rigidbody> ();
		InvokeRepeating ("SearchTarget", 0f, 2f);
	}

	void SearchTarget () {

		for (int i = 0; i < EnemyManager.playerTf.Length; i++) {
			if (dist == 0) {
				dist = (Mathf.Pow ((EnemyManager.playerTf[i].position.x - enemyTf.position.x), 2f) + Mathf.Pow ((EnemyManager.playerTf[i].position.z - enemyTf.position.z), 2f));
				nearestPlayer = i;
			} else if (dist > (Mathf.Pow ((EnemyManager.playerTf[i].position.x - enemyTf.position.x), 2f) + Mathf.Pow ((EnemyManager.playerTf[i].position.z - enemyTf.position.z), 2f))) {
				dist = (Mathf.Pow ((EnemyManager.playerTf[i].position.x - enemyTf.position.x), 2f) + Mathf.Pow ((EnemyManager.playerTf[i].position.z - enemyTf.position.z), 2f));
				nearestPlayer = i;
			}
		}

		target = EnemyManager.playerTf [nearestPlayer];
	}

	void FixedUpdate() {
	
		if (target == null) {
			EnemyManager.UpdatePlayers();
			SearchTarget ();
		}

        enemyRb.AddForce((target.position.x - enemyTf.position.x), canFly ? (target.position.y + 0.75f * target.localScale.y - enemyTf.position.y) : 0f, (target.position.z - enemyTf.position.z));
	
	}

}