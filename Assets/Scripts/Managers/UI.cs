using Godot;
using System;

public partial class UI : CanvasLayer
{
	public ProgressBar hpBar, gasBar;
	public Sprite2D ability1CD;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hpBar = GetNode<ProgressBar>("BarContainer/HealthBar");
		hpBar.Value = 100;

		gasBar = GetNode<ProgressBar>("BarContainer/GasBar");
		gasBar.Value = 100;	
//		ability1CD = GetNode<Sprite2D>("Ability1CD");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        //ability1CD.GetNode<ProgressBar>("ProgressBar").Value -= 1;
    }

    public void UpdatePlayerHealth(double playerHealth)
	{
        hpBar.Value = playerHealth;
	}

    public void UpdatePlayerGas(double playerGas)
    {
        gasBar.Value = playerGas;
    }

    public void UpdateAbilityCooldowns()
	{
		// If cooldown is 5 secs, convert 5 secs into percentage and set progress bar to that value
		// If progressbar value is 0, ability is ready 
	}
}
