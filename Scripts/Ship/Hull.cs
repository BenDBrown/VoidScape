using Godot;
using System;

public partial class Hull : ShipComponent
{
	[Export]
	public int cargoCapacity {get; private set;}
}
