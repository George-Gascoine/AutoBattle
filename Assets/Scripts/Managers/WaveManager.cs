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
        Level levelNode = GetNode<Level>("/root/Level");
        int noOfSpawns = 0;
        for (int a = 0; a < waveToSpawn.enemies.Length; a++)
        {
            noOfSpawns += waveToSpawn.number[a];
        }
        List<Vector2> spawnPoints = new();
        spawnPoints = GetSpawnPoints(noOfSpawns, gameManager.player.Velocity);
        for (int i = 0; i < waveToSpawn.enemies.Length; i++)
        {
            for (int j = 0; j < waveToSpawn.number[i]; j++)
            {
                if(GetTree().GetNodesInGroup("Enemies").Count < 400)
                {
                    Enemy.EnemyData enemyData = gameManager.enemyData.First(enemy => enemy.id == waveToSpawn.enemies[i]);
                    Enemy instance = (Enemy)baseEnemy.Instantiate();
                    instance.data = enemyData;
                    Vector2 chosenPosition = spawnPoints[random.Next(spawnPoints.Count)];
                    instance.Position = chosenPosition;
                    spawnPoints.Remove(chosenPosition);
                    instance.CallDeferred("EnemySetup");
                    levelNode.AddChild(instance);
                    instance.AddToGroup("Enemies");
                    GD.Print("Enemy Number " + GetTree().GetNodesInGroup("Enemies").Count);
                }
            }
        }
    }

    public List<Vector2> GetSpawnPoints(int numberOfPoints, Vector2 direction)
    {
        // Get the camera size
        Vector2 cameraSize = gameManager.player.camera.GetViewportRect().Size;

        // Create the Rect2
        Rect2 rect;
        float distanceBetweenPoints;
        if (direction.Y > 0) // South
        {
            // Calculate the distance between each point
            rect = new Rect2(new Vector2(gameManager.player.GlobalPosition.X - cameraSize.X / 2, gameManager.player.GlobalPosition.Y + cameraSize.Y / 2), new Vector2(cameraSize.X * 2, cameraSize.Y / 3));
            distanceBetweenPoints = rect.Size.X / (numberOfPoints - 1);
        }
        else if (direction.Y < 0) // North
        {
            rect = new Rect2(new Vector2(gameManager.player.GlobalPosition.X - cameraSize.X / 2, gameManager.player.GlobalPosition.Y - cameraSize.Y / 2), new Vector2(cameraSize.X * 2, cameraSize.Y / 3));
            distanceBetweenPoints = rect.Size.X / (numberOfPoints - 1);
        }
        else // East or West
        {
            rect = new Rect2(new Vector2(gameManager.player.GlobalPosition.X + cameraSize.X / 2, gameManager.player.GlobalPosition.Y - cameraSize.Y / 2), new Vector2(cameraSize.X / 3, cameraSize.Y * 2));
            distanceBetweenPoints = rect.Size.Y / (numberOfPoints - 1);
        }

        // Calculate the distance between each point
        // var distanceBetweenPoints = rect.Size.Y / (numberOfPoints - 1);

        // Generate the points
        List<Vector2> points = new();
        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector2 pointToAdd = new();
            if (direction.Y < 0 || direction.Y > 0)
            {
                pointToAdd = new(rect.Position.X + i * distanceBetweenPoints, rect.Position.Y);
            }
            else if (direction.X < 0)
            {
                pointToAdd = new(rect.Position.X, rect.Position.Y + i * distanceBetweenPoints);
                Vector2 cameraPosition = gameManager.player.camera.GlobalPosition;
                pointToAdd = cameraPosition - pointToAdd;
            }
            else
            {
                pointToAdd = new(rect.Position.X, rect.Position.Y + i * distanceBetweenPoints);
            }
            points.Add(pointToAdd);
        }

        return points;
    }

    public Vector2 AdjustForCamera(Vector2 point, Vector2 direction)
    {
        Vector2 pointToAdd = new();
        //// Get the camera position
        Vector2 cameraPosition = gameManager.player.camera.GlobalPosition;
        //if (direction.Y < 0 || direction.X < 0)
        //{
        //    pointToAdd = cameraPosition - point;
        //}
        //else if (direction.Y > 0 || direction.X > 0)
        //{
        //    pointToAdd = cameraPosition + point;
        //}
        pointToAdd = cameraPosition + point;
        return pointToAdd;
    }
}
