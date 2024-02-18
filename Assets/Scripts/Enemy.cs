using Godot;
using System;
using System.Linq;

public partial class Enemy : CharacterBody2D
{

    public GameManager gameManager;
    public EnemyData data;
    public Player player { get; set; }
    public AnimationPlayer animationPlayer { get; set; }
    public AnimationPlayer damagePlayer { get; set; }

    public bool takingDamage = false;
    [Export]
    public int speed { get; set; } = 32;

    public int health = 100;

    public int damage { get; set; }

    private Vector2 _target;

    public bool playerFound;
    public Sprite2D sprite;

    [Export]
    public PackedScene itemDrop;

    public class EnemyData
    {
        public int id;
        public string name;
        public int health;
        public int damage;
        public int speed;
        public int score;
        public int[] abilityIDs;
        public int[][] lootTable; // [dropID, % chance] => [1,20]
        public int[][] spriteData;
    }
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        damagePlayer = GetNode<AnimationPlayer>("DamagePlayer");
        sprite = GetNode<Sprite2D>("Sprite2D");
        CallDeferred("FindPlayer");
    }

    public void EnemySetup()
    {
        string spriteFolder = "res://Assets/Sprites/Enemies/" + data.name + "/";
        Texture2D spriteTexture = (Texture2D)ResourceLoader.Load(spriteFolder + data.name +".png");
        sprite.Texture = spriteTexture;
        SetAnimations();
        speed = data.speed;
        health = data.health;
        damage = data.damage;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (playerFound)
        {
            _target = player.Position;
            Velocity = Position.DirectionTo(_target) * speed;
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
        player = (Player)GetNode("/root/Level/Player"); 
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

    public void SetAnimations()
    {
        Animation walkAnim = animationPlayer.GetAnimation("Walk");
        Vector2 spriteSize = new(data.spriteData[0][0], data.spriteData[0][1]);
        sprite.RegionRect = new Rect2(new Vector2(0, 0), spriteSize);
        int spriteNo = data.spriteData[1][0];
        //walkAnim.Clear();
        walkAnim.Length = 0.25f * spriteNo;
        int keyNo = 0;
        for(float i = 0; i< walkAnim.Length; i += 0.25f)
        {
            walkAnim.TrackInsertKey(0, i, new Rect2());
            walkAnim.TrackSetKeyValue(0, keyNo, new Rect2(new Vector2((spriteSize.X / spriteNo) * i, 0), new Vector2(spriteSize.X / spriteNo, spriteSize.Y)));
            keyNo++;
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

    public int LootRoll()
    {
        int[][] probabilities = data.lootTable;
        // Calculate the total sum of all the probabilities
        int total = probabilities.Sum(p => p[1]);

        // Generate a random number between 0 and the total sum
        Random rnd = new();
        int roll = rnd.Next(total);

        // Find the item that this random number corresponds to
        int sum = 0;
        for (int i = 0; i < probabilities.Length; i++)
        {
            sum += probabilities[i][1];
            if (roll < sum)
                return probabilities[i][0]; // Return the first item that is greater than or equal to the roll
        }

        // If no item is found (which should not happen if probabilities are correct), return -1
        return -1;
    }

    public void DestroyAndDrop()
    {
        gameManager.ScoreUpdate(data.score);
        Drop drop = (Drop)itemDrop.Instantiate();
        Node2D level = GetNode<Node2D>("/root/Level");
        int roll = LootRoll();
        Drop.Data dropData = new();
        dropData = gameManager.dropData.First(findDrop => findDrop.id == roll);
        drop.data = dropData;
        level.CallDeferred("add_child", drop);
        drop.Position = Position;
        QueueFree();
    }
}