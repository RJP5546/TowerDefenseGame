using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private Vector3 direction = Vector3.forward;
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if (transform.position.z > 40 ) { Destroy(this.gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.TryGetComponent<Health>(out Health targetHealth)) { targetHealth.TakeDamage(damage); }
            else { Debug.LogWarning("Target has no health component"); }
            Destroy(gameObject);
        }
    }
}
