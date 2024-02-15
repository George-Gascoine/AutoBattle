using Godot;
using System.Threading;
using Timer = Godot.Timer;

public partial class Particle : RigidBody2D
{
    private Player player;
    private Area2D area;
    private Timer lifeTimer;
    public int lifespan = 2;

    public override void _Ready()
    {
        lifeTimer = GetNode<Timer>("LifeTimer");
        LifeTimerSetup(lifespan);
        player = (Player)GetNode("/root/World/Player");
        SetSpeed(player.Velocity);
        AddCollisionExceptionWith(player);
        AddCollisionExceptionWith(this);
    }

    private void OnAreaEntered(Area2D area)
    {
        // Check if the area is the one you're interested in
        if (area.Name == "DamageCollider")
        {
            GD.Print("Particle collision detected!");
            Enemy enemy = area.GetParent<Enemy>();
            enemy.TakeDamage(player.gasDamage);
        }
    }

    private void SetSpeed(Vector2 speed)
    {
        LinearVelocity = new Vector2(- speed.X, -30);
    }

    private void LifeTimerSetup(int time)
    {
        lifeTimer = GetNode<Timer>("LifeTimer");
        lifeTimer.WaitTime = time;  // Set the time interval for direction change or pause
        lifeTimer.Timeout += () =>
        {
            QueueFree();
        };
        lifeTimer.Start();
    }
}
