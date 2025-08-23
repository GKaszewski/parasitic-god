# Parasitic God
![Made With Godot](https://img.shields.io/badge/Made%20With-Godot-478CBF?style=for-the-badge&logo=godot-engine&logoColor=white)  
![Lines of Code](https://tokei.rs/b1/github/GKaszewski/parasitic-god?style=for-the-badge&category=code)  
![Last Commit](https://img.shields.io/github/last-commit/GKaszewski/parasitic-god?style=for-the-badge)  
![Stars](https://img.shields.io/github/stars/GKaszewski/parasitic-god?style=for-the-badge)
## The Concept

You are a nascent god, tethered to a small tribe of followers on a vibrant, living world. Their worship is your lifeblood, their growth your only purpose. You grant them miracles, blessing them with bountiful harvests and inspiring great works.

But your power comes at a cost. Every miracle that nurtures your civilization also poisons the planet. The soil turns barren, the forests wither, and the sky darkens. You are their savior and their apocalypse.

## The Mission

Guide your people from a simple tribe to a star-faring civilization capable of escaping the dying world. Manage your resources **Faith**, **Followers**, and **Production** while trying to keep the planet's ever-rising **Corruption** at bay.

Unlock new ages of technology, build a network of cities, and perform the final, desperate miracle to launch your followers into the stars before you consume everything.

## Features
* **Exponential Growth**
  Watch a handful of followers grow into a massive civilization with huts, cities, and procedural road networks.

* **A World That Reacts**
  See the direct consequences of your actions as the vibrant globe fades to a corrupted wasteland and forests vanish based on your decisions.

* **Deeply Moddable**
  The entire game from miracles and their effects to the visual tiers of your civilization is driven by simple JSON files. If you can edit a text file, you can mod this game.

* **Strategic Resource Management**
  Balance the generation of multiple resources and make difficult choices. Will you sacrifice followers to purge corruption, or push for industrial growth at any cost?

## Modding Your Universe

This game was built from the ground up to be modified. You can add new miracles, change existing ones, and even define new visual tiers for your civilization.

### Finding the Mods Folder

First, you need to find the game's `user data` directory. The game will create a `Mods` folder here on its first launch.

* **Windows:** `%APPDATA%\Godot\app_userdata\ParasiticGod\Mods\`
* **macOS:** `~/Library/Application Support/Godot/app_userdata/ParasiticGod/Mods/`
* **Linux:** `~/.local/share/godot/app_userdata/ParasiticGod/Mods/`

Inside, you'll find two folders: `Miracles` and `Tiers`.

### Creating a New Miracle

To add a new miracle, simply create a new `.json` file in the `Mods/Miracles` folder. The filename will be its unique **ID** (e.g., `my_cool_miracle.json`).

Here is a template with all available fields:

```json
{
  "name": "My Cool Miracle",
  "faithCost": 100,
  "followersRequired": 50,
  "productionRequired": 0,
  "unlockedByDefault": true,
  "advancesToAge": "",
  "effects": [
    {
      "type": "AddResource",
      "targetResource": "Faith",
      "value": 200
    }
  ]
}
```

### Available Effect Types

This is the core of the modding system. Each miracle can have one or more effects.

| Type | Description | Parameters |
| :--- | :--- | :--- |
| **`AddResource`** | Adds or subtracts from a core stat. | `targetResource` (Stat), `value` (number) |
| **`ConvertResource`** | Trades one resource for another. | `fromResource` (Stat), `fromAmount` (number), `toResource` (Stat), `toAmount` (number) |
| **`ModifyStat`** | Permanently changes a passive stat. | `targetStat` (Stat), `op` ("Add" or "Multiply"), `value` (number) |
| **`ApplyBuff`** | Applies a temporary multiplier. | `targetStat` (Stat), `multiplier` (number), `duration` (seconds) |
| **`UnlockMiracle`** | Unlocks other miracles. | `miraclesToUnlock` (list of miracle IDs) |
| **`DestroySelf`** | Removes the miracle's button after use. | (No parameters) |

**Valid Stat Names:** `Faith`, `Followers`, `Corruption`, `Production`, `ProductionPerSecond`, `CorruptionPerSecond`, `FaithPerFollower`.

### Modifying Tiers

You can change the progression of visuals like followers and huts by editing the files in `Mods/Tiers`. For example, to change when followers get new looks, edit `follower_tiers.json`.

The format is a list of tiers, sorted by their threshold.

```json
{
  "tiers": [
    {
      "tierEnum": "Tier1",
      "threshold": 0,
      "scenePath": "res://Scenes/Followers/followers_tier_1.tscn"
    },
    {
      "tierEnum": "Tier2",
      "threshold": 200,
      "scenePath": "res://Scenes/Followers/followers_tier_2.tscn"
    }
  ]
}
```

* **`tierEnum`**: Must be one of `Tier1`, `Tier2`, `Tier3`, `Tier4`, `Tier5`.
* **`threshold`**: The number of followers (or other stat) needed to unlock this visual.
* **`scenePath`**: The path to the Godot scene (`.tscn`) to display for this tier. You can even point to your own custom scenes if you're an advanced modder\!

-----

## 📊 Project Stats
📦 **Lines of Code:**  
![Lines of code](https://tokei.rs/b1/github/GKaszewski/parasitic-god)

📈 **Repo Activity:**  
![Commit activity](https://img.shields.io/github/commit-activity/m/GKaszewski/parasitic-god)

---

## License

This project is open source. See the [LICENSE](https://www.google.com/search?q=./LICENSE) file for details.

-----

## Contributing

While the core code is complete for the jam, you can help by:

* Reporting bugs or balance issues.
* Creating cool new miracles and sharing them.
* Spreading the word\!