using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeFOVbyClick : MonoBehaviour {

	public Weapon weapon;
	Camera cam;
	float startFOV,lerpto;
	public float strengh = 1f,lerpstrengh=0.4f,lerpstrengh2=0.4f;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
		startFOV=cam.fieldOfView;
		lerpto = startFOV;
	}
	
	// Update is called once per frame
	void Update () {


		lerpto=Mathf.Lerp(lerpto,startFOV,lerpstrengh);

		if(cam)cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,lerpto,lerpstrengh2);
	}
	public void kick(){
		lerpto += strengh*weapon.damage;
	}
}
