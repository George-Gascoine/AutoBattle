using Godot;
using System;

public partial class StartMenu : CanvasLayer
{
	private GameManager gameManager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        gameManager = (GameManager)GetNode("/root/GameManager");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StartButton()
	{
        //gameManager.SceneTransition("Opening");
        Timer timer = new()
        {
            OneShot = true,
            WaitTime = 2f,
        };
        AddChild(timer);
        timer.Start();
        timer.Timeout += () =>
        {
            QueueFree();
        };
    }
    public void LoadButton()
    {

    }
    public void OptionsButton()
    {

    }
    public void ExitButton()
    {
        GetTree().Quit();
    }
}
