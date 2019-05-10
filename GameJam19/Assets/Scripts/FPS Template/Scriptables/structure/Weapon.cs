using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject {
	
	public int id;
	public float damage=20, AtkPerSec=5, range=100;
	public AudioClip[] firesounds;
	public GameObject SpawnThisOnShot;

	public void setWeapon(Weapon newWeapon){
		id=newWeapon.id;
		damage=newWeapon.damage;
		AtkPerSec=newWeapon.AtkPerSec;
		range=newWeapon.range;
		firesounds=newWeapon.firesounds;
		SpawnThisOnShot=newWeapon.SpawnThisOnShot;
	}
}
