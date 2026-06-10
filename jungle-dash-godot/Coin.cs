using Godot;

public partial class Coin : Area2D
{
	[Export]
	public int Value = 1;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			if (CoinManager.Instance != null)
				CoinManager.Instance.AddCoins(Value);
			else
				GD.PrintErr("CoinManager not found! Add CoinManager to the scene.");

			QueueFree();
		}
	}
}
