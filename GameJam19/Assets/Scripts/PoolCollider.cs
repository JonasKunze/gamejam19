using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class PoolCollider : MonoBehaviour
{
    private static PoolCollider instance;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(instance == null);
        instance = this;
    }

    public static PoolCollider Instance()
    {
        return instance;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        WaveMesh.Instance().Splash(other.transform.position, 1);
    }

    /*private void OnParticleCollision(GameObject other)
    {
        WaveMesh.Instance().Splash(other.transform.position, 1);
    }*/
}
