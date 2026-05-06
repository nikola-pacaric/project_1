using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    public float fireRate = 1.5f;
    public float bulletSpeed = 10f;
    public float moveSpeed = 6f;
    public int bulletCount = 1;

    [SerializeField] private float defaultFireRate = 1.5f;
    [SerializeField] private float defaultBulletSpeed = 10f;
    [SerializeField] private float defaultMoveSpeed = 6f;


    public float minFireRate = 0.5f;

    public void UpgradeFireRate(float amount)
    {
        fireRate = Mathf.Max(fireRate - amount, minFireRate);
    }

    public void UpgradeMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void UpgradeBulletCount()
    {
        bulletCount = 2;
    }

    public void ResetStats()
    {
        fireRate = defaultFireRate;
        bulletSpeed = defaultBulletSpeed;
        moveSpeed = defaultMoveSpeed;
        bulletCount = 1;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
