using Godot;
using System;

public partial class Options : Panel
{
	[Signal]
	public delegate void BackPressedEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("BackButton").Pressed += OnBackOptionsPressed;
	}
	
	
	private void OnBackOptionsPressed()
	{
		EmitSignal(SignalName.BackPressed);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
