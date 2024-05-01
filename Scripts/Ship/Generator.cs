using Godot;
using System;

public partial class Generator : ShipComponent
{
	[Export]
	public float efficiency {get; private set;}

	[Export]
	public int maxPowerGenerated {get; private set;}
}
