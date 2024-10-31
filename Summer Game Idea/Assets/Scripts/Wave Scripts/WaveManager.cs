using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Wave
{
    public List<EnemyPoolInfo> PoolInfo;
    public int NumberOfEnemies;
    public float SpawnRate;
}

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private int nextWave = 0;

    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float waveCountdown = 0;

    [SerializeField] private SpawnState spawnerState = SpawnState.STANDBY;

    [SerializeField] private float totalEnemiesInWave;

    public float WaveProgressPercent
    {
        get
        {
            return totalKilledEnemies / totalEnemiesInWave;
        }
    }

    public bool IsWaveRunning;

    //public bool spawnNextWave;

    private bool AreEnemiesAlive
    {
        get
        {
            return currentSpawnedEnemies > 0;
        }
    }

    [SerializeField]private float currentSpawnedEnemies = 0;
    private float totalSpawnedEnemies = 0;
    private float totalKilledEnemies = 0;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        //if on standby do nothing
        if (spawnerState == SpawnState.STANDBY) { return; }
        //if the wave has spawned and are waiting for all enemies to die off
        if (spawnerState == SpawnState.WAITING)
        {
            if (!AreEnemiesAlive)
            {
                //all the enemies have been killed
                IsWaveRunning = false;
                WaveCompleted();
            }
            else { return; }
        }
        //if we are in counting and the timer is at 0, spawn next wave
        if (waveCountdown <= 0 && spawnerState == SpawnState.COUNTING) 
        {
            StartCoroutine(SpawnWave(waves[nextWave]));
        }
        //if we are in COUNTING but the timer is not at 0
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    public void StartCounting()
    {
        spawnerState = SpawnState.COUNTING;
        //setting the spawner state to counting starts the process for the wave
        IsWaveRunning = true;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (EnemyPoolInfo enemyType in wave.PoolInfo) { wave.NumberOfEnemies += enemyType.AmountToPool; }

        spawnerState = SpawnState.SPAWNING;

        //initalize the enemies for the wave
        EnemySpawningPool.Instance.InitializeEnemyQueue(wave.PoolInfo);
        totalEnemiesInWave = wave.NumberOfEnemies;

        //set the enemies to active throughout the wave
        for (int i = 0; i < wave.NumberOfEnemies; i++)
        {
            EnemySpawningPool.Instance.SpawnNextEnemy();
            EnemySpawned();

            yield return new WaitForSeconds(1 / wave.SpawnRate);
        }

        //all enemies are active
        spawnerState = SpawnState.WAITING;

        yield break;
    }

    private void WaveCompleted()
    {
        Debug.Log("wave completed");

        waves[nextWave].NumberOfEnemies = 0;

        spawnerState = SpawnState.STANDBY;
        waveCountdown = timeBetweenWaves;

        nextWave = (nextWave + 1) % waves.Length;

        totalKilledEnemies = 0;
        totalSpawnedEnemies = 0;
        currentSpawnedEnemies = 0;
    }

    public void EnemySpawned()
    {
        totalSpawnedEnemies++;
        currentSpawnedEnemies++;
    }

    public void EnemyRemoved()
    {
        currentSpawnedEnemies--;
        totalKilledEnemies++;
    }
}
