using Godot;
using Godot.Collections;
using System;

public partial class Generator : ShipComponent
{
	[Export]
	public int maxPowerGenerated { get; private set; }

	public override void SetupData(ShipComponentData data)
	{
		base.SetupData(data);
		if (data is GeneratorData generatorData)
		{
			maxPowerGenerated = generatorData.maxPowerGenerated;
		}
		else GD.PushError("tried assigning non generator component data to generator");
	}
}
