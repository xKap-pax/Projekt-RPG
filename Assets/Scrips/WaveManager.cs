/// @file WaveManager.cs
/// @brief Manages enemy wave spawning and wave progression logic.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
/// @brief Holds configuration data for a single wave.
[System.Serializable]
public class WaveData
{
    /// @brief Total duration of the wave in seconds.
    public float duration = 10f;
    
    /// @brief Number of easy enemies to spawn in this wave.
    public int easyEnemies = 5;

    /// @brief Number of hard enemies to spawn in this wave.
    public int hardEnemies = 2;
}

/// @brief Controls wave flow: spawning enemies in sequence, timing, and UI state.
///
/// Attach this to a manager GameObject in the scene.
/// Requires references to enemy prefabs, waypoints, and a start button.
public class WaveManager : MonoBehaviour
{
    /// @brief Array of wave configurations defined in the Inspector.
    public WaveData[] waves;

    /// @brief UI button that triggers the next wave when clicked.
    public Button startWaveButton;

    /// @brief Prefab used to spawn easy-type enemies.
    public GameObject easyEnemyPrefab;

    /// @brief Prefab used to spawn hard-type enemies.
    public GameObject hardEnemyPrefab;

    /// @brief Ordered array of waypoints enemies follow through the map.
    public Transform[] wayPoints;

    /// @brief Index of the wave currently queued or in progress.
    private int currentWaveIndex = 0;

    /// @brief Whether a wave is currently running (blocks re-triggering).
    private bool waveRunning = false;

    public TextMeshProUGUI waveText;


    /// @brief Registers the StartWave callback on the start button.
    void Start()
    {
        startWaveButton.onClick.AddListener(StartWave);
    }

    /// @brief Attempts to start the next wave if no wave is running and waves remain.
    public void StartWave()
    {
        if (!waveRunning && currentWaveIndex < waves.Length)
        {

            StartCoroutine(RunWave());
            
        }
    }

    /// @brief Coroutine that runs a full wave: spawns easy enemies, then hard, then waits.
    ///
    /// Spawning is spread over the first two-thirds of the wave duration.
    /// The final third is a cooldown before re-enabling the button.
    /// @return IEnumerator for coroutine execution.
    IEnumerator RunWave()
    {
        waveText.text = (currentWaveIndex + 1).ToString();
        waveRunning = true;
        startWaveButton.interactable = false;

        WaveData wave = waves[currentWaveIndex];

        for (int i = 0; i < wave.easyEnemies; i++)
        {
            SpawnEnemy(easyEnemyPrefab);
            yield return new WaitForSeconds((wave.duration / 3) / wave.easyEnemies);
        }

        for (int i = 0; i < wave.hardEnemies; i++)
        {
            SpawnEnemy(hardEnemyPrefab);
            yield return new WaitForSeconds((wave.duration / 3) / wave.hardEnemies);
        }

        yield return new WaitForSeconds(wave.duration / 3);

        
        waveRunning = false;
        startWaveButton.interactable = true;
        currentWaveIndex++;
        
    }

    /// @brief Instantiates an enemy from the given prefab at the first waypoint.
    /// @param prefab The enemy prefab to instantiate.
    void SpawnEnemy(GameObject prefab)
    {
        GameObject e = Instantiate(prefab, wayPoints[0].position, Quaternion.identity);
        Enemy enemy = e.GetComponent<Enemy>();
        enemy.waypoints = wayPoints;
    }
}