using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Shaker : MonoBehaviour {
	public Weapon weapon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ShakeCam(float Magnitude,float Roughness, float FadeOutTime){
		CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);
	}
	public void ShakeCamByWeapon(){
		CameraShaker.Instance.ShakeOnce(15+ weapon.damage *.5f, 1+weapon.damage *.2f, 0, (1f/(float)weapon.AtkPerSec));
	}
}
