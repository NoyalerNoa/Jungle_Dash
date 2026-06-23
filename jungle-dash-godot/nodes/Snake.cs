using Godot;
using System;

public partial class Snake : CharacterBody2D
{
	public const float Speed = 50.0f;
	public const float JumpVelocity = -400.0f;
	private int direction = 1;
	private AnimatedSprite2D animatedSprite;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D"); //Bis hier
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

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}


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
