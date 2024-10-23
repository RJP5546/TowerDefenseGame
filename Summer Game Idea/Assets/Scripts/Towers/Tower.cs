using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float range;

    private string enemyTag = "Enemy";
    [SerializeField] private List<GameObject> enemiesInRange;

    [SerializeField] private float attackRate;
    [SerializeField] private float attackCooldown = 0;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform barrelPos;

    private void Update()
    {
        if (attackCooldown > 0f){ attackCooldown -= Time.deltaTime; }

        if (!EnemyInRange()) { return; }

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
        Gizmos.DrawRay(transform.position, Vector3.forward * range);
    }

    private bool EnemyInRange()
    {
        RaycastHit hit;
        Debug.Log("RaycastCheck");
        //layer 9 is the projectile layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, 9))
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                return true;
            }

            else
            {
                return false;
            }
        }
        else { return false; }
    }

    private void Shoot()
    {
        Instantiate(projectile, barrelPos.position, barrelPos.rotation);
    }
}
