using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;

public class textsetter : MonoBehaviour {
    public Text text;
	//public TextMeshPro tmp;
    public FloatVar valuetolisten;
	float lastval=-1;
    public string suffix;
	public bool StartValMinusVal,runinStartOnly;
	float startval;


	// Use this for initialization
	void Start () {
		startval=valuetolisten.value;
		if(runinStartOnly)updatetext();
	}
	
	// Update is called once per frame
	void Update () {
		if(runinStartOnly) return;
		updatetext();
	}
	void updatetext(){
		if(valuetolisten.value == lastval) return;

		float val=StartValMinusVal? (startval-valuetolisten.value): valuetolisten.value;
		string s= val.ToString("0")+suffix;
		if(text) text.text = s;
		//if(tmp) tmp.text = s;
		lastval=val;
	}
}
