using Godot;
using System;

public partial class ForceFieldManager : Node2D
{
	[Export]
	private ActivateOnCondition activateOnCondition;

	[Export]
	private ForceField[] forceFields;

	public override void _Ready()
	{
		activateOnCondition.Activate += DestroyAllForcefields;
	}

	private void DestroyAllForcefields(){
		foreach(ForceField f in forceFields){
			f.DestroyForcefield();
		}
	}
}
