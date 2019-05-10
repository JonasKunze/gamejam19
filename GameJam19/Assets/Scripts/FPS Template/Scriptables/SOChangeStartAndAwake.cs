using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOChangeStartAndAwake : MonoBehaviour {

public bool awake=true,start,addNotSet=true;
public FloatVar[] change,To;

private void Awake() {
	if(!awake || change.Length!=To.Length)return; //error
	if(change==null || To==null) return;

	int i=0;
	foreach(FloatVar t in To){
		change[i].value = addNotSet ? 
		change[i].value +t.value : t.value;
		i++;
	}
			
}
	// Use this for initialization
	void Start () {
		if(!start || change.Length!=To.Length)return; //error
	if(change==null || To==null) return;

	int i=0;
	foreach(FloatVar t in To){
		change[i].value = addNotSet ? 
		change[i].value +t.value : t.value;
		i++;
	}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
