using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class networkenabler : Photon.MonoBehaviour {

	public MonoBehaviour[]scriptsToEnable;
	// Use this for initialization
	void Start () {
		if(photonView.isMine){
			foreach(MonoBehaviour script in scriptsToEnable) script.enabled=true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
