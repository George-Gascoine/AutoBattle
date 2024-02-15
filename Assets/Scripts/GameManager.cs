using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class GameManager : Node2D
{
    public Player player;
    public UI UI { get; set; }
    [Export]
    public PackedScene damageNumber;
    public bool paused;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		paused = false;
        player = (Player)GetNode("/root/World/Player");
        UI = (UI)GetNode("/root/World/UI");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        GetInput();
	}
    public void GetInput()
    {
        if (Input.IsActionJustPressed("1"))
        {
            player.emitter.emitting = !player.emitter.emitting;
        }
    }
    public void CreateDamageNumber(int damage, Vector2 position)
	{
        DamageNumber number = (DamageNumber)damageNumber.Instantiate();
        Node2D world = GetNode<Node2D>("/root/World");
        world.AddChild(number);
		number.Position = position - new Vector2(6,20);
		number.damageTaken = damage;
		number.DisplayDamage();
    }
}
