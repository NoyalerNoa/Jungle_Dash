using Godot;

public partial class MainScene : Node2D
{
	private Options options;
	private Label coinLabel;
	private CoinManager coinManager;

	public override void _Ready()
	{
		options = GetNode<Options>("Player/Options");
		options.Visible = false;
		options.Connect("BackPressed", Callable.From(OnOptionsBackPressed));
		GetNode<Button>("Player/Options/MenuButton").Pressed += OnMenuButtonPressed;

		coinLabel = GetNode<Label>("HUD/CoinLabel");

		// CoinManager holen und Event abonnieren
		coinManager = GetNode<CoinManager>("CoinManager");
		coinManager.CoinChanged += UpdateCoinLabel;

		// Startwert anzeigen (falls Coins schon vorhanden)
		UpdateCoinLabel(coinManager.Coins);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			TogglePauseMenu();
		}
	}

	private void UpdateCoinLabel(int amount)
	{
		coinLabel.Text = $"Punkte: {amount}";
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

	private void OnMenuButtonPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://start_screen.tscn");
	}
}
