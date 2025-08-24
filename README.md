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

## **Modding Your Universe**

This game was built from the ground up to be modified. You can add new miracles, create random events, and even define new visual tiers for your civilization.

### **Finding the Mods Folder**

First, you need to find the game's user data directory. The game will create a Mods folder here on its first launch.

* **Windows:** %APPDATA%\\Godot\\app\_userdata\\ParasiticGod\\Mods\\
* **macOS:** \~/Library/Application Support/Godot/app\_userdata/ParasiticGod/Mods/
* **Linux:** \~/.local/share/godot/app\_userdata/ParasiticGod/Mods/

Inside, you'll find three folders: Miracles, Tiers, and Events. The game also loads a set of base mods from its installation directory (res://Mods), and any files you place in the user folder will be added to or will override the base game's content.

### **Creating a New Miracle**

To add a new miracle, simply create a new .json file in the Mods/Miracles folder. The filename will be its unique **ID** (e.g., my\_cool\_miracle.json).

{  
"name": "My Cool Miracle",  
"faithCost": 100,  
"followersRequired": 50,  
"productionRequired": 0,  
"unlockedByDefault": true,  
"advancesToAge": "The Cool Age",  
"effects": \[  
{  
"type": "AddResource",  
"targetResource": "Faith",  
"value": 200  
}  
\]  
}

### **Creating a New Event**

To add a new random event, create a .json file in the Mods/Events folder.

{  
"id": "event\_my\_event",  
"title": "A Thing Happened\!",  
"description": "Something unexpected occurred. What will you do?",  
"meanTimeToHappen": 120,  
"trigger": {  
"minFollowers": 100,  
"maxCorruption": 50  
},  
"options": \[  
{  
"text": "Do the thing\!",  
"tooltip": "Gain 50 Production.",  
"effects": \[  
{  
"type": "AddResource",  
"targetResource": "Production",  
"value": 50  
}  
\]  
}  
\]  
}

### **Modifying Visual Tiers**

You can change the visual progression of followers, huts, and temples by editing the files in Mods/Tiers. The format is a list of tiers, sorted by their threshold.

{  
"tiers": \[  
{  
"tierEnum": "Tier1",  
"threshold": 0,  
"imagePath": "user://Mods/Tiers/Huts/my\_custom\_hut.png",  
"scale": { "x": 1.0, "y": 1.0 }  
}  
\]  
}

* **tierEnum**: Must be one of Tier1 through Tier10.
* **threshold**: The number of followers needed to unlock this visual.
* **imagePath**: The path to the image file. Can be a user:// path for mods or a res:// path for base game assets.
* **scale**: An optional X/Y scale multiplier for the image.

### **Available Effect Types**

This is the core of the modding system. Both miracles and event options use this list of effects.

| Type | Description | Parameters |
| :---- | :---- | :---- |
| **AddResource** | Adds or subtracts from a core stat. | targetResource (Stat), value (number) |
| **ConvertResource** | Trades one resource for another. | fromResource (Stat), fromAmount (number), toResource (Stat), toAmount (number) |
| **ModifyStat** | Permanently changes a passive stat. | targetStat (Stat), op ("Add" or "Multiply"), value (number) |
| **ApplyBuff** | Applies a temporary multiplier. | buffId (string), targetStat (Stat), multiplier (number), duration (seconds) |
| **UnlockMiracle** | Unlocks other miracles. | miraclesToUnlock (list of miracle IDs) |
| **DestroySelf** | Removes the miracle's button after use. | (No parameters) |
| **Win** | Triggers the game's win condition. | (No parameters) |

**Valid Stat Names:** Faith, Followers, Corruption, Production, ProductionPerSecond, CorruptionPerSecond, FollowersPerSecond, FaithPerFollower, ProductionPerFollower.

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