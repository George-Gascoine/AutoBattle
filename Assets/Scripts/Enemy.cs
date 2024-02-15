using Godot;

public partial class Enemy : CharacterBody2D
{

    public GameManager gameManager;
    public Player player { get; set; }
    public AnimationPlayer animationPlayer { get; set; }
    public AnimationPlayer damagePlayer { get; set; }

    public bool takingDamage = false;
    [Export]
    public int Speed { get; set; } = 32;

    public int health = 100;

    private Vector2 _target;

    public bool playerFound;

    [Export]
    public PackedScene gasDrop;
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        damagePlayer = GetNode<AnimationPlayer>("DamagePlayer");
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
            health -= damage;
            gameManager.CreateDamageNumber(damage, Position);
            if (health <= 0)
            {
                DestroyAndDrop();
            }
            takingDamage = true;
            damagePlayer.Play("TakeDamage");
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

    public void DestroyAndDrop()
    {
        GasCanister canister = (GasCanister)gasDrop.Instantiate();
        Node2D world = GetNode<Node2D>("/root/World");
        world.CallDeferred("add_child", canister);
        canister.Position = Position;
        QueueFree();
    }
}