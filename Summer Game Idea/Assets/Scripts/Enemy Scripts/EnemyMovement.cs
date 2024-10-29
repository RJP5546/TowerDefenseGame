using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField] private AttackRange attackRange;
    [SerializeField] private string[] collisonTags;

    private Vector3 movementDirection = new Vector3( 0 , 0 , -1 );

    private void Awake()
    {
        attackRange = GetComponent<AttackRange>();
    }

    private void Update()
    {
        if (!attackRange.IsEnemyInRange) { transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection, speed * Time.deltaTime); }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "End") 
        {
            WaveManager.Instance.EnemyRemoved();
            Destroy(gameObject); 
        }
    }

}
