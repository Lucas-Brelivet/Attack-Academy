using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    float maxDistance;
    private Stele[] steleList;
    float powerMultiplicator;
    [SerializeField]
    Utility.MagicType currentMagicType = Utility.MagicType.Fire;
    void Start()
    {
        steleList = FindObjectsOfType<Stele>();
    }

    void Update()
    {

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
}
