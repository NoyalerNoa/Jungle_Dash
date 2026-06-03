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
	// Von Claude um automatische Abbremsung zu ermöglichen: Neue Variable für die Bremsrate nach dem Dash
	[Export] public float DashBrakeRate = 2000.0f;

	public Dictionary<string, bool> abilitys = new Dictionary<string, bool>()
	{
		{"Dash", true },
		{"Walljump", true}
	};
	private AnimatedSprite2D animatedSprite; // Von ChatGPT; Prompt: Wie erstelle ich animationen?

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); //Bis hier
		CurrentDashFrames = MaxDashFrames;
	}

	public Vector2 Dash(Vector2 velocity)
	{
		if (animatedSprite.FlipH)
			velocity.X = -DashVelocity;
		else
			velocity.X += DashVelocity - velocity.X;
		CurrentDashFrames--;
		velocity.Y = 0;
		return velocity;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump"))
		{
			if (IsOnFloor())
			{
				velocity.Y = JumpVelocity;
			}
			else if (abilitys["Walljump"])
			{
				if (IsOnWall())
				{
					velocity.Y = JumpVelocity;
					velocity.X = VerticalWalljumpVelocity * GetWallNormal().X;
				}
			}
		}


		// Von Ki weil das automatische nicht ging.
		float direction = Input.GetAxis("move_left", "move_right");


		//Bis hier

		if (Input.IsActionJustPressed("dash") && abilitys["Dash"] && CurrentDashFrames == MaxDashFrames)
		{
			velocity = Dash(velocity);
		}

		if (CurrentDashFrames < MaxDashFrames)
		{
			if (CurrentDashFrames > 0)
			{
				velocity = Dash(velocity);
			}
			else if (CurrentDashFrames > -CooldownFrames)
			{
				// Von Claude um automatische Abbremsung zu ermöglichen: Ersetzt das harte Stoppen durch sanftes Abbremsen Richtung 0
				velocity.X = Mathf.MoveToward(velocity.X, 0, DashBrakeRate * (float)delta);
				CurrentDashFrames--;
			}
		}

		if (CurrentDashFrames + CooldownFrames <= 0)
		{
			CurrentDashFrames = MaxDashFrames;
		}

		if (CurrentDashFrames < MaxDashFrames && CurrentDashFrames > 0)
		{
			animatedSprite.Play("dash");
		}

		else
		{
			// Von Gemini um die Bewegung zu verbessern: Nutzt MoveToward für eine physikalisch weiche Annäherung an die Zielgeschwindigkeit, anstatt die X-Velocity hart zu überschreiben
			velocity.X = Mathf.MoveToward(velocity.X, direction * Speed, Speed * 10.0f * (float)delta);

			if (IsOnFloor())
			{
				if (direction != 0)
				{
					animatedSprite.Play("run");
				}
				else
				{
					animatedSprite.Play("idle");
				}
			}

			else
			{
				if (velocity.Y > 0)
				{
					animatedSprite.Play("jump");
				}
				else
				{
					animatedSprite.Play("fall");
				}
			}
		}


		if (direction > 0)
		{
			animatedSprite.FlipH = false;
		}
		else if (direction < 0)
		{
			animatedSprite.FlipH = true;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
