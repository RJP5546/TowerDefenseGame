using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float damage;

    private AttackRange attackRange;

    private void Start()
    {
        attackRange = GetComponent<AttackRange>();
    }

    public void MeleeAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, attackRange.GetTargetingMask()))
        {
            if (hit.transform.gameObject.CompareTag("Tower"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                ApplyDamage(hit.transform);
            }

            else
            {
                return;
            }
        }
        else { return; }
    }

    private void ApplyDamage(Transform hit)
    {
        if (hit.TryGetComponent<Health>(out Health targetHealth)) { targetHealth.TakeDamage(damage); }
        else { Debug.LogWarning("Target has no health component"); }
    }
}
