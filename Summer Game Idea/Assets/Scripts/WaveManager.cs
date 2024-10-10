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

    public bool spawnNextWave;

    private bool areEnemiesAlive = true;

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        if (spawnerState == SpawnState.WAITING)
        {
            if (!areEnemiesAlive)
            {
                WaveCompleted();
            }
            else { return; }
        }

        if (waveCountdown <= 0 && spawnerState != SpawnState.SPAWNING) 
        {
            StartCoroutine(SpawnWave(waves[nextWave]));
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    public void AreEnemiesAlive()
    {
        if (EnemySpawningPool.Instance.ActiveEnemies <= 0) { areEnemiesAlive = false; }
        else { areEnemiesAlive = true; }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (EnemyPoolInfo enemyType in wave.PoolInfo) { wave.NumberOfEnemies += enemyType.AmountToPool; }

        EnemySpawningPool.Instance.InitializeEnemyQueue(wave.PoolInfo);

        spawnerState = SpawnState.SPAWNING;

        for (int i = 0; i <= wave.NumberOfEnemies; i++)
        {
            
            EnemySpawningPool.Instance.SpawnNextEnemy();

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
}
