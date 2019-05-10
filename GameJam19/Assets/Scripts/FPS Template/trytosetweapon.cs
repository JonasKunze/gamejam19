using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trytosetweapon : Photon.PunBehaviour {


	public Weapon weapon;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void trytosetweap(Weapon w){
		if(photonView.isMine) weapon.setWeapon(w);
	}
}
