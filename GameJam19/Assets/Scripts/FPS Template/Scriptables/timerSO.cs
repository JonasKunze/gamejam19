using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class timerSO : MonoBehaviour {

    public FloatVar time;
	public float timeset;
public UnityEvent response;

	// Use this for initialization
	void Start () {
		time.value=timeset;
	}
	
	// Update is called once per frame
	void Update () {
		if(time.value>0)time.value-=Time.deltaTime;
		else {
			time.value=timeset;
			response.Invoke();
		}
	}
}

