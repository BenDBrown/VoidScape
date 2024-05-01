using Godot;
using System;

public partial class PlayerController : Node
{
	[Export]
	private Ship ship;


	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("forward")) { ship.ForwardThrust(); }
		else if(Input.IsActionJustReleased("forward")) { ship.StopThrusting(); }
		if(Input.IsActionJustReleased("shoot")) { ship.Shoot(); }
	}
}
