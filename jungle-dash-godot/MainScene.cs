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
	private Sprite2D background;
	private PackedScene coinScene;
	private PackedScene snakeScene;
	private Control shopPanel;
	private Label shopStatusLabel;
	private Label levelLabel;
	private Texture2D jungleBackgroundTexture;
	private Texture2D desertBackgroundTexture;
	private bool isInSecondMap;

	public override void _Ready()
	{
		GetTree().Paused = false;
		globalData = GetNode<GlobalData>("/root/GlobalData");
		player = GetNode<PlayerControls>("Player");
		mapLayer = GetNode<TileMapLayer>("TileMap/TileMapLayer");
		background = GetNode<Sprite2D>("Sprite2D");
		jungleBackgroundTexture = background.Texture;
		coinScene = GD.Load<PackedScene>("res://Coin.tscn");
		snakeScene = GD.Load<PackedScene>("res://nodes/snake.tscn");

		options = GetNode<Options>("HUD/Options");
		options.Visible = false;
		options.Connect("BackPressed", Callable.From(OnOptionsBackPressed));
		GetNode<Button>("HUD/Options/MenuButton").Pressed += OnMenuButtonPressed;
		GetNode<Button>("HUD/SaveLoadGroup/Save").Pressed += OnSaveGamePressed;
		GetNode<Button>("HUD/SaveLoadGroup/Load").Pressed += OnLoadGamePressed;

		coinLabel = GetNode<Label>("HUD/CoinLabel");
		coinManager = GetNode<CoinManager>("CoinManager");
		coinManager.CoinChanged += UpdateCoinLabel;

		BuildExtraMapDetails();
		BuildShop();
		SpawnMapRewardsAndEnemies();

		if (globalData.TryConsumeLoadedGame(out PlayerData loadedGame))
			ApplyLoadedGame(loadedGame);
		else
			UpdateCoinLabel(coinManager.Coins);

		Jungle_Dash_Logger.logger?.Debug("Main scene initialized.");
	}

	public override void _Process(double delta)
	{
		if (!isInSecondMap && player.Position.X > 1185.0f)
			EnterSecondMap();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel") || @event.IsActionPressed("esc"))
			TogglePauseMenu();
	}

	private void UpdateCoinLabel(int amount)
	{
		coinLabel.Text = $"Tokens: {amount}";
	}

	private void TogglePauseMenu()
	{
		if (shopPanel != null)
			shopPanel.Visible = false;

		GetTree().Paused = true;
		options.Visible = true;
	}

	private void OnOptionsBackPressed()
	{
		GetTree().Paused = false;
		options.Visible = false;
	}

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

	private void BuildExtraMapDetails()
	{
		PlacePlatform(new Vector2I(8, 30), 10);
		PlacePlatform(new Vector2I(23, 27), 9);
		PlacePlatform(new Vector2I(39, 24), 8);
		PlacePlatform(new Vector2I(53, 29), 11);
		PlacePlatform(new Vector2I(65, 21), 8);
		PlaceColumn(new Vector2I(18, 31), 4);
		PlaceColumn(new Vector2I(35, 28), 5);
		PlaceColumn(new Vector2I(50, 25), 5);
		PlaceColumn(new Vector2I(61, 30), 4);

		PlacePlatform(new Vector2I(84, 31), 12);
		PlacePlatform(new Vector2I(101, 27), 9);
		PlacePlatform(new Vector2I(116, 23), 11);
		PlacePlatform(new Vector2I(133, 29), 12);
		PlacePlatform(new Vector2I(151, 25), 10);
		PlaceColumn(new Vector2I(96, 32), 4);
		PlaceColumn(new Vector2I(127, 24), 6);
		PlaceColumn(new Vector2I(145, 30), 4);
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

	private void BuildShop()
	{
		CanvasLayer hud = GetNode<CanvasLayer>("HUD");

		levelLabel = new Label();
		levelLabel.Text = "Jungle Ridge";
		levelLabel.Position = new Vector2(238, 18);
		levelLabel.Size = new Vector2(260, 36);
		levelLabel.AddThemeFontSizeOverride("font_size", 24);
		levelLabel.AddThemeColorOverride("font_color", new Color(0.75f, 1.0f, 0.72f));
		hud.AddChild(levelLabel);

		Button shopButton = CreateHudButton("Shop", new Vector2(760, 16), new Vector2(116, 42));
		shopButton.Pressed += ToggleShop;
		hud.AddChild(shopButton);

		shopPanel = new Panel();
		shopPanel.Visible = false;
		shopPanel.Position = new Vector2(330, 92);
		shopPanel.Size = new Vector2(492, 292);
		hud.AddChild(shopPanel);

		Label title = new Label();
		title.Text = "Token Shop";
		title.Position = new Vector2(24, 18);
		title.Size = new Vector2(260, 42);
		title.AddThemeFontSizeOverride("font_size", 34);
		title.AddThemeColorOverride("font_color", new Color(1.0f, 0.86f, 0.28f));
		shopPanel.AddChild(title);

		shopStatusLabel = new Label();
		shopStatusLabel.Text = "Kaufe Upgrades mit gesammelten Tokens.";
		shopStatusLabel.Position = new Vector2(24, 224);
		shopStatusLabel.Size = new Vector2(440, 48);
		shopStatusLabel.AddThemeFontSizeOverride("font_size", 18);
		shopPanel.AddChild(shopStatusLabel);

		AddShopButton("Speed +", "5 Tokens", new Vector2(24, 76), 5, () =>
		{
			player.Speed += 120.0f;
			shopStatusLabel.Text = $"Speed gekauft: Laufgeschwindigkeit jetzt {player.Speed:0}.";
		});
		AddShopButton("Jump +", "7 Tokens", new Vector2(184, 76), 7, () =>
		{
			player.JumpVelocity -= 120.0f;
			shopStatusLabel.Text = "Jump gekauft: Du springst jetzt deutlich hoeher.";
		});
		AddShopButton("Dash +", "9 Tokens", new Vector2(344, 76), 9, () =>
		{
			player.DashVelocity += 350.0f;
			player.CooldownFrames = Mathf.Max(8, player.CooldownFrames - 8);
			shopStatusLabel.Text = "Dash gekauft: weiter dashen, schneller wieder bereit.";
		});
		AddShopButton("Checkpoint", "12 Tokens", new Vector2(24, 144), 12, () =>
		{
			player.RespawnPosition = player.Position;
			player.FallRespawnY += 300.0f;
			shopStatusLabel.Text = "Checkpoint gekauft: Respawn ist jetzt hier.";
		});
	}

	private Button CreateHudButton(string text, Vector2 position, Vector2 size)
	{
		Button button = new Button();
		button.Text = text;
		button.Position = position;
		button.Size = size;
		button.FocusMode = Control.FocusModeEnum.None;
		button.AddThemeFontSizeOverride("font_size", 22);
		return button;
	}

	private void AddShopButton(string name, string priceText, Vector2 position, int price, System.Action applyUpgrade)
	{
		Button button = CreateHudButton($"{name}\n{priceText}", position, new Vector2(124, 56));
		button.Pressed += () =>
		{
			if (!coinManager.SpendCoins(price))
			{
				shopStatusLabel.Text = $"Nicht genug Tokens fuer {name}.";
				return;
			}

			applyUpgrade();
		};
		shopPanel.AddChild(button);
	}

	private void ToggleShop()
	{
		shopPanel.Visible = !shopPanel.Visible;
	}

	private void SpawnMapRewardsAndEnemies()
	{
		SpawnCoin(new Vector2(1360, 470));
		SpawnCoin(new Vector2(1412, 470));
		SpawnCoin(new Vector2(1660, 405));
		SpawnCoin(new Vector2(1716, 405));
		SpawnCoin(new Vector2(1948, 340));
		SpawnCoin(new Vector2(2012, 340));
		SpawnCoin(new Vector2(2216, 430));
		SpawnCoin(new Vector2(2280, 430));
		SpawnCoin(new Vector2(2470, 370));

		SpawnSnake(new Vector2(309, 298));
		SpawnSnake(new Vector2(916, 430));
		SpawnSnake(new Vector2(1485, 454));
		SpawnSnake(new Vector2(1810, 392));
		SpawnSnake(new Vector2(2310, 424));
	}

	private void SpawnCoin(Vector2 position)
	{
		Node2D coin = coinScene.Instantiate<Node2D>();
		coin.Position = position;
		AddChild(coin);
	}

	private void SpawnSnake(Vector2 position)
	{
		Snake snake = snakeScene.Instantiate<Snake>();
		snake.Position = position;
		snake.LeftLimit = position.X - 88.0f;
		snake.RightLimit = position.X + 88.0f;
		AddChild(snake);
	}

	private void EnterSecondMap()
	{
		isInSecondMap = true;
		player.Position = new Vector2(1368, 462);
		player.RespawnPosition = player.Position;
		player.MinMapX = 1320.0f;
		player.MaxMapX = 2600.0f;
		background.Texture = GetDesertBackgroundTexture();
		background.Position = new Vector2(576.0f, 324.0f);
		background.Scale = Vector2.One;
		background.Modulate = Colors.White;
		levelLabel.Text = "Sunset Desert";
		shopStatusLabel.Text = "Neue Map erreicht: Sunset Desert!";
	}

	private Texture2D GetDesertBackgroundTexture()
	{
		if (desertBackgroundTexture != null)
			return desertBackgroundTexture;

		Image image = Image.CreateEmpty(1152, 648, false, Image.Format.Rgba8);
		Vector2 sunCenter = new Vector2(894.0f, 116.0f);
		float sunRadius = 54.0f;

		for (int y = 0; y < image.GetHeight(); y++)
		{
			float t = (float)y / image.GetHeight();
			Color sky = new Color(
				Mathf.Lerp(0.48f, 0.98f, t),
				Mathf.Lerp(0.78f, 0.58f, t),
				Mathf.Lerp(0.95f, 0.30f, t),
				1.0f);

			for (int x = 0; x < image.GetWidth(); x++)
			{
				float distanceToSun = new Vector2(x, y).DistanceTo(sunCenter);
				Color pixel = sky;

				if (distanceToSun < sunRadius)
					pixel = new Color(1.0f, 0.86f, 0.32f, 1.0f);
				else if (distanceToSun < sunRadius + 22.0f)
					pixel = pixel.Lerp(new Color(1.0f, 0.78f, 0.32f, 1.0f), 0.35f);

				float backDune = 420.0f + Mathf.Sin(x * 0.012f) * 28.0f + Mathf.Sin(x * 0.004f) * 46.0f;
				float frontDune = 510.0f + Mathf.Sin(x * 0.018f + 1.6f) * 34.0f + Mathf.Sin(x * 0.006f) * 42.0f;

				if (y > backDune)
					pixel = new Color(0.78f, 0.47f, 0.22f, 1.0f);
				if (y > frontDune)
					pixel = new Color(0.96f, 0.65f, 0.30f, 1.0f);

				image.SetPixel(x, y, pixel);
			}
		}

		desertBackgroundTexture = ImageTexture.CreateFromImage(image);
		return desertBackgroundTexture;
	}
}
