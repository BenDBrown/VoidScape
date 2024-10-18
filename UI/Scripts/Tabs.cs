using Godot;
using System;

public partial class Tabs : TabContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetTabDisabled(1, true);
		SetTabDisabled(2, true);
	}
}
