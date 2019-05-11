using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DefaultNamespace;
using UnityEngine;

public class DungeonMaster : MonoBehaviour
{
    enum GameState
    {
        Start,
        Idle,
        ShoppingTour,
        Wave
    };


    [SerializeField] private uint NumberOfWaves;
    [SerializeField] private EnemySpawner[] EnemySpawners;
    [SerializeField] private Shop shop;
    private uint currentWave;
    private GameState gameState;
    [SerializeField] private uint TimeStart;
    private int timer;


    private void Awake()
    {
        gameState = GameState.Start;
        currentWave = 0;
    }

    IEnumerator StartFirstWave(uint TimeStart)
    {
        yield return new WaitForSeconds(TimeStart);
        StartNewWave();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartFirstWave(TimeStart));
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void EnterShop()
    {
        gameState = GameState.ShoppingTour;
    }

    private void StartNewWave()
    {
        if (currentWave < NumberOfWaves)
        {
            currentWave++;
            Debug.Log("Wave " + currentWave + " started");
            gameState = GameState.Wave;

            foreach (EnemySpawner spawner in EnemySpawners)
            {
                spawner.StartSpawning();
            }
        }
    }

    private void EndCurrentWave()
    {
        gameState = GameState.Idle;
    }
}