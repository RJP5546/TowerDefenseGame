using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float damage;

    public void MeleeAttack()
    {
        RaycastHit hit;
        Debug.Log("Melee Attack");
        //layer 9 is the projectile layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, 9))
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
