using Godot;
using System;

public partial class WarningWalls : Node2D
{
	[Export]
	private CanvasLayer canvasLayer;
	[Export]
	private AnimationPlayer animationPlayer;


	public void OnBodyEntered(Node2D node)
	{
		if (node == Game.Instance.PlayerShip)
		{
			canvasLayer.Visible = true;
			animationPlayer.Play("warning_animation");
		}
	}

	public void OnBodyExited(Node2D node)
	{
		if (node == Game.Instance.PlayerShip)
		{
			canvasLayer.Visible = false;
			animationPlayer.Stop();
		}
	}
}
