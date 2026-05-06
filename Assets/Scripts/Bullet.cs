using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float lifetime = 3f;

    [SerializeField] GameObject impactVfxPrefab;
    

    void Start()
    {
        Destroy(gameObject, lifetime);
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if(impactVfxPrefab != null)
            {
                Instantiate(impactVfxPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }


    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
