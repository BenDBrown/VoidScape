using Godot;
using System;

public partial class EnterHubMessage : Control
{
	private bool isOnBody = false;

	private Tween tween;

	private Vector2 OriginalScale;

	private Vector2 spritePos;

	[Export]
	public PlayerShip playership;

	[Export]
	public Sprite2D shipSprite;

	[Export]
	public Control menu;

	public override void _Ready()
	{
		OriginalScale = playership.GlobalScale;
		spritePos = shipSprite.Position;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("confirm") && isOnBody)
		{
			ScaleShip();
		}
	}

	public void OnBodyEntered(Node2D node2D)
	{
		if (node2D is PlayerShip)
		{
			Visible = true;
			isOnBody = true;
		}
	}

	public void ScaleShip()
	{
		tween = GetTree().CreateTween();
		tween.TweenProperty(playership, "position", spritePos, 1f).SetTrans(Tween.TransitionType.Linear);
		tween.TweenProperty(playership, "scale", Vector2.Zero, 0.8f).SetTrans(Tween.TransitionType.Linear);
		tween.Finished += FinishedTweening;

	}

	public void FinishedTweening()
	{
		tween.Finished -= FinishedTweening;
		menu.Visible = true;
		Visible = false;
	}

}
