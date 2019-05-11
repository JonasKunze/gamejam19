using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        WaveMesh.Instance().Splash(other.transform.position, 1);
    }
}
