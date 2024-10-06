using Godot;
using System;

public partial class EnterHubMessage : Control
{
	private bool isOnBody = false;

	[Export]
	public Control menu;

    public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("confirm") && isOnBody)
		{
			menu.Visible = true;
			Visible = false;
		}
	}

	public void OnBodyEntered(Node2D node2D)
	{
		if (node2D is PlayerShip ps)
		{
			Visible = true;
			isOnBody = true;
		}
	}

	public void OnBodyExited(Node2D node2D)
	{
		if (node2D is PlayerShip)
		{
			Visible = false;
			isOnBody = false;
			menu.Visible = false;
		}
	}
}
