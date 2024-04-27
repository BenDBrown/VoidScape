using Godot;
using System;

public partial class FuelTank : Node2D, IShipComponent
{
    [Export]
    private DefenseInfo defenseInfo;
    [Export]
    public int fuelCapacity {get; private set;}

    public override void _Ready()
    {
        GD.Print(defenseInfo.ToString());
    }


    public DefenseInfo GetDefenseInfo() => defenseInfo;

    [Signal]
    public delegate void DestroyedEventHandler(FuelTank fuelTank);
}