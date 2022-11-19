using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector2[] spawnPointsPositions;

    void Start()
    {
        spawnPointsPositions = transform.GetComponents<Transform>().Select(trans => (Vector2) trans.position).ToArray();
    }
    
    void Update()
    {
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.DrawSphere(transform.GetChild(i).position, 0.3f);
        }
    }
}
