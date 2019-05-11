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
    private int currentActiveSpawners;
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
        Instance = this;
        gameState = GameState.Start;
        currentWave = 0;
        currentNumberOfEnemies = 0;
        currentActiveSpawners = 0;
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

            currentActiveSpawners = EnemySpawners.Length;

        }
    }

    private void EndCurrentWave()
    {
        currentNumberOfEnemies = 0;
        currentActiveSpawners = 0;
        gameState = GameState.Idle;
        Shop.OpenShopCanvas();
    }

    public bool IsWaveDone()
    {
        if (gameState != GameState.Wave) return true;

        return ((currentActiveSpawners <= 0) && (currentNumberOfEnemies <= 0));
    }

    public void RegisterEnemyBorn()
    {
        ++currentNumberOfEnemies;
    }
    
    public void RegisterEnemyKilled()
    {
        --currentNumberOfEnemies;
        if (IsWaveDone())
        {
            EndCurrentWave();
            StartCoroutine(CORStartNewWave(TimePause));
        }
    }
    public void RegisterSpawnerIsDone()
    {
        --currentActiveSpawners;
        if (IsWaveDone())
        {
            EndCurrentWave();
            StartCoroutine(CORStartNewWave(TimePause));
        }
    }
}