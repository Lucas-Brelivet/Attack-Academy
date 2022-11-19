using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [Serializable]
    public struct EnemySpawn
    {
        [SerializeField]
        public SpawnerManager.EnemyType type;
        [SerializeField]
        public float delayBeforeNext;
    }

    private Vector2[] spawnPointsPositions;
    [SerializeField] private EnemySpawn[] enemiesToSpawn;

    void Awake()
    {
        spawnPointsPositions = new Vector2[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPointsPositions[i] = transform.GetChild(i).transform.position;
        }
        
        if (spawnPointsPositions.Length <= 0)
        {
            Debug.LogError("No SpawnPoint in children of Spawner: " + name);
            return;
        } 
    }

    public void StartWave()
    {
        StartCoroutine(Wave());
    }

    IEnumerator Wave()
    {
        foreach (EnemySpawn enemySpawn in enemiesToSpawn)
        {
            int index = Random.Range(0, spawnPointsPositions.Length);
            Vector2 randomPos = spawnPointsPositions[index];
            SpawnerManager.Instance.SpawnEnemy(randomPos, enemySpawn.type);
            yield return new WaitForSeconds(enemySpawn.delayBeforeNext);
        }
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
