using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;

public partial class Player : CharacterBody2D
{
    public Character data;

    public GameManager gameManager;
    public Camera2D camera;
    public AbilityManager abilityManager;
    public Sprite2D sprite;
    public UI UI;
    public AnimationPlayer animationPlayer, damagePlayer;
    public ParticleEmitter emitter;
    public Texture2D idle, walk;
    public Area2D damageArea;
    public string direction;
    public List<AbilityManager.Ability> abilities = new();
    public Timer[] abilityCooldowns;

    [Export]
    public float Speed { get; set; } = 64;

    public int health = 100;
    public float gas = 100;
    public float damage = 10;
    public int experience = 0;
    public int level = 1;

    public bool takingDamage;

    public class Character
    {
        public string name;
        public string health;
        public string resource;
        public int resourceAmount;
        public int speed;
        public int[] abilityIDs;
    }


    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        camera = (Camera2D)GetNode("Camera2D");
        abilityManager = GetNode<AbilityManager>("AbilityManager");
        abilityManager.player = this;
        damageArea = GetNode<Area2D>("PickUpArea");
        sprite = (Sprite2D)GetNode("Sprite2D");
        UI = (UI)GetNode("/root/Level/UI");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        damagePlayer = GetNode<AnimationPlayer>("DamagePlayer");
        emitter = GetNode<ParticleEmitter>("ParticleEmitter");
        takingDamage = false;
    }

    public void PlayerSetup()
    {
        foreach(int id in data.abilityIDs)
        {
            AbilityManager.Ability addThis = gameManager.abilityData.First(ability => ability.id == id);
            abilities.Add(addThis);
        }
        abilityCooldowns = new Timer[abilities.Count];
        for(int i = 0; i < abilityCooldowns.Length - 1; i++)
        {
            abilityCooldowns[i] = GetNode<Timer>("Cooldowns/" + "Ability" + (i + 1).ToString() + "CD");
            if (abilities[i].type == "Cooldown")
            {
                abilityCooldowns[i].WaitTime = abilities[i].cooldown;
            }
            else if (abilities[i].type == "Toggle")
            {
                GD.Print("abilitylength" + i);
                abilityCooldowns[i].WaitTime = 0.1;
            }
        }

        string spriteFolder = "res://Assets/Sprites/Characters/" + data.name;
        idle = (Texture2D)ResourceLoader.Load(spriteFolder + "/Idle.png");
        walk = (Texture2D)ResourceLoader.Load(spriteFolder + "/Walk.png");
        Animation idleAnim = animationPlayer.GetAnimation("Idle");
        Animation walkAnim = animationPlayer.GetAnimation("Walk");
        idleAnim.TrackSetKeyValue(0, 0, idle);
        walkAnim.TrackSetKeyValue(0, 0, walk);
    }

    public void GetInput()
    {
        Vector2 inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
        Velocity = inputDirection * Speed;
        if (Input.IsActionJustPressed("1") && abilityCooldowns[0].TimeLeft == 0)
        {
            
            abilityCooldowns[0].Start();
            abilityManager.UseAbility(data.abilityIDs[0]);
        }
        if (Input.IsActionJustPressed("2") && abilityCooldowns[1].TimeLeft == 0)
        {
            abilityCooldowns[1].Start();
            abilityManager.UseAbility(data.abilityIDs[1]);
        }
        if (Input.IsActionJustPressed("3") && abilityCooldowns[2].TimeLeft == 0)
        {
            abilityCooldowns[2].Start();
            abilityManager.UseAbility(data.abilityIDs[2]);
        }
        if (Input.IsActionJustPressed("4") && abilityCooldowns[3].TimeLeft == 0)
        {
            abilityCooldowns[3].Start();
            abilityManager.UseAbility(data.abilityIDs[3]);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        GD.Print(abilityCooldowns[0].TimeLeft);
        var overlapping_areas = damageArea.GetOverlappingAreas();
        foreach(Area2D area in overlapping_areas)
        {
            if(area.Name == "DamageCollider" && !takingDamage)
            {
                TakeDamage(-area.GetParent<Enemy>().damage);
            }
        }
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
            Enemy enemy = area.GetParent<Enemy>();
            int damageTaken = enemy.damage;
            gameManager.CreateDamageNumber(damageTaken, Position);
            TakeDamage(-damageTaken);
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
        health = Math.Max(0, Math.Min(health, 100));
        UI.UpdatePlayerHealth(health);
    }

    public void GasChange(float change)
    {
        gas += change;
        UI.UpdatePlayerGas(gas);
    }

    public void ExperienceChange(int change)
    {
        experience += change;
        UI.UpdatePlayerExperience(experience);
        if (experience >= 100)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        experience -= 100;
        UI.UpdatePlayerExperience(experience);
        UI.skillManager.availableSkillPoints += 1;
        UI.DisplaySkillTree();
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