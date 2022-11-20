using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAttack : MonoBehaviour
{
    public float damage;
    public Utility.MagicType magic;
     Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    public void placeZone()
    {
        print(magic);
        transform.position = GetComponentInParent<Player>().orientation;
        if(magic == Utility.MagicType.Fire)
        {
            animator.Play("zonefir");
        }
        else if (magic == Utility.MagicType.Ice)
        {
            animator.Play("zoneice");
        }
        else if (magic == Utility.MagicType.Wind)
        {
            animator.Play("zoneair");
        }
        else if (magic == Utility.MagicType.Lightning)
        {
            animator.Play("zonethunder");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Entity>().TakeDamage(damage);
        }
    }
}
