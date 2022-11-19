using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAttack : MonoBehaviour
{
    public float damage;

    public void placeZone()
    {
        transform.position = GetComponentInParent<Player>().orientation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Entity>().TakeDamage(damage);
        }
    }
}
