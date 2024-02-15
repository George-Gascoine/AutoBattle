using Godot;
using System;

public partial class ParticleEmitter : Node2D
{
    public Player player;
    public bool emitting;
    [Export]
    public PackedScene particleScene;
    public Timer emitTimer;
    public Vector2 emitterPos;
    public override void _Ready()
    {
        player = GetParent<Player>();
        emitTimer = GetNode<Timer>("EmitTimer");
        emitTimer.WaitTime = .1f;  // Set the time interval for direction change or pause
        emitTimer.Connect("timeout", new Callable(this, "EmitParticle"));
        emitTimer.Start();
    }

    public override void _Process(double delta)
    {
        GD.Print(Engine.GetFramesPerSecond());
    }
    private void EmitParticle()
    {
        if (emitting && player.gas > 0) 
        {
            player.GasChange(-.1f);
            // Create a new particle
            Particle particle = (Particle)particleScene.Instantiate();
            Node2D world = GetNode<Node2D>("/root/World");
            world.AddChild(particle);

            Random modifier = new();
            // Set the position of the particle
            emitterPos = new Vector2(GetParent<Player>().Position.X + 10, GetParent<Player>().Position.Y);
            particle.Position = GlobalPosition + new Vector2(modifier.Next(-5, 5), modifier.Next(-5, 5));
        }
    }
}
