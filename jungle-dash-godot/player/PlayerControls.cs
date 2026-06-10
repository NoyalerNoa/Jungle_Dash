using Godot;
using Godot.Collections;
using System;

public partial class PlayerControls : CharacterBody2D
{
	[Export] public float Speed = 300.0f;
	[Export] public float DashVelocitiy = 1000.0f;
	[Export] public int MaxDashFrames = 10;
	[Export] public int CooldownFrames = 10;
	private int CurrentDashFrames;
	[Export] public float JumpVelocity = -400.0f;
	private AnimatedSprite2D animatedSprite; // Von KI
	public Dictionary<string, bool> abilitys = new Dictionary<string, bool>()
	{
		{"Dash", false }
	};

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); //Bis hier
		CurrentDashFrames = MaxDashFrames;
		AddToGroup("Player");
	}

	public Vector2 Dash(Vector2 velocity)
	{
		if (animatedSprite.FlipH)
			velocity.X -= DashVelocitiy;
		else
			velocity.X += DashVelocitiy;
		CurrentDashFrames--;
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
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.

		// Von Ki weil das automatische nicht ging.
		float direction = Input.GetAxis("move_left", "move_right");


		velocity.X = direction * Speed;

		if (Input.IsActionJustPressed("dash") && CurrentDashFrames == MaxDashFrames)
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
		//Bis hier
		MoveAndSlide();
	}
}
