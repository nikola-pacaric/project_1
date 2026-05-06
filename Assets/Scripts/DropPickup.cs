using UnityEngine;

public enum DropType { FireRate, MoveSpeed, BulletCount/*ostatak upgrads dodati ovde za izbor u inspector*/}

public class DropPickup : MonoBehaviour
{
    [Header("Drop config")]
    public DropType type = DropType.FireRate;
    public float value = 0.2f;
    public float lifespan = 10f;

    [Header("VFX")]
    public float floatAmplitude = 0.1f;
    public float floatFrequency = 2f;
    public float spinSpeed = 60f;

    private Vector3 _startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerUpgrades playerUpgrades = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerUpgrades>();
        _startPos = transform.position;
        if (type == DropType.FireRate && playerUpgrades.fireRate == playerUpgrades.minFireRate) Destroy(gameObject);
        if (lifespan > 0f) Destroy(gameObject, lifespan);
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = _startPos + new Vector3(0f, y, 0f);
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerUpgrades>();
        if (player == null) return;

        switch (type)
        {
            case DropType.FireRate:
                player.UpgradeFireRate(value);
                break;

            case DropType.MoveSpeed:
                player.UpgradeMoveSpeed(value);
                break;
            case DropType.BulletCount:
                player.UpgradeBulletCount();
                break;
        }
        //player.OnPickup(type, value);
        Destroy(gameObject);
    }
}
