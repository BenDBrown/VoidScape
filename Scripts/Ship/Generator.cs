using Godot;
using System;

public partial class Generator : ShipComponent
{
	[Export]
	private float efficiency;

	private int fuelConsumption = 10;
}
