using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }
    
    public enum EnemyType
    {
        Skeleton,
        Dragon,
    }

    [Serializable]
    public struct Enemy
    {
        [SerializeField]
        public EnemyType type;
        [SerializeField]
        public GameObject obj;
    }

    private Dictionary<EnemyType, GameObject> enemiesPrefab;
    [SerializeField] private Enemy[] _enemiesPrefab;
    private List<GameObject> enemiesSpawned = new List<GameObject>();
    private Spawner current;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        enemiesPrefab = new Dictionary<EnemyType, GameObject>(_enemiesPrefab.Length);
        foreach (Enemy enemy in _enemiesPrefab)
        {
            enemiesPrefab.Add(enemy.type, enemy.obj);
        }
        Debug.Log(enemiesPrefab.Keys);

        if (_enemiesPrefab.Length != Enum.GetNames(typeof(EnemyType)).Length)
        {
            Debug.LogError("Enemies Prefab should have all the Enemy Type in SpawnerManager");
        }
    }

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
        
        current.StartWave();
    }

    public void SpawnEnemy(Vector2 position, EnemyType type)
    {
        GameObject spawnedEnemy = Instantiate(enemiesPrefab[type]);
        spawnedEnemy.transform.position = position;
        enemiesSpawned.Add(spawnedEnemy);
    }
}
