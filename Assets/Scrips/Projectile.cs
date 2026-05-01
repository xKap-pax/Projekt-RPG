/// @file Projectile.cs
/// @brief Homing projectile that follows its target and deals damage on contact.

using UnityEngine;

/// @brief Moves toward an assigned target each frame, rotating to face direction of travel.
///
/// On reaching the target, deals 1 damage. If the target dies, awards coins and destroys it.
/// Self-destructs if the target is destroyed before impact.
public class Projectile : MonoBehaviour
{
    /// @brief Movement speed in world units per second.
    public float speed = 8f;

    /// @brief The Transform this projectile is homing toward.
    public Transform target;
    public AudioClip hitSFX;
    /// @brief Moves and rotates the projectile toward its target each frame.
    ///
    /// Destroys itself if target is gone.
    /// Applies 1 damage on contact (distance < 0.15 units).
    /// Awards 2 coins and destroys the enemy if its health reaches zero.
    public GameObject hitPS;
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // Rotate sprite to face direction of travel; subtract 90° to correct for sprite orientation.
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        if (Vector2.Distance(transform.position, target.position) < 0.15f)
        {
            Enemy e = target.GetComponent<Enemy>();
            e.health -= 1;

            if (e.health <= 0)
            {
                CoinManager.Instance.UpdateCoins(2);
                Destroy(target.gameObject);
            }
                AudioManager.Instance.PlaySFX(hitSFX);

            Instantiate(hitPS, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}