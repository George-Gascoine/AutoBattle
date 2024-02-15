using Godot;
using System;

public partial class GasCanister : Area2D
{
	public int gasValue = 5;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnAreaEntered(Area2D area)
	{
		if(area.Name == "PickUpArea")
        {
            Player player = area.GetParent<Player>();
			if(player.gas < (100 - gasValue))
			{
                player.GasChange(gasValue);
                QueueFree();
            }
        }
	}
}
