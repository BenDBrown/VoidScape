using Godot;
using System;

public partial class FuelTank : ShipComponent
{
	[Export]
	public int FuelCapacity {get; private set;}

	public override void SetupData(ShipComponentData data)
	{
		base.SetupData(data);
		if (data is FuelTankData fuelTankData)
		{
			// goodluck Genelle
			FuelCapacity = fuelTankData.fuelCapacity;
		}
		else GD.PushError("tried assigning non fuel tank component data to fuel tank");
	}
}
