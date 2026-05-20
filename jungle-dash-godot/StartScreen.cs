using Godot;
using System;

public partial class StartScreen : Control
{
	private VBoxContainer mainButtons;
	private Panel options;

	public override void _Ready()
	{
		// Nodes taken
		mainButtons = GetNode<VBoxContainer>("MainButtons");
		options = GetNode<Panel>("Options");

		// make it seeing
		mainButtons.Visible = true;
		options.Visible = false;

		// Connect the Buttons
		GetNode<Button>("MainButtons/StartButton").Pressed += OnStartPressed;
		GetNode<Button>("MainButtons/OptionsButton").Pressed += OnOptionsPressed;
		GetNode<Button>("MainButtons/ExitButton").Pressed += OnExitPressed;
		GetNode<Button>("Options/BackButton").Pressed += OnBackOptionsPressed;
	}

	// Start the Game
	private void OnStartPressed()
	{
		GetTree().ChangeSceneToFile("res://main_scene.tscn");
	}

	// Options / Settings
	private void OnOptionsPressed()
	{
		mainButtons.Visible = false;
		options.Visible = true;
	}
	
	private void OnBackOptionsPressed()
	{
		options.Visible = false;
		mainButtons.Visible = true;
	}

	// Leave the Game
	private void OnExitPressed()
	{
		GetTree().Quit();
	}
}
