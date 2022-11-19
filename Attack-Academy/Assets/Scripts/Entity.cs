using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected float maxDistance;
    private Stele[] steleList;
    public Dictionary<Utility.MagicType, float> minDistToStele { get; private set; }
    float powerMultiplicator;
    [SerializeField]
    public Utility.MagicType currentMagicType { get; private set; }

    [SerializeField]
    protected float movementSpeed = 5f;

    public float healthMax = 100f;
    public float health{get; private set;}

    public virtual void Start()
    {
        steleList = FindObjectsOfType<Stele>();
        currentMagicType = Utility.MagicType.Fire;
        minDistToStele = new Dictionary<Utility.MagicType, float>();
        foreach (Utility.MagicType magicType in Enum.GetValues(typeof(Utility.MagicType)))
        {
            minDistToStele.Add(magicType, float.PositiveInfinity);
        }

        health = healthMax;
    }

    public virtual void Update()
    {
        UpdateMinDistToStele();
    }

    public void ComputePowerMultiplicator()
    {
        powerMultiplicator = 1f;
        foreach (Stele stele in steleList)
        {
            if (stele.magicType == currentMagicType)
            {
                float distance = Vector2.Distance(stele.transform.position, this.transform.position);
                if (distance < maxDistance)
                    powerMultiplicator *= distance / maxDistance;
            }
        }
    }

    public void ScrollMagicType(int scrollvalue)
    {
        System.Array values = System.Enum.GetValues(typeof(Utility.MagicType));
        int index = (int)currentMagicType;
        index += scrollvalue;
        if(index < 0)
        {
            index = values.Length-1;
        }
        if(index >= values.Length)
        {
            index = 0;
        }
        currentMagicType = (Utility.MagicType)values.GetValue(index);
    }



    private void UpdateMinDistToStele()
    {
        Dictionary<Utility.MagicType, float> currentMinDistToStele = minDistToStele;
        foreach (var distFromStele in currentMinDistToStele.ToList())
        {
            currentMinDistToStele[distFromStele.Key] = float.PositiveInfinity;
        }
        if (steleList.Length > 0)
        {
            foreach(var stele in steleList)
            {
                float dist = Vector2.Distance(transform.position, stele.transform.position);
                if (currentMinDistToStele[stele.magicType] > dist)
                {
                    currentMinDistToStele[stele.magicType] = dist;
                }
            }

        }
        minDistToStele = currentMinDistToStele;
    }
    
    public void SetMagicType(Utility.MagicType magicType)
    {
        currentMagicType = magicType;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    public abstract void Die(); 

}
