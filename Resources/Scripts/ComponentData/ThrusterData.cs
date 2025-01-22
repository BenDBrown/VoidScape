using Godot;
using System;

[GlobalClass]
public partial class ThrusterData : ShipComponentData
{
	[Export]
	public float Thrust { get; private set; }

	[Export]
	public int FuelUsage = 50;
}
