using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private SpawnerManager sp;
    private Vector2[] spawnPointsPositions;
    [SerializeField] private EnemySpawn[] enemiesToSpawn;

    void Start()
    {
        spawnPointsPositions = transform.GetComponents<Transform>().Select(trans => (Vector2) trans.position).ToArray();
        if (spawnPointsPositions.Length <= 0)
        {
            Debug.LogError("No SpawnPoint in children of Spawner: " + name);
            return;
        }

        sp = transform.parent.GetComponent<SpawnerManager>();

        if (sp == null)
        {
            Debug.LogError("No SpawnerManager in parent of Spawner: " + name);
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
            Vector2 randomPos = spawnPointsPositions[Random.Range(0, spawnPointsPositions.Length)];
            sp.SpawnEnemy(randomPos, enemySpawn.type);
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
