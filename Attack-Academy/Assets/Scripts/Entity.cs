using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    protected float maxDistance;
    private Stele[] steleList;
    public Dictionary<Utility.MagicType, float> minDistToStele { get; private set; }
    float powerMultiplicator;
    [SerializeField]
    protected Utility.MagicType currentMagicType = Utility.MagicType.Fire;

    [SerializeField]
    protected float movementSpeed = 5f;

    public virtual void Start()
    {
        steleList = FindObjectsOfType<Stele>();
        minDistToStele = new Dictionary<Utility.MagicType, float>();
        foreach (Utility.MagicType magicType in Enum.GetValues(typeof(Utility.MagicType)))
        {
            minDistToStele.Add(magicType, float.PositiveInfinity);
        }
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
                powerMultiplicator *= (Vector2.Distance(stele.transform.position, this.transform.position) / maxDistance);
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
}
