using Godot;
using System;

public partial class Snake : CharacterBody2D
{
	public const float Speed = 50.0f;
	public const float JumpVelocity = -400.0f;
	private int direction = 1;
	private Sprite2D deathScreen;
	private AnimatedSprite2D animatedSprite;
	private Camera2D camera;
	private CanvasLayer hud;
	private Timer timer;
	private PlayerControls player;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); //Bis hier
		deathScreen = GetNode<Sprite2D>("../Death_Screen");
		camera = GetNode<Camera2D>("../Player/Camera2D");
		player = GetNode<PlayerControls>("../Player");
		hud = GetNode<CanvasLayer>("../HUD");
		GetNode<Area2D>("Body").BodyEntered += OnBodyEntered;
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;
	}

	private async void OnBodyEntered(Node2D body) // Von Ki
	{
		if (body is PlayerControls)
		{
			deathScreen.GlobalPosition = camera.GlobalPosition;
			GetTree().Paused = true;
			hud.Visible = false;
			deathScreen.Visible = true;
			timer.Start();
		}
	}

	private void OnTimerTimeout()
	{
		timer.Stop();
		player.Position = new Vector2(136, 542);
		hud.Visible = true;
		deathScreen.Visible = false;
		GetTree().Paused = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		var fallDetecter_front = GetNode<Area2D>("Fall_Detecter_Front");
		var fallDetecter_back = GetNode<Area2D>("Fall_Detecter_Back");

		if (fallDetecter_front.GetOverlappingBodies().Count == 0)
		{
			direction = -1;
		}
		else if (fallDetecter_back.GetOverlappingBodies().Count == 0)
		{
			direction = 1;
		}
		else if(IsOnWall())
		{
			direction *= -1;
		}
		Vector2 velocity = Velocity;


		velocity.X = Speed * direction;


		if (direction > 0)
		{
			animatedSprite.FlipH = false;
		}
		else
		{
			animatedSprite.FlipH = true; 
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
