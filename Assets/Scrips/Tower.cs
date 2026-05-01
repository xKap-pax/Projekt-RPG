/// @file Tower.cs
/// @brief Core tower behaviour: target acquisition and projectile firing.

using UnityEngine;

/// @brief Represents a placed tower that automatically targets and shoots the most-progressed enemy in range.
///
/// Attach to any tower prefab. Requires a ProjectilePrefab and a firePoint child Transform.
/// 

[System.Serializable]
public class TowerUpgradeStage
{
    public float range;
    public float fireRate;
    public Sprite sprite;
    public int price;
}
public class Tower : MonoBehaviour
{
    /// @brief Attack range in world units.
    public float range = 2f;

    /// @brief Number of shots fired per second.
    public float fireRate = 1f;

    /// @brief Projectile prefab instantiated on each shot.
    public GameObject ProjectilePrefab;

    /// @brief Transform marking the spawn point of fired projectiles.
    public Transform firePoint;

    /// @brief Internal cooldown timer countdown in seconds.
    private float fireCooldown = 0f;

    /// @brief Coin cost to place this tower.
    public int cost = 5;

    /// @brief Decrements cooldown, finds a target, and fires when ready.
    public TowerUpgradeStage[] upgradeStages;
    public int upgradeStage = 0;
    private SpriteRenderer sr;
    public GameObject towerUpgradeUIPrefab;
    private GameObject currentUI;


    public GameObject cloudPS;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Instantiate(cloudPS, transform.position, Quaternion.identity);
    }
    void Update()
    {
        fireCooldown -= Time.deltaTime;
        Enemy target = FindTarget();

        if (target != null && fireCooldown <= 0f)
        {
            Shoot(target);
            fireCooldown = 1f / fireRate;
        }
    }

    /// @brief Scans all active enemies and returns the one with the highest waypoint progress within range.
    ///
    /// Uses currentWayPoint index as a proxy for path progress.
    /// @return The most-progressed Enemy within range, or null if none found.
    Enemy FindTarget()
    {
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemy best = null;
        float bestProgress = -1f;

        foreach (Enemy e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist <= range)
            {
                if (e.currentWayPoint > bestProgress)
                {
                    best = e;
                    bestProgress = e.currentWayPoint;
                }
            }
        }

        return best;
    }

    /// @brief Spawns a projectile from firePoint aimed at the given enemy.
    /// @param target The enemy to assign as the projectile's target.
    void Shoot(Enemy target)
    {
        GameObject proj = Instantiate(ProjectilePrefab, firePoint.position, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();
        p.target = target.transform;
    }
    public void Upgrade()
    {
        TowerUpgradeStage currentUpgradeStage = upgradeStages[upgradeStage];
        CoinManager.Instance.UpdateCoins(currentUpgradeStage.price);
        range = currentUpgradeStage.range;
        fireRate = currentUpgradeStage.fireRate;
        sr.sprite = currentUpgradeStage.sprite;
        upgradeStage++;
        Instantiate(cloudPS, transform.position, Quaternion.identity);
    }
     private void OnMouseDown()
    {
        if(currentUI == null)
        {
            currentUI = Instantiate(towerUpgradeUIPrefab, Object.FindFirstObjectByType<Canvas>().transform);
        }
        TowerUpgradeUI currentUpgradeUI = currentUI.GetComponent<TowerUpgradeUI>();
        currentUpgradeUI.tower = this;
        currentUI.transform.position = Input.mousePosition + new Vector3(50, -50);

        if(upgradeStage >= upgradeStages.Length) return;
        currentUpgradeUI.priceTxt.text = upgradeStages[upgradeStage].price.ToString();

    }
}