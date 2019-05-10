using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnemySpawner: MonoBehaviour
    {
        [SerializeField] private Transform[] wayPoints;
        [SerializeField] private List<GameObject> enemies;

        private void Start()
        {
            foreach (var prefab in enemies)
            {
                Enemy.Create(prefab, wayPoints);
            }
        }
    }
}