using Godot;

public partial class GlobalData : Node
{
	[Export]
	public string CurrentGameName = "current_player_data";

	[Export]
	public string SaveFilePath = "user://save/";

	// Coins zentral gespeichert - erreichbar aus jeder Szene via GetNode<GlobalData>("/root/GlobalData")
	public int TotalCoins = 0;
	private PlayerData pendingLoadedGame;

	public override void _Ready()
	{
		DirAccess.MakeDirRecursiveAbsolute(SaveFilePath);
	}

	public void SaveGameFunction(PlayerData playerData)
	{
		DirAccess.MakeDirRecursiveAbsolute(SaveFilePath);
		TotalCoins = playerData.Coins;
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
		PlayerData playerData = ResourceLoader.Load<PlayerData>(path);
		if (playerData != null)
			TotalCoins = playerData.Coins;
		return playerData;
	}

	public bool HasSaveGame()
	{
		return ResourceLoader.Exists(SaveFilePath + CurrentGameName + ".tres");
	}

	public void RequestLoadedGame(PlayerData playerData)
	{
		pendingLoadedGame = playerData;
	}

	public bool TryConsumeLoadedGame(out PlayerData playerData)
	{
		playerData = pendingLoadedGame;
		pendingLoadedGame = null;
		return playerData != null;
	}
}
