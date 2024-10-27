using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private float range;

    [SerializeField] private string targetTag = null;

    public bool IsEnemyInRange { get; private set; }

    private void Awake()
    {
        IsEnemyInRange = false;
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
        Debug.Log("RaycastCheck");
        //layer 9 is the projectile layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, 9))
        {
            if (hit.transform.gameObject.CompareTag(targetTag))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                IsEnemyInRange = true;
            }

            else
            {
                IsEnemyInRange = false;
            }
        }
        else { IsEnemyInRange = false; }
    }
}
