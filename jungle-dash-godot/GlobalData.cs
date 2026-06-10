using Godot;

public partial class GlobalData : Node
{
	[Export]
	public string CurrentGameName = "current_player_data";

	[Export]
	public string SaveFilePath = "user://save/";

	// Coins zentral gespeichert - erreichbar aus jeder Szene via GetNode<GlobalData>("/root/GlobalData")
	public int TotalCoins = 0;

	public override void _Ready()
	{
		DirAccess.MakeDirRecursiveAbsolute(SaveFilePath);
	}

	public void SaveGameFunction(PlayerData playerData)
	{
		DirAccess.MakeDirRecursiveAbsolute(SaveFilePath);
		ResourceSaver.Save(
			playerData,
			SaveFilePath + CurrentGameName + ".tres"
		);
	}

	public PlayerData LoadGameFunction()
	{
		string path = SaveFilePath + CurrentGameName + ".tres";
		if (!ResourceLoader.Exists(path))
			return null;
		return ResourceLoader.Load<PlayerData>(path);
	}
}
