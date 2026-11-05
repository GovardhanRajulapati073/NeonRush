
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1;
    public float rotationSpeed = 120f;

    [Header("Collection Effect")]
    public GameObject collectBurstPrefab;

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

        // Add credits
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddCredits(value);
        }

        // Play coin collection sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCoinCollect();
        }

        // Play collection burst effect
        if (collectBurstPrefab != null)
        {
            GameObject effect = Instantiate(
                collectBurstPrefab,
                transform.position,
                Quaternion.identity
            );

            Destroy(effect, 2f);
        }

        // Destroy collected coin
        Destroy(gameObject);
    }
}
