using Godot;
using System;

public partial class StartScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("StartButton").Pressed += _on_start_pressed;
		GetNode<Button>("OptionsButton").Pressed += _on_options_pressed;
		GetNode<Button>("ExitButton").Pressed += _on_exit_pressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	// Start the Game
	private void _on_start_pressed()
	{
	}
	
	// Options / Settings
	private void _on_options_pressed()
	{
	}
	
	// Exit / Leave the Game
	private void _on_exit_pressed()
	{
	}
}
