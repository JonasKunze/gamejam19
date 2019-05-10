using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpd : Photon.MonoBehaviour {

	public Weapon weapon,lastweapon;

	public Transform onlineWeapons,localWeapons;

	// Use this for initialization
	void Start () {
		PhotonView view = GetComponent<PhotonView>();
		if(view && !view.isMine) this.enabled=false;
		if(weapon==null)this.enabled=false;
	}
	
	// Update is called once per frame
	void Update () {
		if(weapon==null)return;
		int id = weapon.id;
		foreach(Transform child in onlineWeapons) if(child.gameObject.activeSelf)child.gameObject.SetActive(false);
		foreach(Transform child in localWeapons) if(child.gameObject.activeSelf) child.gameObject.SetActive(false);
		if (localWeapons.childCount>id)localWeapons.GetChild(id).gameObject.SetActive(true);
		if (onlineWeapons.childCount>id)onlineWeapons.GetChild(id).gameObject.SetActive(true);
	}
}
