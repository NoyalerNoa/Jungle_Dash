using Godot;
using JungleDash_Godot;
using System;

public partial class AudioControl : HSlider
{
	[Export]
	public string AudioBusName = "Music";

	private int busIndex;

	public override void _Ready()
	{
		// Bus finden
		busIndex = AudioServer.GetBusIndex(AudioBusName);

		// Signal verbinden
		ValueChanged += OnValueChanged;
	}

	private void OnValueChanged(double value)
    {
        Jungle_Dash_Logger.logger.Debug("Die Lautstärke wurde geändert.");
        // Slider-Wert in Dezibel umwandeln
        AudioServer.SetBusVolumeDb(
			busIndex,
			Mathf.LinearToDb((float)value)
		);
	}
}
