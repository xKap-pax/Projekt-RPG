# 🗼 BizzareTowerDefence

> A Unity 2D tower defense game featuring wave-based enemy spawning, upgradeable towers, and strategic placement mechanics.

---

## 🎮 Overview

**BizzareTowerDefence** is a 2D tower defense game built in Unity where players place and upgrade towers to stop waves of enemies from reaching the end of the path. Manage your coins wisely — spend them on new towers or upgrade existing ones before the next wave hits.

---

## ✨ Features

- 🌊 **Wave System** — Multiple configurable waves with easy and hard enemy types
- 🗼 **Tower Placement** — Ghost preview system with valid/invalid tile detection on isometric tilemaps
- ⬆️ **Tower Upgrades** — Multi-stage upgrades with unique stats (range, fire rate, sprite) per stage
- 🎯 **Smart Targeting** — Towers automatically target the most-progressed enemy in range
- 🏃 **Homing Projectiles** — Projectiles rotate and home in on their target
- 💰 **Coin Economy** — Earn coins by defeating enemies, spend them on towers and upgrades
- ❤️ **Health System** — Lose health when enemies reach the end; game resets at zero
- 🔊 **Audio** — Sound effects for hits and enemy deaths

---

## 🏗️ Architecture

### Core Scripts

| Script | Responsibility |
|--------|---------------|
| `WaveManager.cs` | Spawns enemy waves with configurable timing and enemy counts |
| `Tower.cs` | Target acquisition, shooting logic, and upgrade handling |
| `TowerPlacer.cs` | Ghost preview and tile-based placement validation |
| `TowerSelection.cs` | UI controller for selecting which tower to place |
| `TowerUpgradeUI.cs` | In-game upgrade panel for placed towers |
| `Enemy.cs` | Waypoint-following movement and health |
| `Projectile.cs` | Homing movement, rotation, and hit detection |
| `GhostTower.cs` | Semi-transparent placement preview with validity color |
| `HealthManager.cs` | Player health tracking and game-over handling |
| `MenuManager.cs` | Scene loading utility for menus |

---

## 🌊 Wave System

Waves are defined via the `WaveData` class and configured in the Unity Inspector:

```csharp
[System.Serializable]
public class WaveData
{
    public float duration;    // Total wave duration in seconds
    public int easyEnemies;   // Number of easy enemies
    public int hardEnemies;   // Number of hard enemies
}
```

**Wave flow:**
1. Player clicks **Start Wave** button
2. Easy enemies spawn over the first ⅓ of the wave duration
3. Hard enemies spawn over the second ⅓
4. Final ⅓ is a cooldown before the next wave can begin
5. Wave counter UI updates automatically

---

## 🗼 Tower System

### Placement
- Select a tower from the UI panel
- A **ghost preview** follows the mouse — **green** = valid tile, **red** = invalid
- Click a valid tile to place; coins are deducted automatically
- Already-occupied tiles are blocked from double-placement

### Targeting
Towers scan all active enemies each frame and prioritize the one **furthest along the path** (highest waypoint index) within range.

### Upgrades
Each tower has multiple `TowerUpgradeStage` entries:

```csharp
[System.Serializable]
public class TowerUpgradeStage
{
    public float range;
    public float fireRate;
    public Sprite sprite;
    public int price;
}
```

Click a placed tower to open the upgrade UI. Upgrades cost coins and improve range, fire rate, and appearance.

---

## 👾 Enemy System

Enemies follow a predefined array of **waypoints** through the map:

- Move toward the next waypoint each frame
- On reaching the final waypoint → deal **1 damage** to the player and self-destruct
- Die when health reaches **0** → award **2 coins**
- Two types: **Easy** (low health) and **Hard** (high health), configured per wave

---

## 💰 Economy

| Action | Coin Change |
|--------|------------|
| Kill an enemy | **+2** |
| Place a tower | **−tower.cost** |
| Upgrade a tower | **−upgradeStage.price** |

If you can't afford a tower or upgrade, the action is blocked automatically.

---

## 🛠️ Setup & Inspector Configuration

### WaveManager
- Assign `WaveData[]` waves in the Inspector
- Link `easyEnemyPrefab`, `hardEnemyPrefab`
- Assign ordered `wayPoints[]` array matching your map path
- Link `startWaveButton` and `waveText` UI elements

### Tower Prefab
- Add `Tower.cs` component
- Set `cost`, `range`, `fireRate`
- Assign `ProjectilePrefab` and `firePoint` child Transform
- Configure `upgradeStages[]` array
- Assign `towerUpgradeUIPrefab` and `cloudPS` particle prefab

### TowerPlacer
- Assign `placementMap` (valid tiles) and `nonPlacableMap` (blocked tiles) Tilemaps
- Assign `ghostPrefab` with a `GhostTower` component

---

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── WaveManager.cs
│   ├── Tower.cs
│   ├── TowerPlacer.cs
│   ├── TowerSelection.cs
│   ├── TowerUpgradeUI.cs
│   ├── Enemy.cs
│   ├── Projectile.cs
│   ├── GhostTower.cs
│   ├── HealthManager.cs
│   └── MenuManager.cs
├── Prefabs/
│   ├── Towers/
│   ├── Enemies/
│   └── Projectiles/
├── Scenes/
│   ├── MainMenu
│   └── Game
└── UI/
```

---

## 🧰 Built With

- **Unity 2D**
- **TextMeshPro** — UI text rendering
- **Unity Tilemaps** — isometric placement grid
- **Unity Audio** — SFX via `AudioManager`

---

## 📄 License

This project is for educational/personal use. Feel free to fork and build upon it.

---

<p align="center">Made with ❤️ in Unity</p>
