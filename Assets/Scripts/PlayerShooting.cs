using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    [SerializeField] AudioClip fireSound;
    //[SerializeField] AudioClip impactSound;

    float fireTimer;
    PlayerUpgrades upgrades;

    void Awake()
    {
        upgrades = GetComponent<PlayerUpgrades>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= upgrades.fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        if (upgrades.bulletCount == 1)
        {
            GameObject enemy = FindClosestEnemy();
            if (enemy == null) return;
            FireBulletAt(enemy);
        }
        else
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0) return;

            System.Array.Sort(enemies, (a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));

            int bulletsToShoot = Mathf.Min(upgrades.bulletCount, enemies.Length);
            for (int i= 0; i< bulletsToShoot; i++)
            {
                FireBulletAt(enemies[i]);
            }
        }
        
    }

    void FireBulletAt(GameObject target)
    {
        Vector2 dir = (target.transform.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf .Rad2Deg;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.AngleAxis(angle - 90f, Vector3.forward));
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * upgrades.bulletSpeed;

        SoundManager.Instance.PlaySfx(fireSound);
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach(var e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = e;
            }
        }
        return closest;
    }
}
