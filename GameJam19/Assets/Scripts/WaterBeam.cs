using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBeam : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private float lastSplashTime;
    private float waitTime = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        var poolCollider = PoolCollider.Instance();
        particleSystem.trigger.SetCollider(0, poolCollider.GetComponent<MeshCollider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnParticleCollision(GameObject other)
    {
        if (particleSystem)
        {
            if (Time.time >= lastSplashTime + waitTime)
            {
                lastSplashTime = Time.time;
                List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
                particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
                if (particles.Count > 0)
                    WaveMesh.Instance().Splash(particles[0].position, 0.3f);
            }
        }
    }
}
