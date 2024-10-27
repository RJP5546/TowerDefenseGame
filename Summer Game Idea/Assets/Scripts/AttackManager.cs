using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] private AttackRange attackRange;

    [SerializeField] private float attackRate;
    [SerializeField] private float attackCooldown = 0;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform barrelPos;

    private void Awake()
    {
        attackRange = GetComponent<AttackRange>();
    }

    private void Update()
    {
        if (attackCooldown > 0f) { attackCooldown -= Time.deltaTime; }

        if (!attackRange.IsEnemyInRange) { return; }

        if (attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1 / attackRate;
        }

        attackCooldown -= Time.deltaTime;
    }

    private void Attack()
    {
        if (projectile != null)
        {
            Instantiate(projectile, barrelPos.position, barrelPos.rotation);
        }
        else { }
    }
}
