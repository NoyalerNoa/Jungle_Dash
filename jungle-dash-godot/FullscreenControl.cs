using Godot;
using System;

public partial class FullscreenControl : CheckButton
{
	public override void _Ready()
	{
		Toggled += OnToggled;

		ButtonPressed =
			DisplayServer.WindowGetMode()
			== DisplayServer.WindowMode.Fullscreen;
	}

	private void OnToggled(bool toggledOn)
	{
		if (toggledOn)
		{
			DisplayServer.WindowSetMode(
				DisplayServer.WindowMode.Fullscreen
			);
		}
		else
		{
			DisplayServer.WindowSetMode(
				DisplayServer.WindowMode.Windowed
			);
		}
	}
}
