using Godot;
using System;
using System.Linq;
using static Level;

public partial class StartMenu : CanvasLayer
{
	private GameManager gameManager;
    [Export]
    public PackedScene levelScene;
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
        //Timer timer = new()
        //{
        //    OneShot = true,
        //    WaitTime = 2f,
        //};
        //AddChild(timer);
        //timer.Start();
        //timer.Timeout += () =>
        //{
        //    QueueFree();
        //};
        GetTree().ChangeSceneToFile("res://Assets/Scenes/Environments/Level.tscn");
        gameManager.CallDeferred("StartRound", 0);
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
