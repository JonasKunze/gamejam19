using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace DefaultNamespace
{
    public class EnemySpawner: MonoBehaviour
    {
        [SerializeField] private WayPoint[] wayPoints;
        [SerializeField] private List<GameObject> enemies;
        [SerializeField] private uint nMicroWaves;
        [SerializeField] private uint tMicroWaves;

        IEnumerator Spawning()
        {
            for (uint i = 0; i < nMicroWaves; ++i){
                foreach (var prefab in enemies)
                {
                    Enemy.Create(prefab, wayPoints);
                }
                yield return new WaitForSeconds(tMicroWaves);
            }
            
        }

        public void StartSpawning()
        {
            StartCoroutine(Spawning());
        }
    }
}