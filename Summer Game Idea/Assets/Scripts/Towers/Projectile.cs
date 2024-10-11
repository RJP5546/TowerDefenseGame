using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 direction = Vector3.forward;
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if (transform.position.z > 40 ) { Destroy(this.gameObject); }
    }
}
