using Godot;
using System.Drawing;
using System.Net.Http;

public partial class Player : CharacterBody2D
{
    public GameManager gameManager;
    public Sprite2D sprite;
    public UI UI;
    public AnimationPlayer animationPlayer, damagePlayer;
    public ParticleEmitter emitter;

    [Export]
    public int Speed { get; set; } = 64;

    public int health = 100;
    public float gas = 100;
    public int gasDamage = 10;

    public bool takingDamage;

    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        sprite = (Sprite2D)GetNode("Sprite2D");
        UI = (UI)GetNode("/root/World/UI");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        damagePlayer = GetNode<AnimationPlayer>("DamagePlayer");
        emitter = GetNode<ParticleEmitter>("ParticleEmitter");
        takingDamage = false;
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
            animationPlayer.Play("Idle");
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

    private void OnAreaEntered(Area2D area)
    {
        if (area.Name == "DamageCollider")
        {
            gameManager.CreateDamageNumber(10, Position);
            GD.Print("Particle collision detected!");
            TakeDamage(-10);
        }
    }

    public void TakeDamage(int damage)
    {
        if(!takingDamage)
        {
            HealthChange(damage);
            damagePlayer.Play("TakeDamage");
            takingDamage = true;
        }
    }

    public void HealthChange(int change)
    {
        health += change;
        UI.UpdatePlayerHealth(health);
    }

    public void GasChange(float change)
    {
        gas += change;
        UI.UpdatePlayerGas(gas);
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