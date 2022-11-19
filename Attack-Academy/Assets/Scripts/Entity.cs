using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected float maxDistance;
    private Stele[] steleList;
    float powerMultiplicator;
    [SerializeField]
    protected Utility.MagicType currentMagicType = Utility.MagicType.Fire;

    [SerializeField]
    protected float movementSpeed = 5f;

    protected void Start()
    {
        steleList = FindObjectsOfType<Stele>();
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
}
