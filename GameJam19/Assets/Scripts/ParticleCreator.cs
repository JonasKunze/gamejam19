using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ParticleCreator : MonoBehaviour
{
    public GameObject waterParticles;
    public GameObject smallWaterParticles;

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

    public void Splash(Vector3 position, float intensity)
    {
        if (intensity < 0.5f)
        {
            Instantiate(smallWaterParticles, position, waterParticles.transform.rotation);
        }
        else
        {
            Instantiate(waterParticles, position, waterParticles.transform.rotation);
        }
    }
}
