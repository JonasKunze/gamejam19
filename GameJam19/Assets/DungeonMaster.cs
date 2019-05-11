using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DefaultNamespace;
using UnityEngine;

public class DungeonMaster : MonoBehaviour
{
    public static DungeonMaster Instance;
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
    [SerializeField] private uint TimeWaveDuration;
    [SerializeField] private uint TimePause;
    private int timer;
    private int currentNumberOfEnemies;


    private void Awake()
    {
        gameState = GameState.Start;
        currentWave = 0;
        currentNumberOfEnemies = 0;
    }

    IEnumerator CORStartNewWave(uint TimeStart)
    {
        yield return new WaitForSeconds(TimeStart);
        StartNewWave();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CORStartNewWave(TimeStart));
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
                spawner.StartSpawning(TimeWaveDuration);
            }
        }
    }

    private void EndCurrentWave()
    {
        gameState = GameState.Idle;
    }

    public void RegisterEnemyBorn()
    {
        ++currentNumberOfEnemies;
    }
    
    public void RegisterEnemyKilled()
    {
        --currentNumberOfEnemies;
        if (currentNumberOfEnemies <= 0)
        {
            EndCurrentWave();
            StartCoroutine(CORStartNewWave(TimePause));
        }
    }
}