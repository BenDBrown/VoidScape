using Godot;
using System;

public partial class Menu : Control
{
	public Vector2 OriginalScale;
	public Ship playership;

	public override void _Ready()
	{
		CallDeferred("Init");
	}

	public void Init()
	{
		Node game = GetTree().Root.GetNode("Game");
		var ship = game.Get("player_ship");
		playership = ship.As<Ship>();
		OriginalScale = playership.GlobalScale;
	}

	public void OnButtonUp()
	{
		Visible = false;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(playership, "scale", OriginalScale, 0.8f).SetTrans(Tween.TransitionType.Linear);
	}
}
