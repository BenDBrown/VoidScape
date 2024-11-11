using Godot;
using System;

public partial class PopUpHover : Node
{

	[Export]
	public CanvasLayer message;


	private Ship playerShip { get => Game.Instance.PlayerShip; }


	public override void _Ready()
	{
		message.Visible = false;
	}

	public void OnBodyEntered(Node2D node2D)
	{
		if (node2D == playerShip)
		{
			message.Visible = true;
		}
	}

	public void OnBodyExited(Node2D node2D)
	{
		if (node2D == playerShip)
		{
			message.Visible = false;
		}
	}


}
