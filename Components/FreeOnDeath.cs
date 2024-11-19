using Godot;
using System;

public partial class FreeOnDeath : Node
{
	public void OnShipDestroyed(Ship ship)
	{
		ship.QueueFree();
	}
}
