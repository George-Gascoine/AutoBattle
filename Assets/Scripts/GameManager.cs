using Godot;
using System;

public partial class GameManager : Node2D
{
	public bool paused;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		paused = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
