using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAttack : MonoBehaviour
{
    public float damage;

    private void Update()
    {
         transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(GetComponentInParent<Player>().orientation.y - transform.position.y, GetComponentInParent<Player>().orientation.x - transform.position.x) * Mathf.Rad2Deg - 90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            print("TOUCHE");
            collision.GetComponent<Entity>().TakeDamage(damage);
        }
    }
}
