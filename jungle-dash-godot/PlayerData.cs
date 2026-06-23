using Godot;
using Godot.Collections;

[GlobalClass]
public partial class PlayerData : Resource
{
	[Export]
	public Vector2 Position { get; set; }

	[Export]
	public float Speed { get; set; }

	[Export]
	public bool DashUnlocked { get; set; }

	[Export]
	public int CurrentDashFrames { get; set; }

	[Export]
	public int Coins { get; set; }

	[Export]
	public float JumpVelocity { get; set; }

	[Export]
	public float DashVelocity { get; set; }
}
