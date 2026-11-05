
using UnityEngine;

public class ShieldPulse : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.06f;

    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float pulse = 1f +
            Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        transform.localScale = originalScale * pulse;
    }
}
