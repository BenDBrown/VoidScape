using Godot;
using Godot.Collections;
using System;

public partial class Generator : ShipComponent
{
	[Export]
	public float efficiency { get; private set; }

	[Export]
	public int maxPowerGenerated { get; private set; }

	protected override void SetupData()
	{
		base.SetupData();
		if (Data is GeneratorData generatorData)
		{
			efficiency = generatorData.efficiency;
			maxPowerGenerated = generatorData.maxPowerGenerated;
		}
		else GD.PushError("tried assigning non generator component data to generator");
	}
}
