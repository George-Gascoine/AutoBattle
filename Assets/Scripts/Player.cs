using Godot;

public partial class Player : CharacterBody2D
{
    public GameManager gameManager;
    public UI UI;
    public AnimationPlayer animationPlayer;

    [Export]
    public int Speed { get; set; } = 64;

    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        UI = (UI)GetNode("/root/World/UI");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void GetInput()
    {
        Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
        Velocity = inputDirection * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
        AnimationCheck();
    }

    public void AnimationCheck()
    {
        if (Velocity != new Vector2(0, 0))
        {
            animationPlayer.Play("Walk");
            if (Velocity.X > 0)
            {
                ChangeDirection("right");
            }
            else
            {
                ChangeDirection("left");
            }
        }
        else
        {
            animationPlayer.Stop();
        }
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
}