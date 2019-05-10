using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Photon.MonoBehaviour {


	public static GameObject[] players;
	public static Transform[] playerTf;
    [SerializeField] Vector3 spawnpoint;
	[SerializeField] bool spawn = false;
	[SerializeField] bool spawnK = false;

    public static Queue<GameObject> deadPepegas;

    Transform pepegaParent;

    int enemyPhotonIds = 100;

	// Use this for initialization
	void Start () {
        UpdatePlayers();
        deadPepegas = new Queue<GameObject>();
        pepegaParent = new GameObject("Pepegas").transform;
        //InvokeRepeating("PepegaTest", 0f, 7f);
        InvokeRepeating("SpawnRandomEnemy", 10f, 20f);
	}

    void SpawnRandomEnemy() {
        UpdatePlayers();
        if (playerTf.Length < NetworkManager.desiredPlayers) {
            return;
        }
        Transform p = playerTf[Random.Range(0, playerTf.Length - 1)];
        if (Random.value < 0.7f) {
            StartCoroutine(SpawnSwarm("Pepega", 25, new Vector3(p.position.x + Random.Range(-125f, 125f), p.position.y, p.position.z + Random.Range(-25f, 25f))));
        } else {
            StartCoroutine(SpawnSwarm("Knuckles", 9, new Vector3(p.position.x + Random.Range(-125f, 125f), p.position.y, p.position.z + Random.Range(-25f, 25f))));
        }
    }

    IEnumerator SpawnSwarm(string name, int count, Vector3 spawnpoint) {
        for (int i = 0; i < count; i++) {
            photonView.RPC("SpawnEnemy", PhotonTargets.All, name, spawnpoint);
            yield return new WaitForSeconds(1f / count);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (spawn) {
			SpawnSingleEnemy ("Pepega");
			spawn = false;
		}
		if (spawnK) {
			GameObject temp = (GameObject)Instantiate(Resources.Load("Enemies/Knuckles"), spawnpoint, Quaternion.identity);
			spawnK = false;
		}
	}

    void PepegaTest() {
        SpawnSingleEnemy("Pepega");
    }

	void SpawnSingleEnemy(string name) {
		if (PhotonNetwork.playerList.Length > 0) {
			photonView.RPC ("SpawnEnemy", PhotonTargets.All, name, spawnpoint);
		}
	}

	[PunRPC]
	void SpawnEnemy(string obj, Vector3 pos) {
        switch (obj) {
            case "Pepega":
                if (deadPepegas.Count > 0) {
                    GameObject spawn = deadPepegas.Dequeue();
                    spawn.transform.position = pos;
                    spawn.SetActive(true);
                    return;
                }
                break;
            default:
                break;
        }
        GameObject temp = (GameObject)Instantiate(Resources.Load("Enemies/" + obj), pos, Quaternion.identity, pepegaParent);
        temp.GetComponent<PhotonView>().viewID = enemyPhotonIds++;
        if (enemyPhotonIds>998) {
            enemyPhotonIds = 100;
        }
	}

	public static void UpdatePlayers() {
		players = GameObject.FindGameObjectsWithTag ("Player");
		playerTf = new Transform[players.Length];
		for (int i = 0; i < players.Length; i++) {
			playerTf [i] = players [i].GetComponent<Transform> ();
		}
	}
}