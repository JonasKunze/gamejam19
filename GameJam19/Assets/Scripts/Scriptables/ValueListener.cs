using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ValueListener : MonoBehaviour {

	public FloatVar goal;
	public float firewhenunderorequal=0;
	public bool listening=true;
    public UnityEvent Response;

	// Use this for initialization
	void Start () {

 		
	}
	
	// Update is called once per frame
	void Update () {
		if(!listening)return; //|| (fvars==null && intvars==null)

		if(allvalid){
			listening=false;
			Response.Invoke();
		}
	}
	bool allvalid{
		get{
			return goal.value<=firewhenunderorequal;
		}
	}
}
