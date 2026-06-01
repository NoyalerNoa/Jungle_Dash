using Godot;
using Godot.Collections;
using System;

public partial class PlayerControls : CharacterBody2D
{
	[Export] public float Speed = 300.0f;
	[Export] public float DashVelocity = 1000.0f;
	[Export] public int MaxDashFrames = 10;
	[Export] public int CooldownFrames = 10;
	private int CurrentDashFrames;
	[Export] public float VerticalWalljumpVelocity = 1500.0f;
	[Export] public float JumpVelocity = -400.0f;

	// Reibung für das automatische Abbremsen nach dem Dash
	[Export] public float DashBrakeSpeed = 2000.0f;

	public Dictionary<string, bool> abilitys = new Dictionary<string, bool>()
	{
		{"Dash", true },
		{"Walljump", true}
	};
	private AnimatedSprite2D animatedSprite;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		CurrentDashFrames = MaxDashFrames;
	}

	public Vector2 Dash(Vector2 velocity)
	{
		if (animatedSprite.FlipH)
			velocity.X = -DashVelocity;
		else
			velocity.X = DashVelocity; // Vereinfacht für konstanten Dash-Schnitt

		CurrentDashFrames--;
		velocity.Y = 0;
		return velocity;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		float direction = Input.GetAxis("move_left", "move_right");

		// Gravitation
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Wandsprung & Sprung
		if (Input.IsActionJustPressed("jump"))
		{
			if (IsOnFloor())
			{
				velocity.Y = JumpVelocity;
			}
			else if (abilitys["Walljump"] && IsOnWall())
			{
				velocity.Y = JumpVelocity;
				velocity.X = VerticalWalljumpVelocity * GetWallNormal().X;
			}
		}

		// Dash starten
		if (Input.IsActionJustPressed("dash") && abilitys["Dash"] && CurrentDashFrames == MaxDashFrames)
		{
			velocity = Dash(velocity);
		}

		// Dash Logik & Abbremsen
		if (CurrentDashFrames < MaxDashFrames)
		{
			if (CurrentDashFrames > 0)
			{
				velocity = Dash(velocity);
			}
			else if (CurrentDashFrames > -CooldownFrames)
			{
				// Automatische Abbremsung statt hartem "velocity.X = 0;"
				velocity.X = Mathf.MoveToward(velocity.X, direction * Speed, DashBrakeSpeed * (float)delta);
				CurrentDashFrames--;
			}
		}

		// Cooldown Reset
		if (CurrentDashFrames + CooldownFrames <= 0)
		{
			CurrentDashFrames = MaxDashFrames;
		}

		// Animationen & Normale Bewegung
		if (CurrentDashFrames < MaxDashFrames && CurrentDashFrames > 0)
		{
			animatedSprite.Play("dash");
		}
		else
		{
			// Normale Bewegung greift nur, wenn wir nicht mehr aktiv bremsen oder das Tempo schon angepasst ist
			if (CurrentDashFrames <= 0)
			{
				velocity.X = Mathf.MoveToward(velocity.X, direction * Speed, Speed * 10.0f * (float)delta);
			}

			if (IsOnFloor())
			{
				if (direction != 0)
					animatedSprite.Play("run");
				else
					animatedSprite.Play("idle");
			}
			else
			{
				if (velocity.Y > 0)
					animatedSprite.Play("jump");
				else
					animatedSprite.Play("fall");
			}
		}

		// Blickrichtung des Sprites
		if (direction > 0)
			animatedSprite.FlipH = false;
		else if (direction < 0)
			animatedSprite.FlipH = true;

		Velocity = velocity;
		MoveAndSlide();
	}
}
