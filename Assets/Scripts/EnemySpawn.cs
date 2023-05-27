using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    GameObject[] enemyPrefabs = {};
    [SerializeField]
    Transform target = null;
    [SerializeField]
    int maxSpawnCount = 50;
    [SerializeField, Min(0)]
    float spawnDelay = 0;
    [SerializeField, Min(0)]
    float spawnInterval = 3;
    [SerializeField]
    bool spawnOnState = false;
    [SerializeField]
    [Tooltip("ê∂ê¨îÕàÕA")]
    private Vector3 rangeA;
    [SerializeField]
    [Tooltip("ê∂ê¨îÕàÕB")]
    private Vector3 rangeB;

    Transform thisTransform;
    WaitForSeconds spawnDelayWait;
    WaitForSeconds spawnWait;
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = transform;
        spawnDelayWait = new WaitForSeconds(spawnDelay);
        spawnWait = new WaitForSeconds(spawnInterval);

        if (spawnOnState)
        {
            StartSpawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawn()
    {
        StartCoroutine(nameof(SpawnTimer));
    }

    public void StopSpaen()
    {
        StopCoroutine(nameof(SpawnTimer));
    }

    IEnumerator SpawnTimer()
    {
        yield return spawnDelayWait;

        for(int i = 0; i < maxSpawnCount; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(rangeA.x, rangeB.x),rangeA.y,rangeA.z);
            EnemyControll enemy = Instantiate(enemyPrefabs[Random.Range(0,enemyPrefabs.Length)],randomPosition,Quaternion.identity).GetComponent<EnemyControll>();
            enemy.Target = target;
            yield return spawnWait;
        }
    }
}
