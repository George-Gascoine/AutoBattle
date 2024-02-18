using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static Godot.OpenXRInterface;
using static Player;

public partial class WaveManager : Node2D
{
    public GameManager gameManager;
    public Level parentLevel;
    public int lastExecutionSecond = 0;
    public bool waveChecked;
    public Random random = new();
    public float radius = 100f; // Radius for the random position
    [Export]
    public PackedScene baseEnemy;
    public class Wave
    {
        public int id;
        public int[] enemies;
        public int[] number;
        public string[] locations;
    }

    public Dictionary<int, Wave> waveTimings = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        parentLevel = GetParent<Level>();
        waveChecked = false;
        PrepareWaves();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!waveChecked)
        {
            // Check if a full second has passed
            if (gameManager.time.Seconds > lastExecutionSecond)
            {
                // Update the last execution second
                lastExecutionSecond = gameManager.time.Seconds;
                // Execute your method here
                WaveCheck();
            }
        }
    }

    public void PrepareWaves()
    {
        waveTimings.Clear();
        for (int i = 0; i < parentLevel.data.waveIDs.Length; i++)
        {
            Wave addWave = gameManager.waveData.First(wave => wave.id == parentLevel.data.waveIDs[i]);
            waveTimings.Add(parentLevel.data.times[i], addWave);
        }
    }

    public void WaveCheck()
    {
        foreach (KeyValuePair<int, Wave> wave in waveTimings)
        {
            if (wave.Key == gameManager.time.Seconds)
            {
                SpawnWaves(wave.Value);
            }
        }
    }

    public void SpawnWaves(Wave waveToSpawn)
    {
        GD.Print("Spawning");
        Level levelNode = GetNode<Level>("/root/Level");
        int noOfSpawns = 0;
        for (int a = 0; a < waveToSpawn.enemies.Length; a++)
        {
            noOfSpawns += waveToSpawn.number[a];
        }
        List<Vector2> spawnPoints = new();
        spawnPoints = GetSpawnPoints(noOfSpawns);
        for (int i = 0; i < waveToSpawn.enemies.Length; i++)
        {
            for (int j = 0; j < waveToSpawn.number[i]; j++)
            { 
                Enemy.EnemyData enemyData = gameManager.enemyData.First(enemy => enemy.id == waveToSpawn.enemies[i]);
                Enemy instance = (Enemy)baseEnemy.Instantiate();
                instance.data = enemyData;
                Vector2 chosenPosition = spawnPoints[random.Next(spawnPoints.Count)];
                instance.Position = chosenPosition;
                spawnPoints.Remove(chosenPosition);
                instance.CallDeferred("EnemySetup");
                levelNode.AddChild(instance);
            }
        }
    }

    public List<Vector2> GetSpawnPoints(int numberOfPoints)
    {
        // Get the camera size
        Vector2 cameraSize = gameManager.player.camera.GetViewportRect().Size;

        // Create the Rect2
        var rect = new Rect2(new Vector2(cameraSize.X * 2 / 3, 0), new Vector2(cameraSize.X / 3, cameraSize.Y));

        // Calculate the distance between each point
        var distanceBetweenPoints = rect.Size.Y / (numberOfPoints - 1);

        // Generate the points
        List<Vector2> points = new();
        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector2 pointToAdd = new(rect.Position.X, rect.Position.Y + i * distanceBetweenPoints);
            points.Add(pointToAdd);
        }

        return points;
    }
}
