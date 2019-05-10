using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class activechild : MonoBehaviour {
    public Transform trans;
    public IntRef valuetolisten;

    private void Start()
    {
        updatechild();
    }
    // Update is called once per frame
    void Update () {

	}
    public void updatechild()
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            if (valuetolisten.Value < 0) return;
            if (i < valuetolisten.Value) trans.GetChild(i).gameObject.SetActive(true);
            else trans.GetChild(i).gameObject.SetActive(false);
        }
    }
}
