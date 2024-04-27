using Godot;
using System;

public partial class Generator : Node2D, IShipComponent
{
    [Export]
    private DefenseInfo defenseInfo;

	[Export]
	private float efficiency;

	private int fuelConsumption = 10;

    public DefenseInfo GetDefenseInfo() => defenseInfo;
}
