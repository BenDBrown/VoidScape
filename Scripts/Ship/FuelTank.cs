using Godot;
using System;

public partial class FuelTank : ShipComponent
{
    [Export]
    public int fuelCapacity {get; private set;}
}
