using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] int health = 1;
    [SerializeField] int scoreValue = 10;

    [Range(0f, 1f)][SerializeField] float dropChance = 0.2f;
    [SerializeField] GameObject[] dropPrefabs;
    [SerializeField] GameObject deathVfxPrefab;
    Transform player;

    [SerializeField] AudioClip deathSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Game Over!");
            GameManager.Instance.EndGame();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.Instance.AddScore(scoreValue);
            GameManager.Instance.RegisterKill();
            TrySpawnDrop();
            if (deathVfxPrefab != null)
            {
                Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);
            }
            SoundManager.Instance.PlaySfx(deathSound);
            Destroy(gameObject);
        }
    }

    void TrySpawnDrop()
    {
        if (dropPrefabs == null || dropPrefabs.Length == 0) return;
        if (Random.value > dropChance) return;

        GameObject prefab = dropPrefabs[Random.Range(0, dropPrefabs.Length)];
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
