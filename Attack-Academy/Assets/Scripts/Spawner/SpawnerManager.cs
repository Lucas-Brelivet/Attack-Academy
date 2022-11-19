using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }
    
    public enum EnemyType
    {
        Skeleton,
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
    [HideInInspector] public float[] spawnerStartTimer;

    // Time when the next spawner will start
    public float spawnerTimeWait { get; private set; }
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
        
        Invoke("StartSpawners", 1f);
    }

    void StartSpawners()
    {
        StartCoroutine(StartSpawnersCoroutine());
    }

    IEnumerator StartSpawnersCoroutine()
    {
        float t = 0;
        int i = 0;
        foreach (Spawner s in transform.GetComponentsInChildren<Spawner>())
        {
            spawnerTimeWait = Time.time + spawnerStartTimer[i] - t;
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
            // SerializedObject serializedObject = new SerializedObject(sp);
            // SerializedProperty serializedProperty = serializedObject.FindProperty("spawnerStartTimer");
            // _spawnerStartTimer = new List<float>(serializedProperty.arraySize);
            // for (int i = 0; i < serializedProperty.arraySize; i++)
            // {
            //     _spawnerStartTimer[i] = serializedProperty.GetArrayElementAtIndex(i).floatValue;
            // }
        }
    }
    #endif
}
