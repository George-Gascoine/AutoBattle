using Godot;
using System;

public partial class UI : CanvasLayer
{
    public GameManager gameManager;
    public Label timerLabel, scoreLabel;
    public ProgressBar hpBar, gasBar, experienceBar;
    public int[] abilities;
    public Sprite2D ability1Icon, ability2Icon, ability3Icon, ability4Icon;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gameManager = (GameManager)GetNode("/root/GameManager");
        //		ability1CD = GetNode<Sprite2D>("Ability1CD");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        //ability1CD.GetNode<ProgressBar>("ProgressBar").Value -= 1;
    }


    public void UISetup()
    {
        AbilitySetup();

        timerLabel = GetNode<Label>("ScoreContainer/RoundTimer");
        scoreLabel = GetNode<Label>("ScoreContainer/RoundScore");
        timerLabel.Text = "";
        scoreLabel.Text = "0";

        hpBar = GetNode<ProgressBar>("BarContainer/HealthBar");
        hpBar.Value = 100;

        gasBar = GetNode<ProgressBar>("BarContainer/GasBar");
        gasBar.Value = 100;

        experienceBar = GetNode<ProgressBar>("ExperienceContainer/ExperienceBar");
        experienceBar.Value = 0;
    }
    public void AbilitySetup()
    {
        Godot.Collections.Array<Node> abilityIcons = GetTree().GetNodesInGroup("AbilityIcons");
        for (int i = 0; i < abilityIcons.Count; i++)
        {
            Sprite2D icon = (Sprite2D)abilityIcons[i].GetNode("Icon");
            gameManager.GetSprite(icon, abilities[i], 16, 32, 32, "res://Assets/Sprites/Abilities/AbilityIcons.png");
        }
    }

    public void UpdateRoundTimer(string time)
    {
        timerLabel.Text = time;
    }

    public void UpdateRoundScore(int score)
    {
        scoreLabel.Text = score.ToString();
    }

    public void UpdatePlayerHealth(double playerHealth)
    {
        hpBar.Value = playerHealth;
    }

    public void UpdatePlayerGas(double playerGas)
    {
        gasBar.Value = playerGas;
    }

    public void UpdatePlayerExperience(double playerExperience)
    {
        experienceBar.Value = playerExperience;
    }


    public void UpdateAbilityCooldowns()
    {
        // If cooldown is 5 secs, convert 5 secs into percentage and set progress bar to that value
        // If progressbar value is 0, ability is ready 
    }
}
