using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawner : Photon.PunBehaviour {
	public Weapon weapon;
	public Transform spawnhere;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void spawn(){
		if(weapon.SpawnThisOnShot) spawnshot();
	}
	[PunRPC]
	void spawnshot(){
		PhotonNetwork.Instantiate(weapon.SpawnThisOnShot.name,spawnhere.position,spawnhere.rotation,0);
	}
}
