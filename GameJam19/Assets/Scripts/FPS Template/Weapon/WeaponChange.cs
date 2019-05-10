using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : Photon.MonoBehaviour {

	public Weapon weapon;
	public WeaponSystem weaponSystem;
	public bool cheatallweapons=true;

	public bool on;
	// Use this for initialization
	void Start () {
		on=false;
		if(!photonView.isMine) this.enabled=false;
		if (cheatallweapons) on = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKeyDown("o")) on=true;
		if(!on)return;

		if(Input.GetKeyDown("1")){
			weapon.id=0;
			weapon.setWeapon(weaponSystem.allweapons[weapon.id]);
		} else if(Input.GetKeyDown("2")){
			weapon.id=1;
			weapon.setWeapon(weaponSystem.allweapons[weapon.id]);
		} else if(Input.GetKeyDown("3")){
			weapon.id=2;
			weapon.setWeapon(weaponSystem.allweapons[weapon.id]);
		}else if(Input.GetKeyDown("4")){
			weapon.id=3;
			weapon.setWeapon(weaponSystem.allweapons[weapon.id]);
		}else if(Input.GetKeyDown("5")){
			weapon.id=3;
			weapon.setWeapon(weaponSystem.allweapons[weapon.id]);
		}
		//scroll
		if(Input.mouseScrollDelta.y<0){
			if((weapon.id+1) >= weaponSystem.allweapons.Length) return;
			else {weapon.id++;weapon.setWeapon(weaponSystem.allweapons[weapon.id]); }
		}
		else if(Input.mouseScrollDelta.y>0){
			if(weapon.id <=0) return;
			else {weapon.id--; weapon.setWeapon(weaponSystem.allweapons[weapon.id]);}
		}
	}
}
