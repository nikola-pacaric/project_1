using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float intenisty = 0.2f;
    [SerializeField] float duration = 2f;

    public IEnumerator Shake()
    {
        Vector3 origin = transform.localPosition;
        float t = 0f;
        while(t < duration)
        {
            transform.localPosition = origin + (Vector3)Random.insideUnitCircle * intenisty;
            t += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = origin;
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
