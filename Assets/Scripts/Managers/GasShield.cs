using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class GasShield : Area2D
{
	public int damage;
	public int pushBackStrength = 2;
	public Timer shieldTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GlobalPosition = GetParent<AbilityManager>().GetParent<Player>().GlobalPosition;
        shieldTimer = GetNode<Timer>("ShieldTimer");
        shieldTimer.WaitTime = 5f;  // Set the time interval for direction change or pause
        shieldTimer.Timeout += () =>
        {
            QueueFree();
        };
        shieldTimer.Start();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        GlobalPosition = GetParent<AbilityManager>().GetParent<Player>().GlobalPosition;
        var overlappingAreas = GetOverlappingAreas();

        foreach (Area2D area in overlappingAreas)
        {
            if (area.Name == "DamageCollider")
            {
                Enemy enemy = area.GetParent<Enemy>();
                enemy.PushBack(2);
            }
        }
    }

	public void OnAreaEntered(Area2D area) 
	{ 
	
	}
}
