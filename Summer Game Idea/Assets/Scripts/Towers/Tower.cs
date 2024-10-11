using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Vector3 range;
    [SerializeField] private Vector3 center;

    private string enemyTag = "Enemy";
    [SerializeField] private List<GameObject> enemiesInRange;

    [SerializeField] private float attackRate;
    private float attackCooldown = 0;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform barrelPos;

    private void Update()
    {
        if(enemiesInRange.Count == 0) { return; }
        if(attackCooldown <= 0f)
        {
            Shoot();
            attackCooldown = 1 / attackRate;
        }

        attackCooldown -= Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, range);
    }

    private void Shoot()
    {
        Instantiate(projectile, barrelPos.position, barrelPos.rotation);
    }
}
