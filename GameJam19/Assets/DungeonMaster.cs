using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Analytics;

public class DungeonMaster : MonoBehaviour
{
    public static DungeonMaster Instance;

    enum GameState
    {
        Init,
        Start,
        Wave,
        ShoppingTour,
        Victory,
        GameOver
    };


    [SerializeField] private uint NumberOfWaves;
    [SerializeField] private EnemySpawner[] EnemySpawners;
    [SerializeField] private Shop shop;
    private GameState gameState;
    [SerializeField] private uint TimeStart;
    [SerializeField] private uint TimeWaveDuration;
    [SerializeField] private uint TimePause;
    [SerializeField] private int HP = 10;
    [SerializeField] private GameObject bus;
    

    private uint currentWave;
    private int currentActiveSpawners;
    private int currentNumberOfEnemies;

    public GameObject gameoverScreen,victoryScreen;


    private void Awake()
    {
        Instance = this;
        gameState = GameState.Init;
        currentWave = 0;
        currentNumberOfEnemies = 0;
        currentActiveSpawners = 0;
    }

    void Start()
    {
        gameState = GameState.Start;

        StartCoroutine(CORStartNewWave(TimeStart));
    }

    IEnumerator CORStartNewWave(uint TimeStart)
    {
        yield return new WaitForSeconds(TimeStart);
        while (!PhotonNetwork.inRoom)
            yield return 0;
        StartNewWave();
    }

    private void StartNewWave()
    {
        gameState = GameState.Wave;
        currentNumberOfEnemies = 0;
        currentWave++;
        Debug.Log("Wave " + currentWave + " started");

        foreach (EnemySpawner spawner in EnemySpawners)
        {
            spawner.StartSpawning(TimeWaveDuration);
        }

        currentActiveSpawners = EnemySpawners.Length;
        
        Destroy(Instantiate(bus), 30);
    }

    private bool IsWaveDone()
    {
        if (gameState != GameState.Wave) return true;

        return ((currentActiveSpawners <= 0) && (currentNumberOfEnemies <= 0));
    }

    public void MakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
            RegisterPlayerDead();
    }

    public void RegisterPlayerDead()
    {
        GameOver();
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
        }
    }

    public void RegisterSpawnerIsDone()
    {
        --currentActiveSpawners;
        if (IsWaveDone())
        {
            EndCurrentWave();
        }
    }

    private void EndCurrentWave()
    {
        currentNumberOfEnemies = 0;
        currentActiveSpawners = 0;
        if (currentWave < NumberOfWaves)
        {
            StartShopppingTour();
        }
        else
        {
            Victory();
        }
    }

    private void StartShopppingTour()
    {
      //  gameState = GameState.ShoppingTour;
      //  Shop.OpenShopCanvas();
    }

    private void EndShopppingTour()
    {
        // TODO Called by canvas button
        Shop.CloseShopCanvas();
        StartNewWave();
    }

    private void Victory()
    {
        gameState = GameState.Victory;
        victoryScreen.SetActive(true);
    }

    private void GameOver()
    {
        gameState = GameState.GameOver;
        gameoverScreen.SetActive(true);
    }
}