using UnityEngine;

public class SurvivalSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float initialSpawninterval = 2f;
    [SerializeField] Transform player;

    [SerializeField] GameObject specialEnemyPrefab;
    [SerializeField] float specialSpawnTime = 120f;
    private bool specialEnemySpawned = false;

    float spawnTimer;
    float elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        float currentInterval = Mathf.Max(0.25f, initialSpawninterval - (elapsedTime / 65f));

        if (spawnTimer >= currentInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }

        if (!specialEnemySpawned && elapsedTime >= specialSpawnTime)
        {
            SpawnSpecialEnemy();
            specialEnemySpawned = true;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = GetRandomSpawnPosition();

        float minutes = elapsedTime / 60f;
        int maxIndex = 0;

        if (minutes >= 1.5f) maxIndex = 1;
        if (minutes >= 2.5f) maxIndex = 2;

        int index = Random.Range(0, maxIndex + 1);
        Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity);
    }

    void SpawnSpecialEnemy()
    {
        Vector2 spawnPos = GetRandomSpawnPosition();
        Instantiate(specialEnemyPrefab, spawnPos, Quaternion.identity);
    }

    Vector2 GetRandomSpawnPosition()
    {
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: return new Vector2(Random.Range(-camWidth / 2, camWidth / 2), camHeight / 2 + 1.5f);
            case 1: return new Vector2(Random.Range(-camWidth / 2, camWidth / 2), -camHeight / 2 - 1.5f);
            case 2: return new Vector2(-camWidth / 2 - 1.5f, Random.Range(-camHeight / 2, camHeight / 2));
            case 3: return new Vector2(camWidth / 2 + 1.5f, Random.Range(-camHeight / 2, camHeight / 2));
        }
        return Vector2.zero;
    }

    public void ResetSpawner()
    {
        elapsedTime = 0f;
        spawnTimer = 0f;
        specialEnemySpawned = false;
    }
}
