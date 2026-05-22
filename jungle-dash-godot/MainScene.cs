using Godot;
using System;

public partial class MainScene : Node2D
{
	private Options options;
	private Button backButton;

	public override void _Ready()
	{
		options = GetNode<Options>("Options");

		options.Visible = false;
		
		options.Connect("BackPressed", Callable.From(OnOptionsBackPressed));
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			TogglePauseMenu();
		}
	}

	private void TogglePauseMenu()
	{
		GetTree().Paused = true;
		options.Visible = true;
	}

	private void OnOptionsBackPressed()
	{
		GetTree().Paused = false;
		options.Visible = false;
	}
}
