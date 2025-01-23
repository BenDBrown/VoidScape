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

	[Export]
	public Vector2 ThrustDirection {get; private set;} = Vector2.Up;

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

    public override void Mirror()
    {
        base.Mirror();
		if(ThrustDirection == Vector2.Right) ThrustDirection = Vector2.Left;
		else if(ThrustDirection == Vector2.Left) ThrustDirection = Vector2.Right;
    }

    public override void RotateRight()
    {
        base.RotateRight();
		if(ThrustDirection == Vector2.Up) ThrustDirection = Vector2.Right;
		else if(ThrustDirection == Vector2.Right) ThrustDirection = Vector2.Down;
		else if (ThrustDirection == Vector2.Down) ThrustDirection = Vector2.Left;
		else if (ThrustDirection == Vector2.Left) ThrustDirection = Vector2.Up;
		else GD.PrintErr($"thruster direction was not in cardinal direction vector: {ThrustDirection.X},{ThrustDirection.Y}");
    }

    public override void RotateLeft()
    {
        base.RotateLeft();
		if(ThrustDirection == Vector2.Up) ThrustDirection = Vector2.Left;
		else if (ThrustDirection == Vector2.Left) ThrustDirection = Vector2.Down;
		else if (ThrustDirection == Vector2.Down) ThrustDirection = Vector2.Right;
		else if(ThrustDirection == Vector2.Right) ThrustDirection = Vector2.Up;
		else GD.PrintErr($"thruster direction was not in cardinal direction vector: {ThrustDirection.X},{ThrustDirection.Y}");
    }

    public void SetThrustAnimationActive(bool active)
	{
		thrustAnim.Visible = active;
		if(active) thrustAnim.Play("thrust");
		else thrustAnim.Pause();
	}

	
}
