using Godot;

public partial class CoinManager : Node
{
	public static CoinManager Instance { get; private set; }

	public int Coins { get; private set; } = 0;

	private GlobalData globalData;

	public override void _Ready()
	{
		Instance = this;
		globalData = GetNode<GlobalData>("/root/GlobalData");
		// Coins aus GlobalData laden damit sie Szenenübergreifend erhalten bleiben
		Coins = globalData.TotalCoins;
	}

	public delegate void CoinChangedHandler(int amount);
	public event CoinChangedHandler CoinChanged;

	public void AddCoins(int amount)
	{
		SetCoins(Coins + amount);
	}

	public void ResetCoins()
	{
		SetCoins(0);
	}

	public void SetCoins(int amount)
	{
		Coins = amount;
		globalData.TotalCoins = Coins;
		CoinChanged?.Invoke(Coins);
	}
}
