using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class writeInSO : MonoBehaviour {


	public FloatVar current;

	public FloatVar[] floats;
	public bool onlysaveifbetter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SaveRun(){
		if(onlysaveifbetter && Time.timeSinceLevelLoad >= current.value) return;
		current.value = Time.timeSinceLevelLoad;
	}
	public void SaveRun(int index){
		current=floats[index];
		SaveRun();
	}
}
