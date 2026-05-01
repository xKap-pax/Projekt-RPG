/// @file CoinManager.cs
/// @brief Global singleton that tracks the player's coin balance and updates the UI.

using UnityEngine;
using TMPro;

/// @brief Singleton manager for the coin economy.
///
/// Holds the current coin balance and keeps the HUD text in sync on every change.
/// Access from any script via CoinManager.Instance.
///
/// @note Only one CoinManager should exist in the scene at a time.
/// No duplicate protection is implemented — if multiple instances exist, the last one to Awake wins.
public class CoinManager : MonoBehaviour
{
    /// @brief Global singleton reference. Set on Awake.
    public static CoinManager Instance;

    /// @brief Current coin balance. Modify only through UpdateCoins() to keep UI in sync.
    public int coins;

    /// @brief HUD TextMeshPro element that displays the current coin count.
    public TextMeshProUGUI coinTxt;

    /// @brief Registers this instance as the singleton and initializes the UI display.
    private void Awake()
    {
        Instance = this;
        UpdateCoins(0);
    }

    /// @brief Adds or subtracts coins and refreshes the HUD text immediately.
    ///
    /// Pass a positive value to award coins, negative to spend them.
    /// Does not guard against the balance going below zero.
    ///
    /// @param changeAmount Amount to add (positive) or subtract (negative) from the balance.
    public void UpdateCoins(int changeAmount)
    {
        coins += changeAmount;
        coinTxt.text = coins.ToString();
    }
}