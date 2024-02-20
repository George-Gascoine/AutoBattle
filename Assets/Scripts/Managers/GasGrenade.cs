using Godot;

public partial class GasGrenade : Node2D
{
	public GameManager gameManager;
	public AnimationPlayer animPlayer;
    public Area2D area;
	public Timer explosionTimer;
	public CollisionShape2D collisionShape;
	public int damage = 10;
	public bool exploding;
    public Vector2 direction = new(1, 0);
    public float speed = 2f;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        explosionTimer = GetNode<Timer>("ExplosionTimer");
        area = GetNode<Area2D>("Area2D");
        explosionTimer.WaitTime = 2f;  // Set the time interval for direction change or pause
        explosionTimer.Timeout += () =>
        {
            speed = 0;
            animPlayer.Play("Explode");
			exploding = true;
        };
        explosionTimer.Start();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        GlobalPosition += speed * direction * (float)delta;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (exploding)
        {
            var overlappingAreas = area.GetOverlappingAreas();

            foreach (Area2D area in overlappingAreas)
            {
                if (area.Name == "DamageCollider")
                {
                    Enemy enemy = area.GetParent<Enemy>();
                    enemy.TakeDamage(damage);
                }
            }
        }
    }


    public void OnAreaEntered(Area2D area)
	{
    }

	public void OnAnimationEnd(string animation)
	{
		switch (animation)
		{
			case "Explode":
				QueueFree();
				break;
			default:
				break;
		}
	}
}
