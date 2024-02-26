using Godot;
using System;
using static Drop;

public partial class AbilityManager : Node
{
    public Player player;
    [Export]
    public PackedScene gasGrenade;
    [Export]
    public PackedScene gasShield;

    public bool shielded = false;
    public class Ability
    {
        public int id;
        public string name;
        public string effect;
        public string type;
        public int cooldown;
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void UseAbility(int abilityID)
    {
        Node2D level = GetNode<Node2D>("/root/Level");
        switch (abilityID)
        {
            case 0:
                player.emitter.emitting = !player.emitter.emitting;
                GD.Print("emitting " + player.emitter.emitting);
                break;
            case 1:
                GasGrenade grenade = (GasGrenade)gasGrenade.Instantiate();
                
                grenade.GlobalPosition = player.GlobalPosition;
                grenade.direction = player.Velocity;
                level.CallDeferred("add_child", grenade);
                break;
            case 2:
                    GasShield shield = (GasShield)gasShield.Instantiate();
                    shield.GlobalPosition = player.GlobalPosition;
                    AddChild(shield);
                break;
            case 3:
                player.emitter.emitting = !player.emitter.emitting;
                break;
        }
    }
}
