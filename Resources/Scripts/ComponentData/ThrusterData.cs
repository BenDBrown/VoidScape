using Godot;
using System;

public partial class ThrusterData : ShipComponentData
{
	[Export]
	public float Thrust { get; private set; }
}
