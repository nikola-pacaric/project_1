using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 boundsMin = new Vector2(-9f, -4.5f);
    [SerializeField] Vector2 boundsMax = new Vector2(9f, 4.5f);

    PlayerUpgrades upgrades;

    void Awake()
    {
        upgrades = GetComponent<PlayerUpgrades>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(h, v).normalized;
        transform.Translate(dir * upgrades.moveSpeed * Time.deltaTime);

        // Clamp position
        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, boundsMin.x, boundsMax.x);
        pos.y = Mathf.Clamp(pos.y, boundsMin.y, boundsMax.y);
        transform.position = pos;
    }
}
