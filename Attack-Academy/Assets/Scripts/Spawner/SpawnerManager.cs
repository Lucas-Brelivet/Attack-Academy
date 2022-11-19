using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesPrefab;
    private Spawner current;

    void Start()
    {
        if (transform.childCount <= 0)
        {
            Debug.LogError("No Spawner in children of SpawnerManager");
            return;
        }

        current = transform.GetChild(0).GetComponent<Spawner>();

        if (current == null)
        {
            Debug.LogError("No Spawner in child of SpawnerManager");
            return;
        }
    }

    void Update()
    {
        
    }
}
