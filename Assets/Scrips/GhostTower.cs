/// @file GhostTower.cs
/// @brief Visual ghost (placement preview) for a tower, indicating placement validity by color.

using UnityEngine;

/// @brief Renders a semi-transparent tower preview that changes color based on whether the tile is valid.
///
/// Green = valid placement. Red = invalid placement.
/// Alpha is set to 50% on Awake to distinguish the ghost from a real placed tower.
public class GhostTower : MonoBehaviour
{
    /// @brief Cached reference to the SpriteRenderer on this GameObject.
    SpriteRenderer sr;

    /// @brief Initializes the SpriteRenderer and sets the ghost alpha to 50%.
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 0.5f;
        sr.color = c;
    }

    /// @brief Updates the ghost color to reflect placement validity.
    ///
    /// Note: this overwrites the alpha set in Awake, making the ghost fully opaque when colored.
    /// Consider preserving alpha if a tinted semi-transparent look is desired.
    /// @param isValid True to color green (valid), false to color red (invalid).
    public void SetValid(bool isValid)
    {
        sr.color = isValid ? Color.green : Color.red;
    }
}