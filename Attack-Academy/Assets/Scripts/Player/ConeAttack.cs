using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAttack : MonoBehaviour
{
    public float damage;
    public Utility.MagicType magic;

    SpriteRenderer render;
    public Sprite[] dictSprite;
    private void Start()
    {
        render = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        if (magic == Utility.MagicType.Fire)
        {
            render.sprite = dictSprite[0];
        }
        else if (magic == Utility.MagicType.Ice)
        {
            render.sprite = dictSprite[1];
        }
        else if (magic == Utility.MagicType.Wind)
        {
            render.sprite = dictSprite[3];
        }
        else 
        {
            render.sprite = dictSprite[3];
        }
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(GetComponentInParent<Player>().orientation.y - transform.position.y, GetComponentInParent<Player>().orientation.x - transform.position.x) * Mathf.Rad2Deg + 90);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        { 
            collision.GetComponent<Entity>().TakeDamage(damage);
        }
    }
}
