using Godot;
using System;

public partial class Rune : Area2D
{
	[Export(PropertyHint.Enum, "Dash,Walljump")] public string Fähigkeit; // Von Ki, weil es mir get set nicht ging
	private PlayerControls player;
	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play(Fähigkeit);
		player = GetNode<PlayerControls>("../Player");
		BodyEntered += OnBodyEntered;
		this.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node2D body) // Von Ki
	{
		if (body.Name == "Player")
		{
			player.abilitys[Fähigkeit] = true;
			this.Visible = false;
		}
	}
}
