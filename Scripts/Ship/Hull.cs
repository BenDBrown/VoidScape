using Godot;
using System;

public partial class Hull : Node2D, IShipComponent
{
	[Export]
    private DefenseInfo defenseInfo;

    public DefenseInfo GetDefenseInfo() => defenseInfo;
}
