
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerRunner player =
            other.GetComponentInParent<PlayerRunner>();

        if (player == null)
            return;

        // Shield absorbs one obstacle collision.
        if (player.UseShield())
        {
            Destroy(gameObject);
            Debug.Log("Shield protected the player!");
            return;
        }

        // Play crash sound when player hits obstacle.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerCrash();
        }

        // No shield: trigger game over.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
    }
}
