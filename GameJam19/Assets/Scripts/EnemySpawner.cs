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
        [SerializeField] private float delay = 0;

        IEnumerator Spawning(float waveDuration)
        {
            yield return new WaitForSeconds(delay);
            float tMicroWaves = waveDuration / (nMicroWaves + 1);
            for (uint i = 0; i < nMicroWaves - 1; ++i){
                foreach (var prefab in enemies)
                {
                    Enemy.Create(prefab, wayPoints);
                }
                yield return new WaitForSeconds(tMicroWaves);
            }
            // Last microWave
            foreach (var prefab in enemies)
            {
                Enemy.Create(prefab, wayPoints);
            }
            DungeonMaster.Instance.RegisterSpawnerIsDone();
            
        }

        public void StartSpawning(uint waveDuration)
        {
            StartCoroutine(Spawning(waveDuration));
        }
    }
}