using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    public UnityEvent onDie;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageToTake)
    {
        currentHealth -= damageToTake;
        if (currentHealth <= 0) { Die(); }
    }

    private void Die()
    {
        onDie.Invoke();
        if (this.gameObject.CompareTag("Enemy"))
        {
            WaveManager.Instance.SpawnedEnemies -= 1;
            WaveManager.Instance.AreEnemiesAlive();
        }
        Destroy(gameObject);
    }
}