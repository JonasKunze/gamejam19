using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBeam : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private uint nHitsForSplash = 15;
    private uint currentHits = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnParticleCollision(GameObject other)
    {
        if (particleSystem)
        {
            currentHits++;
            if (currentHits >= nHitsForSplash)
            {
                currentHits = 0;
                List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
                particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
                if (particles.Count > 0)
                    WaveMesh.Instance().Splash(particles[0].position, 1);
            }
        }
    }
}
