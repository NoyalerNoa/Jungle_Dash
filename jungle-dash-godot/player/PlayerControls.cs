using Godot;
using System;

public partial class PlayerControls : CharacterBody2D
{
	[Export] public float Speed = 300.0f;
	[Export] public float JumpVelocity = -400.0f;
	private AnimatedSprite2D animatedSprite; // Von KI

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	} //Bis hier

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
