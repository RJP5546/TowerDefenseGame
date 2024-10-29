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

    [SerializeField] private SpawnState spawnerState = SpawnState.COUNTING;

    [SerializeField] private float totalEnemiesInWave;

    public float WaveProgressPercent
    {
        get
        {
            return totalSpawnedEnemies / totalEnemiesInWave;
        }
    }

    public bool spawnNextWave;

    private bool AreEnemiesAlive
    {
        get
        {
            return currentSpawnedEnemies > 0;
        }
    }

    private float currentSpawnedEnemies = 0;
    private float totalSpawnedEnemies = 0;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if (spawnerState == SpawnState.WAITING)
        {
            if (!AreEnemiesAlive)
            {
                WaveProgressTracker.Instance.EndtOfWave();
                WaveCompleted();
            }
            else { return; }
        }

        if (waveCountdown <= 0 && spawnerState == SpawnState.COUNTING) 
        {
            StartCoroutine(SpawnWave(waves[nextWave]));
            WaveProgressTracker.Instance.StartOfWave();
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (EnemyPoolInfo enemyType in wave.PoolInfo) { wave.NumberOfEnemies += enemyType.AmountToPool; }

        spawnerState = SpawnState.SPAWNING;

        EnemySpawningPool.Instance.InitializeEnemyQueue(wave.PoolInfo);
        totalEnemiesInWave = wave.NumberOfEnemies;

        for (int i = 0; i <= wave.NumberOfEnemies; i++)
        {
            EnemySpawningPool.Instance.SpawnNextEnemy();
            EnemySpawned();

            yield return new WaitForSeconds(1 / wave.SpawnRate);
        }

        spawnerState = SpawnState.WAITING;

        yield break;
    }

    private void WaveCompleted()
    {
        Debug.Log("wave completed");

        waves[nextWave].NumberOfEnemies = 0;

        spawnerState = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        nextWave = (nextWave + 1) % waves.Length;
    }

    public void EnemySpawned()
    {
        totalSpawnedEnemies++;
        currentSpawnedEnemies++;
    }

    public void EnemyRemoved()
    {
        currentSpawnedEnemies--;
    }
}
