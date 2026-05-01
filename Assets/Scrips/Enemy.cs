using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed=2f;
    public int health = 1;
    public Transform[] waypoints;
    public int currentWayPoint = 0;
    public AudioClip deathSFX;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        Transform target = waypoints[currentWayPoint];
        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentWayPoint++;

            if (currentWayPoint >= waypoints.Length)
            {
                HealthManager.Instance.UpdateHealth(-1);
                AudioManager.Instance.PlaySFX(deathSFX);
                Destroy(gameObject);
            }
        }
    }
}
