using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

/// <summary>
/// Rebuilds the StageScene1 floor and obstacle layout for map 10 on every run.
/// Existing scene tile assets and colliders are reused so the normal combat setup stays intact.
/// </summary>
public class ProceduralMapGenerator : MonoBehaviour
{
    private const string RunCountKey = "Map10RunCount";
    private const int GroundY = -6;
    private const int StartX = -12;
    private const int EndX = 90;

    private static int currentSeed;
    private static int currentRun;
    private GUIStyle infoStyle;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterSceneHook()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "StageScene1" || PlayerPrefs.GetInt("SelectedMap", 0) != 10)
            return;

        GameObject host = new GameObject("ProceduralMapGenerator");
        ProceduralMapGenerator generator = host.AddComponent<ProceduralMapGenerator>();
        generator.Generate();
    }

    private void Generate()
    {
        Tilemap floor = FindTilemap("Floor");
        Tilemap walls = FindTilemap("Wall");
        if (floor == null || walls == null)
        {
            Debug.LogError("Map 10 could not find the Floor and Wall tilemaps in StageScene1.");
            enabled = false;
            return;
        }

        TileBase floorTile = FindFirstTile(floor);
        TileBase wallTile = FindFirstTile(walls);
        if (floorTile == null || wallTile == null)
        {
            Debug.LogError("Map 10 needs at least one existing floor tile and wall tile as generation templates.");
            enabled = false;
            return;
        }

        currentSeed = unchecked((int)(DateTime.UtcNow.Ticks ^ Environment.TickCount));
        currentRun = PlayerPrefs.GetInt(RunCountKey, 0) + 1;
        PlayerPrefs.SetInt(RunCountKey, currentRun);
        PlayerPrefs.SetInt("Map10LastSeed", currentSeed);
        PlayerPrefs.Save();

        System.Random random = new System.Random(currentSeed);
        floor.ClearAllTiles();
        walls.ClearAllTiles();

        // A continuous base keeps the existing side-scrolling controls playable.
        for (int x = StartX; x <= EndX; x++)
        {
            floor.SetTile(new Vector3Int(x, GroundY, 0), floorTile);
            walls.SetTile(new Vector3Int(x, GroundY - 1, 0), wallTile);
            walls.SetTile(new Vector3Int(x, GroundY - 2, 0), wallTile);
        }

        // Build varied obstacle groups with clear lanes between them for jumping and combat.
        int cursor = 3;
        int obstacleGroups = 8 + Math.Min(currentRun / 2, 5);
        for (int group = 0; group < obstacleGroups && cursor < EndX - 7; group++)
        {
            cursor += random.Next(4, 9);
            int width = random.Next(1, 3);
            int height = random.Next(1, 4);
            for (int dx = 0; dx < width; dx++)
            {
                for (int dy = 0; dy < height; dy++)
                    walls.SetTile(new Vector3Int(cursor + dx, GroundY + 1 + dy, 0), wallTile);
            }

            // Some groups add a short raised platform, producing different jump routes.
            if (random.NextDouble() < 0.35)
            {
                int platformStart = cursor + random.Next(3, 6);
                int platformY = GroundY + random.Next(3, 5);
                int platformWidth = random.Next(2, 5);
                for (int dx = 0; dx < platformWidth; dx++)
                    floor.SetTile(new Vector3Int(platformStart + dx, platformY, 0), floorTile);
            }

            cursor += random.Next(4, 8);
        }

        Debug.Log($"Map 10 generated. Run {currentRun}, seed {currentSeed}.");
    }

    private static Tilemap FindTilemap(string objectName)
    {
        GameObject target = GameObject.Find(objectName);
        return target != null ? target.GetComponent<Tilemap>() : null;
    }

    private static TileBase FindFirstTile(Tilemap tilemap)
    {
        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);
            if (tile != null)
                return tile;
        }
        return null;
    }

    private void OnGUI()
    {
        if (infoStyle == null)
        {
            infoStyle = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 16,
                normal = { textColor = Color.white }
            };
        }

        GUI.Box(new Rect(12f, 12f, 270f, 34f), $"RANDOM RUN {currentRun}  |  SEED {currentSeed}", infoStyle);
    }
}
