using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ParticleCreator : MonoBehaviour
{
    public GameObject waterParticles;

    private static ParticleCreator instance;
    
    private void Start()
    {
        Debug.Assert(instance == null);
        instance = this;
    }

    public static ParticleCreator Instance()
    {
        return instance;
    }

    public void Splash(Vector3 position)
    {
        Instantiate(waterParticles, position, waterParticles.transform.rotation);
    }
}
