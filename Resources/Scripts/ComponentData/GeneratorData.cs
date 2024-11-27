using Godot;
using System;

[GlobalClass]
public partial class GeneratorData : ShipComponentData
{
	[Export]
	public float efficiency { get; private set; }

	[Export]
	public int maxPowerGenerated { get; private set; }
}