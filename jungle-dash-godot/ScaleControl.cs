using Godot;
using System;

public partial class ScaleControl : OptionButton
{
	private float[] options = { 1f, 0.75f, 0.5f, 0.25f };

	public override void _Ready()
	{
		ItemSelected += OnItemSelected;
	}

	private void OnItemSelected(long index)
	{
		float value = options[index];

		GD.Print(value);

		GetTree().Root.ContentScaleFactor = value;
	}
}
