using Godot;

public partial class Enemy : CharacterBody2D
{
    public GameManager gameManager;
    public Player player { get; set; }
    public AnimationPlayer animationPlayer { get; set; }

    public bool takingDamage = false;
    [Export]
    public int Speed { get; set; } = 32;

    private Vector2 _target;

    public bool playerFound;
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        CallDeferred("FindPlayer");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (playerFound)
        {
            _target = player.Position;
            Velocity = Position.DirectionTo(_target) * Speed;
            MoveAndSlide();
        }
        if (!gameManager.paused && !takingDamage) 
        {
            if (Position.DirectionTo(_target).X < 0)
            {
                ChangeDirection("left");
            }
            if (Position.DirectionTo(_target).X > 0)
            {
                ChangeDirection("right");
            }
            animationPlayer.Play("Walk");
        }
    }

    public void FindPlayer()
    { 
        player = (Player)GetNode("/root/World/Player"); 
        playerFound = true;
    }

    public void ChangeDirection(string direction)
    {
        switch (direction) 
        {
            case "left":
                GlobalTransform = new Transform2D(new Vector2(-1, 0), new Vector2(0, 1), new Vector2(Position.X, Position.Y));
                break;
            case "right":
                GlobalTransform = new Transform2D(new Vector2(1, 0), new Vector2(0, 1), new Vector2(Position.X, Position.Y));
                break;
            default:
                break;
        }
    }


    public void TakeDamage(int damage)
    {
        if (!takingDamage)
        {
            takingDamage = true;
            animationPlayer.Play("TakeDamage");
        }
    }

    public void OnAnimationEnd(string animName)
    {
        switch (animName)
        {
            case "TakeDamage":
                takingDamage = false;
                break;
        }
    }
}