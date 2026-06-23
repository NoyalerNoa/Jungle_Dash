using Godot;

public partial class Snake : CharacterBody2D
{
	[Export] public float Speed = 55.0f;
	[Export] public float LeftLimit = 0.0f;
	[Export] public float RightLimit = 0.0f;

	private int direction = 1;
	private Sprite2D deathScreen;
	private AnimatedSprite2D animatedSprite;
	private Camera2D camera;
	private CanvasLayer hud;
	private Timer timer;
	private PlayerControls player;
	private Area2D fallDetecterFront;
	private Area2D fallDetecterBack;
	private bool isRespawningPlayer;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		deathScreen = GetNodeOrNull<Sprite2D>("../Death_Screen");
		player = GetNodeOrNull<PlayerControls>("../Player");
		camera = GetNodeOrNull<Camera2D>("../Player/Camera2D");
		hud = GetNodeOrNull<CanvasLayer>("../HUD");
		fallDetecterFront = GetNodeOrNull<Area2D>("Fall_Detecter_Front");
		fallDetecterBack = GetNodeOrNull<Area2D>("Fall_Detecter_Back");

		if (LeftLimit == 0.0f && RightLimit == 0.0f)
		{
			LeftLimit = Position.X - 90.0f;
			RightLimit = Position.X + 90.0f;
		}

		GetNode<Area2D>("Body").BodyEntered += OnBodyEntered;
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is PlayerControls && !isRespawningPlayer)
		{
			isRespawningPlayer = true;
			if (deathScreen != null && camera != null)
			{
				deathScreen.GlobalPosition = camera.GlobalPosition;
				deathScreen.Visible = true;
			}

			GetTree().Paused = true;
			if (hud != null)
				hud.Visible = false;
			timer.Start();
		}
	}

	private void OnTimerTimeout()
	{
		timer.Stop();
		if (player != null)
		{
			player.Position = player.RespawnPosition;
			player.Velocity = Vector2.Zero;
		}

		if (hud != null)
			hud.Visible = true;
		if (deathScreen != null)
			deathScreen.Visible = false;

		GetTree().Paused = false;
		isRespawningPlayer = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
			velocity += GetGravity() * (float)delta;

		bool frontHasFloor = fallDetecterFront == null || fallDetecterFront.GetOverlappingBodies().Count > 0;
		bool backHasFloor = fallDetecterBack == null || fallDetecterBack.GetOverlappingBodies().Count > 0;

		if (Position.X <= LeftLimit)
			direction = 1;
		else if (Position.X >= RightLimit)
			direction = -1;
		else if (IsOnWall())
			direction *= -1;
		else if (IsOnFloor() && direction > 0 && !frontHasFloor)
			direction = -1;
		else if (IsOnFloor() && direction < 0 && !backHasFloor)
			direction = 1;

		velocity.X = Speed * direction;

		if (animatedSprite != null)
			animatedSprite.FlipH = direction < 0;

		Velocity = velocity;
		MoveAndSlide();
	}
}
