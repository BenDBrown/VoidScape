using Godot;
using System;

[GlobalClass]
public partial class FuelTankData : ShipComponentData
{
	[Export]
	public int fuelCapacity {get; private set;}
}
