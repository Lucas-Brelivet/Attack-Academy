using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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
    // public Dictionary<int, float> spawnerStartTimer;
    [HideInInspector] public float[] spawnerStartTimer;

    private List<GameObject> enemiesSpawned = new List<GameObject>();

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

        StartCoroutine(StartSpawners());
    }

    IEnumerator StartSpawners()
    {
        float t = 0;
        int i = 0;
        foreach (Spawner s in transform.GetComponentsInChildren<Spawner>())
        {
            yield return new WaitForSeconds(spawnerStartTimer[i] - t);
            s.StartWave();
            t += spawnerStartTimer[i];
            i++;
        }
    }

    public void SpawnEnemy(Vector2 position, EnemyType type)
    {
        GameObject spawnedEnemy = Instantiate(enemiesPrefab[type]);
        spawnedEnemy.transform.position = position;
        enemiesSpawned.Add(spawnedEnemy);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpawnerManager))]
public class ClientEditor : Editor
{
    private List<float> _spawnerStartTimer = new List<float>();
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpawnerManager sp = (SpawnerManager)target;

        Spawner[] spawners = sp.transform.GetComponentsInChildren<Spawner>();
        for (int i = 0; i < spawners.Length; i++)
        {
            if (_spawnerStartTimer.Count <= i)
            {
                _spawnerStartTimer.Add(0);
            }

            _spawnerStartTimer[i] = Mathf.Clamp(
                EditorGUILayout.FloatField(spawners[i].name + " time", _spawnerStartTimer[i]),
                0,
                float.PositiveInfinity);
        }

        sp.spawnerStartTimer = _spawnerStartTimer.ToArray();
    }

    void Awake()
    {
        SpawnerManager sp = (SpawnerManager)target;
        _spawnerStartTimer = sp.spawnerStartTimer.ToList();
    }
}
#endif
