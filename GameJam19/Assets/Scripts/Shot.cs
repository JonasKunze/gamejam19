using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

	public float dmg=1,force=200;

	public ConstantForce constantForce;
	// Use this for initialization
	void Start () {
		constantForce.relativeForce=Vector3.forward*force;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
