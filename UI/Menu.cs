using Godot;
using System;

public partial class Menu : Control
{
	public Vector2 OriginalScale;

	[Export]
	public PlayerShip playership;

	public override void _Ready()
	{
		OriginalScale = playership.GlobalScale;
	}

	public void OnButtonUp()
	{
		Visible = false;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(playership, "scale", OriginalScale, 0.8f).SetTrans(Tween.TransitionType.Linear);
	}
}
