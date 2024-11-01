using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private float range;

    [SerializeField] private string targetTag = null;

    private static LayerMask projectileLayer = 9;
    private static LayerMask towerLayer = 10;
    private static LayerMask enemyLayer = 11;
    public int targetingMask;

    public bool IsEnemyInRange { get; private set; }

    private void Awake()
    {
        IsEnemyInRange = false;
        if (this.gameObject.CompareTag("Tower"))
        {
            //sets our targeting ray to hit everything but these two layers
             targetingMask = ~(1 << projectileLayer | 1 << towerLayer);
        }
        else if (this.gameObject.CompareTag("Enemy"))
        {
            targetingMask = ~( 1 << projectileLayer | 1 << enemyLayer);
        }
    }

    private void Update()
    {
        EnemyInRange();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * range);
    }

    public void EnemyInRange()
    {
        RaycastHit hit;

        //layer 9 is the projectile layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, targetingMask))
        {
            if (hit.transform.gameObject.CompareTag(targetTag))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                IsEnemyInRange = true;
            }

            else
            {
                IsEnemyInRange = false;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            }
        }
        else { IsEnemyInRange = false; }
    }

    public int GetTargetingMask()
    {
        return targetingMask;
    }
}
