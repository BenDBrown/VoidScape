using Godot;
using System;

public partial class FuelTank : Node2D, IShipComponent
{
    [Export]
    private DefenseInfo defenseInfo;
    [Export]
    public int fuelCapacity {get; private set;}

    public DefenseInfo GetDefenseInfo() => defenseInfo;
}
