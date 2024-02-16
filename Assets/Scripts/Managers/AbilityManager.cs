using Godot;
using System;

public partial class AbilityManager : Node
{
    public Player player;
    public class Ability
    {
        public int id;
        public string name;
        public string effect;
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
        switch (abilityID)
        {
            case 0:
                player.emitter.emitting = !player.emitter.emitting;
                break;
            case 1:
                player.emitter.emitting = !player.emitter.emitting;
                break;
            case 2:
                player.emitter.emitting = !player.emitter.emitting;
                break;
            case 3:
                player.emitter.emitting = !player.emitter.emitting;
                break;
        }
    }
}
