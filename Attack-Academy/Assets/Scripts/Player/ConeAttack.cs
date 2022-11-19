using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAttack : MonoBehaviour
{
    public float damage;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            print("TOUCHE");
            collision.GetComponent<Entity>().TakeDamage(damage);
        }
    }
}
