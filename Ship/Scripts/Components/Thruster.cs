using Godot;
using System;

public partial class Thruster : ShipComponent, IPowerable
{
	[Export]
	private int powerdraw;

	[Export]
	private float thrust;

	public int GetPowerDraw() => powerdraw;

	public float GetThrust() => thrust;

	protected override void SetupData()
	{
		base.SetupData();
		if (Data is ThrusterData thrustData)
		{
			thrust = thrustData.Thrust;
		}
	}
}
