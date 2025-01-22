using Godot;
using System;

public partial class Thruster : ShipComponent
{
	[Export]
	private AnimatedSprite2D thrustAnim;

	[Export]
	private int fuelUsage;

	[Export]
	private float thrust;

	public int GetFuelUsage() => fuelUsage;

	public float GetThrust() => thrust;

	public override void SetupData(ShipComponentData data)
	{
		base.SetupData(data);
		if (data is ThrusterData thrustData)
		{
			thrust = thrustData.Thrust;
			fuelUsage = thrustData.FuelUsage;
		}
	}

	public void SetThrustAnimationActive(bool active)
	{
		thrustAnim.Visible = active;
		if(active) thrustAnim.Play("thrust");
		else thrustAnim.Pause();
	}
}
