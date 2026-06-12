using Godot;
using System;

public partial class StartScreen : Control
{
	
	
	private VBoxContainer mainButtons;
	private GlobalData globalData;
	private Options options; // Typ von Panel auf Options geändert

	public override void _Ready()
	{
		GetTree().Paused = false;
		globalData = GetNode<GlobalData>("/root/GlobalData");
		mainButtons = GetNode<VBoxContainer>("MainButtons");
		options = GetNode<Options>("Options"); // Cast zur eigenen Klasse

		mainButtons.Visible = true;
		options.Visible = false;

		// StartScreen Buttons
		GetNode<Button>("MainButtons/StartButton").Pressed += OnStartPressed;
		GetNode<Button>("MainButtons/LoadButton").Pressed += OnLoadPressed;
		GetNode<Button>("MainButtons/OptionsButton").Pressed += OnOptionsPressed;
		GetNode<Button>("MainButtons/ExitButton").Pressed += OnExitPressed;

		// Event von der Options-Szene abonnieren
		options.Connect("BackPressed", Callable.From(OnOptionsBackPressed));
	}

	private void OnStartPressed()
	{
		globalData.TotalCoins = 0;
		GetTree().ChangeSceneToFile("res://main_scene.tscn");
	}

	private void OnLoadPressed()
	{
		PlayerData playerData = globalData.LoadGameFunction();
		if (playerData == null)
		{
			GD.PrintErr("No save game found.");
			return;
		}

		globalData.RequestLoadedGame(playerData);
		GetTree().ChangeSceneToFile("res://main_scene.tscn");
	}

	private void OnOptionsPressed()
	{
		mainButtons.Visible = false;
		options.Visible = true;
	}
	
	private void OnOptionsBackPressed()
	{
		options.Visible = false;
		mainButtons.Visible = true;
	}

	private void OnExitPressed()
	{
		GetTree().Quit();
	}
}
