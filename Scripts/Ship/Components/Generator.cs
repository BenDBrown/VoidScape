using Godot;
using Godot.Collections;
using System;

public partial class Generator : ShipComponent
{
	[Export]
	public float efficiency {get; private set;}

	[Export]
	public int maxPowerGenerated {get; private set;}
	
	public override Dictionary GetInfo()
	{
		Dictionary dict = base.GetInfo();
		dict[nameof(efficiency)] = efficiency;
		dict[nameof(maxPowerGenerated)] = maxPowerGenerated;
		return dict;
	}
	
	public override void SetInfo(Dictionary info)
	{
		base.SetInfo(info);
		efficiency = (float)info[nameof(efficiency)];
		maxPowerGenerated = (int)info[nameof(maxPowerGenerated)];
	}
}
