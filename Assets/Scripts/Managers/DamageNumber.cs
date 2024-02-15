using Godot;
using System;

public partial class DamageNumber : Label
{
	public int damageTaken;
	public Timer destroyTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void DisplayDamage()
	{
        Text = damageTaken.ToString();
        destroyTimer = GetNode<Timer>("DestroyTimer");
        destroyTimer.WaitTime = 1f;  // Set the time interval for direction change or pause
        destroyTimer.Timeout += () =>
        {
            QueueFree();
        };
        destroyTimer.Start();
    }
}
