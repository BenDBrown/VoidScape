using Godot;
using System;

public partial class DeathWalls : Node2D
{
	public void OnBodyEntered(Node2D node)
	{
		if (node == Game.Instance.PlayerShip)
		{
			if (node is Ship ship) ship.ShipDestroyed();
		}
	}
}
