using Godot;
using System;

public partial class Thruster : ShipComponent, IPowerable
{
	[Export]
	private AnimatedSprite2D thrustAnim;

	[Export]
	private int powerdraw;

	[Export]
	private float thrust;

	public int GetPowerDraw() => powerdraw;

	public float GetThrust() => thrust;

	public override void SetupData(ShipComponentData data)
	{
		base.SetupData(data);
		if (data is ThrusterData thrustData)
		{
			thrust = thrustData.Thrust;
			powerdraw = thrustData.Powerdraw;
		}
	}

	public void SetThrustAnimationActive(bool active)
	{
		thrustAnim.Visible = active;
		if(active) thrustAnim.Play("thrust");
		else thrustAnim.Pause();
	}
}
