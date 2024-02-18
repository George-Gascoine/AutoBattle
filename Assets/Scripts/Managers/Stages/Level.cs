using Godot;
using System;
using System.Collections.Generic;
using static GameManager;

public partial class Level : Node2D
{
	[Export]
	public int id;
	public class LevelData
	{
		public int id;
		public string name;
		public int[] waveIDs;
		public int[] times;
	}

	public LevelData data;

	public WaveManager waveManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		waveManager = GetNode<WaveManager>("WaveManager");
		waveManager.CallDeferred("PrepareWaves");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
