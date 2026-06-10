using Godot;
using System;

public partial class MainScene : Node2D
{
	private Options options;
<<<<<<< Updated upstream
	private Button backButton;

	public override void _Ready()
	{
		options = GetNode<Options>("Options");

=======
	private Label coinLabel;
	private CoinManager coinManager;
	private GlobalData globalData;
	private PlayerControls player;

	public override void _Ready()
	{
		GetTree().Paused = false;
		globalData = GetNode<GlobalData>("/root/GlobalData");
		player = GetNode<PlayerControls>("Player");
		options = GetNode<Options>("Player/Options");
>>>>>>> Stashed changes
		options.Visible = false;
		
		options.Connect("BackPressed", Callable.From(OnOptionsBackPressed));
<<<<<<< Updated upstream
=======
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
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======

	private void OnMenuButtonPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://start_screen.tscn");
	}

	private void OnSaveGamePressed()
	{
		PlayerData playerData = player.CreatePlayerData(coinManager.Coins);
		globalData.SaveGameFunction(playerData);
		GD.Print("Game saved.");
	}

	private void OnLoadGamePressed()
	{
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
>>>>>>> Stashed changes
}
