using Godot;
using System.Text.Json;

public partial class GameManager : Node
{
	private string SavePath = "user://savegame.json";

	public override void _Ready()
	{
	}

	// BUTTON SIGNAL
	private void OnSaveButtonPressed()
	{
		SaveGame();
	}

	private void SaveGame()
	{
		var saveData = new SaveData
		{
			PlayerX = GetNode<Node2D>("Player").Position.X,
			PlayerY = GetNode<Node2D>("Player").Position.Y,
			Coins = 10
		};

		string json = JsonSerializer.Serialize(saveData);

		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
		file.StoreString(json);

		GD.Print("Game Saved!");
	}

	private void LoadGame()
	{
		if (!FileAccess.FileExists(SavePath))
			return;

		using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
		string json = file.GetAsText();

		var saveData = JsonSerializer.Deserialize<SaveData>(json);

		GetNode<Node2D>("Player").Position = new Vector2(saveData.PlayerX, saveData.PlayerY);

		GD.Print("Game Loaded!");
	}
}

// DATENSTRUKTUR
public class SaveData
{
	public float PlayerX { get; set; }
	public float PlayerY { get; set; }
	public int Coins { get; set; }
}
