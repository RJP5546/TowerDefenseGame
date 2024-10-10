using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField] private bool isAtObstacle;
    [SerializeField] private string[] collisonTags;

    private Vector3 movementDirection = new Vector3( 0 , 0 , -1 );

    private void Update()
    {
        if (!isAtObstacle) { transform.position = Vector3.MoveTowards(transform.position, transform.position + movementDirection, speed * Time.deltaTime); }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collisonTags.Contains(other.gameObject.tag)) { isAtObstacle = true; }
        if (other.gameObject.tag == "End") 
        {
            EnemySpawningPool.Instance.ActiveEnemies -= 1;
            WaveManager.Instance.AreEnemiesAlive();
            Destroy(gameObject); 
        }
    }

}
