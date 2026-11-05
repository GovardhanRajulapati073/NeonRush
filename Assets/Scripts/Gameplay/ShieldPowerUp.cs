
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        PlayerRunner player =
            other.GetComponent<PlayerRunner>();

        if (player == null)
            player = other.GetComponentInParent<PlayerRunner>();

        if (player == null)
            return;

        collected = true;

        // Activate shield
        player.ActivateShield();

        // Play shield activation sound
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayShieldActivate();
        }

        Debug.Log("SHIELD ACTIVATED!");

        Destroy(gameObject);
    }
}
