using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Drop : Area2D
{
	public GameManager gameManager;
	public Data data;
	public Sprite2D sprite;
	public class Data
	{
		public int id;
		public string name;
		public string effect;
		public int amount;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameManager = (GameManager)GetNode("/root/GameManager");
        sprite = GetNode<Sprite2D>("Sprite2D");
		CallDeferred("ItemSetup");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ItemSetup()
	{
		gameManager.GetSprite(sprite, data.id, 16, 12, 12, "res://Assets/Sprites/Drops/Drops.png");
	}

	public void OnAreaEntered(Area2D area)
	{
		if(area.Name == "PickUpArea")
        {
            Player player = area.GetParent<Player>();
            PickUpEffect(player);
        }
	}

	public void PickUpEffect(Player player)
	{
		switch (data.id)
		{
			case 0:
				// Gas Canister
				if (player.gas < (100 - data.amount))
				{
					player.GasChange(data.amount);
					QueueFree();
				}
				break;
			case 1:
				// Health Replenish
                if (player.health < (100 - data.amount))
                {
                    player.HealthChange(data.amount);
                    QueueFree();
                }
                break;
			case 2:
                // Experience Gain
                    player.ExperienceChange(data.amount);
                    QueueFree();
                break;
		}
	}
}
