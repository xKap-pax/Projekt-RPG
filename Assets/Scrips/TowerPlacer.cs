/// @file TowerPlacer.cs
/// @brief Handles ghost tower preview and final tower placement on the tilemap.

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

/// @brief Reads mouse input each frame to show a placement ghost and place towers on valid tiles.
///
/// Requires two Tilemaps: one for valid placement tiles and one marking blocked zones.
/// Works in conjunction with TowerSelection to know which prefab to place.
public class TowerPlacer : MonoBehaviour
{
    /// @brief Tilemap that defines valid tower placement cells.
    public Tilemap placementMap;

    /// @brief Tilemap that marks cells where placement is forbidden.
    public Tilemap nonPlacableMap;

    /// @brief Prefab used to render the ghost (preview) of the tower being placed.
    public GameObject ghostPrefab;

    /// @brief Tracks cells that already have a tower, preventing double-placement.
    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();

    /// @brief The currently active ghost tower instance, or null if no tower is selected.
    private GameObject ghostInstance;

    /// @brief Called every frame to update hover preview and handle click placement.
    void Update()
    {
        HandlePlacementHover();
        HandlePlacementClick();
    }

    /// @brief Updates the ghost tower's position and validity color based on mouse position.
    ///
    /// Destroys the ghost if no tower is selected.
    /// Creates a new ghost if one does not exist.
    /// Colors the ghost green if the hovered cell is valid, red if not.
    void HandlePlacementHover()
    {
        if (TowerSelection.selectedTowerPrefab == null)
        {
            if (ghostInstance != null)
            {
                Destroy(ghostInstance);
            }
            return;
        }

        if (ghostInstance == null)
        {
            ghostInstance = Instantiate(ghostPrefab);
        }

        ghostInstance.GetComponent<SpriteRenderer>().sprite =
            TowerSelection.selectedTowerPrefab.GetComponent<SpriteRenderer>().sprite;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int cellPos = placementMap.WorldToCell(mouseWorldPos);
        Vector3 worldCenter = placementMap.GetCellCenterWorld(cellPos);
        worldCenter.z = 0;

        // Offset upward slightly to align with isometric tile center.
        ghostInstance.transform.position = worldCenter + new Vector3(0, placementMap.cellSize.y * 0.25f);

        bool valid = placementMap.HasTile(cellPos) && !occupiedTiles.Contains(cellPos);
        ghostInstance.GetComponent<GhostTower>().SetValid(valid);
    }

    /// @brief Handles a left mouse click to place a tower on a valid tile.
    ///
    /// Placement is blocked if: no tower is selected, the click hits UI,
    /// the cell has no tile, or the cell is already occupied.
    /// On success: instantiates the tower, deducts coins, clears selection,
    /// and marks the cell as occupied.
    void HandlePlacementClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (TowerSelection.selectedTowerPrefab == null) return;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int cellPos = placementMap.WorldToCell(mouseWorldPos);

        if (!placementMap.HasTile(cellPos) || occupiedTiles.Contains(cellPos)) return;

        Instantiate(TowerSelection.selectedTowerPrefab, ghostInstance.transform.position, Quaternion.identity);
        CoinManager.Instance.UpdateCoins(-TowerSelection.selectedTowerPrefab.GetComponent<Tower>().cost);
        TowerSelection.selectedTowerPrefab = null;
        occupiedTiles.Add(cellPos);
    }
}