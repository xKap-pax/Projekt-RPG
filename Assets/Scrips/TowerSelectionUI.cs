/// @file TowerSelection.cs
/// @brief Handles player selection of tower prefabs from the UI.

using UnityEngine;

/// @brief UI controller that tracks which tower prefab the player has selected.
///
/// Attach to each tower selection button's GameObject.
/// Uses a static field so TowerPlacer can read the selection without a direct reference.
public class TowerSelection : MonoBehaviour
{
    /// @brief The currently selected tower prefab. Null means nothing is selected.
    ///
    /// Shared across all TowerSelection instances via static access.
    public static GameObject selectedTowerPrefab;

    /// @brief Selects or deselects a tower prefab.
    ///
    /// Clicking the already-selected tower deselects it.
    /// If the player cannot afford the tower, selection is blocked.
    /// @param towerPrefab The tower prefab to select.
    public void selectTower(GameObject towerPrefab)
    {
        if (towerPrefab == selectedTowerPrefab)
        {
            selectedTowerPrefab = null;
            return;
        }

        if (towerPrefab.GetComponent<Tower>().cost > CoinManager.Instance.coins)
        {
            return;
        }

        selectedTowerPrefab = towerPrefab;
    }
}