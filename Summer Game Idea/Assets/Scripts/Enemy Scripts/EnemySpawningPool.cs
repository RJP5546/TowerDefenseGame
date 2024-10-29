using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyPoolInfo
{
    public GameObject Enemy;
    public int AmountToPool;
    private int amountPooled;

    public void PoolEnemy()
    {
        amountPooled++;
    }

    public bool IsFinishedPooling()
    {
        return amountPooled == AmountToPool;
    }

    public void ResetPool()
    {
        amountPooled = 0;
    }
}


public class EnemySpawningPool : Singleton<EnemySpawningPool>
{
    //public List<EnemyPoolInfo> tempEnemies;

    public Queue<GameObject> EnemyQueue = new Queue<GameObject>();


    /// <summary>
    /// Fills a queue with the correct number of each item to pool.
    public void InitializeEnemyQueue(List<EnemyPoolInfo> waveEnemies)
    {
        //Make a copy to prevent the original from being changed
        //tempEnemies.Clear();

        Debug.Log("Initalizing enemy queue");

        var tempEnemies = new List<EnemyPoolInfo>(waveEnemies);

        Debug.Log("tempEnemies length: " + tempEnemies.Count);

        //Tracks which item we are accessing
        int tempEnemyIndex = 0;

        //While there are enemies left to spawn
        while (tempEnemies.Count > 0)
        {
            tempEnemyIndex = UnityEngine.Random.Range(0, tempEnemies.Count);

            //Get a reference to the next enemy for the pool
            EnemyPoolInfo nextEnemy = tempEnemies[tempEnemyIndex];

            //avoid null tempEnemies (could be removed)
            if (nextEnemy.Enemy == null || nextEnemy.AmountToPool <= 0)
            {
                tempEnemies.RemoveAt(tempEnemyIndex);
                continue;
            }

            nextEnemy.PoolEnemy();

            //spawn an enemyInstance of that item and add it to the queue
            GameObject enemyInstance = Instantiate(nextEnemy.Enemy);
            enemyInstance.SetActive(false);
            EnemyQueue.Enqueue(enemyInstance);

            WaveManager.Instance.SpawnedEnemies += 1;

            if (nextEnemy.IsFinishedPooling())
            {
                tempEnemies.RemoveAt(tempEnemyIndex);
            }
        }

        foreach (EnemyPoolInfo enemy in waveEnemies) { enemy.ResetPool(); }
        tempEnemies = null;
    }

    public GameObject SpawnNextEnemy()
    {
        if (EnemyQueue.Count <= 0)
        {
            Debug.Log("Queue Empty");
            return null;
        }

        GameObject result = EnemyQueue.Dequeue();

        result.SetActive(true);
        result.transform.position = Waypoints.Lanes[UnityEngine.Random.Range(0, Waypoints.Lanes.Length)].position;
        
        return result;
    }

}
