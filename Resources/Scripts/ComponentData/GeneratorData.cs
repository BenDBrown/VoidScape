using Godot;
using System;

[GlobalClass]
public partial class GeneratorData : ShipComponentData
{
	[Export]
	public int maxPowerGenerated { get; private set; }
}