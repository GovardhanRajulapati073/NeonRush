
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float rotationSpeed = 120f;
    public float boostMultiplier = 1.5f;
    public float boostDuration = 5f;

    void Update()
    {
        transform.Rotate(
            0f,
            rotationSpeed * Time.deltaTime,
            0f
        );
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerRunner player =
            other.GetComponentInParent<PlayerRunner>();

        if (player == null)
            return;

        // Activate speed boost
        player.ActivateSpeedBoost(
            boostMultiplier,
            boostDuration
        );

        // Play speed boost sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySpeedBoostActivate();
        }

        Destroy(gameObject);
    }
}
