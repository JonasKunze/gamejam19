using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomChildEnabler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.GetChild(Random.Range(0,transform.childCount)).gameObject.SetActive(true);
	}
}
