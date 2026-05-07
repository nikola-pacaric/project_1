# Project 1 - 2D Survival Shooter
Project 1 is a small Unity 2D survival shooter built as a portfolio project. The player moves around a confined arena, automatically fires at nearby enemies, survives increasingly aggressive enemy waves, collects upgrades, and submits the final run score to an online leaderboard.

This repository represents the game as it exists from an earlier stage of my learning process. Some systems are simple, direct, and prototype-like by design. The project is presented with those decisions visible, because they show the level of experience and implementation choices I had at the time.

## Project Structure
The active Unity project lives in this repository. Related deployment pieces exist beside it in different repos:

- `project_1` - Unity source project and main Git repository.
- `survival-backend` - Node.js / Express / MongoDB backend used by the leaderboard.
- `survival-frontend` - exported Unity WebGL build that loads the game in the browser.

## Engine and Packages
The project uses Unity 6 with the 2D Universal Render Pipeline setup.

The active build scenes are:
- `MainMenu`
- `MainScene`

## Gameplay Overview
The player starts in the main game scene and tries to survive for as long as possible. Enemies spawn outside the camera view and move toward the player. The player does not manually aim; instead, the shooting system automatically targets enemies and fires at intervals.

The core loop is:
1. Move around the arena.
2. Avoid enemies.
3. Automatically shoot the closest enemies.
4. Kill enemies to gain score.
5. Collect upgrade drops.
6. Survive as long as possible.
7. On death, submit the run result to the backend.
8. Enter a player name and view leaderboard results.

## Implemented Systems

### Game State and Run Tracking
`GameManager.cs` controls the central state of a run.

The run ID is generated with a GUID when a run starts. When the game ends, the manager sends the run data to the backend.
The manager also exposes events for score, kills, and game-over changes so UI can update without constantly polling every value manually.

### Player Movement
`PlayerController.cs` handles simple 2D movement.

### Automatic Shooting
`PlayerShooting.cs` handles the auto-fire system.

The shooting system:
- Fires based on the current fire-rate value.
- Finds enemies by tag.
- Targets the closest enemy when firing one bullet.
- Sorts enemies by distance when multi-shot is unlocked.
- Instantiates bullet prefabs from a fire point.
- Applies Rigidbody2D velocity toward the selected target.
- Plays a firing sound through the sound manager.

This is a direct gameplay implementation. It favors clear behavior over optimization, so enemy lookup uses scene searches such as `FindGameObjectsWithTag`.

### Bullet Combat
`Bullet.cs` controls bullet lifetime and enemy collision.

### Enemy Behavior
`EnemyBase.cs` is the shared enemy script.

Enemies:
- Locate the player by tag.
- Move directly toward the player.
- Trigger game over when colliding with the player.
- Take damage from bullets.
- Add score when killed.
- Register kills with the game manager.
- Spawn upgrade drops based on a random chance.
- Play death VFX and audio.

Different enemy prefabs reuse this base behavior with different serialized values and visuals.

### Survival Spawning
`SurvivalSpawner.cs` controls enemy spawning.

The spawner:
- Spawns enemies just outside the camera bounds.
- Picks a random side of the screen.
- Decreases spawn interval over time.
- Introduces stronger enemy types as the run gets longer.
- Spawns a special enemy after a configured survival time.
- Resets its timers when the player retries.

Enemy progression is time-based.

### Upgrade Drops
`DropPickup.cs` implements collectible upgrades.
Current drop types:
- Fire rate upgrade
- Movement speed upgrade
- Bullet count upgrade

`PlayerUpgrades.cs` stores and applies player upgrade values:
- Fire rate
- Bullet speed
- Movement speed
- Bullet count

### UI and Menus
`UIController.cs` controls the in-game HUD and game-over screen.

`MainMenu.cs` controls the main menu scene.

`LeaderboardUI.cs` is used in the main menu leaderboard view. It fetches leaderboard data from the backend and creates UI rows from a prefab.

### Backend API Integration
`BackendAPI.cs` is the Unity-side API layer.

It uses `UnityWebRequest` to communicate with a deployed backend hosted on Render.

Implemented calls:
- `SendRunData` - sends a completed run to the backend.
- `UpdateRunName` - updates the display name for an existing run.
- `GetLeaderboard` - fetches leaderboard data.

The data model sent to the backend includes:
- `runId`
- `displayName`
- `score`
- `enemiesKilled`
- `timeSurvived`

The leaderboard JSON response is parsed with a small `JsonHelper` wrapper because Unity's built-in `JsonUtility` does not parse a top-level JSON array directly.

### Backend Service
The related `survival-backend` project is a small Node.js backend.

It uses:
- Express
- CORS
- MongoDB driver
- dotenv
- MongoDB Atlas

Backend endpoints:
- `GET /` - basic health check.
- `POST /runs` - saves a completed run.
- `PATCH /runs/:id` - updates a run display name.
- `PUT /runs/:id` - updates a run display name for Unity.
- `GET /leaderboard` - returns the top 10 runs.

Leaderboard sorting is based on:
1. Highest score first.
2. Earlier creation time as a tie-breaker.

### WebGL Frontend Build
The related `survival-frontend` folder contains an exported Unity WebGL build.

### Audio
Audio is handled through `SoundManager.cs`.

The project includes:
- Background music loop
- Laser/fire sound
- Enemy death sound

### Camera Feedback
`CameraShake.cs` provides a short camera shake coroutine used during the game-over sequence.

The shake effect offsets the camera locally for a configured duration and intensity, then restores the original position.

## Assets

### Prefabs
The project contains prefabs for:
- Player bullets
- Enemy types
- Special enemy
- Upgrade drops
- Leaderboard UI rows
- Death burst VFX
- Laser impact VFX

### Sprites and Animation
The project uses sprite sheets for:
- Player character
- Enemy characters
- Tile/map visuals

There are animation clips and controllers for the player and enemy sprites.

### Tilemap and Visual Setup
The game uses Unity 2D tilemap assets for the play area. The project also includes URP 2D renderer settings and a global volume profile.

## Current Limitations and Rough Edges
This project intentionally remains close to the version built during an earlier learning stage. Some known rough edges are:
- The backend URL is hardcoded in the Unity script.
- The run is saved on game over before the player enters a display name, then the name is updated afterward.
- If name update happens before the backend insert finishes, the backend can fail to find the run.
- Some systems use direct scene searches such as `FindGameObjectsWithTag`, which is simple but not ideal for scaling.
- The backend has minimal validation and error handling.
- The WebGL frontend is the default Unity export page, not a custom designed web page.
- Some naming is inconsistent because the project evolved from earlier `project_no_sql` versions.
- The README and project structure were originally minimal, so this document was added later as portfolio documentation.

## What This Project Demonstrates
This project demonstrates:
- Unity 2D gameplay scripting
- Scene-based menu/game flow
- Player movement and bounds control
- Automatic targeting and shooting
- Enemy spawning and progression
- Collectible upgrades
- Score, kills, and timer tracking
- Game-over and retry flow
- Basic UI with TextMesh Pro
- Runtime prefab spawning
- Audio playback
- Camera feedback
- Unity WebGL export
- REST API communication from Unity
- Online leaderboard persistence with Express and MongoDB

The result is a complete small game loop with local gameplay systems and an external backend connection.
