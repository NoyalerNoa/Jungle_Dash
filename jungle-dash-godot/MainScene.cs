using Godot;
using JungleDash_Godot;

public partial class MainScene : Node2D
{
	private Options options;
	private Label coinLabel;
	private CoinManager coinManager;
	private GlobalData globalData;
	private PlayerControls player;
	private TileMapLayer mapLayer;

	public override void _Ready()
	{
		GetTree().Paused = false;
		globalData = GetNode<GlobalData>("/root/GlobalData");
		player = GetNode<PlayerControls>("Player");
		mapLayer = GetNode<TileMapLayer>("TileMap/TileMapLayer");
		options = GetNode<Options>("Player/Options");
		options.Visible = false;
		options.Connect("BackPressed", Callable.From(OnOptionsBackPressed));
		GetNode<Button>("Player/Options/MenuButton").Pressed += OnMenuButtonPressed;
		GetNode<Button>("HUD/SaveLoadGroup/Save").Pressed += OnSaveGamePressed;
		GetNode<Button>("HUD/SaveLoadGroup/Load").Pressed += OnLoadGamePressed;

		coinLabel = GetNode<Label>("HUD/CoinLabel");

		// CoinManager holen und Event abonnieren
		coinManager = GetNode<CoinManager>("CoinManager");
		coinManager.CoinChanged += UpdateCoinLabel;

		if (globalData.TryConsumeLoadedGame(out PlayerData loadedGame))
			ApplyLoadedGame(loadedGame);
		else
			UpdateCoinLabel(coinManager.Coins);
		Jungle_Dash_Logger.logger.Debug($"Die Hauptszene wurde initialisiert.");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel") || @event.IsActionPressed("esc"))
		{
			TogglePauseMenu();
		}
	}

	private void UpdateCoinLabel(int amount)
	{
		Jungle_Dash_Logger.logger.Debug($"Der Geldstand wird auf {amount} Coins gesetzt.");
		coinLabel.Text = $"Punkte: {amount}";
	}

	private void TogglePauseMenu()
	{
		Jungle_Dash_Logger.logger.Debug("Die Optionen wurden geöffnet.");
		GetTree().Paused = true;
		options.Visible = true;
	}

	private void OnOptionsBackPressed()
	{
		Jungle_Dash_Logger.logger.Debug("Die Optionen wurden geschlossen.");
		GetTree().Paused = false;
		options.Visible = false;
	}

	private void OnMenuButtonPressed()
	{
		Jungle_Dash_Logger.logger.Debug("Es wird zum Startbildschirm gewechselt.");
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://start_screen.tscn");
	}

	private void OnSaveGamePressed()
	{
		Jungle_Dash_Logger.logger.Debug("Das Spiel wird gespeichert.");
		PlayerData playerData = player.CreatePlayerData(coinManager.Coins);
		globalData.SaveGameFunction(playerData);
		GD.Print("Game saved.");
	}

	private void OnLoadGamePressed()
	{
		Jungle_Dash_Logger.logger.Debug("Das Spiel wird geladen.");
		PlayerData playerData = globalData.LoadGameFunction();
		if (playerData == null)
		{
			GD.PrintErr("No save game found.");
			return;
		}

		ApplyLoadedGame(playerData);
		GD.Print("Game loaded.");
	}

	private void ApplyLoadedGame(PlayerData playerData)
	{
		player.ApplyPlayerData(playerData);
		coinManager.SetCoins(playerData.Coins);
	}

	private void BuildExtraMapDetails()
	{
		Jungle_Dash_Logger.logger.Debug("Es werden extra Map-Blöcke gebaut.");
		PlacePlatform(new Vector2I(8, 30), 10);
		PlacePlatform(new Vector2I(23, 27), 9);
		PlacePlatform(new Vector2I(39, 24), 8);
		PlacePlatform(new Vector2I(53, 29), 11);
		PlacePlatform(new Vector2I(65, 21), 8);

		PlaceColumn(new Vector2I(18, 31), 4);
		PlaceColumn(new Vector2I(35, 28), 5);
		PlaceColumn(new Vector2I(50, 25), 5);
		PlaceColumn(new Vector2I(61, 30), 4);
	}

	private void PlacePlatform(Vector2I startCell, int width)
	{
		for (int x = 0; x < width; x++)
		{
			Vector2I atlas = x == 0 ? new Vector2I(1, 1) : x == width - 1 ? new Vector2I(5, 1) : new Vector2I(3, 1);
			mapLayer.SetCell(startCell + new Vector2I(x, 0), 1, atlas);

			if (x > 0 && x < width - 1)
				mapLayer.SetCell(startCell + new Vector2I(x, 1), 1, new Vector2I(3, 3));
		}
	}

	private void PlaceColumn(Vector2I startCell, int height)
	{
		for (int y = 0; y < height; y++)
			mapLayer.SetCell(startCell + new Vector2I(0, y), 1, new Vector2I(3, 3));
	}
}
