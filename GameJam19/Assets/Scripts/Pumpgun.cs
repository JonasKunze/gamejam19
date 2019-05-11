using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpgun : MonoBehaviour
{

    public GameObject waterBeam;
    public GameObject spawnPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        GameObject obj = Instantiate(waterBeam, spawnPosition.transform.position, spawnPosition.transform.rotation);
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        ps.Play();
    }
}
